using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for ME_SMS
/// </summary>
public class ME_SMS
{

    MySqlConnection SMS = new MySqlConnection(ConfigurationManager.ConnectionStrings["SMS"].ToString());

    SqlConnection connDetalis = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());


	public string ME_SMS1(string BRA_CODE,string TYPE,string MSG)
	{
        string Permission = "QQ";
        string NO = "";

        SqlCommand cmdDetails = new SqlCommand();
        SqlCommand cmdDetails8 = new SqlCommand();
        connDetalis.Open();

        SqlCommand cmdGetDocNo = new SqlCommand();
        SqlDataReader drcmdGetDocNo;

        cmdGetDocNo.CommandType = CommandType.Text;
        cmdGetDocNo.Connection = connDetalis;

        cmdGetDocNo.CommandText = "SELECT TEL FROM SMS_ME WHERE B_CODE = '" + BRA_CODE + "' AND TYPE = '" + TYPE + "'";
 
        drcmdGetDocNo = cmdGetDocNo.ExecuteReader();

        if (drcmdGetDocNo.HasRows)
        {
            while (drcmdGetDocNo.Read())
            {

                NO = (drcmdGetDocNo[0].ToString());

                string AA = Clauses(NO, MSG);
                
            }
        }

        connDetalis.Close();
        connDetalis.Dispose();

        return Permission.Trim();

	}

    public string Clauses(string NO,string MSG)
    {

        string RecNos = "";

        //MySqlCommand cmd_Update = new MySqlCommand();
        //SMS.Open();

        //cmd_Update.CommandType = CommandType.Text;
        //cmd_Update.Connection = SMS;

        //cmd_Update.CommandText = "INSERT INTO sms_msg (number,msg) VALUES('" + NO + "', '" + MSG + "')";

        //cmd_Update.ExecuteScalar();
        //cmd_Update.Dispose();

        //SMS.Close();

        sms abc = new sms();
        string aa = abc.sendsms(NO.Trim(), MSG.Trim());


        return RecNos;

    }

}
