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
using System.Collections;
using System.Net.Mail;
using System.Security.Principal;

public partial class MainPage : System.Web.UI.Page
{

    string UserName = "";
    string Domain = "";
    HttpCookie USERID = new HttpCookie("USERID");
    HttpCookie BRANCHID = new HttpCookie("BRANCHID");
    HttpCookie EMAILID = new HttpCookie("EMAILID");
    HttpCookie EMAILNAME = new HttpCookie("EMAILNAME");
    HttpCookie ROLECODE = new HttpCookie("ROLECODE");
    HttpCookie EMPLOYEEID = new HttpCookie("EMPLOYEEID");

    protected void Page_Load(object sender, EventArgs e)
    {
        UserName = User.Identity.Name;

        if (Left(UserName, 4) == "HNBA")
        {
            Domain = Left(UserName, 4);
            Session["DOMAIN"] = Domain.ToString();
            UserName = Right(UserName, (UserName.Length) - 5);
            Session["USER_ID"] = UserName.ToString();
            GetUser(UserName.ToString());
        }
        else
        {
            Domain = Left(UserName, 6);
            Session["DOMAIN"] = Domain.ToString();
            UserName = Right(UserName, (UserName.Length) - 7);
            Session["USER_ID"] = UserName.ToString();
            GetUserBranch(UserName.ToString());
        }



        SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
        SqlCommand cmd_Log = new SqlCommand();
        SqlDataReader dr_Log;
        conn_Log.Open();
        cmd_Log.CommandType = CommandType.Text;
        cmd_Log.Connection = conn_Log;


        cmd_Log.CommandText = "SELECT USER_CAT FROM SYS_USERS WHERE USER_ID = '" + Session["USER_ID"].ToString() + "' AND USER_STATUS = 'Y'";

        dr_Log = cmd_Log.ExecuteReader();
        if (dr_Log.HasRows)
        {
            while (dr_Log.Read())
            {
                Session["ROLE_CODE"] = dr_Log[0].ToString();
                Response.Cookies.Add(ROLECODE);
                Response.Cookies["ROLECODE"].Value = Session["ROLE_CODE"].ToString();
            }
        }
        else
        {
            Session["ROLE_CODE"] = "";
            Response.Cookies.Add(ROLECODE);
            Response.Cookies["ROLECODE"].Value = Session["ROLE_CODE"].ToString();
        }

        dr_Log.Close();
        dr_Log.Dispose();
    }

    public DirectoryEntry GetDirectoryObject()
    {
        DirectoryEntry oDE;
        oDE = new DirectoryEntry("LDAP://192.168.10.251");
        return oDE;
    }

