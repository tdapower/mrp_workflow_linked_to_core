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
using System.Data.OracleClient;

public partial class BenchMarkMonitoringReport3 : System.Web.UI.Page
{
    TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
    ConnectionInfo crConnectionInfo = new ConnectionInfo();
    CrystalDecisions.CrystalReports.Engine.Database crDatabase;
    CrystalDecisions.CrystalReports.Engine.Tables crTables;
    string reportDescription;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["reportDescription"] != null)
            {
                if (Request.Params["reportDescription"] != "")
                {
                    reportDescription = Request.Params["reportDescription"].ToString();
                }
            }


            crConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings["REPORT_DB_SERVER_NAME"].ToString();
            crConnectionInfo.UserID = "hnba_crc";
            crConnectionInfo.Password = "HNBACRC";

            crDatabase = CrystalReportSource1.ReportDocument.Database;

            crTables = crDatabase.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            {
                crTableLogOnInfo = crTable.LogOnInfo;
                crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogOnInfo);
            }
            CrystalDecisions.Web.Parameter CRpm = new CrystalDecisions.Web.Parameter();
            CRpm.Name = "reportDescription";
            CRpm.DefaultValue = reportDescription;
            CrystalReportSource1.Report.Parameters.Add(CRpm);

            CrystalReportViewer1.ReportSource = CrystalReportSource1;
            CrystalReportViewer1.RefreshReport();


            //CrystalReportSource1.ReportDocument.Close();
            //CrystalReportSource1.ReportDocument.Dispose();
            GC.Collect();

        }


    }
    protected void CrystalReportViewer1_Init(object sender, EventArgs e)
    {
        crConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings["REPORT_DB_SERVER_NAME"].ToString();
        crConnectionInfo.UserID = "hnba_crc";
        crConnectionInfo.Password = "HNBACRC";


        crDatabase = CrystalReportSource1.ReportDocument.Database;
        crTables = crDatabase.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
        {
            crTableLogOnInfo = crTable.LogOnInfo;
            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
            crTable.ApplyLogOnInfo(crTableLogOnInfo);
        }
        CrystalReportViewer1.ReportSource = CrystalReportSource1;
        //CrystalReportSource1.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Banchmark Summery-Assign");
        CrystalReportViewer1.RefreshReport();


        //CrystalReportSource1.ReportDocument.Close();
        //CrystalReportSource1.ReportDocument.Dispose();
        GC.Collect();
    }
}
