using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Data.OracleClient;
/// <summary>
/// Summary description for GenerateNumbers
/// </summary>
public class GenerateNumbers
{
	public GenerateNumbers()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public  string GetNewNoPrefixYearValue(string sPrefix, int digits,string value)
    {
        string newNo = "";
        string zero = "0000000000000000000000000000";
        int length = value.Length;
        int diff = digits - length;
        string text = zero.Substring(1, diff);
        text = text + value;

        newNo = sPrefix + "/" + GetServerYear() + "/" + text;

        return newNo;
    }


    public string GetServerYear()
    {
        string year = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
        con.Open();

        OracleCommand cmd = new OracleCommand();
        OracleDataReader dr;

        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        cmd.CommandText = "SELECT to_char(SYSDATE,'YYYY') FROM DUAL";

        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                year = dr[0].ToString();
            }
        }
        return year;
    }



    //public static string FixZero(string str, int digits)
    //{
    //    string zero = "0000000000000000000000000000";
    //    int length = str.Length;
    //    int diff = digits - length;
    //    string z = zero.Substring(1, diff);
    //    z = z + str;
    //    return z;
    //}
}
