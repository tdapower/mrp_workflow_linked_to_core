//******************************************
// Author            :Tharindu Athapattu
// Date              :11/04/2013
// Reviewed By       :
// Description       : User Role Setup Form
//******************************************
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Text;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Net;
using System.DirectoryServices;
using System.Net.Mail;
using System.IO;

public partial class UserRoleSetup : System.Web.UI.Page
{

    OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
    OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

    

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            validatePageAuthentication();

            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);

            ClearComponents();
            initializeValues();

            Session.Remove("UserRoleSetupMode");

            pnlUserRoleGrid.Visible = false;
        }
        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        string b = browser.Browser;

    }

    private void validatePageAuthentication()
    {
        if (Request.Params["pagecode"] != null)
        {
            if (Request.Params["pagecode"] != "")
            {
                UserAuthentication userAuthentication = new UserAuthentication();
                if (!userAuthentication.IsAuthorizeForThisPage(Context.User.Identity.Name, Request.Params["pagecode"].ToString()))
                {
                    Response.Redirect("~/NoPermission.aspx");
                }
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchData();
        ClearComponents();
    }

    private void SearchData()
    {
        string SQL = "";
        lblError.Text = "";
        grdUserRoles.DataSource = null;
        grdUserRoles.DataBind();


        if ((txtSearchUserRoleName.Text == "") && (txtSearchDescription.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchUserRoleName.Text != "")
        {

            SQL = "(LOWER(T.USER_ROLE_NAME) LIKE '%" + txtSearchUserRoleName.Text.ToLower() + "%') AND";
        }

        if (txtSearchDescription.Text != "")
        {

            SQL = SQL + "(LOWER(T.DESCRIPTION) LIKE '%" + txtSearchDescription.Text.ToLower() + "%') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);

        String selectQuery = "";
        selectQuery = "   SELECT T.USER_ROLE_CODE AS \"User Role Code\" ,T.USER_ROLE_NAME AS \"User Role Name\",T.DESCRIPTION AS \"Description\"  FROM WF_ADMIN_USER_ROLES T  " +
                      " WHERE (" + SQL + ") ORDER BY T.USER_ROLE_NAME ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdUserRoles.DataSource = myOleDbDataReader;
            grdUserRoles.DataBind();

            pnlUserRoleGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserRoleSetup.aspx");
    }


    public DirectoryEntry GetDirectoryObject()
    {
        DirectoryEntry oDE;
        oDE = new DirectoryEntry("LDAP://192.168.10.251");
        return oDE;
    }


    public DirectoryEntry GetLoginName(string EmployeeID)
    {
        DirectoryEntry de = GetDirectoryObject();
        DirectorySearcher deSearch = new DirectorySearcher();
        deSearch.SearchRoot = de;

        deSearch.Filter = "(&(objectClass=user)(EmployeeID=" + EmployeeID + "))";
        deSearch.SearchScope = SearchScope.Subtree;
        SearchResult results = deSearch.FindOne();


        if (!(results == null))
        {

            de = new DirectoryEntry(results.Path);
            Session["USER"] = de.Properties["SAMAccountName"][0].ToString();
            return de;
        }
        else
        {
            Session["USER"] = "";
            return null;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtUserRoleName.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the User Role Name";
            Timer1.Enabled = true;
            return;
        }


        if (txtDescription.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Description";
            Timer1.Enabled = true;
            return;
        }
        if (Session["UserRoleSetupMode"].ToString() == "NEW")
        {
            if (CheckUserRoleAlreadyExist(txtUserRoleName.Text))
            {
                lblMsg.Text = "Enetered User Role Name Already Exists";
                Timer1.Enabled = true;
                return;
            }
        }

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["UserRoleSetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_WF_ADMIN_USER_ROLES");
            }
            else if (Session["UserRoleSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_WF_ADMIN_USER_ROLES");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            if (Session["UserRoleSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_USER_ROLE_CODE", OracleType.Number, 5).Value = txtUserRoleCode.Text;
            }
            spProcess.Parameters.Add("V_USER_ROLE_NAME", OracleType.VarChar, 100).Value = txtUserRoleName.Text;
            spProcess.Parameters.Add("V_DESCRIPTION", OracleType.VarChar, 250).Value = txtDescription.Text;



            spProcess.ExecuteNonQuery();
            conProcess.Close();

            ClearComponents();
            SearchData();
            lblMsg.Text = "Successfully saved";
            Timer1.Enabled = true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }

    }

    private void ClearComponents()
    {
        txtUserRoleCode.Text = "";
        txtUserRoleName.Text = "";
        txtDescription.Text = "";

        txtUserRoleCode.Enabled = false;
        txtUserRoleName.Enabled = false;
        txtDescription.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
       // btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtUserRoleCode.Enabled = true;
        txtUserRoleName.Enabled = true;
        txtDescription.Enabled = true;

        txtUserRoleCode.Text = "";
        txtUserRoleName.Text = "";
        txtDescription.Text = "";

        btnSave.Enabled = true;

        Session["UserRoleSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtUserRoleCode.Text == "")
        {
            lblMsg.Text = "Please Select An User Role";
            return;
        }


        txtUserRoleName.Enabled = true;
        txtDescription.Enabled = true;

        btnSave.Enabled = true;

        Session["UserRoleSetupMode"] = "UPDATE";
    }

    protected void grdUserRoles_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtUserRoleCode.Text = grdUserRoles.SelectedRow.Cells[1].Text.Trim();
        txtUserRoleName.Text = grdUserRoles.SelectedRow.Cells[2].Text.Trim();
        txtDescription.Text = grdUserRoles.SelectedRow.Cells[3].Text.Trim();


        btnAlter.Enabled = true;
    }

    protected void grdUserRoles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";
    }


    private bool CheckUserRoleAlreadyExist(string UserRoleName)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT USER_ROLE_NAME FROM WF_ADMIN_USER_ROLES WHERE USER_ROLE_NAME='" + UserRoleName + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            returnVal = true;
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }


    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }
}
