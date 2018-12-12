//******************************************
// Author            : Tharindu Athapattu
// Date              : 12/12/2017
// Reviewed By       :
// Description       : BankEmail 
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
using Telerik.Web.UI;

public partial class BankEmail : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            validatePageAuthentication();


            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);

            ClearComponents();
            initializeValues();

            Session.Remove("SetupMode");

            pnlSearchResultGrid.Visible = false;
        }


    }
    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";

        loadBankCodes();
        loadHNBABranch();
    }


    private void loadBankCodes()
    {
        ddlBankCode.Items.Clear();
        ddlBankCode.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT BANK_CODE FROM MRP_WF_BANKS ORDER BY BANK_CODE ASC ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBankCode.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void loadHNBABranch()
    {
        ddlHNBABranchCode.Items.Clear();
        ddlHNBABranchCode.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT HNB_CODE FROM mrp_wf_hnba_email ORDER BY HNB_CODE ASC ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlHNBABranchCode.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
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
        grdSearchResult.DataSource = null;
        grdSearchResult.DataBind();

        if ((txtSearchBankCode.Text == "") && (txtSearchHNBABranchCode.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchBankCode.Text != "")
        {
            SQL = SQL + "(LOWER(L.BANK_CODE) LIKE '%" + txtSearchBankCode.Text.ToLower() + "%') AND";
        }
        if (txtSearchHNBABranchCode.Text != "")
        {
            SQL = SQL + "(LOWER(L.HNBA_BRANCH_CODE) LIKE '%" + txtSearchHNBABranchCode.Text.ToLower() + "%') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);




        String selectQuery = "";
        selectQuery = "SELECT " +
                    " L.BANK_CODE  AS \"Bank Code\",L.HNBA_BRANCH_CODE AS \"HNBA Branch Code\", L.HNB_EMAIL AS \" Bank Email\"," +
                    "L.BANCASS_EMAIL AS \"Bancassurance Email\" FROM MRP_WF_BANKS_EMAIL L  " +
                      " WHERE (" + SQL + ") ORDER BY L.BANK_CODE ASC";



        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdSearchResult.DataSource = myOleDbDataReader;
            grdSearchResult.DataBind();

            pnlSearchResultGrid.Visible = true;
        }
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("BankEmail.aspx");
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (ddlBankCode.SelectedValue == "0")
        {
            lblMsg.Text = "Please select Bank Code";
            Timer1.Enabled = true;
            return;
        }
        if (ddlHNBABranchCode.SelectedValue == "0")
        {
            lblMsg.Text = "Please select HNBA Branch Code";
            Timer1.Enabled = true;
            return;
        }

        if (txtBankEmail.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Email of Bank";
            Timer1.Enabled = true;
            return;
        }

        if (txtBancassuranceEmail.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Email of Bancassurance";
            Timer1.Enabled = true;
            return;
        }



        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["SetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_BANKS_EMAIL");
            }
            else if (Session["SetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_BANKS_EMAIL");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;

            spProcess.Parameters.Add("V_BANK_CODE", OracleType.VarChar).Value = ddlBankCode.SelectedValue;

            spProcess.Parameters.Add("V_HNB_EMAIL", OracleType.VarChar).Value = txtBankEmail.Text;
            spProcess.Parameters.Add("V_BANCASS_EMAIL", OracleType.VarChar).Value = txtBancassuranceEmail.Text;
            spProcess.Parameters.Add("V_HNBA_BRANCH_CODE", OracleType.VarChar).Value = ddlHNBABranchCode.SelectedValue;



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
        ddlBankCode.Enabled = false;
        ddlHNBABranchCode.Enabled = false;
        txtBankEmail.Enabled = false;
        txtBancassuranceEmail.Enabled = false;

        ddlBankCode.SelectedValue = "0";
        ddlHNBABranchCode.SelectedValue = "0";
        txtBankEmail.Text = "";
        txtBancassuranceEmail.Text = "";

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;


    }




    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ddlBankCode.Enabled = true;
        ddlHNBABranchCode.Enabled = true;
        txtBankEmail.Enabled = true;
        txtBancassuranceEmail.Enabled = true;

        ddlBankCode.SelectedValue = "0";
        ddlHNBABranchCode.SelectedValue = "0";
        txtBankEmail.Text = "";
        txtBancassuranceEmail.Text = "";

        btnSave.Enabled = true;

        Session["SetupMode"] = "NEW";
    }


    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (ddlBankCode.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select a Bank Code to alter";
            return;
        }

        //ddlBankCode.Enabled = true;
        //ddlHNBABranchCode.Enabled = true;
        txtBankEmail.Enabled = true;
        txtBancassuranceEmail.Enabled = true;

        btnSave.Enabled = true;

        Session["SetupMode"] = "UPDATE";
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadDetails(grdSearchResult.SelectedRow.Cells[1].Text.Trim());


        btnAlter.Enabled = true;
    }

    private string loadDetails(string id)
    {

        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "   SELECT L.BANK_CODE ,L.HNBA_BRANCH_CODE, L.HNB_EMAIL,L.BANCASS_EMAIL FROM MRP_WF_BANKS_EMAIL L  " +
                      " WHERE L.BANK_CODE='" + id + "'";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            ddlBankCode.SelectedValue = dr[0].ToString();
            ddlHNBABranchCode.SelectedValue = dr[1].ToString();


            txtBankEmail.Text = dr[2].ToString();
            txtBancassuranceEmail.Text = dr[3].ToString();



        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
    }



    protected void grdSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //  e.Row.Cells[1].Visible = false;
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }
}
