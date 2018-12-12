//******************************************
// Author            : Tharindu Athapattu
// Date              : 12/12/2017
// Reviewed By       :
// Description       : MedicalLab Form
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

public partial class MedicalLab : System.Web.UI.Page
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

        if ((txtSearchPartyCode.Text == "") && (txtSearchLabName.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchPartyCode.Text != "")
        {

            SQL = SQL + "(LOWER(L.PTY_PARTY_CODE) LIKE '%" + txtSearchPartyCode.Text.ToLower() + "%') AND";
        }
        if (txtSearchLabName.Text != "")
        {

            SQL = SQL + "(LOWER(L.PVR_BUSINESS_NAME) LIKE '%" + txtSearchLabName.Text.ToLower() + "%') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);




        String selectQuery = "";
        selectQuery = "   SELECT " +
                    " L.PTY_PARTY_CODE  AS \"Party Code\",L.PVR_BUSINESS_NAME AS \"Name\", L.CON_ADDRESS_LINE_1 AS \"Address Line 1\"," +
                    "L.CON_ADDRESS_LINE_2 AS \"Address Line 2\",L.CON_ADDRESS_LINE_3 AS \"Address Line 3\" FROM MRP_WF_MEDICAL_LABS L  " +
                      " WHERE (" + SQL + ") ORDER BY L.PVR_BUSINESS_NAME ASC";



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
        Response.Redirect("MedicalLab.aspx");
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtPartyCode.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Party Code";
            Timer1.Enabled = true;
            return;
        }


        if (txtLabName.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Name of Lab";
            Timer1.Enabled = true;
            return;
        }
        if (Session["SetupMode"].ToString() == "NEW")
        {
            if (!validateDuplicates(txtPartyCode.Text))
            {
                lblMsg.Text = "Medical Lab already available";
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
                spProcess = new OracleCommand("INSERT_MRP_WF_MEDICAL_LAB");
            }
            else if (Session["SetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_MEDICAL_LAB");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;

            spProcess.Parameters.Add("V_PTY_PARTY_CODE", OracleType.VarChar, 20).Value = txtPartyCode.Text;

            spProcess.Parameters.Add("V_PVR_BUSINESS_NAME", OracleType.VarChar, 150).Value = txtLabName.Text;
            spProcess.Parameters.Add("V_CON_ADDRESS_LINE_1", OracleType.VarChar, 150).Value = txtAddressLine1.Text;
            spProcess.Parameters.Add("V_CON_ADDRESS_LINE_2", OracleType.VarChar, 150).Value = txtAddressLine2.Text;
            spProcess.Parameters.Add("V_CON_ADDRESS_LINE_3", OracleType.VarChar, 150).Value = txtAddressLine3.Text;



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
        txtPartyCode.Enabled = false;
        txtLabName.Enabled = false;
        txtAddressLine1.Enabled = false;
        txtAddressLine2.Enabled = false;
        txtAddressLine3.Enabled = false;



        txtPartyCode.Text = "";
        txtLabName.Text = "";
        txtAddressLine1.Text = "";
        txtAddressLine2.Text = "";
        txtAddressLine3.Text = "";

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;



    }




    protected void btnAddNew_Click(object sender, EventArgs e)
    {

        txtPartyCode.Enabled = true;
        txtLabName.Enabled = true;
        txtAddressLine1.Enabled = true;
        txtAddressLine2.Enabled = true;
        txtAddressLine3.Enabled = true;


        txtPartyCode.Text = "";
        txtLabName.Text = "";
        txtAddressLine1.Text = "";
        txtAddressLine2.Text = "";
        txtAddressLine3.Text = "";

        btnSave.Enabled = true;

        Session["SetupMode"] = "NEW";
    }


    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtPartyCode.Text == "")
        {
            lblMsg.Text = "Please Select A Lab to alter";
            return;
        }

        txtLabName.Enabled = true;
        txtAddressLine1.Enabled = true;
        txtAddressLine2.Enabled = true;
        txtAddressLine3.Enabled = true;

        btnSave.Enabled = true;

        Session["SetupMode"] = "UPDATE";
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadMediacalLabDetails(grdSearchResult.SelectedRow.Cells[1].Text.Trim());


        btnAlter.Enabled = true;
    }

    private string loadMediacalLabDetails(string PartyCode)
    {

        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "   SELECT L.PTY_PARTY_CODE ,L.PVR_BUSINESS_NAME, L.CON_ADDRESS_LINE_1,L.CON_ADDRESS_LINE_2,L.CON_ADDRESS_LINE_3 FROM MRP_WF_MEDICAL_LABS L  " +
                      " WHERE L.PTY_PARTY_CODE='" + PartyCode + "'";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            txtPartyCode.Text = PartyCode;
            txtLabName.Text = dr[1].ToString();
            txtAddressLine1.Text = dr[2].ToString();
            txtAddressLine2.Text = dr[3].ToString();
            txtAddressLine3.Text = dr[4].ToString();




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
        selectQuery = "SELECT T.PTY_PARTY_CODE FROM MRP_WF_MEDICAL_LABS T WHERE T.PTY_PARTY_CODE='" + id + "'";


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
