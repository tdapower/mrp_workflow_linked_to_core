//******************************************
// Author            :Tharindu Athapattu
// Date              :29/01/2013
// Reviewed By       :
// Description       :Re Insurance Confirmation Report 
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

public partial class ReInsuranceConfirmationReport : System.Web.UI.Page
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
                CRpm.DefaultValue = reportDescription;
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
          //  CrystalReportSource1.ReportDocument.SetParameterValue("reportDescription", reportDescription);
            CrystalReportViewer1.ReportSource = CrystalReportSource1;


            CrystalDecisions.Web.Parameter CRpm = new CrystalDecisions.Web.Parameter();
            CRpm.Name = "reportDescription";
            CRpm.DefaultValue = reportDescription;
            CrystalReportSource1.Report.Parameters.Add(CRpm);


            CrystalDecisions.Web.Parameter CRpm3 = new CrystalDecisions.Web.Parameter();
            CRpm3.Name = "wfType";
            CRpm3.DefaultValue = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();
            CrystalReportSource1.Report.Parameters.Add(CRpm3);
           

            CrystalReportViewer1.RefreshReport();


            GC.Collect();
        }
        catch (Exception ee)
        {

        }
    }
}
