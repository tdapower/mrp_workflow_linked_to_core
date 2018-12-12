using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Text;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Net;
using System.DirectoryServices;
using System.Net.Mail;
using System.IO;
using MsgBox;

/// <summary>
/// Common class for UserAuthentication
/// </summary>
public class UserAuthentication
{
    public UserAuthentication()
    {

    }

    public bool IsAuthorizeForThisPage(String UserName, String SubMenuCode)
    {

        try
        {
           


            bool returnVal = false;
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            OracleDataReader dr;

            con.Open();

       
            String selectQuery = "";

            int userRoleCode = 0;

            userRoleCode = getUserRoleCodeOfCurrentUser(UserName);
            selectQuery = "SELECT SB.SUB_MENU_CODE FROM WF_ADMIN_SUB_MENU SB " +
                        " INNER JOIN WF_ADMIN_USEROLE_PRIVILEGE T ON SB.MAIN_MENU_CODE=T.MAIN_MENU_CODE AND SB.SUB_MENU_CODE=T.SUB_MENU_CODE " +
                        " WHERE  T.USER_ROLE_CODE=:V_USER_ROLE_CODE AND SB.SUB_MENU_CODE=:V_SUB_MENU_CODE AND T.PRIVILEGE_GIVEN=1 ";



            OracleCommand cmd = new OracleCommand(selectQuery, con);
            cmd.Parameters.Add(new OracleParameter("V_USER_ROLE_CODE", userRoleCode));
            cmd.Parameters.Add(new OracleParameter("V_SUB_MENU_CODE", SubMenuCode));

            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                returnVal = true;
            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            return returnVal;

        }
        catch (Exception ee)
        {
            return false;
        }
    }

    public int getUserRoleCodeOfCurrentUser(string UserName)
    {

        if (Left(UserName, 4) == "HNBA")
        {
            UserName = Right(UserName, (UserName.Length) - 5);
        }
        else
        {
            UserName = Right(UserName, (UserName.Length) - 7);
        }


        int userRoleCode = 0;

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";
        selectQuery = "SELECT USER_ROLE_CODE FROM WF_ADMIN_USERS WHERE STATUS=1 AND USER_CODE=:V_USER_CODE";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_USER_CODE", UserName));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            userRoleCode = Convert.ToInt32(dr[0].ToString());

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();



        return userRoleCode;

    }


    public string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }

    public string Mid(string text, int start, int end)
    {
        return text.Substring(start, end);
    }

    public string Mid(string text, int start)
    {
        return text.Substring(start, text.Length - start);
    }

    public bool IsAuthorizeForThisPageWithPageName(String UserName, String url)
    {

        try
        {
            

            bool returnVal = false;
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            OracleDataReader dr;

            con.Open();

            String selectQuery = "";

            int userRoleCode = 0;

            userRoleCode = getUserRoleCodeOfCurrentUser(UserName);
            selectQuery = "SELECT SB.SUB_MENU_CODE FROM WF_ADMIN_SUB_MENU SB " +
                        " INNER JOIN WF_ADMIN_USEROLE_PRIVILEGE T ON SB.MAIN_MENU_CODE=T.MAIN_MENU_CODE AND SB.SUB_MENU_CODE=T.SUB_MENU_CODE " +
                        " WHERE  T.USER_ROLE_CODE=:V_USER_ROLE_CODE AND SB.PAGE_PATH=:V_URL AND T.PRIVILEGE_GIVEN=1 ";



            OracleCommand cmd = new OracleCommand(selectQuery, con);
            cmd.Parameters.Add(new OracleParameter("V_USER_ROLE_CODE", userRoleCode));
            cmd.Parameters.Add(new OracleParameter("V_URL", url));

            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                returnVal = true;
            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            return returnVal;

        }
        catch (Exception ee)
        {
            return false;
        }
    }
}
