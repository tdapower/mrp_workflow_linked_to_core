//******************************************
// Author            :Tharindu Athapattu
// Date              :19/06/2013
// Reviewed By       :
// Description       :MRP WF Benchmark Monitoring Report 1
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.OracleClient;

public partial class MRPWFBenchmarkMonitoringRPT1 : System.Web.UI.Page
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
            try
            {
                if (Request.Params["reportDescription"] != null)
                {
                    if (Request.Params["reportDescription"] != "")
                    {
                        reportDescription = Request.Params["reportDescription"].ToString();
                    }
                }

                 //crConnectionInfo.ServerName = "RACPROD";
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
            }
            catch (Exception ee)
            {

            }
        }

    }
    protected void CrystalReportViewer1_Init(object sender, EventArgs e)
    {
        try
        {
            // crConnectionInfo.ServerName = "RACPROD";
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
            CrystalReportViewer1.RefreshReport();
        }
        catch (Exception ee)
        {

        }
    }
}
