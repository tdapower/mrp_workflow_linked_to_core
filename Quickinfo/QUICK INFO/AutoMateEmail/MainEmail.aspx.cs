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
    string SQL = "";
   



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

            SQL = "T.MAE_EMAIL_DESC LIKE '" + txtSearchMainMenuName.Text.ToUpper() + "' AND";
        }

        if (txtSearchDescription.Text != "")
        {

            SQL = SQL + "T.MAE_EMAIL_SUBJECT LIKE '" + txtSearchDescription.Text.ToUpper() + "' AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.MAE_EMAIL_DESC AS \"Email Description\" ,T.MAE_EMAIL_SUBJECT AS \"Email Subject\",T.MAE_EMAIL_BRANCH_WISE AS \"Branch Wise\",T.MAE_EMAIL_HEADER_COLOUMNS,T.MAE_EMAIL_MERGE_QUERY,T.MAE_EMAIL_ID,T.MAE_EMAIL_DB_TYPE AS \"DB Type\",T.MAE_EMAIL_EXCEL_ATTACHED AS \"Excel Attached\"  FROM WF_MIS_AUTOMATED_EMAIL T  " +
                      " WHERE (" + SQL + ") ORDER BY T.MAE_EMAIL_DESC ASC";

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
        Response.Redirect("MainEmail.aspx");
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
        if (Session["MainMenuSetupMode"].ToString() == "NEW")
        {
            if (CheckMainMenuAlreadyExist(txtEmailDesc.Text))
            {
                lblMsg.Text = "Enetered Email Description Already Exists";
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
                spProcess = new OracleCommand("INSERT_AUTOMATED_MIS_EMAIL");
            }
            else if (Session["MainMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_AUTOMATED_MIS_EMAIL");
            }

            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;

            if (Session["MainMenuSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_EMAIL_ID", OracleType.Number).Value = Convert.ToDouble(lblEmailID.Text);
            }

            spProcess.Parameters.Add("V_EMAIL_DESC", OracleType.VarChar, 100).Value = txtEmailDesc.Text.Trim() == "" ? "-" : txtEmailDesc.Text.ToUpper();
            spProcess.Parameters.Add("V_EMAIL_SUBJECT", OracleType.VarChar, 100).Value = txtEmailSubject.Text.Trim() == "" ? "-" : txtEmailSubject.Text;
            spProcess.Parameters.Add("V_EMAIL_HEADER_COL", OracleType.Int32).Value = txtEmailHeader.Text.Trim() == "" ? 0 : Convert.ToInt32(txtEmailHeader.Text);
            spProcess.Parameters.Add("V_EMAIL_QUERY", OracleType.LongVarChar).Value = txtEmailSQL.Text.Trim() == "" ? "-" : txtEmailSQL.Text.ToUpper();
            spProcess.Parameters.Add("V_EMAIL_BRANCH_IND", OracleType.VarChar, 3).Value = cboEmailBranch.SelectedValue;

            spProcess.Parameters.Add("V_ERROR", OracleType.VarChar, 50).Direction = ParameterDirection.Output;
            spProcess.Parameters.Add("V_ERROR", OracleType.VarChar, 50);
            spProcess.Parameters["V_ERROR"].Direction = ParameterDirection.Output;

            spProcess.Parameters.Add("V_MAE_EMAIL_DB_TYPE", OracleType.VarChar, 10).Value = cboDB.SelectedValue;

            spProcess.Parameters.Add("V_MAIL_ATTACH", OracleType.VarChar, 10).Value = cboAttachment.SelectedValue;

           
            spProcess.ExecuteNonQuery();
            conProcess.Close();

            string errortext = Convert.ToString(spProcess.Parameters["V_ERROR"].Value);

            grdMainMenus.DataSource = null;
            grdMainMenus.DataBind();


            OracleCommand myOleDbCommand = new OracleCommand();
            myConnection.Open();

            myOleDbCommand.Connection = myConnection;


            if (txtSearchMainMenuName.Text != "")
            {

                SQL = "T.MAE_EMAIL_DESC LIKE '" + txtEmailDesc.Text + "' AND";
            }

            //SQL = SQL.Substring(0, SQL.Length - 3);
            SQL = "T.MAE_EMAIL_DESC LIKE '" + txtEmailDesc.Text + "'";


            String selectQuery = "";
            selectQuery = "   SELECT T.MAE_EMAIL_DESC AS \"Email Description\" ,T.MAE_EMAIL_SUBJECT AS \"Email Subject\",T.MAE_EMAIL_BRANCH_WISE AS \"Branch Wise\",T.MAE_EMAIL_HEADER_COLOUMNS,T.MAE_EMAIL_MERGE_QUERY,T.MAE_EMAIL_ID,T.MAE_EMAIL_DB_TYPE AS \"DB Type\",T.MAE_EMAIL_EXCEL_ATTACHED AS \"Excel Attached\"  FROM WF_MIS_AUTOMATED_EMAIL T  " +
                          " WHERE (" + SQL + ") ORDER BY T.MAE_EMAIL_DESC ASC";

            myOleDbCommand.CommandText = selectQuery;

            OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
            if (myOleDbDataReader.HasRows == true)
            {
                DataTable dbTable = new DataTable();
                grdMainMenus.DataSource = myOleDbDataReader;
                grdMainMenus.DataBind();

                pnlMainMenuGrid.Visible = true;
            }

            myConnection.Close();

            txtEmailDesc.Enabled = false;
            txtEmailHeader.Enabled = false;
            txtEmailSQL.Enabled = false;
            txtEmailSubject.Enabled = false;
            cboEmailBranch.Enabled = false;
            cboDB.Enabled = false;
            cboAttachment.Enabled = false;


            btnAddNew.Enabled = true;
            btnAlter.Enabled = false;
            btnSave.Enabled = false;


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
        txtEmailDesc.Text = "";
        txtEmailHeader.Text = "";
        txtEmailSQL.Text = "";
        txtEmailSubject.Text = "";
        cboEmailBranch.Text = "-";
        cboDB.Text = "ORACLE";
        cboAttachment.Text = "-";

        txtEmailDesc.Enabled = false;
        txtEmailHeader.Enabled = false;
        txtEmailSQL.Enabled = false;
        txtEmailSubject.Enabled = false;
        cboEmailBranch.Enabled = false;
        cboDB.Enabled = false;
        cboAttachment.Enabled = false;


        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtEmailDesc.Text = "";
        txtEmailHeader.Text = "";
        txtEmailSQL.Text = "";
        txtEmailSubject.Text = "";
        cboEmailBranch.Text = "-";
        cboDB.Text = "ORACLE";
        cboAttachment.Text = "-";

        txtEmailDesc.Enabled = true;
        txtEmailHeader.Enabled = true;
        txtEmailSQL.Enabled = true;
        txtEmailSubject.Enabled = true;
        cboEmailBranch.Enabled = true;
        cboDB.Enabled = true;
        cboAttachment.Enabled = true;

        btnSave.Enabled = true;
        btnAddNew.Enabled = false;
        btnAlter.Enabled = false;
        

        Session["MainMenuSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        txtEmailDesc.Enabled = false;
        txtEmailHeader.Enabled = true;
        txtEmailSQL.Enabled = true;
        txtEmailSubject.Enabled = true;
        cboEmailBranch.Enabled = true;
        cboDB.Enabled = true;
        cboAttachment.Enabled = true;

        btnSave.Enabled = true;
        btnAddNew.Enabled = false;
        btnAlter.Enabled = false;

        Session["MainMenuSetupMode"] = "UPDATE";
    }

    protected void grdMainMenus_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblEmailID.Text = grdMainMenus.SelectedRow.Cells[6].Text;
        txtEmailDesc.Text = grdMainMenus.SelectedRow.Cells[1].Text;
        txtEmailSubject.Text = grdMainMenus.SelectedRow.Cells[2].Text;
        txtEmailHeader.Text = grdMainMenus.SelectedRow.Cells[4].Text;
        txtEmailSQL.Text = Email_SQL_Query(grdMainMenus.SelectedRow.Cells[1].Text);
        cboEmailBranch.Text = grdMainMenus.SelectedRow.Cells[3].Text;
        cboDB.Text = grdMainMenus.SelectedRow.Cells[7].Text;
        cboAttachment.Text = grdMainMenus.SelectedRow.Cells[8].Text;

        btnAlter.Enabled = true;

        grdMainMenus.DataSource = null;
        grdMainMenus.DataBind();


        OracleCommand myOleDbCommand = new OracleCommand();

        myConnection.Open();

        myOleDbCommand.Connection = myConnection;


        if (txtSearchMainMenuName.Text != "")
        {

            SQL = "T.MAE_EMAIL_DESC LIKE '" + txtEmailDesc.Text + "' AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.MAE_EMAIL_DESC AS \"Email Description\" ,T.MAE_EMAIL_SUBJECT AS \"Email Subject\",T.MAE_EMAIL_BRANCH_WISE AS \"Branch Wise\",T.MAE_EMAIL_HEADER_COLOUMNS,T.MAE_EMAIL_MERGE_QUERY,T.MAE_EMAIL_ID,T.MAE_EMAIL_DB_TYPE AS \"DB Type\",T.MAE_EMAIL_EXCEL_ATTACHED AS \"Excel Attached\"  FROM WF_MIS_AUTOMATED_EMAIL T  " +
                      " WHERE (" + SQL + ") ORDER BY T.MAE_EMAIL_DESC ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdMainMenus.DataSource = myOleDbDataReader;
            grdMainMenus.DataBind();

            pnlMainMenuGrid.Visible = true;
        }

        myConnection.Close();
    }

    protected void grdMainMenus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[8].Visible = false;
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
        selectQuery = "SELECT MAE_EMAIL_DESC FROM WF_MIS_AUTOMATED_EMAIL T WHERE T.MAE_EMAIL_DESC ='" + MainMenuName + "'";

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

    private string Email_SQL_Query(string MainMenuName)
    {
        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT T.MAE_EMAIL_MERGE_QUERY FROM WF_MIS_AUTOMATED_EMAIL T WHERE T.MAE_EMAIL_DESC = '" + MainMenuName + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                returnVal = Convert.ToString(dr[0].ToString());
            }
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
