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
/// Summary description for DashBoardBLL
/// </summary>
public class DashBoardBLL
{    //View Register in DashBoard-Not Updated
    public DataTable DashBoard_NotUpdated(string User)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER", User);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewRegister_DashBoard, inputParameter);
        return a;

    }
    //View Pendings in DashBoard-Exceeded
    public DataTable DashBoard_CuttOffExceeded(string User,DateTime Date)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER", User);
        inputParameter.Add("V_DATE", Date);


        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewCutOffExceeded, inputParameter);
        return a;

    }

    //View First reminders due today
    public DataTable DashBoard_ViewFirstReminder(string User)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER", User);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewFirstReminder, inputParameter);
        return a;

    }

    //View Second reminders due today
    public DataTable DashBoard_ViewSecReminder(string User)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();

        inputParameter.Add("V_USER", User);

        DataTable a = DatabaseUtility.SelectDataWithInputParameters(GenericStoredProcedure.ViewSecReminder, inputParameter);
        return a;

    }

    public void UserLog(string User)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_USER", User);

        DatabaseUtility.PopulateData(GenericStoredProcedure.UserLog, inputParameter, outputParameter, outputParameterValues);
    }

}
