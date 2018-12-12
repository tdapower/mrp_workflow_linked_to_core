//******************************************
// Author            : Tharindu Athapattu
// Date              : 12/12/2017
// Reviewed By       :
// Description       : HNBABranch Form
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

public partial class HNBABranch : System.Web.UI.Page
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


        ddlStatus.Items.Clear();
        ddlStatus.Items.Add(new ListItem("--- Select One ---", "0"));
        ddlStatus.Items.Add(new ListItem("BRANCH", "BRANCH"));
        ddlStatus.Items.Add(new ListItem("BANCASSURANCE", "BANCASSURANCE"));

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

        if ((txtSearchHNBABranchCode.Text == "") && (txtSearchBranchName.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchHNBABranchCode.Text != "")
        {
            SQL = SQL + "(LOWER(L.HNB_CODE) LIKE '%" + txtSearchHNBABranchCode.Text.ToLower() + "%') AND";
        }
        if (txtSearchBranchName.Text != "")
        {
            SQL = SQL + "(LOWER(L.HNB_BRANCH) LIKE '%" + txtSearchBranchName.Text.ToLower() + "%') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);




        String selectQuery = "";
        selectQuery = "SELECT " +
                    " L.HNB_CODE  AS \"HNBA Branch Code\",L.HNB_BRANCH AS \"Branch Name\", L.HNBA_EMAIL AS \"E-mail\"," +
                    "L.STATUS AS \"Status\" FROM MRP_WF_HNBA_EMAIL L  " +
                      " WHERE (" + SQL + ") ORDER BY L.HNB_CODE ASC";



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
        Response.Redirect("HNBABranch.aspx");
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtHNBABranchCode.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Branch Code";
            Timer1.Enabled = true;
            return;
        }


        if (txtBranchName.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Name of Brnach";
            Timer1.Enabled = true;
            return;
        }

        if (Session["SetupMode"].ToString() == "NEW")
        {
            if (!validateDuplicates(txtHNBABranchCode.Text))
            {
                lblMsg.Text = "Branch already available";
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
                spProcess = new OracleCommand("INSERT_MRP_WF_HNBA_EMAIL");
            }
            else if (Session["SetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_HNBA_EMAIL");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;

            spProcess.Parameters.Add("V_HNB_CODE", OracleType.VarChar, 20).Value = txtHNBABranchCode.Text;

            spProcess.Parameters.Add("V_HNB_BRANCH", OracleType.VarChar, 150).Value = txtBranchName.Text;
            spProcess.Parameters.Add("V_HNBA_EMAIL", OracleType.VarChar, 150).Value = txtEMail.Text;
            spProcess.Parameters.Add("V_STATUS", OracleType.VarChar, 150).Value = ddlStatus.SelectedValue;



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
        txtHNBABranchCode.Enabled = false;
        txtBranchName.Enabled = false;
        txtEMail.Enabled = false;
        ddlStatus.Enabled = false;



        txtHNBABranchCode.Text = "";
        txtBranchName.Text = "";
        txtEMail.Text = "";
        ddlStatus.SelectedValue = "0";

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;



    }




    protected void btnAddNew_Click(object sender, EventArgs e)
    {

        txtHNBABranchCode.Enabled = true;
        txtBranchName.Enabled = true;
        txtEMail.Enabled = true;
        ddlStatus.Enabled = true;


        txtHNBABranchCode.Text = "";
        txtBranchName.Text = "";
        txtEMail.Text = "";
        ddlStatus.SelectedValue = "0";

        btnSave.Enabled = true;

        Session["SetupMode"] = "NEW";
    }


    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtHNBABranchCode.Text == "")
        {
            lblMsg.Text = "Please Select a Branch to alter";
            return;
        }

        //txtHNBABranchCode.Enabled = true;
        txtBranchName.Enabled = true;
        txtEMail.Enabled = true;
        ddlStatus.Enabled = true;

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

        selectQuery = "   SELECT L.HNB_CODE ,L.HNB_BRANCH, L.HNBA_EMAIL,L.STATUS FROM MRP_WF_HNBA_EMAIL L  " +
                      " WHERE L.HNB_CODE='" + id + "'";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            txtHNBABranchCode.Text = id;
            txtBranchName.Text = dr[1].ToString();
            txtEMail.Text = dr[2].ToString();
            ddlStatus.SelectedValue = dr[3].ToString();




        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
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
        selectQuery = "SELECT T.HNB_CODE FROM MRP_WF_HNBA_EMAIL T WHERE T.HNB_CODE='" + id + "'";


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
