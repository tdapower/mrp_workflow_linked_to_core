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
using Oracle.DataAccess.Types;

using Oracle.DataAccess.Client;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class MRPWorkflow_Common_CrystalDocumentViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string proposalNo = "";
            string docType = "";
            string intRateWording = "";
            if (Request.QueryString["ProposalNo"] == null || Request.QueryString["ProposalNo"] == "")
            {
                return;
            }

            if (Request.QueryString["ProposalNo"] != null || Request.QueryString["ProposalNo"] != "")
            {
                proposalNo = Request.QueryString["ProposalNo"].ToString();
            }

            if (Request.QueryString["IntRateWording"] != null || Request.QueryString["IntRateWording"] != "")
            {
                intRateWording = Request.QueryString["IntRateWording"].ToString();
            }



            if (Request.QueryString["DocType"] != null)
            {
                docType = Request.QueryString["DocType"].ToString();
            }

            if (proposalNo != "" && docType != "")
            {
                loadDocument(proposalNo, docType, intRateWording);
            }
        }
    }

    private void loadDocument(string proposalNo, string docType,string intRateWording)
    {
       
       String UserCode = Context.User.Identity.Name;
        if (Left(UserCode, 4) == "HNBA")
        {
            UserCode = Right(UserCode, (UserCode.Length) - 5);
        }
        else
        {
            UserCode = Right(UserCode, (UserCode.Length) - 7);
        }

        TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
        ConnectionInfo crConnectionInfo = new ConnectionInfo();

        //Crystal Report Properties
        CrystalDecisions.CrystalReports.Engine.Database crDatabase;
        CrystalDecisions.CrystalReports.Engine.Tables crTables;
        CrystalDecisions.CrystalReports.Engine.Table crTable;

        if (docType == "pending_cover")
        {
           
         

            ReportDocument crystalReport = new ReportDocument();



            if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
            {
                crystalReport.Load(Server.MapPath("../Documents/letters/MRP_PENDING_COVER.rpt"));
            }
            else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
            {
                crystalReport.Load(Server.MapPath("../Documents/letters/MCR_PENDING_COVER.rpt"));
            }
          //  crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");
            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", System.Configuration.ConfigurationManager.AppSettings["REPORT_DB_SERVER_NAME"].ToString(), "");


            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;



            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonDisplayName", getUserName(UserCode));
            crystalReport.SetParameterValue("signPersonName", UserCode);
            crystalReport.SetParameterValue("signPersonDesignation", loadDesignationOfPerson(UserCode));
            crystalReport.SetParameterValue("intRateWording", intRateWording);


           // LetterViewer.ReportSource = crystalReport;
            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Pending Cover");

            LetterViewer.RefreshReport();

            crystalReport.Close();
            crystalReport.Dispose();
            GC.Collect();
        }
        else if (docType == "confirmation_cover")
        {
          

            ReportDocument crystalReport = new ReportDocument();

            crystalReport.Load(Server.MapPath("../Documents/letters/MRP_CONFIRMATION_COVER.rpt"));


            crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", System.Configuration.ConfigurationManager.AppSettings["REPORT_DB_SERVER_NAME"].ToString(), "");
           // crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

            LetterViewer.SelectionFormula = "{MRP_WORKFLOW.PROPOSAL_NO}=\"" + @proposalNo + "\"";
            LetterViewer.ReportSource = crystalReport;

            crystalReport.SetParameterValue(0, proposalNo);
            crystalReport.SetParameterValue("signPersonDisplayName", getUserName(UserCode));
            crystalReport.SetParameterValue("signPersonName", UserCode);
            crystalReport.SetParameterValue("signPersonDesignation", loadDesignationOfPerson(UserCode));
            crystalReport.SetParameterValue("intRateWording", intRateWording);
           // crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CONFIRMATION COVER");


            crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Confirmation Cover");
           // LetterViewer.ReportSource = crystalReport;

            LetterViewer.RefreshReport();


            crystalReport.Close();
            crystalReport.Dispose();
            GC.Collect();
        }



    }
    private string getUserName(string userCode)
    {
        string userName = "";



        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

    
        String selectQuery = "";

        string MRPUserCodes = System.Configuration.ConfigurationManager.AppSettings["MRPUserCodes"].ToString();


        selectQuery = "   SELECT T.USER_NAME FROM WF_ADMIN_USERS T  " +
           " WHERE T.USER_CODE=:V_USER_CODE";

        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_USER_CODE", userCode));


        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            userName = dr[0].ToString();


        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return userName;
    }


    private string loadDesignationOfPerson(string iSignPersonCode)
    {
        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

       
        String selectQuery = "";

        selectQuery = "   SELECT T.DESIGNATION FROM MRP_WF_SIGN_PERSON T  " +
                     "  WHERE T.USER_CODE=:V_USER_CODE";



        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_USER_CODE", iSignPersonCode));

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

    public string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }

    public string Mid(string text, int start, int end)
    {
        return text.Substring(start, end);
    }

    public string Mid(string text, int start)
    {
        return text.Substring(start, text.Length - start);
    }
}
