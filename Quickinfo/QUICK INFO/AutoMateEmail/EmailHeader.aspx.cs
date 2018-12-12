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

        if (ddlSearchMainMenu.SelectedValue.ToString() == "--- Select One ---")
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (ddlSearchMainMenu.SelectedValue.ToString() != "--- Select One ---")
        {

            SQL = "(T.MAE_EMAIL_DESC = '" + ddlSearchMainMenu.SelectedValue.ToString() + "') AND";
        }

        SQL = SQL.Substring(0,SQL.Length - 3);

        String selectQuery = "";

        selectQuery = "SELECT T.MAE_EMAIL_ID,T.MAE_EMAIL_DESC AS \"E-Mail Desc\",T.MAE_EMAIL_HEADER AS \"E-Mail Header\",T.MAE_EMAIL_STATUS AS \"Header Status\",T.MAE_EMAIL_SORT FROM WF_MIS_AUTOMATED_EMAIL_HEADER T " +
                      " WHERE (" + SQL + ") ORDER BY T.MAE_EMAIL_ID ASC";

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
        Response.Redirect("EmailHeader.aspx");
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
        if (Session["SubMenuSetupMode"].ToString() == "NEW")
        {
            if (CheckSubMenuAlreadyExist(ddlMainMenu.SelectedValue.ToString(), txtSubMenuName.Text.Trim()))
            {
                lblMsg.Text = "Enetered E-Mail Header Already Exists for selected E-Mail Description";
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
                spProcess = new OracleCommand("INSERT_WF_MIS_EMAIL_HEADER");
            }
            else if (Session["SubMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_WF_MIS_EMAIL_HEADER");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;

            if (Session["SubMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_MAE_EMAIL_ID", OracleType.Number, 10).Value = txtSubMenuCode.Text;
            }

            spProcess.Parameters.Add("V_MAE_EMAIL_DESC", OracleType.VarChar, 100).Value = ddlMainMenu.SelectedValue;
            spProcess.Parameters.Add("V_MAE_EMAIL_HEADER", OracleType.VarChar, 100).Value = txtSubMenuName.Text;
            spProcess.Parameters.Add("V_MAE_EMAIL_STATUS", OracleType.Int32).Value = Convert.ToInt32(cboStatus.SelectedValue);


            spProcess.Parameters.Add("V_ERROR", OracleType.VarChar, 50).Direction = ParameterDirection.Output;
            spProcess.Parameters.Add("V_ERROR", OracleType.VarChar, 50);
            spProcess.Parameters["V_ERROR"].Direction = ParameterDirection.Output;

            if (Session["SubMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_SORT_ID", OracleType.Int32).Value = Convert.ToInt32(lblSortId.Text);
            }

            spProcess.ExecuteNonQuery();
            conProcess.Close();

            string errortext = Convert.ToString(spProcess.Parameters["V_ERROR"].Value);

            ClearComponents();
            SearchData();

            lblMsg.Text = errortext.ToString();
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
        cboStatus.Text = "0";

        ddlMainMenu.Enabled = false;
        txtSubMenuCode.Enabled = false;
        txtSubMenuName.Enabled = false;
        cboStatus.Enabled = false;


        btnAddNew.Enabled = true;
        btnAlter.Enabled = true;
        btnSave.Enabled = false;
      //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {

        ddlMainMenu.SelectedValue = "0";
        txtSubMenuCode.Text = "";
        txtSubMenuName.Text = "";
        cboStatus.Text = "1";

        ddlMainMenu.Enabled = true;
        txtSubMenuCode.Enabled = true;
        txtSubMenuName.Enabled = true;
        cboStatus.Enabled = true;

        btnSave.Enabled = true;
        btnAddNew.Enabled = false;
        btnAlter.Enabled = false;

        Session["SubMenuSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtSubMenuCode.Text == "")
        {
            lblMsg.Text = "Please Select A Sub Menu Name";
            return;
        }


        ddlMainMenu.Enabled = false;
        txtSubMenuCode.Enabled = true;
        txtSubMenuName.Enabled = true;
        cboStatus.Enabled = true;

        btnSave.Enabled = true;
        btnAddNew.Enabled = false;
        btnAlter.Enabled = false;

        Session["SubMenuSetupMode"] = "UPDATE";
    }

    protected void grdSubMenus_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtSubMenuCode.Text = grdSubMenus.SelectedRow.Cells[1].Text.Trim();
        ddlMainMenu.Text = grdSubMenus.SelectedRow.Cells[2].Text.Trim();
        txtSubMenuName.Text = grdSubMenus.SelectedRow.Cells[3].Text.Trim();
        cboStatus.Text = grdSubMenus.SelectedRow.Cells[4].Text.Trim();
        lblSortId.Text = grdSubMenus.SelectedRow.Cells[5].Text.Trim();

    }

    protected void grdSubMenus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[5].Visible = false;

    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";

        ddlMainMenu.Items.Clear();
        ddlMainMenu.Items.Add(new ListItem("--- Select One ---", "--- Select One ---"));

        ddlSearchMainMenu.Items.Clear();
        ddlSearchMainMenu.Items.Add(new ListItem("--- Select One ---", "--- Select One ---"));

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
        selectQuery = "SELECT T.MAE_EMAIL_ID,T.MAE_EMAIL_DESC,T.MAE_EMAIL_HEADER,T.MAE_EMAIL_STATUS FROM WF_MIS_AUTOMATED_EMAIL_HEADER T WHERE T.MAE_EMAIL_DESC = '" + MainMenuCode + "' AND UPPER(T.MAE_EMAIL_HEADER) = '" + SubMenuName + "'";

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
        selectQuery = " SELECT T.MAE_EMAIL_DESC,T.MAE_EMAIL_DESC FROM WF_MIS_AUTOMATED_EMAIL T ORDER BY T.MAE_EMAIL_ID ASC";

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
