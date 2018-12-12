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


public partial class ManageMedicalPayments : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            if (Request.Params["ProposalNo"] != null)
            {
                if (Request.Params["ProposalNo"] != "")
                {
                    txtProposalNo.Text = Request.Params["ProposalNo"].ToString();
                    LoadMedicalPaymentHistory(txtProposalNo.Text);
                }
            }

            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);

            txtPaidAmount.Attributes.Add("onkeyup", "jsValidateNum(this)");

            setPaymentsGridGridHeaders();
            loadMedicalLabs();


            txtBillReceivedDate.Enabled = false;
            txtPaymentVoucherSentDate.Enabled = false;
            txtChequeReceivedDate.Enabled = false;
            txtChequeNo.Enabled = false;
            txtMailedDate.Enabled = false;


            ddlPaymentMode.Enabled = false;
            txtRemarks.Enabled = false;

            ddlMedicalLab.Enabled = false;
            txtPaidAmount.Enabled = false;

            btnAddToMedicalPaymentsGrid.Enabled = false;

            btnAlter.Enabled = false;
            btnSave.Enabled = false;



            btnAddBillReceivedDate.Enabled = false;
            btnAddChequeReceivedDate.Enabled = false;
            btnAddMailedDate.Enabled = false;
            btnAddPaymentVoucherSentDate.Enabled = false;

            ddlPaymentMode.Items.Clear();
            ddlPaymentMode.Items.Add(new ListItem("--- Select One ---", "0"));
            ddlPaymentMode.Items.Add(new ListItem("Credit", "Credit"));
            ddlPaymentMode.Items.Add(new ListItem("Customer Paid", "Customer Paid"));

            Session.Remove("SetupMode");
        }


    }

    private void setPaymentsGridGridHeaders()
    {
        DataTable tblMediaclPayments = new DataTable();
        tblMediaclPayments.Columns.Add("MedicalLabCode", System.Type.GetType("System.String"));
        tblMediaclPayments.Columns.Add("MedicalLabName", System.Type.GetType("System.String"));
        tblMediaclPayments.Columns.Add("Payment", System.Type.GetType("System.String"));
        Session["MedicalPayments"] = tblMediaclPayments;
    }

    private void loadMedicalLabs()
    {
        ddlMedicalLab.Items.Clear();
        ddlMedicalLab.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = " select PTY_PARTY_CODE,PVR_BUSINESS_NAME from mrp_wf_medical_labs " +
                     "  ORDER BY PVR_BUSINESS_NAME ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlMedicalLab.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }
    private void LoadMedicalPaymentHistory(string sProposalNo)
    {

        grdSearchResults.DataSource = null;
        grdSearchResults.DataBind();

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand myOleDbCommand = new OracleCommand();
        myOleDbConnection.Open();
        myOleDbCommand.Connection = myOleDbConnection;

        String selectQuery = "";

        selectQuery = " SELECT    " +
                    " MP.PAYMENT_ID   AS \"Seq.\"," + //0
                    " TO_DATE(MP.BILL_RECEIVED_DATE,'DD/MM/RRRR' )  AS \"Bill Received Date\"," +//1
                    "  TO_DATE(MP.PAYMENT_VOUCHER_SENT_DATE,'DD/MM/RRRR' )  AS \"Payment Voucher Sent Date\"," +//2
                    "  TO_DATE(MP.CHEQUE_RECEIVED_DATE,'DD/MM/RRRR' )  AS \"Cheque Received Date\"," +//3
                    " MP.CHEQUE_NO	 AS \"Cheque No.\"," +//4
                    " TO_DATE(MP.MAILED_DATE,'DD/MM/RRRR' ) 	 AS \"Mailed Date\"," +//5
                    " MP.SYS_DATE   AS \"Added Date\"," +//6
                    " AU.USER_NAME  AS \"Entered By\"," +//7
                    " MP.PAYMENT_MODE   AS \"Paymet Mode\"," +//8
                    " MP.REMARKS  AS \"Remarks\"" +//9
                    " FROM " +
                    " MRP_WF_MEDICAL_PAYMENT MP " +
                    " INNER JOIN WF_ADMIN_USERS AU ON AU.USER_CODE=MP.USER_CODE " +
                    " WHERE MP.PROPOSAL_NO='" + sProposalNo + "' " +
                    " ORDER BY MP.SYS_DATE";




        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdSearchResults.DataSource = myOleDbDataReader;
            grdSearchResults.DataBind();
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtBillReceivedDate.Text = "";
        txtPaymentVoucherSentDate.Text = "";
        txtChequeReceivedDate.Text = "";
        txtChequeNo.Text = "";
        txtMailedDate.Text = "";

        ddlPaymentMode.SelectedValue = "0";
        txtRemarks.Text = "";

        ddlMedicalLab.SelectedValue = "0";
        txtPaidAmount.Text = "";


        txtBillReceivedDate.Enabled = true;
        txtPaymentVoucherSentDate.Enabled = true;
        txtChequeReceivedDate.Enabled = true;
        txtChequeNo.Enabled = true;
        txtMailedDate.Enabled = true;


        ddlPaymentMode.Enabled = true;
        txtRemarks.Enabled = true;

        ddlMedicalLab.Enabled = true;
        txtPaidAmount.Enabled = true;

        btnAddToMedicalPaymentsGrid.Enabled = true;


        btnAddBillReceivedDate.Enabled = true;
        btnAddChequeReceivedDate.Enabled = true;
        btnAddMailedDate.Enabled = true;
        btnAddPaymentVoucherSentDate.Enabled = true;

        grdMedicalPayments.DataSource = null;
        grdMedicalPayments.DataBind();

        btnSave.Enabled = true;
        Session["SetupMode"] = "NEW";
    }
    protected void btnAlter_Click(object sender, EventArgs e)
    {

        if (txtMedicalPaymentID.Text == "")
        {
            lblMsg.Text = "Please Select A Payment";
            return;
        }

        txtBillReceivedDate.Enabled = true;
        txtPaymentVoucherSentDate.Enabled = true;
        txtChequeReceivedDate.Enabled = true;
        txtChequeNo.Enabled = true;
        txtMailedDate.Enabled = true;


        ddlPaymentMode.Enabled = true;
        txtRemarks.Enabled = true;

        ddlMedicalLab.Enabled = false;
        txtPaidAmount.Enabled = false;

        btnAddToMedicalPaymentsGrid.Enabled = false;

        btnSave.Enabled = true;



        if (txtBillReceivedDate.Text == "")
        {
            btnAddBillReceivedDate.Enabled = true;
        }
        if (txtChequeReceivedDate.Text == "")
        {
            btnAddChequeReceivedDate.Enabled = true;
        }
        if (txtMailedDate.Text == "")
        {
            btnAddMailedDate.Enabled = true;
        }
        if (txtPaymentVoucherSentDate.Text == "")
        {
            btnAddPaymentVoucherSentDate.Enabled = true;
        }



        Session["SetupMode"] = "UPDATE";

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtProposalNo.Text.Trim() == "")
        {
            lblMsg.Text = "Invalid Proposal Number";
            Timer1.Enabled = true;
            return;
        }
        //if (txtBillReceivedDate.Text != "")
        //{
        //    int result = DateTime.Compare(Convert.ToDateTime(txtBillReceivedDate.Text), DateTime.Today);
        //    if (result < 0)
        //    {
        //        lblMsg.Text = "Bill Received Date cannot be earlier date than today";
        //        Timer1.Enabled = true;
        //        return;
        //    }
        //}
        //if (txtPaymentVoucherSentDate.Text != "")
        //{
        //    int result = DateTime.Compare(Convert.ToDateTime(txtPaymentVoucherSentDate.Text), DateTime.Today);
        //    if (result < 0)
        //    {
        //        lblMsg.Text = "Payment Voucher Sent Date cannot be earlier date than today";
        //        Timer1.Enabled = true;
        //        return;
        //    }
        //}
        //if (txtChequeReceivedDate.Text != "")
        //{
        //    int result = DateTime.Compare(Convert.ToDateTime(txtChequeReceivedDate.Text), DateTime.Today);
        //    if (result < 0)
        //    {
        //        lblMsg.Text = "Cheque Received Date cannot be earlier date than today";
        //        Timer1.Enabled = true;
        //        return;
        //    }
        //}

        //if (txtMailedDate.Text != "")
        //{
        //    int result = DateTime.Compare(Convert.ToDateTime(txtMailedDate.Text), DateTime.Today);
        //    if (result < 0)
        //    {
        //        lblMsg.Text = "Mailed Date cannot be earlier date than today";
        //        Timer1.Enabled = true;
        //        return;
        //    }
        //}


        if (grdMedicalPayments.Rows.Count < 1)
        {
            lblMsg.Text = "There should be at least one payment done to a lab";
            Timer1.Enabled = true;
            return;
        }

        String UserName = Context.User.Identity.Name;


        if (Left(UserName, 4) == "HNBA")
        {
            UserName = Right(UserName, (UserName.Length) - 5);
        }
        else
        {
            UserName = Right(UserName, (UserName.Length) - 7);
        }







        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            if (Session["SetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_MEDICAL_PAYMENT");


                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = txtProposalNo.Text.Trim();
                if (txtBillReceivedDate.Text != "")
                {
                    spProcess.Parameters.Add("V_BILL_RECEIVED_DATE", OracleType.DateTime).Value = txtBillReceivedDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_BILL_RECEIVED_DATE", OracleType.DateTime).Value = DBNull.Value;
                }

                if (txtPaymentVoucherSentDate.Text != "")
                {
                    spProcess.Parameters.Add("V_PAYMENT_VOUCHER_SENT_DATE", OracleType.DateTime).Value = txtPaymentVoucherSentDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_PAYMENT_VOUCHER_SENT_DATE", OracleType.DateTime).Value = DBNull.Value;
                }

                if (txtChequeReceivedDate.Text != "")
                {
                    spProcess.Parameters.Add("V_CHEQUE_RECEIVED_DATE", OracleType.DateTime).Value = txtChequeReceivedDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_CHEQUE_RECEIVED_DATE", OracleType.DateTime).Value = DBNull.Value;
                }


                if (txtChequeNo.Text != "")
                {

                    spProcess.Parameters.Add("V_CHEQUE_NO", OracleType.VarChar).Value = txtChequeNo.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_CHEQUE_NO", OracleType.VarChar).Value = DBNull.Value;
                }

                if (txtMailedDate.Text != "")
                {
                    spProcess.Parameters.Add("V_MAILED_DATE", OracleType.DateTime).Value = txtMailedDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_MAILED_DATE", OracleType.DateTime).Value = DBNull.Value;
                }


                spProcess.Parameters.Add("V_PAYMENT_MODE", OracleType.VarChar, 100).Value = ddlPaymentMode.SelectedValue;


                spProcess.Parameters.Add("V_REMARKS", OracleType.VarChar).Value = txtRemarks.Text;


                spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = UserName;

                spProcess.Parameters.Add("V_NEW_PAYMENT_ID", OracleType.Number).Direction = ParameterDirection.Output;
                spProcess.Parameters["V_NEW_PAYMENT_ID"].Direction = ParameterDirection.Output;


                spProcess.ExecuteNonQuery();
                conProcess.Close();


                string newPaymentID = "";
                newPaymentID = Convert.ToString(spProcess.Parameters["V_NEW_PAYMENT_ID"].Value);

                savePaymentItemsGrid(txtProposalNo.Text, newPaymentID);

            }
            else if (Session["SetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_MEDICAL_PAYMENT");


                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = txtProposalNo.Text.Trim();
                spProcess.Parameters.Add("V_PAYMENT_ID", OracleType.VarChar).Value = txtMedicalPaymentID.Text.Trim();


                if (txtBillReceivedDate.Text != "")
                {
                    spProcess.Parameters.Add("V_BILL_RECEIVED_DATE", OracleType.DateTime).Value = txtBillReceivedDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_BILL_RECEIVED_DATE", OracleType.DateTime).Value = DBNull.Value;
                }

                if (txtPaymentVoucherSentDate.Text != "")
                {
                    spProcess.Parameters.Add("V_PAYMENT_VOUCHER_SENT_DATE", OracleType.DateTime).Value = txtPaymentVoucherSentDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_PAYMENT_VOUCHER_SENT_DATE", OracleType.DateTime).Value = DBNull.Value;
                }

                if (txtChequeReceivedDate.Text != "")
                {
                    spProcess.Parameters.Add("V_CHEQUE_RECEIVED_DATE", OracleType.DateTime).Value = txtChequeReceivedDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_CHEQUE_RECEIVED_DATE", OracleType.DateTime).Value = DBNull.Value;
                }
                spProcess.Parameters.Add("V_CHEQUE_NO", OracleType.VarChar).Value = txtChequeNo.Text;

                if (txtMailedDate.Text != "")
                {
                    spProcess.Parameters.Add("V_MAILED_DATE", OracleType.DateTime).Value = txtMailedDate.Text;
                }
                else
                {
                    spProcess.Parameters.Add("V_MAILED_DATE", OracleType.DateTime).Value = DBNull.Value;
                }


                spProcess.Parameters.Add("V_PAYMENT_MODE", OracleType.VarChar, 100).Value = ddlPaymentMode.SelectedValue;


                spProcess.Parameters.Add("V_REMARKS", OracleType.VarChar).Value = txtRemarks.Text;


                spProcess.ExecuteNonQuery();
                conProcess.Close();




                deletePaymentItems(txtProposalNo.Text, txtMedicalPaymentID.Text);
                savePaymentItemsGrid(txtProposalNo.Text, txtMedicalPaymentID.Text);


            }







            lblMsg.Text = "Successfully saved";
            Timer1.Enabled = true;

            txtBillReceivedDate.Enabled = false;
            txtPaymentVoucherSentDate.Enabled = false;
            txtChequeReceivedDate.Enabled = false;
            txtChequeNo.Enabled = false;
            txtMailedDate.Enabled = false;


            ddlPaymentMode.Enabled = false;
            txtRemarks.Enabled = false;

            ddlMedicalLab.Enabled = false;
            txtPaidAmount.Enabled = false;

            btnAddToMedicalPaymentsGrid.Enabled = false;

            LoadMedicalPaymentHistory(txtProposalNo.Text);
            btnSave.Enabled = false;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }

    }


    private void savePaymentItemsGrid(string sProposalNo, string sNewPaymentID)
    {


        for (int i = 0; i < grdMedicalPayments.Rows.Count; i++)
        {

            try
            {
                OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conProcess.Open();
                OracleCommand spProcess = null;

                spProcess = new OracleCommand("INSERT_MRP_WF_MEDICAL_PMT_ITS");
                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = txtProposalNo.Text.Trim();
                spProcess.Parameters.Add("V_PAYMENT_ID", OracleType.Number).Value = sNewPaymentID;
                spProcess.Parameters.Add("V_PTY_PARTY_CODE", OracleType.VarChar).Value = grdMedicalPayments.Rows[i].Cells[0].Text;
                spProcess.Parameters.Add("V_PAID_AMOUNT", OracleType.Number).Value = grdMedicalPayments.Rows[i].Cells[2].Text;


                spProcess.ExecuteNonQuery();
                conProcess.Close();

            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error while saving";
                Timer1.Enabled = true;
            }
        }


    }




    private void deletePaymentItems(string sProposalNo, string sPaymentID)
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            string strQuery = "";

            strQuery = "DELETE FROM MRP_WF_MEDICAL_PAYMENT_ITEMS WHERE PROPOSAL_NO='" + sProposalNo + "' AND PAYMENT_ID=" + sPaymentID;

            spProcess = new OracleCommand(strQuery, conProcess);

            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {

        }

    }

    protected void btnAddToMedicalPaymentsGrid_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["MedicalPayments"];
        DataRow dr = dt.NewRow();

        dr["MedicalLabCode"] = ddlMedicalLab.SelectedValue.ToString();
        dr["MedicalLabName"] = ddlMedicalLab.SelectedItem.Text;
        dr["Payment"] = txtPaidAmount.Text;
        dt.Rows.Add(dr);

        Session["MedicalPayments"] = dt;
        grdMedicalPayments.DataSource = dt;
        grdMedicalPayments.DataBind();

        ddlMedicalLab.SelectedValue = "0";
        txtPaidAmount.Text = "";
    }

    protected void grdSearchResults_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtMedicalPaymentID.Text = grdSearchResults.SelectedRow.Cells[1].Text.Trim();
        txtBillReceivedDate.Text = (grdSearchResults.SelectedRow.Cells[2].Text.Trim() != "&nbsp;") ? grdSearchResults.SelectedRow.Cells[2].Text.Trim() : "";
        txtPaymentVoucherSentDate.Text = (grdSearchResults.SelectedRow.Cells[3].Text.Trim() != "&nbsp;") ? grdSearchResults.SelectedRow.Cells[3].Text.Trim() : "";
        txtChequeReceivedDate.Text = (grdSearchResults.SelectedRow.Cells[4].Text.Trim() != "&nbsp;") ? grdSearchResults.SelectedRow.Cells[4].Text.Trim() : "";
        txtChequeNo.Text = (grdSearchResults.SelectedRow.Cells[5].Text.Trim() != "&nbsp;") ? grdSearchResults.SelectedRow.Cells[5].Text.Trim() : "";
        txtMailedDate.Text = (grdSearchResults.SelectedRow.Cells[6].Text.Trim() != "&nbsp;") ? grdSearchResults.SelectedRow.Cells[6].Text.Trim() : "";

        if (grdSearchResults.SelectedRow.Cells[9].Text.Trim() != "&nbsp;")
        {
            ddlPaymentMode.SelectedValue = (grdSearchResults.SelectedRow.Cells[9].Text.Trim() != "&nbsp;") ? grdSearchResults.SelectedRow.Cells[9].Text.Trim() : "";
        }
        txtRemarks.Text = (grdSearchResults.SelectedRow.Cells[10].Text.Trim() != "&nbsp;") ? grdSearchResults.SelectedRow.Cells[10].Text.Trim() : "";


        if (txtBillReceivedDate.Text != "")
        {
            btnAddBillReceivedDate.Enabled = false;
        }
        if (txtChequeReceivedDate.Text != "")
        {
            btnAddChequeReceivedDate.Enabled = false;
        }
        if (txtMailedDate.Text != "")
        {
            btnAddMailedDate.Enabled = false;
        }
        if (txtPaymentVoucherSentDate.Text != "")
        {
            btnAddPaymentVoucherSentDate.Enabled = false;
        }


        LoadPaymentItems(txtProposalNo.Text, grdSearchResults.SelectedRow.Cells[1].Text.Trim());
        btnAlter.Enabled = true;
    }


    private void LoadPaymentItems(string sProposalNo, string sPaymentID)
    {
        grdMedicalPayments.DataSource = null;
        grdMedicalPayments.DataBind();

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand myOleDbCommand = new OracleCommand();
        myOleDbConnection.Open();
        myOleDbCommand.Connection = myOleDbConnection;

        String selectQuery = "";

        selectQuery = "SELECT  " +
                         " MPI.PTY_PARTY_CODE    AS \"Lab Code.\"  , " +
                      "  ML.PVR_BUSINESS_NAME  AS \"Medical Lab\", " +
                      "  MPI.PAID_AMOUNT	 AS \"Paid Amount\" " +
                      "  FROM MRP_WF_MEDICAL_PAYMENT_ITEMS  MPI " +
                      "  INNER JOIN   mrp_wf_medical_labs ML ON ML.PTY_PARTY_CODE=MPI.PTY_PARTY_CODE " +
                      " WHERE MPI.PROPOSAL_NO='" + sProposalNo + "' AND MPI.PAYMENT_ID=" + sPaymentID;




        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdMedicalPayments.DataSource = myOleDbDataReader;
            grdMedicalPayments.DataBind();
        }
    }


    protected void grdSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    public string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }

    protected void btnAddBillReceivedDate_Click(object sender, EventArgs e)
    {
        txtBillReceivedDate.Text = DateTime.Today.ToShortDateString();
    }
    protected void btnAddPaymentVoucherSentDate_Click(object sender, EventArgs e)
    {
        txtPaymentVoucherSentDate.Text = DateTime.Today.ToShortDateString();
    }
    protected void btnAddChequeReceivedDate_Click(object sender, EventArgs e)
    {
        txtChequeReceivedDate.Text = DateTime.Today.ToShortDateString();
    }
    protected void btnAddMailedDate_Click(object sender, EventArgs e)
    {
        txtMailedDate.Text = DateTime.Today.ToShortDateString();
    }
}
