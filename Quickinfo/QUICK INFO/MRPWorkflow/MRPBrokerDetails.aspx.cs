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
using System.IO;
using MsgBox;

public partial class MRPWorkflow_MRPBrokerDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            validatePageAuthentication();

            loadBrokerList();
            loadBrokerDetails();
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


    private void loadBrokerDetails()
    {
        DataTable dt1 = new DataTable();

        OracleConnection conOR = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader drOR;
        OracleDataAdapter daOR = new OracleDataAdapter();
        OracleCommand cmdOR = new OracleCommand();
        try
        {

            conOR.Open();

            cmdOR.Connection = conOR;
            String selectQueryOR = "";

            selectQueryOR = "SELECT t.BROKER_CODE , t.BROKER_NAME, t.BROKER_EMAILS  FROM MRP_WF_BROKERS t order by t.BROKER_CODE";

            cmdOR.CommandText = selectQueryOR;

            daOR.SelectCommand = cmdOR;
            daOR.Fill(dt1);

            grdBrokers.DataSource = dt1;
            grdBrokers.DataBind();



            cmdOR.Dispose();
            conOR.Close();
            conOR.Dispose();
        }
        catch (Exception e)
        {
        }

    }
    private void loadBrokerList()
    {
        ddlBrokerCode.Items.Clear();
        ddlBrokerCode.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "SELECT  PRT.PTY_PARTY_CODE AGENT_CODE,  PTV.PVR_BUSINESS_NAME AGENT_NAME FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, T_STAKE_HOLDER_FUNCTION STF WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBrokerCode.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    protected void grdBrokers_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnUpdate.Enabled = true;
        btnAdd.Enabled = false;

        ddlBrokerCode.Enabled = false;

        ddlBrokerCode.SelectedValue = grdBrokers.SelectedRow.Cells[1].Text.Trim();
        txtBrokerName.Text = grdBrokers.SelectedRow.Cells[2].Text.Trim();
        txtBrokerEmails.Text = grdBrokers.SelectedRow.Cells[3].Text.Trim();

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("MRPBrokerDetails.aspx");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {



        if (ddlBrokerCode.SelectedValue != "0")
        {
            if (!validateDuplicates(ddlBrokerCode.SelectedValue))
            {
                lblMsg.Text = "Broker code already available";
                return;
            }
        }
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            spProcess = new OracleCommand("INSERT_MRP_WF_BROKERS");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_BROKER_CODE", OracleType.VarChar).Value = ddlBrokerCode.SelectedValue;
            spProcess.Parameters.Add("V_BROKER_NAME", OracleType.VarChar).Value = ddlBrokerCode.SelectedItem.ToString();
            spProcess.Parameters.Add("V_BROKER_EMAILS", OracleType.VarChar).Value = txtBrokerEmails.Text;

            spProcess.ExecuteNonQuery();
            conProcess.Close();


            lblMsg.Text = "Successfully saved";
            loadBrokerDetails();
            clearFields();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";

        }


    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            spProcess = new OracleCommand("UPDATE_MRP_WF_BROKERS");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_BROKER_CODE", OracleType.VarChar).Value = ddlBrokerCode.SelectedValue;
            spProcess.Parameters.Add("V_BROKER_NAME", OracleType.VarChar).Value = ddlBrokerCode.SelectedItem.ToString();
            spProcess.Parameters.Add("V_BROKER_EMAILS", OracleType.VarChar).Value = txtBrokerEmails.Text;


            spProcess.ExecuteNonQuery();
            conProcess.Close();


            lblMsg.Text = "Successfully updated";
            loadBrokerDetails();
            clearFields();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while updating";

        }


    }

    private bool validateDuplicates(string brokerCode)
    {
        bool returnVal = true;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
    

        String selectQuery = "";
        selectQuery = "SELECT T.BROKER_CODE FROM MRP_WF_BROKERS T WHERE T.BROKER_CODE='" + brokerCode + "'";


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
    private void clearFields()
    {


        ddlBrokerCode.Enabled = true;


        ddlBrokerCode.SelectedValue = "0";
        txtBrokerName.Text = "";
        txtBrokerEmails.Text = "";

        btnUpdate.Enabled = false;
        btnAdd.Enabled = true;
    }


}
