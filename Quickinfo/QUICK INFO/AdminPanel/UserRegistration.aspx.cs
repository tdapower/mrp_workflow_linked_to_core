//******************************************
// Author            :Tharindu Athapattu
// Date              :11/04/2013
// Reviewed By       :
// Description       : User Registration Form
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

public partial class UserRegistration : System.Web.UI.Page
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

            txtEPF.Attributes.Add("onkeyup", "jsValidateNum(this)");
            txtSearchEPF.Attributes.Add("onkeyup", "jsValidateNum(this)");
            ClearComponents();
            initializeValues();

            Session.Remove("UserRegMode");

            pnlUserGrid.Visible = false;
        }


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
        lblError1.Text = "";
        grdUsers.DataSource = null;
        grdUsers.DataBind();

        if ((txtSearchUserCode.Text == "") && (txtSearchUserName.Text == "") && (txtSearchEPF.Text == "") && (ddlSearchUserRole.SelectedValue.ToString() == "0") && (ddlSearchCompany.SelectedValue.ToString() == "0"))
        {
            lblMsg.Text = "Search text cannot be blank";
            return;
        }


        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchUserCode.Text != "")
        {

            SQL = "(LOWER(T.USER_CODE) LIKE '%" + txtSearchUserCode.Text.ToLower() + "%') AND";
        }

        if (txtSearchUserName.Text != "")
        {

            SQL = SQL + "(LOWER(T.USER_NAME) LIKE '%" + txtSearchUserName.Text.ToLower() + "%') AND";
        }
        if (txtSearchEPF.Text != "")
        {
            SQL = SQL + "(LOWER(T.EPF_NO) LIKE '%" + txtSearchEPF.Text.ToLower() + "%') AND";
        }
        if (ddlSearchUserRole.SelectedValue.ToString() != "0")
        {

            SQL = SQL + "(T.USER_ROLE_CODE LIKE '%" + ddlSearchUserRole.SelectedValue.ToString() + "%') AND";
        }
        if (ddlSearchCompany.SelectedValue.ToString() != "0")
        {

            SQL = SQL + "(T.COMPANY_CODE LIKE '%" + ddlSearchCompany.SelectedValue.ToString() + "%') AND";
        }


        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.USER_CODE AS \"User Code\" ,T.USER_NAME AS \"User Name\",T.COMPANY_CODE AS \"Companmy\",T.EPF_NO AS \"EPF NO\",T.USER_ROLE_CODE,UR.USER_ROLE_NAME AS \"User Role\",(CASE T.STATUS WHEN 1 THEN 'Active' ELSE 'In-Active' END) \"Status\" , T.USER_EMAIL AS \"E-Mail\" FROM WF_ADMIN_USERS T  " +
            " INNER JOIN WF_ADMIN_USER_ROLES UR ON T.USER_ROLE_CODE=UR.USER_ROLE_CODE " +
            " WHERE (" + SQL + ") ORDER BY T.USER_CODE ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdUsers.DataSource = myOleDbDataReader;
            grdUsers.DataBind();

            pnlUserGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserRegistration.aspx");
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
        if (txtUserCode.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter the User Code";
            Timer1.Enabled = true;
            return;
        }


        if (txtUserName.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter the User Name";
            Timer1.Enabled = true;
            return;
        }


        if (txtEPF.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter the EPF";
            Timer1.Enabled = true;
            return;
        }
        if (txtEmail.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter user E-mail";
            Timer1.Enabled = true;
            return;
        }
        if (ddlUserRole.SelectedValue == "" || ddlUserRole.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the User Role";
            Timer1.Enabled = true;
            return;
        }

        if (ddlCompany.SelectedValue == "" || ddlCompany.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select User Company";
            Timer1.Enabled = true;
            return;
        }


        if (Session["UserRegMode"].ToString() == "NEW")
        {
            if (CheckUserCodeAlreadyExist(txtUserCode.Text))
            {
                lblMsg.Text = "Enetered User Code Already Exists";
                Timer1.Enabled = true;
                return;
            }
        }



        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["UserRegMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_WF_ADMIN_USERS");
            }
            else if (Session["UserRegMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_WF_ADMIN_USERS");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar, 50).Value = txtUserCode.Text;
            spProcess.Parameters.Add("V_USER_NAME", OracleType.VarChar, 50).Value = txtUserName.Text;
            spProcess.Parameters.Add("V_EPF_NO", OracleType.Number, 6).Value = Convert.ToInt32(txtEPF.Text);

            spProcess.Parameters.Add("V_USER_EMAIL", OracleType.VarChar, 100).Value = txtEmail.Text.Trim();
            spProcess.Parameters.Add("V_USER_ROLE_CODE", OracleType.Number, 5).Value = Convert.ToInt32(ddlUserRole.SelectedValue);

            spProcess.Parameters.Add("V_USER_COMPANY", OracleType.VarChar).Value = ddlCompany.SelectedValue;


            if (rdbtnInActive.Checked)
            {
                spProcess.Parameters.Add("V_STATUS", OracleType.Number, 1).Value = 0;
            }
            else
            {
                spProcess.Parameters.Add("V_STATUS", OracleType.Number, 1).Value = 1;
            }


            spProcess.ExecuteNonQuery();
            conProcess.Close();

            ClearComponents();
            SearchData();
            lblMsg.Text = "Successfully Saved";
            Timer1.Enabled = true;

            //Response.Redirect("UserRegistration.aspx");
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error While Saving";
            Timer1.Enabled = true;
        }

    }

    private bool CheckUserCodeAlreadyExist(string UserCode)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT USER_CODE FROM WF_ADMIN_USERS WHERE USER_CODE='" + UserCode + "'";

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

    private void ClearComponents()
    {
        txtUserCode.Text = "";
        txtUserName.Text = "";
        txtEPF.Text = "";
        txtEmail.Text = "";
        ddlUserRole.SelectedValue = "0";
        ddlCompany.SelectedValue = "0";

        txtUserCode.Enabled = false;
        txtUserName.Enabled = false;
        txtEPF.Enabled = false;
        txtEmail.Enabled = false;
        ddlUserRole.Enabled = false;
        ddlCompany.Enabled = false;

        rdbtnActive.Enabled = false;
        rdbtnInActive.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        // btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtUserCode.Enabled = true;
        txtUserName.Enabled = true;
        txtEPF.Enabled = true;
        txtEmail.Enabled = true;
        ddlUserRole.Enabled = true;
        ddlCompany.Enabled = true;

        rdbtnActive.Enabled = true;
        rdbtnInActive.Enabled = true;


        txtUserCode.Text = "";
        txtUserName.Text = "";
        txtEPF.Text = "";
        txtEmail.Text = "";
        ddlUserRole.SelectedValue = "0";
        rdbtnActive.Checked = true;

        btnSave.Enabled = true;

        Session["UserRegMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtUserCode.Text == "")
        {
            lblMsg.Text = "Please Select An User";
            Timer1.Enabled = true;
            return;
        }

        //txtUserCode.Enabled = true;
        txtUserName.Enabled = true;
        txtEPF.Enabled = true;
        txtEmail.Enabled = true;
        ddlUserRole.Enabled = true;
        ddlCompany.Enabled = true;


        rdbtnActive.Enabled = true;
        rdbtnInActive.Enabled = true;

        btnSave.Enabled = true;

        Session["UserRegMode"] = "UPDATE";
    }

    protected void grdUsers_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtUserCode.Text = grdUsers.SelectedRow.Cells[1].Text.Trim();
        txtUserName.Text = grdUsers.SelectedRow.Cells[2].Text.Trim();
        if (grdUsers.SelectedRow.Cells[3].Text.Trim() != "&nbsp;" && grdUsers.SelectedRow.Cells[3].Text.Trim() != "")
        {

            ddlCompany.SelectedValue = grdUsers.SelectedRow.Cells[3].Text.Trim();
        }
        txtEPF.Text = grdUsers.SelectedRow.Cells[4].Text.Trim();
        txtEmail.Text = grdUsers.SelectedRow.Cells[8].Text.Trim();
        ddlUserRole.SelectedValue = grdUsers.SelectedRow.Cells[5].Text.Trim();

        if (grdUsers.SelectedRow.Cells[6].Text.Trim() == "Active")
        {
            rdbtnActive.Checked = true;
            rdbtnInActive.Checked = false;
        }
        else
        {
            rdbtnActive.Checked = false;
            rdbtnInActive.Checked = true;
        }

        btnAlter.Enabled = true;
    }

    protected void grdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[4].Visible = false;
    }


    private void initializeValues()
    {
        loadUserRoles();
        loadCompanies();
        lblError1.Text = "";
        lblMsg.Text = "";
    }

    private void loadCompanies()
    {
        ddlCompany.Items.Clear();

        ddlCompany.Items.Add(new ListItem("--- Select One ---", "0"));
        ddlCompany.Items.Add(new ListItem("Life", "Life"));
        ddlCompany.Items.Add(new ListItem("General", "General"));

        ddlSearchCompany.Items.Add(new ListItem("--- Select One ---", "0"));
        ddlSearchCompany.Items.Add(new ListItem("Life", "Life"));
        ddlSearchCompany.Items.Add(new ListItem("General", "General"));



    }

    private void loadUserRoles()
    {
        ddlUserRole.Items.Clear();
        ddlUserRole.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchUserRole.Items.Clear();
        ddlSearchUserRole.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT USER_ROLE_CODE,USER_ROLE_NAME FROM WF_ADMIN_USER_ROLES";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlUserRole.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlSearchUserRole.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }
}
