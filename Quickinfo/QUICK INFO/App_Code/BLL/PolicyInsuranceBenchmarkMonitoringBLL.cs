using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using System.Web;

/// <summary>
/// Summary description for PolicyInsuranceBenchmarkMonitoringBLL
/// </summary>
public class PolicyInsuranceBenchmarkMonitoringBLL
{
    public DataTable GetAssinedUsers()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectAssighnedUsers);
        return dataTable;
    }

    public DataTable GetApproveUsers()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectApproveUsers);
        return dataTable;
    }

    public DataTable GetBranch()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectBranchesForBenchMarkMonitor);
        return dataTable;
    }


    //Report 1 - Labour Details
    public DataTable Payment_Sheet_Labour(string claimid, Int32 version)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CLAIM_ID", claimid);
        inputParameter.Add("V_VERSION", version);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.PaymnetSheetLabour, inputParameter);
        return a;
    }

    //Search Spare Parts
    public DataTable Search_Spare_Parts(string Make, string Model, string Chassi, string SparePart, string caseno)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_MAKE", Make);
        inputParameter.Add("V_MODEL", Model);
        inputParameter.Add("V_CHASSI", Chassi);
        inputParameter.Add("V_SPARE", SparePart);
        inputParameter.Add("V_CASE_NO", caseno);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SearchSpareParts, inputParameter);
        return a;
    }

    //Report 1 - Deduction Details
    public DataTable Payment_Sheet_Deduction(string claimid, Int32 version)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CLAIM_ID", claimid);
        inputParameter.Add("V_VERSION", version);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.PaymnetSheetDeduction, inputParameter);
        return a;

    }

    //Report 1 - Deduction Details
    public DataTable Delivery_Order_Item(string claimid, string deliveryno)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CLAIM_ID", claimid);
        inputParameter.Add("V_DELIVERY_NO", deliveryno);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.DeliveryItem, inputParameter);
        return a;

    }

    //Report 1 - Replacement Details
    public DataTable Payment_Sheet_Replacement(string claimid, Int32 version)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CLAIM_ID", claimid);
        inputParameter.Add("V_VERSION", version);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.PaymnetSheetReplacement, inputParameter);
        return a;

    }

    //Report 1
    public DataTable SelectReport1(string caseNo, string AssinedUser, string ApprovedUser, DateTime FrmDate, DateTime Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_ASSIGHNEDUSER_CODE", AssinedUser);
        inputParameter.Add("V_APPROVED_CODE", ApprovedUser);
        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport1, inputParameter);
        return a;

    }

    public DataTable SelectReport11(string caseNo, string AssinedUser, DateTime FrmDate, DateTime Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_ASSIGHNEDUSER_CODE", AssinedUser);
        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport11, inputParameter);
        return a;

    }

    public DataTable SelectReportBenchMarkAgeAnalysis(DateTime FrmDate, DateTime Todate,string fastTrack,string userType)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_FASTTRACK", fastTrack);
        inputParameter.Add("V_USER_TYPE", userType);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReportBenchMarkAgeAnalysis, inputParameter);
        return a;

    }

    //Report 7
    public DataTable SelectReport7(string caseNo, string AssinedUser, string ApprovedUser, DateTime FrmDate, DateTime Todate, string Zonal)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_ASSIGHNEDUSER_CODE", AssinedUser);
        inputParameter.Add("V_APPROVED_CODE", ApprovedUser);
        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_ZONAL", Zonal);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport7, inputParameter);
        return a;

    }


    //New Business Document
    public DataTable SelectNewBusinessDocument(Int32 documentID,string policyId)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_DOCID", documentID);
        inputParameter.Add("V_POLICY_ID", policyId);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectNewBusinessDocument, inputParameter);
        return a;
    }

    //Report 12
    public DataTable SelectReport12(string caseNo, string AssinedUser, DateTime FrmDate, DateTime Todate, string Zonal)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_ASSIGHNEDUSER_CODE", AssinedUser);
        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_ZONAL", Zonal);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport12, inputParameter);
        return a;

    }


    //Report 2(Assined Userwise Benchmark summary)
    public DataTable SelectReport2(DateTime FrmDate, DateTime Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport2, inputParameter);
        return a;

    }

    public DataTable SelectReport2FastTrack(DateTime FrmDate, DateTime Todate,string fastTrack)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_FASTTRACK", fastTrack);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport2FastTrack, inputParameter);
        return a;

    }




    //Report 3(Assined Userwise Benchmark summary - Zonal Wise)
    public DataTable SelectReport6(DateTime FrmDate, DateTime Todate, string Zonal)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_ZONAL", Zonal);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport6, inputParameter);
        return a;

    }

    //Report 3(Assined Userwise Benchmark summary - Zonal Wise)
    public DataTable SelectReport6FastTrack(DateTime FrmDate, DateTime Todate, string Zonal,string fastTrack)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_ZONAL", Zonal);
        inputParameter.Add("V_FASTTRACK", fastTrack);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport6FastTrack, inputParameter);
        return a;

    }

    //Report 4(Approved Userwise Benchmark summary)
    public DataTable SelectReport4(DateTime FrmDate, DateTime Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport4, inputParameter);
        return a;

    }

    //Report 4(Approved Userwise Benchmark summary)
    public DataTable SelectReport4FastTrack(DateTime FrmDate, DateTime Todate,string fastTrack)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_FASTTRACK", fastTrack);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport4FastTrack, inputParameter);
        return a;

    }

    //Report 4(Approved Userwise Benchmark summary)
    public DataTable SelectReport10(DateTime FrmDate, DateTime Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport10, inputParameter);
        return a;

    }

    //Report 5(Approved Userwise Benchmark summary - Zonal Wise)
    public DataTable SelectReport5(DateTime FrmDate, DateTime Todate, string Zonal)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_ZONAL", Zonal);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport5, inputParameter);
        return a;

    }


    //Report 5(Approved Userwise Benchmark summary - Zonal Wise)
    public DataTable SelectReport5FastTrack(DateTime FrmDate, DateTime Todate, string Zonal,string fastTrack)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_ZONAL", Zonal);
        inputParameter.Add("V_FASTTRACK", fastTrack);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport5FastTrack, inputParameter);
        return a;

    }

    //Report 3
    public DataTable SelectReport3(string caseNo, string PolicyNo, string ProposalNo, string Branch)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_POLICY_NO", PolicyNo);
        inputParameter.Add("V_PROPOSAL_NO", ProposalNo);
        inputParameter.Add("V_BRANCH", Branch);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport3, inputParameter);
        return a;

    }


    public void DeleteReport()
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        DatabaseUtility.PopulateData(GenericStoredProcedure.DeleteReports, inputParameter, outputParameter, outputParameterValues);
    }


    //Pendings Recieved Report
    public DataTable PendingsRecieved(DateTime FrmDate, DateTime Todate, string reportcode, string Zonal)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_REPORT", reportcode.ToString());
        inputParameter.Add("V_ZONAL", Zonal.ToString());

        DataTable a;
        a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.PendingsRecievedReport, inputParameter);
        return a;

    }

    //Pendings Proposal Report
    public DataTable PendingsProposal(DateTime FrmDate, DateTime Todate, string reportcode, string Zonal)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_REPORT", reportcode.ToString());
        inputParameter.Add("V_ZONAL", Zonal.ToString());

        DataTable a;
        a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.PendingsProposalReport, inputParameter);
        return a;

    }

    //Refund Proposal Report
    public DataTable RefundProposal(DateTime FrmDate, DateTime Todate, string reportcode, string Zonal)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_REPORT", reportcode.ToString());
        inputParameter.Add("V_ZONAL", Zonal.ToString());


        DataTable a;
        a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.RefundProposalReport, inputParameter);
        return a;

    }


    public void DeleteReportPendings()
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        DatabaseUtility.PopulateData(GenericStoredProcedure.DeleteReportPendingsRecieved, inputParameter, outputParameter, outputParameterValues);
    }

    //Proposal Details Report
    public DataTable ProposalDetails(string caseNo, string branch, string status, DateTime FrmDate, DateTime Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_BRANCH", branch);
        inputParameter.Add("V_STATUS", status);
        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ProposalDetailsReport, inputParameter);
        return a;

    }

    public void DeleteReportProposalDetails()
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        DatabaseUtility.PopulateData(GenericStoredProcedure.DeleteReportProposalDetails, inputParameter, outputParameter, outputParameterValues);
    }

    public DataTable GatProposalStatus()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectProposalStatus);
        return dataTable;
    }

    //MRP WF Benchmark Report
    public DataTable SelectMRPWFBenchmarkReport(string caseNo, string AssinedUser, string ApprovedUser, string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_ASSIGNED_USER", AssinedUser);
        inputParameter.Add("V_APPROVED_USER", ApprovedUser);
        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));

        inputParameter.Add("V_WORKFLOW_TYPE", HttpContext.Current.Request.Cookies["WORKFLOW_CHOICE"].Value.ToString());


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkReport, inputParameter);
        return a;

    }


    //MRP WF Benchmark Summary Report
    public DataTable SelectMRPWFBenchmarkSummaryReport(string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReport, inputParameter);
        return a;

    }


    //MRP WF Benchmark Summary Report 2
    public DataTable SelectMRPWFBenchmarkSummaryReport4(string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReport4, inputParameter);
        return a;

    }


    //MRP WF Benchmark Summary Report - Approved Users
    public DataTable SelectMRPWFBenchmarkSummaryReport2(string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReport2, inputParameter);
        return a;

    }

    //MRP WF Benchmark Summary Report 3 - Approved Users
    public DataTable SelectMRPWFBenchmarkSummaryReport3(string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReport3, inputParameter);
        return a;

    }

    //MRP System Quotation Summary Report
    public DataTable SelectMRPSQuotationSummaryReport(string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPSQuotationSummaryReport, inputParameter);
        return a;

    }

    // MRP WF Status Wise Proposals Report
    public DataTable SelectMRPWFStatusWiseProposalsReport(string caseNo, string AssinedUser, string ApprovedUser, string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_CASE", caseNo);
        inputParameter.Add("V_ASSIGNED_USER", AssinedUser);
        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));

        inputParameter.Add("V_WORKFLOW_TYPE", HttpContext.Current.Request.Cookies["WORKFLOW_CHOICE"].Value.ToString());

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFStatusWiseProposalsReport, inputParameter);
        return a;

    }

    //Report 5(Approved Userwise Benchmark summary)
    public DataTable SelectReport15(DateTime FrmDate, DateTime Todate,string Report)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_REPORT", Report);



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectReport15, inputParameter);
        return a;

    }

    //MRP WF Benchmark Summary Reports
    public DataTable SelectMRPWFBenchmarkSummaryReports(string ReporrtType, string FrmDate, string Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", Convert.ToDateTime(FrmDate));
        inputParameter.Add("V_ENDDATE", Convert.ToDateTime(Todate));
        inputParameter.Add("V_WORKFLOW_TYPE", HttpContext.Current.Request.Cookies["WORKFLOW_CHOICE"].Value.ToString());

        DataTable dt=null;

        switch (ReporrtType)
        {
            case "PendingLetters":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReportPendingLetters, inputParameter);
                break;
            case "PendingLettersSupervisor":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReportPendingLetters2, inputParameter);
                break;
            case "CoverNote":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReportCoverNote, inputParameter);
                break;
            case "CoverNoteSupervisor":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReportCoverNote2, inputParameter);
                break;
            case "PolicyIssue":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReportPolicyIssue, inputParameter);
                break;
            case "PolicyIssueSupervisor":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSummaryReportPolicyIssue2, inputParameter);
                break;
            case "PendingComplete":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkSPendingComplete, inputParameter);
                break;
            case "FSTBenchMark":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFFSTBenchmarkSummaryReport, inputParameter);
                break;
            case "FSTBenchMarkDaily":
                dt = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFFSTBenchmarkDailyReport, inputParameter);
                break;

        }





        return dt;

    }


    //MRPWFBenchmark Job Allocation Summary
    public DataTable SelectMRPWFBenchmarkJobAlloSummary(DateTime FrmDate, DateTime Todate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", FrmDate);
        inputParameter.Add("V_ENDDATE", Todate);
        inputParameter.Add("V_WORKFLOW_TYPE", HttpContext.Current.Request.Cookies["WORKFLOW_CHOICE"].Value.ToString());



        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectMRPWFBenchmarkJobAlloSummary, inputParameter);
        return a;

    }

}
