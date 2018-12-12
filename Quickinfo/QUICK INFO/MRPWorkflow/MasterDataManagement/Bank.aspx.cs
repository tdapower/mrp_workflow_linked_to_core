//******************************************
// Author            : Tharindu Athapattu
// Date              : 12/12/2017
// Reviewed By       :
// Description       : Bank 
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

public partial class Bank : System.Web.UI.Page
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

        loadBankTypes();

    }


    private void loadBankTypes()
    {
        ddlBankType.Items.Clear();
        ddlBankType.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchBankType.Items.Clear();
        ddlSearchBankType.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT BANK_TYPE_NAME FROM MRP_WF_BANK_TYPE ORDER BY BANK_TYPE_NAME ASC ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBankType.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
                ddlSearchBankType.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));

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

        if ((txtSearchBankCode.Text == "") && (txtSearchBankName.Text == "") && (txtSearchBranchName.Text == "") && (ddlSearchBankType.SelectedValue == "0"))
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
        if (txtSearchBankName.Text != "")
        {
            SQL = SQL + "(LOWER(L.BANK_NAME) LIKE '%" + txtSearchBankName.Text.ToLower() + "%') AND";
        }
        if (txtSearchBranchName.Text != "")
        {
            SQL = SQL + "(LOWER(L.BRANCH_NAME) LIKE '%" + txtSearchBranchName.Text.ToLower() + "%') AND";
        }
        if (ddlSearchBankType.SelectedValue != "")
        {
            SQL = SQL + "(LOWER(L.BANK_TYPE) = LOWER('" + ddlSearchBankType.SelectedValue.ToLower() + "')) AND";
        }



        SQL = SQL.Substring(0, SQL.Length - 3);




        String selectQuery = "";
        selectQuery = "SELECT " +
                    " L.BANK_CODE  AS \"Bank Code\",L.BANK_TYPE AS \"Bank Type\", L.BANK_NAME AS \"Bank Name\"," +
                    "L.BRANCH_NAME AS \"Branch Name \" FROM MRP_WF_BANKS L  " +
                      " WHERE (" + SQL + ") ORDER BY L.BANK_NAME ASC";



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
        Response.Redirect("Bank.aspx");
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtBankCode.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Bank Code";
            Timer1.Enabled = true;
            return;
        }
        if (ddlBankType.SelectedValue == "0")
        {
            lblMsg.Text = "Please Enter Name of Bank Type";
            Timer1.Enabled = true;
            return;
        }

        if (txtBankName.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Name of Bank";
            Timer1.Enabled = true;
            return;
        }

        if (txtBranchName.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Name of Bank Branch";
            Timer1.Enabled = true;
            return;
        }

        if (Session["SetupMode"].ToString() == "NEW")
        {
            if (!validateDuplicates(txtBankCode.Text))
            {
                lblMsg.Text = "Bank already available";
                Timer1.Enabled = true;
                return;
            }
        }

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["SetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_BANK");
            }
            else if (Session["SetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_BANK");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;

            spProcess.Parameters.Add("V_BANK_CODE", OracleType.VarChar).Value = txtBankCode.Text;

            spProcess.Parameters.Add("V_BANK_TYPE", OracleType.VarChar).Value = ddlBankType.SelectedValue;
            spProcess.Parameters.Add("V_BANK_NAME", OracleType.VarChar).Value = txtBankName.Text;
            spProcess.Parameters.Add("V_BRANCH_NAME", OracleType.VarChar).Value = txtBranchName.Text;



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
        txtBankCode.Enabled = false;
        txtBankName.Enabled = false;
        txtBranchName.Enabled = false;
        ddlBankType.Enabled = false;


        txtBankCode.Text = "";
        txtBankName.Text = "";
        txtBranchName.Text = "";
        ddlBankType.SelectedValue = "0";

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;



    }




    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtBankCode.Enabled = true;
        txtBankName.Enabled = true;
        txtBranchName.Enabled = true;
        ddlBankType.Enabled = true;


        txtBankCode.Text = "";
        txtBankName.Text = "";
        txtBranchName.Text = "";
        ddlBankType.SelectedValue = "0";


        btnSave.Enabled = true;

        Session["SetupMode"] = "NEW";
    }


    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtBankCode.Text == "")
        {
            lblMsg.Text = "Please Select a Bank to alter";
            return;
        }

        // txtBankCode.Enabled = true;
        txtBankName.Enabled = true;
        txtBranchName.Enabled = true;
        ddlBankType.Enabled = true;

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

        selectQuery = "   SELECT L.BANK_CODE ,L.BANK_TYPE, L.BANK_NAME,L.BRANCH_NAME FROM MRP_WF_BANKS L  " +
                      " WHERE L.BANK_CODE='" + id + "'";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            txtBankCode.Text = id;

            ddlBankType.SelectedValue = dr[1].ToString();
            txtBankName.Text = dr[2].ToString();
            txtBranchName.Text = dr[3].ToString();



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

    private bool validateDuplicates(string id)
    {
        bool returnVal = true;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;


        String selectQuery = "";
        selectQuery = "SELECT T.BANK_CODE FROM MRP_WF_BANKS T WHERE T.BANK_CODE='" + id + "'";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            returnVal = false;

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
