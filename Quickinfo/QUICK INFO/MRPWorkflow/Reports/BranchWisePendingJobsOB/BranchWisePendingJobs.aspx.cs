//******************************************
// Author            :Tharindu Athapattu
// Date              :35/03/2014
// Reviewed By       :
// Description       :Branch Wise Pending Jobs Report 
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

public partial class BranchWisePendingJobs : System.Web.UI.Page
{
    TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
    ConnectionInfo crConnectionInfo = new ConnectionInfo();
    CrystalDecisions.CrystalReports.Engine.Database crDatabase;
    CrystalDecisions.CrystalReports.Engine.Tables crTables;
    string reportDesription;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["reportDescription"] != null)
            {
                if (Request.Params["reportDescription"] != "")
                {
                    reportDesription = Request.Params["reportDescription"].ToString();
                }
            }

            try
            {
                crConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings["REPORT_DB_SERVER_NAME"].ToString();
                //crConnectionInfo.ServerName = "HNBUAT";
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
                CrystalDecisions.Web.Parameter CRpm1 = new CrystalDecisions.Web.Parameter();
                CRpm1.Name = "fromDate";
                CRpm1.DefaultValue = Request.Params["fromDateTime"];
                CrystalReportSource1.Report.Parameters.Add(CRpm1);

                CrystalDecisions.Web.Parameter CRpm2 = new CrystalDecisions.Web.Parameter();
                CRpm2.Name = "toDate";
                CRpm2.DefaultValue = Request.Params["toDateTime"];
                CrystalReportSource1.Report.Parameters.Add(CRpm2);

                CrystalDecisions.Web.Parameter CRpm = new CrystalDecisions.Web.Parameter();
                CRpm.Name = "reportDescription";
                CRpm.DefaultValue = reportDesription;
                CrystalReportSource1.Report.Parameters.Add(CRpm);

                CrystalDecisions.Web.Parameter CRpm3 = new CrystalDecisions.Web.Parameter();
                CRpm3.Name = "wfType";
                CRpm3.DefaultValue = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();
                CrystalReportSource1.Report.Parameters.Add(CRpm3);


                CrystalReportViewer1.ReportSource = CrystalReportSource1;

                CrystalReportViewer1.RefreshReport();

                GC.Collect();
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
            crConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings["REPORT_DB_SERVER_NAME"].ToString();
            //crConnectionInfo.ServerName = "HNBUAT";
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
          //  CrystalReportSource1.ReportDocument.SetParameterValue("reportDesription", reportDesription);
            CrystalReportViewer1.ReportSource = CrystalReportSource1;


            CrystalDecisions.Web.Parameter CRpm = new CrystalDecisions.Web.Parameter();
            CRpm.Name = "reportDescription";
            CRpm.DefaultValue = reportDesription;
            CrystalReportSource1.Report.Parameters.Add(CRpm);

            CrystalDecisions.Web.Parameter CRpm3 = new CrystalDecisions.Web.Parameter();
            CRpm3.Name = "wfType";
            CRpm3.DefaultValue = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();
            CrystalReportSource1.Report.Parameters.Add(CRpm3);

            CrystalReportViewer1.RefreshReport();
        }
        catch (Exception ee)
        {

        }
    }
}
