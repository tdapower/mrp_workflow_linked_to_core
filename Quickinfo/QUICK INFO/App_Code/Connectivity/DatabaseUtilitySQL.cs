//******************************************
// Authur            :Shanika Amarasinghe
// Date              :25/05/2012
// Reviewed By       :
// Description       : Connection Path to the SQL data base
//******************************************

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;


public static class DatabaseUtilitySQL
{
    private static SqlConnection OpenConnection()
    {
        SqlConnection _conn = new SqlConnection();
        _conn.ConnectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
        _conn.Open();
        return _conn;
    }


    private static void closeConnection(SqlConnection _conn)
    {
        if (_conn.State == ConnectionState.Open)
            _conn.Close();
    }


    public static DataTable SelectData(string storedProcedureName, Dictionary<string, object> inputParameter)
    {
        DataSet dataSet = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand command = new SqlCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtilitySQL.OpenConnection();


        adapter.SelectCommand = command;


        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            SqlParameter parameter = new SqlParameter(items.Key, items.Value == null ? DBNull.Value : items.Value);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }
        command.CommandTimeout = 0;

        adapter.Fill(dataSet);
        //SqlDataReader dataReader = command.ExecuteReader();
        DatabaseUtilitySQL.closeConnection(command.Connection);
        return dataSet.Tables[0]; //dataReader;


    }


    public static int PopulateData(string storedProcedureName, Dictionary<string, object> inputParameter, Dictionary<string, object> outputParameter, Dictionary<string, object> outputParameterValues)
    {
        SqlCommand command = new SqlCommand();
        command.CommandText = storedProcedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Connection = DatabaseUtilitySQL.OpenConnection();
        command.CommandTimeout = 0;


        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            SqlParameter parameter = new SqlParameter(items.Key, items.Value);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }


        foreach (KeyValuePair<string, object> items in outputParameter)
        {
            ArrayList parameterItem = (ArrayList)items.Value;
            SqlParameter parameter = new SqlParameter(items.Key, (SqlDbType)parameterItem[0], (int)parameterItem[1]);
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);
        }


        int noOfRecord = command.ExecuteNonQuery();

        DatabaseUtilitySQL.closeConnection(command.Connection);

        foreach (SqlParameter parameter in command.Parameters)
        {
            if (parameter.Direction == ParameterDirection.Output)
                outputParameterValues.Add(parameter.ParameterName, parameter.Value);
        }


        return noOfRecord;
    }

    public static DataTable SelectData(DataTable dataTable, string storedProcedureName, Dictionary<string, object> inputParameter)
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand command = new SqlCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtilitySQL.OpenConnection();
        command.CommandTimeout = 0;
        adapter.SelectCommand = command;


        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            SqlParameter parameter = new SqlParameter(items.Key, items.Value == null ? DBNull.Value : items.Value);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }
        adapter.Fill(dataTable);
        DatabaseUtilitySQL.closeConnection(command.Connection);
        return dataTable;
    }


    public static DataSet SelectDataReturningDataSet(string storedProcedureName, Dictionary<string, object> inputParameter)
    {
        DataSet dataSet = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand command = new SqlCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtilitySQL.OpenConnection();
        command.CommandTimeout = 0;
        adapter.SelectCommand = command;


        foreach (KeyValuePair<string, object> items in inputParameter)
        {
            SqlParameter parameter = new SqlParameter(items.Key, items.Value == null ? DBNull.Value : items.Value);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }
        adapter.Fill(dataSet);
        DatabaseUtilitySQL.closeConnection(command.Connection);
        return dataSet;
    }


    public static DataSet SelectDataReturningDataSetNoInputs(string storedProcedureName)
    {
        DataSet dataSet = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand command = new SqlCommand();


        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedureName;
        command.Connection = DatabaseUtilitySQL.OpenConnection();
        command.CommandTimeout = 0;
        adapter.SelectCommand = command;


        adapter.Fill(dataSet);
        DatabaseUtilitySQL.closeConnection(command.Connection);
        return dataSet;
    }
}

