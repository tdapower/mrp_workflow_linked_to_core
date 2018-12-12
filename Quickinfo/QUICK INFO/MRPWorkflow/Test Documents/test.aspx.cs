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


public partial class MRPWorkflow_Documents_test : System.Web.UI.Page
{
    //'CrystalReport1' must be the name the CrystalReport
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

            //CheckRole check_role = new CheckRole();
            //Session["check_role"] = check_role.CheckRole1(Session["ROLE_CODE"].ToString(), "T0027", 0);
            //if (Session["check_role"].ToString().Trim() == "NO")
            //{
            //    //lblError1.Text = "You dont Have Priviladges to do this......!";
            //    Response.Redirect("Permissin.aspx");
            //    return;
            //}

            if (!IsPostBack)
            {

                //gridDownload.DataSource = GetData();
                //gridDownload.DataBind();



            }

            //crConnectionInfo.ServerName = "HNBUAT";
            //crConnectionInfo.UserID = "hnba_crc";
            //crConnectionInfo.Password = "HNBACRC";


            //crDatabase = CrystalReportSource1.ReportDocument.Database;
            //crTables = crDatabase.Tables;
            //foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            //{
            //    crTableLogOnInfo = crTable.LogOnInfo;
            //    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
            //    crTable.ApplyLogOnInfo(crTableLogOnInfo);
            //}
            //CrystalReportViewer1.ReportSource = CrystalReportSource1;
            //CrystalReportViewer1.RefreshReport();
        }

    }
    protected void CrystalReportViewer1_Init(object sender, EventArgs e)
    {
        //crConnectionInfo.ServerName = "HNBUAT";
        //crConnectionInfo.UserID = "hnba_crc";
        //crConnectionInfo.Password = "HNBACRC";



        //crDatabase = CrystalReportSource1.ReportDocument.Database;
        //crTables = crDatabase.Tables;
        //foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
        //{
        //    crTableLogOnInfo = crTable.LogOnInfo;
        //    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
        //    crTable.ApplyLogOnInfo(crTableLogOnInfo);
        //}
        //CrystalReportViewer1.ReportSource = CrystalReportSource1;
        //CrystalReportViewer1.RefreshReport();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        // Get the report document
        ReportDocument repDoc = CrystalReportSource1.ReportDocument;
        // Stop buffering the response
        Response.Buffer = false;
        // Clear the response content and headers
        Response.ClearContent();
        Response.ClearHeaders();
        try
        {
            // Export the Report to Response stream in PDF format and file name Customers
            repDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Customers");
            // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ex = null;
        }
    }


    protected void btnSaveToDB_Click(object sender, EventArgs e)
    {
        ReportDocument repDoc = CrystalReportSource1.ReportDocument;
        Response.Buffer = false;
        Response.ClearContent();
        Response.ClearHeaders();
        try
        {

            // repDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Customers");
            //repDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Customers");
            saveFile(repDoc);



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ex = null;
        }
    }



    private void saveFile(ReportDocument repDoc)
    {
        try
        {

            MemoryStream memStream = new MemoryStream();
            memStream = (MemoryStream)repDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            byte[] byt;
            byt = memStream.ToArray();


            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            //spProcess = new OracleCommand("INSERT_MRP_WORKFLOW_DOCUMENTS");

            //spProcess.CommandType = CommandType.StoredProcedure;
            //spProcess.Connection = conProcess;
            //spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = "r";
            //spProcess.Parameters.Add("V_STATUS_CODE", OracleType.Number, 5).Value = 1;
            //spProcess.Parameters.Add("V_DOCUMENT_TYPE", OracleType.VarChar, 150).Value = "er";
            // spProcess.Parameters.Add("V_DOCUMENT", OracleType.Clob, 0).Value = sConvertdHex;


            int newDocId = 0;
            newDocId = getNewDocumentID();
            string strQuery = "";

            strQuery = "INSERT INTO MRP_WORKFLOW_DOCUMENTS(DOCUMENT_ID, PROPOSAL_NO, STATUS_CODE, DOCUMENT_TYPE,DOCUMENT) VALUES (";
            strQuery += "" + newDocId + ", ";
            strQuery += "'" + "2012/0498/HNB" + "', ";
            strQuery += "'" + 2 + "', ";
            strQuery += "'" + "yyyy" + "', ";
            strQuery += " :Document)";


            OracleParameter blobParameter = new OracleParameter();
            blobParameter.ParameterName = "Document";
            // blobParameter.OracleType = OracleType.Blob;
            blobParameter.Direction = ParameterDirection.Input;
            // blobParameter.Value = byteData;
            //blobParameter.Value = memStream.ToArray();
            blobParameter.Value = byt;



            spProcess = new OracleCommand(strQuery, conProcess);
            spProcess.Parameters.Add(blobParameter);


            spProcess.ExecuteNonQuery();
            conProcess.Close();



        }
        catch (Exception ex)
        {
            //lblMsg.Text = "Error While Saving";
            //Timer1.Enabled = true;
        }

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        ReportDocument repDoc = CrystalReportSource1.ReportDocument;
        Response.Buffer = false;
        Response.ClearContent();
        Response.ClearHeaders();
        try
        {


            uploadFile(repDoc);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ex = null;
        }
    }
    private void uploadFile(ReportDocument repDoc)
    {
        try
        {


            int intlength = fileUploadDocument.PostedFile.ContentLength;
            Byte[] byteData = new Byte[intlength];
            fileUploadDocument.PostedFile.InputStream.Read(byteData, 0, intlength);



            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            //spProcess = new OracleCommand("INSERT_MRP_WORKFLOW_DOCUMENTS");

            //spProcess.CommandType = CommandType.StoredProcedure;
            //spProcess.Connection = conProcess;
            //spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = "r";
            //spProcess.Parameters.Add("V_STATUS_CODE", OracleType.Number, 5).Value = 1;
            //spProcess.Parameters.Add("V_DOCUMENT_TYPE", OracleType.VarChar, 150).Value = "er";
            // spProcess.Parameters.Add("V_DOCUMENT", OracleType.Clob, 0).Value = sConvertdHex;

            string strQuery = "";

            strQuery = "INSERT INTO MRP_WORKFLOW_DOCUMENTS(DOCUMENT_ID, PROPOSAL_NO, STATUS_CODE, DOCUMENT_TYPE,DOCUMENT) VALUES (";
            strQuery += "7, ";
            strQuery += "'" + "2012/0498/HNB" + "', ";
            strQuery += "'" + 2 + "', ";
            strQuery += "'" + "ssd" + "', ";
            strQuery += " :Document)";


            OracleParameter blobParameter = new OracleParameter();
            blobParameter.ParameterName = "Document";
            // blobParameter.OracleType = OracleType.Blob;
            blobParameter.Direction = ParameterDirection.Input;
            blobParameter.Value = byteData;
            //blobParameter.Value = memStream.ToArray();

            spProcess = new OracleCommand(strQuery, conProcess);
            spProcess.Parameters.Add(blobParameter);


            spProcess.ExecuteNonQuery();
            conProcess.Close();



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
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = " SELECT CASE WHEN MAX(DOCUMENT_ID)  IS NULL THEN 1 ELSE TO_NUMBER((MAX(DOCUMENT_ID)+1)) END AS DOCUMENT_ID FROM MRP_WORKFLOW_DOCUMENTS";

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
        con.Close();
        con.Dispose();

        return documentId;
    }


    DataTable GetData()
    {
        DataTable dt = new DataTable();
        string con = ConfigurationManager.ConnectionStrings["ORAWF"].ToString();
        OracleConnection conn = new OracleConnection(con);
        conn.Open();
        OracleCommand cmd = new OracleCommand("select DOCUMENT_ID,DOCUMENT_TYPE from MRP_WORKFLOW_DOCUMENTS ", conn);
        OracleDataAdapter adpt = new OracleDataAdapter(cmd);
        adpt.Fill(dt);
        return dt;
    }


    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow row = lnkbtn.NamingContainer as GridViewRow;
        string documentID = gridDownload.DataKeys[row.RowIndex].Value.ToString();
        downloadFile(documentID);
    }

    private void downloadFile(string iDocumentID)
    {



        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT DOCUMENT FROM MRP_WORKFLOW_DOCUMENTS WHERE DOCUMENT_ID=" + iDocumentID;

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();



        if (dr.HasRows)
        {
            dr.Read();

            OracleBlob blob = dr.GetOracleBlob(0);
            // Response.Charset = "";
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);




            Response.ContentType = "application/pdf";
            Response.BinaryWrite(blob.Value);
            Response.End();



            //if (dr[0] != DBNull.Value)
            //{

            //    byte[] byteArray = (Byte[])dr[0];
            //    using (FileStream fs = new FileStream("test", FileMode.CreateNew, FileAccess.Write))
            //    {
            //        fs.Write(byteArray, 0, byteArray.Length);
            //    }
            //}
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }

    protected void btnSendSms_Click(object sender, EventArgs e)
    {
        string NO = "";

        sms abc = new sms();
        NO = "0759110260";
        string aa = abc.sendsms(NO.Trim(),"test");
        
    }
}
