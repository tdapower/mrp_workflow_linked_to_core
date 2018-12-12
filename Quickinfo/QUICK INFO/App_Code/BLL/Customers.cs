//******************************************
// Authur            :Shanika Amarasinghe
// Date              :25/05/2012
// Reviewed By       :
// Description       : Demo Page
//******************************************
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
/// Summary description for Customers
/// </summary>
public class Customers
{

    //Select
    public DataTable GetCustomer()
    {
       
        DataTable dataTable = DatabaseUtility.SelectData(GenericStoredProcedure.SelectCustomer);

        return dataTable;
    }


    public void CustomerInsert(string CustomerName, string Address)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter = new Dictionary<string, object>();
        Dictionary<string, object> outputParameterValues = new Dictionary<string, object>();


        inputParameter.Add("V_CUSTOMER_ID", CustomerName);
        inputParameter.Add("V_CUSTOMERNAME", Address);

        DatabaseUtility.PopulateData(GenericStoredProcedure.InsertCustomer, inputParameter, outputParameter, outputParameterValues);
    }


    //Update
    public bool CustomerUpdate(string ID, string CustomerName)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();


        inputParameter.Add("V_CUSTOMERID", ID);
        inputParameter.Add("V_CUSTOMERNAME", CustomerName);


        Dictionary<string, object> outputParamater = new Dictionary<string, object>();
        Dictionary<string, object> outputParameters = new Dictionary<string, object>();


        DatabaseUtility.PopulateData(GenericStoredProcedure.UpdateCustomer, inputParameter, outputParamater, outputParameters);
        return true;
    }

    public bool CustomerDelete(string ID)
    {
        Dictionary<string, object> inputParameter = new Dictionary<string, object>();

        inputParameter.Add("V_CUSTOMERID", ID);

        Dictionary<string, object> outputParamater = new Dictionary<string, object>();
        Dictionary<string, object> outputParameter1 = new Dictionary<string, object>();

        DatabaseUtility.PopulateData(GenericStoredProcedure.DeleteCustomer, inputParameter, outputParamater, outputParameter1);
        return true;
    }




}
