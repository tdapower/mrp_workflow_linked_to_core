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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Data.OleDb;
//using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Net;
using System.DirectoryServices;
using System.Net.Mail;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;


public partial class MRPWorkflow_Documents_WorkflowDocuments : System.Web.UI.Page
{

    OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

    TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
    ConnectionInfo crConnectionInfo = new ConnectionInfo();

    //Crystal Report Properties
    CrystalDecisions.CrystalReports.Engine.Database crDatabase;
    CrystalDecisions.CrystalReports.Engine.Tables crTables;
    CrystalDecisions.CrystalReports.Engine.Table crTable;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);


            if (Request.Params["StatusCode"] != null)
            {
                //if (Request.Params["ProposalNo"] != "")
                //{
                //    SearchData(Request.Params["ProposalNo"].ToString());
                //}

                if (Request.Params["StatusCode"] != "")
                {

                    LoadDocuments(Request.Params["StatusCode"].ToString());
                }

            }
            //  LetterViewer.ReportSource = Session("report");
            loadSigningPersons();
            loadMedicalLabs();
        }

        if (IsPostBack)
        {
            loadReport();

        }

        // crConnectionInfo.ServerName = "HNBUAT";
        // crConnectionInfo.UserID = "hnba_crc";
        // crConnectionInfo.Password = "HNBACRC";


        //crDatabase = LettertSource.ReportDocument.Database;
        // crTables = crDatabase.Tables;
        // foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
        // {
        //     crTableLogOnInfo = crTable.LogOnInfo;
        //     crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
        //     crTable.ApplyLogOnInfo(crTableLogOnInfo);
        // }
        // LetterViewer.ReportSource = "~/MRPWorkflow/Documents/letters/MRP_PROPOSAL_CANCELLATION_LETTER.rpt";
        // LetterViewer.RefreshReport();






    }



    //private void SearchData(string sProposalNo)
    //{

    //    grdDocuments.DataSource = null;
    //    grdDocuments.DataBind();

    //    OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
    //    OracleCommand myOleDbCommand = new OracleCommand();
    //    myOleDbConnection.Open();
    //    myOleDbCommand.Connection = myOleDbConnection;

    //    String selectQuery = "";
    //    selectQuery = " SELECT DOCUMENT_ID,DOCUMENT_TYPE FROM MRP_WORKFLOW_DOCUMENTS WHERE PROPOSAL_NO='" + sProposalNo + "'";

    //    myOleDbCommand.CommandText = selectQuery;

    //    OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
    //    if (myOleDbDataReader.HasRows == true)
    //    {
    //        DataTable dbTable = new DataTable();
    //        grdDocuments.DataSource = myOleDbDataReader;
    //        grdDocuments.DataBind();
    //    }
    //}





    private void LoadDocuments(string sStatusCode)
    {

        grdDocuments.DataSource = null;
        grdDocuments.DataBind();


        OracleCommand myOleDbCommand = new OracleCommand();
        myOleDbConnection.Open();
        myOleDbCommand.Connection = myOleDbConnection;

        String selectQuery = "";
        //selectQuery = " SELECT DOCUMENT_ID,DOCUMENT_TITLE FROM MRP_DOCUMENTS WHERE STATUS_CODE=" + sStatusCode + "";

        selectQuery = " SELECT DOCUMENT_ID,DOCUMENT_TITLE FROM MRP_DOCUMENTS WHERE status_code='" + sStatusCode + "' or status_code like '%," + sStatusCode + "' OR status_code like '" + sStatusCode + ",%'  OR status_code like '%," + sStatusCode + ",%' ORDER BY DOCUMENT_TITLE";


        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdDocuments.DataSource = myOleDbDataReader;
            grdDocuments.DataBind();
        }
        else
        {
            lblMsg.Text = "There are no letters/documents for selected status.";
        }
    }



    protected void lnkViewDocument_Click(object sender, EventArgs e)
    {
        if (ddlSigningPerson.SelectedValue == "" || ddlSigningPerson.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the Signing Person";
            Timer1.Enabled = true;
            return;
        }




        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow row = lnkbtn.NamingContainer as GridViewRow;
        string documentID = grdDocuments.DataKeys[row.RowIndex].Value.ToString();
        txtDocumentId.Text = documentID;
        Session["documentID"] = documentID;

        if (documentID == "3" || documentID == "11")
        {
            if (ddlMedicalLab.SelectedValue == "" || ddlMedicalLab.SelectedValue == "0")
            {
                lblMsg.Text = "Please Select the medical Lab";
                Timer1.Enabled = true;
                return;
            }
        }

        // loadReport();
        // Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "javascript:jsViewDocuments(" + documentID + ");", true);

        string proposalNo = "";
        proposalNo = Request.Params["ProposalNo"].ToString();

        ReportDocument crystalReport = new ReportDocument();
        if (documentID == "1")//MRP PROPOSAL CANCELLATION LETTER
        {
            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/MRP_PROPOSAL_CANCELLATION_LETTER.rpt"));
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PROPOSAL CANCELLATION LETTER");


        }

        if (documentID == "2")//MORTGAGE REDUCING POLICY - EXCESS PREMIUM REFUND LETTER
        {
            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/EXCESS_PREMIUM_REFUND_LETTER.rpt"));
             //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "EXCESS PREMIUM REFUND LETTER");



        }

        if (documentID == "3")//REQUEST FOR MEDICAL EXAMINATION REPORTS LIFE ASSURE 1
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/ASSURE1_REQUEST_FOR_MEDICAL_EXAMINATION_REPORTS.rpt"));
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.SetParameterValue("labCode", ddlMedicalLab.SelectedValue.ToString());
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "MEDICAL EXAMINATION REPORTS");



        }


        if (documentID == "4")//PENDING DOCUMENTS REQUESTING LETTER
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/PENDING_DOCUMENTS_REQUESTING_LETTER.rpt"));
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Pending Letter");
            // crystalReport.PrintToPrinter(1, false, 0, 0);

            //session.Add("report", crystalReport);
            //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);



        }

        if (documentID == "5")//MRP PENDING COVER
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/MRP_PENDING_COVER.rpt"));
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PENDING COVER");
            // crystalReport.PrintToPrinter(1, false, 0, 0);

            //session.Add("report", crystalReport);
            //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);



        }


        if (documentID == "6")//MRP CONFIRMATION COVER
        {
            if (checkIsPendingPaymentAdded(proposalNo))
            {

                crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/MRP_CONFIRMATION_COVER.rpt"));
                //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
                crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

                LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
                LetterViewer.ReportSource = crystalReport;

                crystalReport.SetParameterValue(0, proposalNo);
                crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
                crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
                crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "CONFIRMATION COVER");
                // crystalReport.PrintToPrinter(1, false, 0, 0);

                //session.Add("report", crystalReport);
                //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);
            }
            else
            {
                lblMsg.Text = "There are no pending payment for this policy to generate Confirmation Cover";

            }

        }

        if (documentID == "7")//MRP REMINDERS
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/MRP_REMINDERS.rpt"));
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");


            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "REMINDERS");
            // crystalReport.PrintToPrinter(1, false, 0, 0);

            //session.Add("report", crystalReport);
            //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);


        }


        if (documentID == "8")//MEDICAL REIMBURSEMENT LETTER
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/MEDICAL_REIMBURSEMENT_LETTER.rpt"));
             //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");


            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "MEDICAL REIMBURSEMENT LETTER");
            // crystalReport.PrintToPrinter(1, false, 0, 0);

            //session.Add("report", crystalReport);
            //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);


        }


        if (documentID == "9")//FURTHER MEDICAL LETTER
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/FURTHER_MEDICAL_LETTER.rpt"));
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");


            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.SetParameterValue("labCode", ddlMedicalLab.SelectedValue.ToString());
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "FURTHER MEDICAL LETTER");
            // crystalReport.PrintToPrinter(1, false, 0, 0);

            //session.Add("report", crystalReport);
            //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);


        }

        if (documentID == "10")//REQUEST FOR MEDICAL EXAMINATION REPORTS OVER SEAS
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/REQUEST_FOR_MEDICAL_EXAMINATION_REPORTS_OVERSEAS.rpt"));
         //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");


            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "REQUEST FOR MEDICAL EXAMINATION REPORTS (OVER SEAS CUSTOMERS)");
          
            // crystalReport.PrintToPrinter(1, false, 0, 0);

            //session.Add("report", crystalReport);
            //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);


        }

        if (documentID == "11")//REQUEST FOR MEDICAL EXAMINATION REPORTS LIFE ASSURE 2
        {

            crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/ASSURE2_REQUEST_FOR_MEDICAL_EXAMINATION_REPORTS.rpt"));
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");
            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonName", ddlSigningPerson.SelectedItem.ToString());
            crystalReport.SetParameterValue("signPersonDesignation", txtDesignation.Text);
            crystalReport.SetParameterValue("labCode", ddlMedicalLab.SelectedValue.ToString());
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "MEDICAL EXAMINATION REPORTS");



        }


        //Save the generated document to the database

        Response.Buffer = false;
        Response.ClearContent();
        Response.ClearHeaders();
        //try
        //{
        //    saveFile(crystalReport);
        //}
        //catch (Exception ex)
        //{
        //    //Console.WriteLine(ex.Message);
        //    //ex = null;
        //}


    }

    private bool checkIsPendingPaymentAdded(string sProposalNo)//to check whether pending payment is added to this policy
    {


        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";



        selectQuery = "SELECT PCD.proposal_no,PCD.pending_doc_code,PCD.IS_FAX_PENDING  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                        " WHERE  PCD.PROPOSAL_NO='" + sProposalNo + "'  AND PCD.pending_doc_code=3";

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


    private void loadReport()
    {
        //string documentID = "";

        //if (Session["documentID"] != null)
        //{
        //    documentID = Session["documentID"].ToString();
        //    if (documentID == "1")//MRP PROPOSAL CANCELLATION LETTER
        //    {
        //        ReportDocument crystalReport = new ReportDocument();
        //        crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/MRP_PROPOSAL_CANCELLATION_LETTER.rpt"));
        //        //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
        //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

        //        string proposalNo = "";
        //        proposalNo = Request.Params["ProposalNo"].ToString();
        //        LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
        //        LetterViewer.ReportSource = crystalReport;

        //        //session.Add("report", crystalReport);
        //        //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);


        //        //Save the generated document to the database

        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        try
        //        {
        //            saveFile(crystalReport);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Console.WriteLine(ex.Message);
        //            //ex = null;
        //        }
        //    }

        //    if (documentID == "4")//PENDING DOCUMENTS REQUESTING LETTER
        //    {
        //        ReportDocument crystalReport = new ReportDocument();
        //        crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/PENDING_DOCUMENTS_REQUESTING_LETTER.rpt"));
        //        crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");

        //        string proposalNo = "";
        //        proposalNo = Request.Params["ProposalNo"].ToString();
        //        LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
        //        LetterViewer.ReportSource = crystalReport;

        //       // crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Pending Letter");
        //       // crystalReport.PrintToPrinter(1, false, 0, 0);

        //        //session.Add("report", crystalReport);
        //        //LetterViewer.reportsource = Ctype(session("report"), ReportDocument);

        //        //Save the generated document to the database

        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        try
        //        {
        //            saveFile(crystalReport);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Console.WriteLine(ex.Message);
        //            //ex = null;
        //        }
        //    }

        //}
    }



    private void saveFile(ReportDocument repDoc)
    {
        try
        {

            MemoryStream memStream = new MemoryStream();
            memStream = (MemoryStream)repDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            byte[] byt;
            byt = memStream.ToArray();


            //OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());



            OracleCommand spProcess = null;



            int newDocId = 0;
            newDocId = getNewDocumentID();

            string proposalNo = "";
            proposalNo = Request.Params["ProposalNo"].ToString();

            string statusCode = "";
            statusCode = Request.Params["StatusCode"].ToString();

            string Date = GetServerDate();
            DateTime iDate = Convert.ToDateTime(Date.ToString());


            string strQuery = "";

            strQuery = "INSERT INTO MRP_WORKFLOW_DOCUMENTS(WF_DOCUMENT_ID, PROPOSAL_NO, STATUS_CODE, DOCUMENT_ID,DOCUMENT,SYS_DATE) VALUES (";
            strQuery += "" + newDocId + ", ";
            strQuery += "'" + proposalNo + "', ";
            strQuery += "" + statusCode + ", ";
            strQuery += "" + txtDocumentId.Text + ", ";
            strQuery += " :Document,";
            strQuery += ":SysDate)";


            OracleParameter blobParameter = new OracleParameter();
            blobParameter.ParameterName = "Document";
            // blobParameter.OracleType = OracleType.Blob;
            blobParameter.Direction = ParameterDirection.Input;
            blobParameter.Value = byt;

            OracleParameter dateParameter = new OracleParameter();
            dateParameter.ParameterName = "Sys_Date";
            //dateParameter.OracleDbType = OracleDbType.Date;
            dateParameter.Direction = ParameterDirection.Input;
            dateParameter.Value = iDate;


            if (myOleDbConnection.State == ConnectionState.Closed)
            {
                myOleDbConnection.Open();
            }

            spProcess = new OracleCommand(strQuery, myOleDbConnection);
            spProcess.Parameters.Add(blobParameter);
            spProcess.Parameters.Add(dateParameter);

            spProcess.ExecuteNonQuery();
            myOleDbConnection.Close();



        }
        catch (Exception ex)
        {
            //lblMsg.Text = "Error While Saving";
            //Timer1.Enabled = true;
        }

    }

    private int getNewDocumentID()
    {
        int documentId = 0;
        // OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;
        if (myOleDbConnection.State == ConnectionState.Closed)
        {
            myOleDbConnection.Open();
        }
        OracleCommand cmd = new OracleCommand();
        cmd.Connection = myOleDbConnection;
        String selectQuery = "";
        selectQuery = " SELECT CASE WHEN MAX(WF_DOCUMENT_ID)  IS NULL THEN 1 ELSE TO_NUMBER((MAX(WF_DOCUMENT_ID)+1)) END AS WF_DOCUMENT_ID FROM MRP_WORKFLOW_DOCUMENTS";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            documentId = Convert.ToInt32(dr[0].ToString());
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        myOleDbConnection.Close();
        //  myOleDbConnection.Dispose();

        return documentId;
    }

    public string GetServerDate()
    {
        string ServerDate = "";
        SqlConnection connDetalis = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
        SqlCommand cmdDetails = new SqlCommand();
        SqlCommand cmdDetails8 = new SqlCommand();
        connDetalis.Open();

        SqlCommand cmdGetDocNo = new SqlCommand();
        SqlDataReader drcmdGetDocNo;

        cmdGetDocNo.CommandType = CommandType.Text;
        cmdGetDocNo.Connection = connDetalis;
        cmdGetDocNo.CommandText = "SELECT getdate()";

        drcmdGetDocNo = cmdGetDocNo.ExecuteReader();

        if (drcmdGetDocNo.HasRows)
        {
            while (drcmdGetDocNo.Read())
            {
                ServerDate = drcmdGetDocNo[0].ToString();
            }
        }
        return ServerDate;
    }

    private void loadSigningPersons()
    {
        ddlSigningPerson.Items.Clear();
        ddlSigningPerson.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "   SELECT T.SIGN_PERSON_CODE,T.SIGN_PERSON_NAME   FROM MRP_WF_SIGN_PERSON T  " +
                     "  ORDER BY T.SIGN_PERSON_NAME ASC";




        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlSigningPerson.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));


            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private string loadDesignationOfPerson(string iSignPersonCode)
    {
        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "   SELECT T.DESIGNATION FROM MRP_WF_SIGN_PERSON T  " +
                     "  WHERE T.SIGN_PERSON_CODE=" + iSignPersonCode;


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            returnValue = dr[0].ToString();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
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





    protected void LetterViewer_Init(object sender, EventArgs e)
    {
        /* crConnectionInfo.ServerName = "HNBUAT";
       crConnectionInfo.UserID = "hnba_crc";
       crConnectionInfo.Password = "HNBACRC";



       crDatabase = LettertSource.ReportDocument.Database;
         crTables = crDatabase.Tables;
         foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
         {
             crTableLogOnInfo = crTable.LogOnInfo;
             crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
             crTable.ApplyLogOnInfo(crTableLogOnInfo);
         }
       LetterViewer.ReportSource = "~/MRPWorkflow/Documents/letters/MRP_PROPOSAL_CANCELLATION_LETTER.rpt";
       LetterViewer.RefreshReport();*/
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }

    protected void ddlSigningPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSigningPerson.SelectedValue != "" || ddlSigningPerson.SelectedValue != "0")
        {
            txtDesignation.Text = loadDesignationOfPerson(ddlSigningPerson.SelectedValue);
        }
    }
}
