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

/// <summary>
/// Summary description for Reports
/// </summary>
public class ReportsBLL
{
    //Reports Table
    public DataTable GetReports()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.ViewReports);
        return dataTable;
    }

    //Load Data to reports
    //Status Underwriting Level-Report1
    public DataTable SelectReportStatusUL(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportStatusUnderwritingLevel, inputParameter);
        return a;

    }
    //Age Analysis of jobs-Report2
    public DataTable SelectReportCurrentStatusSL(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportStatusSupervisorLevel, inputParameter);
        return a;

    }

    //Age Analysis of jobs-Report3
    public DataTable SelectReportAgeAnalysisJobs(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportAgeAnalysis_NotAttendedjobs, inputParameter);
        return a;

    }
    //Total Jobs-Report4
    public DataTable SelectReportTotalJobs(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportTotalJobs, inputParameter);
        return a;

    }
    //Total Jobs-Report5
    public DataTable SelectPendingJobsForBranch(string Branch)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_BRANCH", Branch);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportPendingJobsBranchWise, inputParameter);
        return a;

    }

    //Total Jobs-Report6
    public DataTable SelectReportAgeAnalysisPendings(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportAgeAnalysis_PendingJobs, inputParameter);
        return a;

    }

    //Total Jobs-Report7
    public DataTable SelectReportJobWiseAnalysis(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportJobWiseAnalysis, inputParameter);
        return a;

    }

    //Total Jobs-Report8
    public DataTable SelectReportBenchMarkAnalysis(string User_code, DateTime startdate,DateTime enddate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);
        inputParameter.Add("V_STARTDATE", startdate);
        inputParameter.Add("V_ENDDATE", enddate);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportBenchmarkAnalysis, inputParameter);
        return a;

    }

    public DataTable CurrentStatusJobWise(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER", User_code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewReportCurrentStatusSupervisoryLevelJobWise, inputParameter);
        return a;

    }

    //Delete Report Records
    public void DeleteRecords(string User_code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER_CODE", User_code);

        DatabaseUtility.PopulateData(GenericStoredProcedure.DeleteReportRecords, inputParameter, outputParameter, outputParameterValues);
    }

    //Delete Report Records-Pending Jobs Branch Wise
    public void DeleteRecordsPendingsToBranch()
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        DatabaseUtility.PopulateData(GenericStoredProcedure.DeleteReportRecordsPendingJobs, inputParameter, outputParameter, outputParameterValues);
    }

    //public DataTable ViewTotalRegister()
    //{
    //    DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.ViewFullRegister);
    //    return dataTable;
    //}


    public DataTable ViewTotalRegister(DateTime startdate, DateTime enddate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", startdate);
        inputParameter.Add("V_ENDDATE", enddate);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewFullRegister, inputParameter);
        return a;

    }
    public DataTable ViewTotalPendings(DateTime startdate, DateTime enddate)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_STARTDATE", startdate);
        inputParameter.Add("V_ENDDATE", enddate);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewFullPendings, inputParameter);
        return a;

    }
}