    public DirectoryEntry GetUser(string UserName)
    {
        DirectoryEntry de = GetDirectoryObject();
        DirectorySearcher deSearch = new DirectorySearcher();
        deSearch.SearchRoot = de;

        deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
        deSearch.SearchScope = SearchScope.Subtree;
        SearchResult results = deSearch.FindOne();


        if (!(results == null))
        {

            de = new DirectoryEntry(results.Path);
            Session["EmployeeID"] = de.Properties["EmployeeID"][0].ToString();
            Session["DisplayName"] = de.Properties["displayName"][0].ToString();
            Session["HnbaEmail"] = de.Properties["Mail"][0].ToString();
            Session["Departmnet"] = de.Properties["Department"].Value.ToString();
            //
            Session["Branch"] = de.Properties["postalCode"].Value.ToString();

            //Session["USER_ID"] = "isuru";

            Response.Cookies.Add(USERID);
            Response.Cookies["USERID"].Value = Session["USER_ID"].ToString();

            Response.Cookies.Add(BRANCHID);
            Response.Cookies["BRANCHID"].Value = Session["Branch"].ToString();

            Response.Cookies.Add(EMAILID);
            Response.Cookies["EMAILID"].Value = Session["HnbaEmail"].ToString();

            Response.Cookies.Add(EMAILNAME);
            Response.Cookies["EMAILNAME"].Value = Session["DisplayName"].ToString();

            Response.Cookies.Add(EMPLOYEEID);
            Response.Cookies["EMPLOYEEID"].Value = Session["EmployeeID"].ToString();
            

            //j = temp.IndexOf(",");
            //string temp1 = temp.Substring(j + 1);
            //k = temp1.IndexOf(",");
            //string temp2 = temp1.Substring(k + 1);
            //temp = Left(temp, temp.IndexOf(","));
            //temp1 = Left(temp1, temp1.IndexOf(","));

            return de;
        }
        else
        {
            Session["EmployeeID"] = "";
            Session["DisplayName"] = "";
            Session["HnbaEmail"] = "";
            Session["Departmnet"] = "";
            //
            Session["Branch"] = "";

            Response.Cookies.Add(BRANCHID);
            Response.Cookies["BRANCHID"].Value = "";

            Response.Cookies.Add(EMAILID);
            Response.Cookies["EMAILID"].Value = "";

            Response.Cookies.Add(EMAILNAME);
            Response.Cookies["EMAILNAME"].Value = "";

            Response.Cookies.Add(EMPLOYEEID);
            Response.Cookies["EMPLOYEEID"].Value = "";

            return null;
        }
    }
    public DirectoryEntry GetEmail(string UserName)
    {
        DirectoryEntry de = GetDirectoryObjectBranch();
        DirectorySearcher deSearch = new DirectorySearcher();
        deSearch.SearchRoot = de;

        deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
        deSearch.SearchScope = SearchScope.Subtree;
        SearchResult results = deSearch.FindOne();


        if (!(results == null))
        {

            de = new DirectoryEntry(results.Path);
            Session["ToDisplayName"] = de.Properties["displayName"][0].ToString();
            Session["ToEmail"] = de.Properties["Mail"][0].ToString();
            return de;
        }
        else
        {
            Session["EMAIL"] = "anuranga@hnbassurance.com";
            return null;
        }
    }
    public DirectoryEntry GetDirectoryObjectBranch()
    {
        DirectoryEntry oDEbranch;
        oDEbranch = new DirectoryEntry("LDAP://10.100.200.241");
        //oDEbranch = new DirectoryEntry("LDAP://BRANCH.HNBA.INT");
        //oDEbranch = new DirectoryEntry("LDAP://BRANCH.HNBA.INT/CN=Users,GC=HNBA,GC=INT");
        return oDEbranch;
    }

    public DirectoryEntry GetUserBranch(string UserName)
    {
        DirectoryEntry de = GetDirectoryObjectBranch();
        DirectorySearcher deSearch = new DirectorySearcher();
        deSearch.SearchRoot = de;

        deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
        deSearch.SearchScope = SearchScope.Subtree;
        SearchResult results = deSearch.FindOne();

        //Session["EmployeeID"] = "Test";

        if (!(results == null))
        {
            de = new DirectoryEntry(results.Path);
            Session["EmployeeID"] = de.Properties["EmployeeID"][0].ToString();
            Session["DisplayName"] = de.Properties["displayName"][0].ToString();
            Session["HnbaEmail"] = de.Properties["Mail"][0].ToString();
            Session["Departmnet"] = de.Properties["Department"].Value.ToString();
            //
            Session["Branch"] = de.Properties["postalCode"].Value.ToString();

            Response.Cookies.Add(BRANCHID);
            Response.Cookies["BRANCHID"].Value = Session["Branch"].ToString();

            Response.Cookies.Add(EMAILID);
            Response.Cookies["EMAILID"].Value = Session["HnbaEmail"].ToString();

            Response.Cookies.Add(EMAILNAME);
            Response.Cookies["EMAILNAME"].Value = Session["DisplayName"].ToString();

            Response.Cookies.Add(EMPLOYEEID);
            Response.Cookies["EMPLOYEEID"].Value = Session["EmployeeID"].ToString();

            return de;
        }
        else
        {
            Session["EmployeeID"] = "";
            Session["DisplayName"] = "";
            Session["HnbaEmail"] = "";
            Session["Departmnet"] = "";
            //
            Session["Branch"] = "";

            Response.Cookies.Add(BRANCHID);
            Response.Cookies["BRANCHID"].Value = "";

            Response.Cookies.Add(EMAILID);
            Response.Cookies["EMAILID"].Value = "";

            Response.Cookies.Add(EMAILNAME);
            Response.Cookies["EMAILNAME"].Value = "";

            Response.Cookies.Add(EMPLOYEEID);
            Response.Cookies["EMPLOYEEID"].Value = "";



            return null;

        }
    }

    public static string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public static string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }

    public static string Mid(string text, int start, int end)
    {
        return text.Substring(start, end);
    }

    public static string Mid(string text, int start)
    {
        return text.Substring(start, text.Length - start);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
    }
}
