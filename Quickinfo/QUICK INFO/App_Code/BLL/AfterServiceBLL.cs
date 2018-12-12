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
/// Summary description for AfterServiceBLL
/// </summary>
public class AfterServiceBLL
{
    //PHS Register-Search(PL/SQL)
    public DataTable SelectPolicy (string CaseNo, string PolicyNo,string ProNo, string CusName)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        inputParameter.Add("V_CASE_NO", CaseNo);
        inputParameter.Add("V_POL_NO", PolicyNo);
        inputParameter.Add("V_PRO_NO", ProNo);
        inputParameter.Add("V_CUS_NAME", CusName);
        
        DataTable a = DatabaseUtilityForSearch.SelectDataWithInputParameters(GenericStoredProcedure.SelectPolicy, inputParameter);
        return a;

    }
    //Search(PL/SQL) After Loading to the grid view
    public DataTable SelectPolicyByPolicyNo(string PolicyNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_POL_NO", PolicyNo);


        DataTable a = DatabaseUtilityForSearch.SelectDataWithInputParameters(GenericStoredProcedure.SelectPolicyByPolicyNumber, inputParameter);
        return a;

    }


    //MRP Register-Search(SQL)

    public DataTable SelectPolicyMRP(string CaseNo, string PolicyNo, string ProNo, string CusName)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        inputParameter.Add("@CASE_NO", CaseNo);
        inputParameter.Add("@POL_NO", PolicyNo);
        inputParameter.Add("@PRO_NO", ProNo);
        inputParameter.Add("@CUS_NAME", CusName);

        DataTable a = DatabaseUtilitySQL.SelectData(GenericStoredProcedure.SelectPolicyFromMRP, inputParameter);
        return a;

    }

    public DataTable SelectPolicyByPolicyNoMRP(string PolicyNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        inputParameter.Add("@POL_NO", PolicyNo);
        DataTable a = DatabaseUtilitySQL.SelectData(GenericStoredProcedure.SelectPolicyByPolicyNumberMRP, inputParameter);
        return a;

    }

    //Referance Data 
    //JobType Referance Table
    public DataTable GetJobType()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectJobType);
        return dataTable;
    }

    //Branch Referance Table
    public DataTable GetBranch()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectBranch);
        return dataTable;
    }
    //Status Referance Table
    public DataTable GetStatus()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectStatus);
        return dataTable;
    }
    //IF Passed Referance Table
    public DataTable GetIFPassed()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectIFPassed);
        return dataTable;
    }
    //Pending Reason Referance Table
    public DataTable GetPendingReason()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectPendingReason);
        return dataTable;
    }
    //Users Referance Table
    public DataTable GetUsers()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectUsers);
        return dataTable;
    }
    //Pending Category Referance Table
    public DataTable GetPendingCat()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectPendingCategory);
        return dataTable;
    }
    //User Log
    public DataTable GetUserLog()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectUserLog);
        return dataTable;
    }
    //Insert Register
    public void RegisterInsert(string PolicyNo, string JobType, string Mobile, string Branch, string DocPath, string RegType, string Cusname, string RequestUser, string RequestMail, string JobWiseDocs)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_POLICY_NO", PolicyNo);
        inputParameter.Add("V_JOBTYPE", JobType);
        inputParameter.Add("V_MOBILENO", Mobile);
        inputParameter.Add("V_BRANCH", Branch);
        inputParameter.Add("V_DOCUMENT", DocPath);
        inputParameter.Add("V_REGISTER_TYPE", RegType);
        inputParameter.Add("V_CUSNAME", Cusname);
        inputParameter.Add("V_REQUEST_USER", RequestUser);
        inputParameter.Add("V_REQUESTOREMAIL", RequestMail);
        inputParameter.Add("V_PENDINGDOCS", JobWiseDocs);

        DatabaseUtility.PopulateData(GenericStoredProcedure.RegisterInsert, inputParameter, outputParameter, outputParameterValues);
    }


    //Get Job No
    public DataTable GetJobNo()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.GetJobNoAtInsert);
        return dataTable;
    }

    //GetJob-After Insert
    public DataTable SelectJobAfterInsert(string CaseNo, string JobNo, string PolNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        inputParameter.Add("V_CASE_NO", CaseNo);
        inputParameter.Add("V_JOB_NO", JobNo);
        inputParameter.Add("V_POL_NO", PolNo);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectJobAfterInsert, inputParameter);
        return a;

    }
    //Select Job By Job No
    public DataTable SelectJobByJobNo(string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectJobByJobNo, inputParameter);
        return a;

    }


    //Update Register
    public void RegisterUpdate(string JobNo, string Status, string IfPassed,  string Remarks, string Document)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);
        inputParameter.Add("V_STATUS", Status);
        inputParameter.Add("V_IFPASSED", IfPassed);
        //inputParameter.Add("V_PENDINGREASON", PendingReason);
        inputParameter.Add("V_REMARKS", Remarks);
        inputParameter.Add("V_DOCUMENT", Document);

        DatabaseUtility.PopulateData(GenericStoredProcedure.UpdateRegister, inputParameter, outputParameter, outputParameterValues);
    }
    //Update Assighn User Email
    public void UpdateAssighnUserMail(string JobNo, string Email)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);
        inputParameter.Add("V_USER_EMAIL", Email);

        DatabaseUtility.PopulateData(GenericStoredProcedure.UpdateRegisterAssignUserMail, inputParameter, outputParameter, outputParameterValues);
    }

    //Get Documents
    public DataTable GetDocs(string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_JOB_NO", JobNo);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.GetDocuments, inputParameter);
        return a;

    }
    //Insert Pending Reason Table
    public void InsertPendingReason(string JobNo,string PendingReason,string status,string type)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);
        inputParameter.Add("V_PENDINGREASON", PendingReason);
        inputParameter.Add("V_STATUS", status);
        inputParameter.Add("V_TYPE", type);



        DatabaseUtility.PopulateData(GenericStoredProcedure.InsertPendings, inputParameter, outputParameter, outputParameterValues);
    }

    //Get Pending Reasons from table
    public DataTable GetPendings(string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectPendings, inputParameter);
        return a;

    }
    public DataTable GetOtherPendings(string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectOtherPendings, inputParameter);
        return a;

    }

    //Referance Page
    //JobType
    public void PopulateJobType(string Description, string Type,string ID)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOBDESCRIPTION", Description);
        inputParameter.Add("V_TYPE", Type);
        inputParameter.Add("V_JOBTYPEID", ID);

        DatabaseUtility.PopulateData(GenericStoredProcedure.PopulateJobTypeRef, inputParameter, outputParameter, outputParameterValues);
    }
    //IF Passed
    public void PopulateIFPassed(string Description, string Type,string ID)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_DESCRIPTION", Description);
        inputParameter.Add("V_TYPE", Type);
        inputParameter.Add("V_ID", ID);

        DatabaseUtility.PopulateData(GenericStoredProcedure.PopulateIFPassedRef, inputParameter, outputParameter, outputParameterValues);
    }
    //Pending Reason
    public void PopulatePendingReason(string Description, string Type,string ID)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_DESCRIPTION", Description);
        inputParameter.Add("V_TYPE", Type);
        inputParameter.Add("V_PENDIND_REASONID", ID);

        DatabaseUtility.PopulateData(GenericStoredProcedure.PopulatePendingReasonRef, inputParameter, outputParameter, outputParameterValues);
    }
    //Users
    public void PopulateUsers(string EPFNO,  string StaffName,string Username,string Type)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_EPF", EPFNO);
        inputParameter.Add("V_STAFFMEMBER", StaffName);
        inputParameter.Add("V_USERNEME", Username);
        inputParameter.Add("V_TYPE", Type);


        DatabaseUtility.PopulateData(GenericStoredProcedure.PopulateUsers, inputParameter, outputParameter, outputParameterValues);
    }
    //----END-Referance Page

    //Fill Register For Allocation
    public DataTable GetRegisterForAllocation()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.GetJobsForAllocation);
        return dataTable;
    }
    //Update Register Table-Allocated User
    public void RegisterUserUpdate(string JobNo, string User)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);
        inputParameter.Add("V_USER", User);

        DatabaseUtility.PopulateData(GenericStoredProcedure.UpdateRegisterUserAllocation, inputParameter, outputParameter, outputParameterValues);
    }

    //Get Email Address - Branch
    public DataTable GetMailAddress(string Code)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        inputParameter.Add("V_CODE", Code);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.GetMailAddress, inputParameter);
        return a;

    }

    //Get Email Address -Requestor
    public DataTable GetMailAddressUser(string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        inputParameter.Add("V_JOB_NO", JobNo);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.GetMailAddressUser, inputParameter);
        return a;

    }

    //Covering Letter-Report
    public DataTable GetDailyCoveringLetter(string Branch, string UserName)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_BRANCH", Branch);
        inputParameter.Add("V_USER_NAME", UserName);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.CoveringLetter, inputParameter);
        return a;

    }
    public void DeleteRecordsLetter()
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();



        DatabaseUtility.PopulateData(GenericStoredProcedure.DeleterCoveringLetterRecords, inputParameter, outputParameter, outputParameterValues);
    }
    //Select Documents Job Wise
    public DataTable GetJobTypeDocuments(string JobType)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_TYPE", JobType);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectPendingDocumentsJobWise, inputParameter);
        return a;

    }


    //Upload Other Documents at insert
    public void OtherDocumentsUpload(string JobNo,string Document)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);
        inputParameter.Add("V_DOCUMENT", Document);

        DatabaseUtility.PopulateData(GenericStoredProcedure.InsertOtherDocuments, inputParameter, outputParameter, outputParameterValues);
    }

    public DataTable SelectCompleteDate(string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOBNO", JobNo);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.JobCompleteDateForSMS, inputParameter);
        return a;

    }

    public DataTable GetUsers_Levels()
    {
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectUsersLevels);
        return dataTable;
    }

}
