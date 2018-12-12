//******************************************
// Author            :Tharindu Athapattu
// Date              :28/06/2013
// Reviewed By       :
// Description       :Received Pendings Report
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

public partial class ReceivedPendings : System.Web.UI.Page
{
    TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
    ConnectionInfo crConnectionInfo = new ConnectionInfo();
    CrystalDecisions.CrystalReports.Engine.Database crDatabase;
    CrystalDecisions.CrystalReports.Engine.Tables crTables;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //try
            //{
            //    // crConnectionInfo.ServerName = "RACPROD";
            //    crConnectionInfo.ServerName = "HNBUAT";
            //    crConnectionInfo.UserID = "hnba_crc";
            //    crConnectionInfo.Password = "HNBACRC";

            //    crDatabase = CrystalReportSource1.ReportDocument.Database;

            //    CrystalReportSource1.ReportDocument.SetParameterValue("LifeAssure1", 1);
            //    CrystalReportSource1.ReportDocument.SetParameterValue("LifeAssure2", 2);

            //    crTables = crDatabase.Tables;
            //    foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            //    {
            //        crTableLogOnInfo = crTable.LogOnInfo;
            //        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
            //        crTable.ApplyLogOnInfo(crTableLogOnInfo);
            //    }

            //  CrystalReportViewer1.report
            //    CrystalReportViewer1.ReportSource = CrystalReportSource1;
            //    CrystalReportViewer1.RefreshReport();
            //}
            //catch (Exception ee)
            //{

            //}




            //ReportDocument crystalReport = new ReportDocument();
            //crystalReport.Load(Server.MapPath("RECEIVED_PENDINGS.rpt"));
            ////crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
            //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");

            //CrystalReportViewer1.ReportSource = crystalReport;

            //crystalReport.SetParameterValue("fromDate", Request.Params["fromDateTime"]);
            //crystalReport.SetParameterValue("toDate", Request.Params["toDateTime"]);
            //crystalReport.SetParameterValue("assignedUser", Request.Params["AssignedUser"]);
            
            //crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PROPOSAL CANCELLATION LETTER");



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

                CrystalDecisions.Web.Parameter CRpm3 = new CrystalDecisions.Web.Parameter();
                CRpm3.Name = "assignedUser";
                CRpm3.DefaultValue = Request.Params["AssignedUser"];
                CrystalReportSource1.Report.Parameters.Add(CRpm3);

                CrystalDecisions.Web.Parameter CRpm4 = new CrystalDecisions.Web.Parameter();
                CRpm4.Name = "pendClearedUser";
                CRpm4.DefaultValue = Request.Params["PendClearedUser"];
                CrystalReportSource1.Report.Parameters.Add(CRpm4);

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
        //try
        //{
        //    // crConnectionInfo.ServerName = "RACPROD";
        //    crConnectionInfo.ServerName = "HNBUAT";
        //    crConnectionInfo.UserID = "hnba_crc";
        //    crConnectionInfo.Password = "HNBACRC";


        //    crDatabase = CrystalReportSource1.ReportDocument.Database;

        //    CrystalReportSource1.ReportDocument.SetParameterValue("LifeAssure1", 1);
        //    CrystalReportSource1.ReportDocument.SetParameterValue("LifeAssure2", 2);

        //    crTables = crDatabase.Tables;
        //    foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
        //    {
        //        crTableLogOnInfo = crTable.LogOnInfo;
        //        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
        //        crTable.ApplyLogOnInfo(crTableLogOnInfo);
        //    }

        //    CrystalReportViewer1.ReportSource = CrystalReportSource1;

        //    CrystalReportViewer1.RefreshReport();
        //}
        //catch (Exception ee)
        //{

        //}



      //  ReportDocument crystalReport = new ReportDocument();
      //  crystalReport.Load(Server.MapPath("RECEIVED_PENDINGS.rpt"));
      //  //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
      //  crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");


      //  CrystalReportViewer1.ReportSource = crystalReport;

      //  crystalReport.SetParameterValue("fromDate", Request.Params["fromDateTime"]);
      //  crystalReport.SetParameterValue("toDate", Request.Params["toDateTime"]);
      //  crystalReport.SetParameterValue("assignedUser", Request.Params["AssignedUser"]);


      //crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PROPOSAL CANCELLATION LETTER");

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

            CrystalDecisions.Web.Parameter CRpm3 = new CrystalDecisions.Web.Parameter();
            CRpm3.Name = "assignedUser";
            CRpm3.DefaultValue = Request.Params["AssignedUser"];
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
