//******************************************
// Author            :Tharindu Athapattu
// Date              :17/04/2013
// Reviewed By       :
// Description       : Sub Menu Setup Form
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

public partial class SubMenuSetup : System.Web.UI.Page
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

            Session.Remove("SubMenuSetupMode");

            pnlSubMenuGrid.Visible = false;
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
        grdSubMenus.DataSource = null;
        grdSubMenus.DataBind();

        if ((txtSearchSubMenuName.Text == "") && (txtSearchDescription.Text == "") && (ddlSearchMainMenu.SelectedValue.ToString() == "0"))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchSubMenuName.Text != "")
        {

            SQL = "(LOWER(T.SUB_MENU_NAME) LIKE '%" + txtSearchSubMenuName.Text.ToLower() + "%') AND";
        }

        if (txtSearchDescription.Text != "")
        {

            SQL = SQL + "(LOWER(T.DESCRIPTION) LIKE '%" + txtSearchDescription.Text.ToLower() + "%') AND";
        }
        if (ddlSearchMainMenu.SelectedValue.ToString() != "0")
        {

            SQL = SQL + "(T.MAIN_MENU_CODE LIKE '%" + ddlSearchMainMenu.SelectedValue.ToString() + "%') AND";
        }

        SQL = SQL.Substring(0,SQL.Length - 3);

        String selectQuery = "";
        selectQuery = "  SELECT T.MAIN_MENU_CODE AS \"Main Menu Code\" ,MM.MAIN_MENU_NAME AS \"Main Menu Name\", " +
                      " T.SUB_MENU_CODE AS \"Sub Menu Code\", T.SUB_MENU_NAME AS \"Sub Menu Name\",T.PAGE_PATH AS \"Page Path\", T.DESCRIPTION AS \"Description\"  " +
                      " FROM WF_ADMIN_SUB_MENU T " +
                      " INNER JOIN WF_ADMIN_MAIN_MENU MM ON T.MAIN_MENU_CODE=MM.MAIN_MENU_CODE" +
                      " WHERE (" + SQL + ") ORDER BY MM.MAIN_MENU_NAME,T.SUB_MENU_NAME ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdSubMenus.DataSource = myOleDbDataReader;
            grdSubMenus.DataBind();

            pnlSubMenuGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("SubMenuSetup.aspx");
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


        if (ddlMainMenu.SelectedValue == "" || ddlMainMenu.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the Main Menu";
            Timer1.Enabled = true;
            return;
        }

        if (txtSubMenuName.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Sub Menu Name";
            Timer1.Enabled = true;
            return;
        }


        if (txtDescription.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Description";
            Timer1.Enabled = true;
            return;
        }

        if (txtPagePath.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Page Path";
            Timer1.Enabled = true;
            return;
        }


        if (Session["SubMenuSetupMode"].ToString() == "NEW")
        {
            if (CheckSubMenuAlreadyExist(ddlMainMenu.SelectedValue.ToString(), txtSubMenuName.Text))
            {
                lblMsg.Text = "Enetered Sub Menu Name Already Exists for selected Main Menu";
                Timer1.Enabled = true;
                return;
            }
        }

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["SubMenuSetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_WF_ADMIN_SUB_MENU");
            }
            else if (Session["SubMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_WF_ADMIN_SUB_MENU");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            if (Session["SubMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_SUB_MENU_CODE", OracleType.Number, 5).Value = txtSubMenuCode.Text;
            }
            spProcess.Parameters.Add("V_MAIN_MENU_CODE", OracleType.Number, 5).Value = ddlMainMenu.SelectedValue.ToString();
            spProcess.Parameters.Add("V_SUB_MENU_NAME", OracleType.VarChar, 100).Value = txtSubMenuName.Text;
            spProcess.Parameters.Add("V_PAGE_PATH", OracleType.VarChar, 250).Value = txtPagePath.Text;
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
        ddlMainMenu.SelectedValue = "0";
        txtSubMenuCode.Text = "";
        txtSubMenuName.Text = "";
        txtPagePath.Text = "";
        txtDescription.Text = "";

        ddlMainMenu.Enabled = false;
        txtSubMenuCode.Enabled = false;
        txtSubMenuName.Enabled = false;
        txtPagePath.Enabled = false;
        txtDescription.Enabled = false;


        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
      //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {

        ddlMainMenu.Enabled = true;
        txtSubMenuCode.Enabled = true;
        txtSubMenuName.Enabled = true;
        txtPagePath.Enabled = true;
        txtDescription.Enabled = true;

        ddlMainMenu.SelectedValue = "0";
        txtSubMenuCode.Text = "";
        txtSubMenuName.Text = "";
        txtPagePath.Text = "";
        txtDescription.Text = "";

        btnSave.Enabled = true;

        Session["SubMenuSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtSubMenuCode.Text == "")
        {
            lblMsg.Text = "Please Select A Sub Menu Name";
            return;
        }

        ddlMainMenu.Enabled = true;
        //txtSubMenuCode.Enabled = true;
        txtSubMenuName.Enabled = true;
        txtPagePath.Enabled = true;
        txtDescription.Enabled = true;

        btnSave.Enabled = true;

        Session["SubMenuSetupMode"] = "UPDATE";
    }

    protected void grdSubMenus_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlMainMenu.SelectedValue = grdSubMenus.SelectedRow.Cells[1].Text.Trim();
        txtSubMenuCode.Text = grdSubMenus.SelectedRow.Cells[3].Text.Trim();
        txtSubMenuName.Text = grdSubMenus.SelectedRow.Cells[4].Text.Trim();
        txtPagePath.Text = grdSubMenus.SelectedRow.Cells[5].Text.Trim();
        txtDescription.Text = grdSubMenus.SelectedRow.Cells[6].Text.Trim();

        btnAlter.Enabled = true;

    }

    protected void grdSubMenus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;

    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";

        ddlMainMenu.Items.Clear();
        ddlMainMenu.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchMainMenu.Items.Clear();
        ddlSearchMainMenu.Items.Add(new ListItem("--- Select One ---", "0"));

        loadMainMenus();
    }


    private bool CheckSubMenuAlreadyExist(string MainMenuCode, string SubMenuName)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT SUB_MENU_NAME FROM WF_ADMIN_SUB_MENU WHERE MAIN_MENU_CODE='" + MainMenuCode + "' AND SUB_MENU_NAME='" + SubMenuName + "'";

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

    private void loadMainMenus()
    {
        ddlMainMenu.Items.Clear();
        ddlMainMenu.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchMainMenu.Items.Clear();
        ddlSearchMainMenu.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = " SELECT T.MAIN_MENU_CODE,T.MAIN_MENU_NAME   FROM WF_ADMIN_MAIN_MENU T ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlMainMenu.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlSearchMainMenu.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

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
