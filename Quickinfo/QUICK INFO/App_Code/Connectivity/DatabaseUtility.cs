//******************************************
// Authur            :Shanika Amarasinghe
// Date              :13/06/2012
// Reviewed By       :
// Description       : Connection Path to the data base
//******************************************

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
//using System.Data.OracleClient;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;


public static class DatabaseUtility
{
    private static OracleConnection OpenConnection()
    {
        OracleConnection _conn = new OracleConnection();
        
        _conn.ConnectionString = ConfigurationManager.ConnectionStrings["CGConnectionString"].ConnectionString;
        _conn.Open();
        return _conn;
    }


    private static void closeConnection(OracleConnection _conn)
    {
        if (_conn.State == ConnectionState.Open)
            _conn.Close();
    }

    //Fill Data With No Inputs
    public static DataTable SelectData(string storedProcedureName)
    {
        DataSet dataSet = new DataSet();
        OracleDataAdapter adapter = new OracleDataAdapter();
        OracleCommand command = new OracleCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtility.OpenConnection();


        adapter.SelectCommand = command;


        OracleParameter parameter = new OracleParameter();
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        command.Parameters.Add(parameter);


        command.CommandTimeout = 0;

        adapter.Fill(dataSet);
        //SqlDataReader dataReader = command.ExecuteReader();
        DatabaseUtility.closeConnection(command.Connection);
        return dataSet.Tables[0]; //dataReader;


    }


    public static DataTable SelectDataWithInputParameters(string storedProcedureName, Dictionary<string, object> inputParameter)
    {
        DataSet dataSet = new DataSet();
        OracleDataAdapter adapter = new OracleDataAdapter();
        OracleCommand command = new OracleCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtility.OpenConnection();


        adapter.SelectCommand = command;

        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            OracleParameter parameter = new OracleParameter(items.Key, items.Value == null ? DBNull.Value : items.Value);
            //  parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

        }
        OracleParameter parameter1 = new OracleParameter();
        parameter1.OracleDbType = OracleDbType.RefCursor;
        parameter1.Direction = ParameterDirection.Output;
        command.Parameters.Add(parameter1);


        command.CommandTimeout = 0;

        adapter.Fill(dataSet);
        //SqlDataReader dataReader = command.ExecuteReader();
        DatabaseUtility.closeConnection(command.Connection);
        return dataSet.Tables[0]; //dataReader;

    }


    public static int PopulateData(string storedProcedureName, Dictionary<string, object> inputParameter, Dictionary<string, object> outputParameter, Dictionary<string, object> outputParameterValues)
    {
        OracleCommand command = new OracleCommand();
        command.CommandText = storedProcedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Connection = DatabaseUtility.OpenConnection();
        command.CommandTimeout = 0;



        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            OracleParameter parameter = new OracleParameter(items.Key, items.Value);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }


        foreach (KeyValuePair<string, object> items in outputParameter)
        {
            ArrayList parameterItem = (ArrayList)items.Value;
            OracleParameter parameter = new OracleParameter(items.Key, (OracleDbType)parameterItem[0], (int)parameterItem[1]);
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);
        }


        int noOfRecord = command.ExecuteNonQuery();

        DatabaseUtility.closeConnection(command.Connection);

        foreach (OracleParameter parameter in command.Parameters)
        {
            if (parameter.Direction == ParameterDirection.Output)
                outputParameterValues.Add(parameter.ParameterName, parameter.Value);
        }


        return noOfRecord;
    }

    public static DataTable SelectData(DataTable dataTable, string storedProcedureName, Dictionary<string, object> inputParameter)
    {
        OracleDataAdapter adapter = new OracleDataAdapter();
        OracleCommand command = new OracleCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtility.OpenConnection();
        command.CommandTimeout = 0;
        adapter.SelectCommand = command;


        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            OracleParameter parameter = new OracleParameter(items.Key, items.Value == null ? DBNull.Value : items.Value);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }
        adapter.Fill(dataTable);
        DatabaseUtility.closeConnection(command.Connection);
        return dataTable;
    }


    public static DataSet SelectDataReturningDataSet(string storedProcedureName, Dictionary<string, object> inputParameter)
    {
        DataSet dataSet = new DataSet();
        OracleDataAdapter adapter = new OracleDataAdapter();
        OracleCommand command = new OracleCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtility.OpenConnection();
        command.CommandTimeout = 0;
        adapter.SelectCommand = command;


        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            SqlParameter parameter = new SqlParameter(items.Key, items.Value == null ? DBNull.Value : items.Value);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }
        adapter.Fill(dataSet);
        DatabaseUtility.closeConnection(command.Connection);
        return dataSet;
    }


    public static DataSet SelectDataReturningDataSetNoInputs(string storedProcedureName)
    {
        DataSet dataSet = new DataSet();
        OracleDataAdapter adapter = new OracleDataAdapter();
        OracleCommand command = new OracleCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtility.OpenConnection();
        command.CommandTimeout = 0;
        adapter.SelectCommand = command;


        adapter.Fill(dataSet);
        DatabaseUtility.closeConnection(command.Connection);
        return dataSet;
    }





}