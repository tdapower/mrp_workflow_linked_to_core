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
/// Summary description for ReminderBLL
/// </summary>
public class ReminderBLL
{
    //Reminder Page
    //Register-Search for reminder
    public DataTable SelectJobForReminder(string CaseNo, string PolicyNo, string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        inputParameter.Add("V_CASE_NO", CaseNo);
        inputParameter.Add("V_POL_NO", PolicyNo);
        inputParameter.Add("V_JOB_NO", JobNo);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectJobForReminder, inputParameter);
        return a;

    }

    //Get Pending Reasons Table According to job No
    public DataTable GetPendingTableForReminder(string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_JOB_NO", JobNo);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.SelectPendingTableForJob, inputParameter);
        return a;

    }

    //Update PendingReasonToJobNo Table
    public void UpdatePendingReasonTable(string Type, string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_TYPE", Type);
        inputParameter.Add("V_JOB_NO", JobNo);


        DatabaseUtility.PopulateData(GenericStoredProcedure.UpdatePendingReasonTable, inputParameter, outputParameter, outputParameterValues);
    }

    //Update PendingReasonToJobNo Table-Clear Pendings
    public void UpdatePendings_Clear(string PendingReason, string JobNo)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_PENDING_REASON", PendingReason);
        inputParameter.Add("V_JOB_NO", JobNo);


        DatabaseUtility.PopulateData(GenericStoredProcedure.UpdateClearPendings, inputParameter, outputParameter, outputParameterValues);
    }

    public void EventsLogSMS_EMAIL(string JobNo , string Event)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_JOB_NO", JobNo);
        inputParameter.Add("V_EVENT", Event);


        DatabaseUtility.PopulateData(GenericStoredProcedure.SMS_EMAIL_LOG, inputParameter, outputParameter, outputParameterValues);
    }

   
}
