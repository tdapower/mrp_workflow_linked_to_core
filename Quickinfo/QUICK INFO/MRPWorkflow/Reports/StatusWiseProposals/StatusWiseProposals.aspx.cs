//******************************************
// Author            :Tharindu Athapattu
// Date              :25/07/2013
// Reviewed By       :
// Description       :Status Wise Proposals Report 
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

public partial class StatusWiseProposals : System.Web.UI.Page
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
            if (Request.Params["reportDesription"] != null)
            {
                if (Request.Params["reportDesription"] != "")
                {
                    reportDesription = Request.Params["reportDesription"].ToString();
                }
            }

            try
            {
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
                CRpm.Name = "reportDesription";
                CRpm.DefaultValue = reportDesription;
                CrystalReportSource1.Report.Parameters.Add(CRpm);


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
          //  CrystalReportSource1.ReportDocument.SetParameterValue("reportDesription", reportDesription);
            CrystalReportViewer1.ReportSource = CrystalReportSource1;


            CrystalDecisions.Web.Parameter CRpm = new CrystalDecisions.Web.Parameter();
            CRpm.Name = "reportDesription";
            CRpm.DefaultValue = reportDesription;
            CrystalReportSource1.Report.Parameters.Add(CRpm);

            CrystalReportViewer1.RefreshReport();


            GC.Collect();
        }
        catch (Exception ee)
        {

        }
    }
}
