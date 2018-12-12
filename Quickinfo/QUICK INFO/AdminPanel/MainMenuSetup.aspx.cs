//******************************************
// Author            :Tharindu Athapattu
// Date              :11/04/2013
// Reviewed By       :
// Description       : Main Menu Setup Form
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

public partial class MainMenuSetup : System.Web.UI.Page
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

            Session.Remove("MainMenuSetupMode");

            pnlMainMenuGrid.Visible = false;
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
        lblError.Text = "";
        grdMainMenus.DataSource = null;
        grdMainMenus.DataBind();

        if ((txtSearchMainMenuName.Text == "") && (txtSearchDescription.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchMainMenuName.Text != "")
        {

            SQL = "(LOWER(T.MAIN_MENU_NAME) LIKE '%" + txtSearchMainMenuName.Text.ToLower() + "%') AND";
        }

        if (txtSearchDescription.Text != "")
        {

            SQL = SQL + "(LOWER(T.DESCRIPTION) LIKE '%" + txtSearchDescription.Text.ToLower() + "%') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.MAIN_MENU_CODE AS \"Main Menu Code\" ,T.MAIN_MENU_NAME AS \"Main Menu Name\",T.DESCRIPTION AS \"Description\"  FROM WF_ADMIN_MAIN_MENU T  " +
                      " WHERE (" + SQL + ") ORDER BY T.MAIN_MENU_NAME ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdMainMenus.DataSource = myOleDbDataReader;
            grdMainMenus.DataBind();

            pnlMainMenuGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("MainMenuSetup.aspx");
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

        if (txtMainMenuName.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Main Menu Name";
            Timer1.Enabled = true;
            return;
        }


        if (txtDescription.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Description";
            Timer1.Enabled = true;
            return;
        }
        if (Session["MainMenuSetupMode"].ToString() == "NEW")
        {
            if (CheckMainMenuAlreadyExist(txtMainMenuName.Text))
            {
                lblMsg.Text = "Enetered Main Menu Name Already Exists";
                Timer1.Enabled = true;
                return;
            }
        }

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["MainMenuSetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_WF_ADMIN_MAIN_MENU");
            }
            else if (Session["MainMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_WF_ADMIN_MAIN_MENU");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            if (Session["MainMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_MAIN_MENU_CODE", OracleType.Number, 5).Value = txtMainMenuCode.Text;
            }
            spProcess.Parameters.Add("V_MAIN_MENU_NAME", OracleType.VarChar, 100).Value = txtMainMenuName.Text;
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
        txtMainMenuCode.Text = "";
        txtMainMenuName.Text = "";
        txtDescription.Text = "";

        txtMainMenuCode.Enabled = false;
        txtMainMenuName.Enabled = false;
        txtDescription.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtMainMenuCode.Enabled = true;
        txtMainMenuName.Enabled = true;
        txtDescription.Enabled = true;

        txtMainMenuCode.Text = "";
        txtMainMenuName.Text = "";
        txtDescription.Text = "";

        btnSave.Enabled = true;

        Session["MainMenuSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtMainMenuCode.Text == "")
        {
            lblMsg.Text = "Please Select A Menu Name";
            return;
        }


        txtMainMenuName.Enabled = true;
        txtDescription.Enabled = true;

        btnSave.Enabled = true;

        Session["MainMenuSetupMode"] = "UPDATE";
    }

    protected void grdMainMenus_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtMainMenuCode.Text = grdMainMenus.SelectedRow.Cells[1].Text.Trim();
        txtMainMenuName.Text = grdMainMenus.SelectedRow.Cells[2].Text.Trim();
        txtDescription.Text = grdMainMenus.SelectedRow.Cells[3].Text.Trim();


        btnAlter.Enabled = true;
    }

    protected void grdMainMenus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";
    }


    private bool CheckMainMenuAlreadyExist(string MainMenuName)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT MAIN_MENU_NAME FROM WF_ADMIN_MAIN_MENU WHERE MAIN_MENU_NAME='" + MainMenuName + "'";

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
