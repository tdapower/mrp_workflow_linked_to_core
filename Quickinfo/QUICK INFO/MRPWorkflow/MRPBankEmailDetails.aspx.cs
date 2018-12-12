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

public partial class MRPWorkflow_MRPBankEmailDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        loadBankData();
        
    }


   

    private void loadBankData()
    {
        DataTable dt1 = new DataTable();
 
        OracleConnection conOR = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader drOR;
        OracleDataAdapter daOR = new OracleDataAdapter();
        OracleCommand cmdOR = new OracleCommand();
        conOR.Open();


       

            cmdOR.Connection = conOR;
            String selectQueryOR = "";

            selectQueryOR = "SELECT t.*,(select m.hnba_email from mrp_wf_hnba_email m where m.hnb_code=t.hnba_branch_code and m.status = 'BRANCH') HNBA_EMAIL FROM mrp_wf_banks_email t order by t.bank_code";

            cmdOR.CommandText = selectQueryOR;

            daOR.SelectCommand = cmdOR;
            daOR.Fill(dt1);

            grdEmailList.DataSource = dt1;
            grdEmailList.DataBind();

        

        cmdOR.Dispose();
        conOR.Close();
        conOR.Dispose();


    }


    protected void grdEmailList_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnUpdate.Enabled = true;
        btnInsert.Enabled = false;


       txtBankCode.Text= grdEmailList.SelectedRow.Cells[1].Text.Trim();
       txtHNBEmail.Text = grdEmailList.SelectedRow.Cells[2].Text.Trim();
       txtBanAssuranceEmail.Text = grdEmailList.SelectedRow.Cells[3].Text.Trim();
       txtHNBABranchCode.Text = grdEmailList.SelectedRow.Cells[4].Text.Trim();
       txtHNBAEmail.Text = grdEmailList.SelectedRow.Cells[5].Text.Trim();

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("MRPBankEmailDetails.aspx");
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            spProcess = new OracleCommand("INSERT_MRP_WF_INSERT_EMAIL");
                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_BANKCODE", OracleType.VarChar, 100).Value = txtBankCode.Text;
                spProcess.Parameters.Add("V_HNBEMAIL", OracleType.VarChar, 100).Value = txtHNBEmail.Text;
                spProcess.Parameters.Add("V_BANKASS_EMAIL", OracleType.VarChar, 100).Value = txtBanAssuranceEmail.Text;
                spProcess.Parameters.Add("V_HNBA_BRANCH", OracleType.VarChar, 100).Value = txtHNBABranchCode.Text;
                spProcess.Parameters.Add("V_HNBAEMAIL", OracleType.VarChar, 100).Value = txtHNBAEmail.Text;
              
            spProcess.ExecuteNonQuery();
            conProcess.Close();


            lblMsg.Text = "Successfully saved";

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




            spProcess = new OracleCommand("UPDATE_MRP_WF_UPDATE_EMAIL");
                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_BANKCODE", OracleType.VarChar, 100).Value = txtBankCode.Text;
                spProcess.Parameters.Add("V_HNBEMAIL", OracleType.VarChar, 100).Value = txtHNBEmail.Text;
                spProcess.Parameters.Add("V_BANKASS_EMAIL", OracleType.VarChar, 100).Value = txtBanAssuranceEmail.Text;
                spProcess.Parameters.Add("V_HNBA_BRANCH", OracleType.VarChar, 100).Value = txtHNBABranchCode.Text;
                spProcess.Parameters.Add("V_HNBAEMAIL", OracleType.VarChar, 100).Value = txtHNBAEmail.Text;
              

            spProcess.ExecuteNonQuery();
            conProcess.Close();


            lblMsg.Text = "Successfully saved";
            
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
           
        }

    
    }
}
