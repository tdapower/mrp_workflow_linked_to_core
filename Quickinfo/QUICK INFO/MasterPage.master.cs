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

public partial class MasterPage : System.Web.UI.MasterPage
{

    String UserName;
    String DomainName;
    String UserBranch;
    String UserDepartment;
    String UserDisplay;
    String UserWorkflowChoice;

    HttpCookie USERROLE = new HttpCookie("USERROLE");


    protected void Page_Load(object sender, EventArgs e)
    {
        // this.Header.DataBind();
        //UserName = My.User.Name;
        UserName = Context.User.Identity.Name;


        if (Left(UserName, 4) == "HNBA")
        {
            UserName = Right(UserName, (UserName.Length) - 5);
            UserDetails(UserName);
            Label5.Text = UserDisplay.ToUpper();
            Label7.Text = UserBranch;
        }
        else
        {
            UserName = Right(UserName, (UserName.Length) - 7);
            UserBranchDetails(UserName);
            Label5.Text = UserDisplay.ToUpper();
            Label7.Text = UserBranch;
        }

        BuildMenu();


        if (Request.Cookies["WORKFLOW_CHOICE"] == null)
        {
            Response.Cookies["WORKFLOW_CHOICE"].Value = System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString();
        }


        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            headerDiv.Style.Add("background-color", "STEELBLUE");
            menuStyle.Href = "~/NewSideMenu/menu.css";
            styleSheet.Href = "~/Styles/StyleSheet.css";
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            headerDiv.Style.Add("background-color", "Purple");
            menuStyle.Href = "~/NewSideMenu/MCRmenu.css";
            styleSheet.Href = "~/Styles/MCRStyleSheet.css";


        }
        if (!IsPostBack)
        {
            if (Request.Cookies["WORKFLOW_CHOICE"] == null)
            {
                HttpCookie WORKFLOW_CHOICE = new HttpCookie("WORKFLOW_CHOICE");
                UserWorkflowChoice = getWorkflowChoiceOfUser(UserName);


                if (UserWorkflowChoice != null && UserWorkflowChoice != "")
                {
                    lblWorkflowChoice.Text = UserWorkflowChoice;
                    Response.Cookies.Add(WORKFLOW_CHOICE);
                    Response.Cookies["WORKFLOW_CHOICE"].Value = UserWorkflowChoice;
                }
                else
                {
                    lblWorkflowChoice.Text = UserWorkflowChoice;
                    Response.Cookies.Add(WORKFLOW_CHOICE);
                    Response.Cookies["WORKFLOW_CHOICE"].Value = System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString();
                }
            }
            else
            {
                lblWorkflowChoice.Text = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();
            }
        }
    }

    private void UserDetails(String EPFNumber)
    {
        System.DirectoryServices.DirectoryEntry dirEntry;
        System.DirectoryServices.DirectorySearcher dirSearcher;
        dirEntry = new System.DirectoryServices.DirectoryEntry("LDAP://192.168.10.251");
        //dirEntry = New System.DirectoryServices.DirectoryEntry("LDAP://10.100.200.241");

        dirSearcher = new System.DirectoryServices.DirectorySearcher(dirEntry);
        //dirSearcher.Filter = "(samAccountName=" & UserName & ")";
        dirSearcher.Filter = "(&(objectClass=user)(SAMAccountName=" + EPFNumber + "))";

        //Use the .FindOne() Method to stop as soon as a match is found

        SearchResult sr = dirSearcher.FindOne();

        System.DirectoryServices.DirectoryEntry de = sr.GetDirectoryEntry();

        if (sr == null)
        {// 'return false if user isn't found
            UserBranch = "";
            UserDepartment = "";
            UserDisplay = "";
        }
        else
        {
            //UserBranch = de.Properties("postalCode").Value.ToString();
            //UserDepartment = de.Properties("Department").Value.ToString();
            //UserDisplay = de.Properties("displayname").Value.ToString();

            UserBranch = de.Properties["postalCode"].Value.ToString();
            UserDepartment = de.Properties["Department"].Value.ToString();
            UserDisplay = de.Properties["displayname"].Value.ToString();
        }
    }


    private void UserBranchDetails(string EPFNumber)
    {
        System.DirectoryServices.DirectoryEntry dirEntry;
        System.DirectoryServices.DirectorySearcher dirSearcher;
        //dirEntry = New System.DirectoryServices.DirectoryEntry("LDAP://192.168.10.251")
        dirEntry = new System.DirectoryServices.DirectoryEntry("LDAP://10.100.200.241");

        dirSearcher = new System.DirectoryServices.DirectorySearcher(dirEntry);
        //dirSearcher.Filter = "(samAccountName=" & UserName & ")"
        dirSearcher.Filter = "(&(objectClass=user)(SAMAccountName=" + EPFNumber + "))";

        //Use the .FindOne() Method to stop as soon as a match is found

        SearchResult sr = dirSearcher.FindOne();

        System.DirectoryServices.DirectoryEntry de = sr.GetDirectoryEntry();

        if (sr == null)
        {
            UserBranch = "";
            UserDepartment = "";
            UserDisplay = "";
        }
        else
        {

            UserBranch = de.Properties["postalCode"].Value.ToString();
            UserDepartment = de.Properties["Department"].Value.ToString();
            UserDisplay = de.Properties["displayname"].Value.ToString();

        }



    }


    private void BuildMenu()
    {
        StringBuilder menuHTMLCode = new StringBuilder();


        //For test purpose
        menuHTMLCode.Append("<nav>");
        menuHTMLCode.Append("<ul id=\"nav\">");


        //End For test purpose

        /////////////////////////////////////////
        try
        {
            DataTable dtMainMenu = new DataTable();
            DataTable dtSubMenu = new DataTable();
            dtMainMenu = FillMainMenuTable();
            dtSubMenu = FillSubMenuTable();
            DataSet ds = new DataSet();
            ds.Tables.Add(dtMainMenu);
            ds.Tables.Add(dtSubMenu);
            ds.Relations.Add("subMenus", dtMainMenu.Columns["MAIN_MENU_CODE"], dtSubMenu.Columns["MAIN_MENU_CODE"]);

            if (ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow masterRow in ds.Tables[0].Rows)
                {
                    if (masterRow.GetChildRows("subMenus").Length > 0)
                    {
                        menuHTMLCode.Append("<li><a href=\"#\">" + (string)masterRow["MAIN_MENU_NAME"] + "</a>");
                        menuHTMLCode.Append("<ul>");
                        foreach (DataRow childRow in masterRow.GetChildRows("subMenus"))
                        {
                            //menuHTMLCode.Append("<li><a href=\"" + Page.ResolveUrl((string)childRow["PAGE_PATH"]) + "\">" + (string)childRow["SUB_MENU_NAME"] + "</a></li>");
                            menuHTMLCode.Append("<li><a href=\"" + Page.ResolveUrl((string)childRow["PAGE_PATH"]) + "?pagecode=" + childRow["SUB_MENU_CODE"].ToString() + "\">" + (string)childRow["SUB_MENU_NAME"] + "</a></li>");
                        }
                        menuHTMLCode.Append("</ul>");
                        menuHTMLCode.Append("</li>");
                    }
                }

            }
        }
        catch (Exception ex)
        {
            // throw new Exception("Unable to populate treeview" + ex.Message);

        }

        //////////////////////////////////////




        menuHTMLCode.Append("</ul>");
        menuHTMLCode.Append("</nav>");



        ltrlMenu.Text = menuHTMLCode.ToString();

    }

    private DataTable FillMainMenuTable()
    {
        DataTable dtMainMenu = new DataTable();

        dtMainMenu.Columns.Add("MAIN_MENU_CODE", Type.GetType("System.Int32"));
        dtMainMenu.Columns.Add("MAIN_MENU_NAME", Type.GetType("System.String"));

        DataRow drMainMenu;


        ////
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT MAIN_MENU_CODE,MAIN_MENU_NAME FROM WF_ADMIN_MAIN_MENU ORDER BY MAIN_MENU_CODE,MAIN_MENU_NAME";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();


        if (dr.HasRows)
        {
            while (dr.Read())
            {
                drMainMenu = dtMainMenu.NewRow();
                drMainMenu[0] = Convert.ToInt32(dr[0].ToString());
                drMainMenu[1] = dr[1].ToString();
                dtMainMenu.Rows.Add(drMainMenu);

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        ////

        return dtMainMenu;
    }

    private DataTable FillSubMenuTable()
    {
        DataTable dtSubMenu = new DataTable();

        dtSubMenu.Columns.Add("SUB_MENU_CODE", Type.GetType("System.Int32"));
        dtSubMenu.Columns.Add("MAIN_MENU_CODE", Type.GetType("System.Int32"));
        dtSubMenu.Columns.Add("SUB_MENU_NAME", Type.GetType("System.String"));
        dtSubMenu.Columns.Add("PAGE_PATH", Type.GetType("System.String"));


        DataRow drSubMenu;

        ////

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        //selectQuery = "SELECT SB.SUB_MENU_CODE,SB.MAIN_MENU_CODE,SB.SUB_MENU_NAME,SB.PAGE_PATH FROM WF_ADMIN_SUB_MENU SB "+
        //    " ORDER BY SB.SUB_MENU_CODE,SB.SUB_MENU_NAME";


        int userRoleCode = 0;

        userRoleCode = getUserRoleCodeOfCurrentUser(UserName);

        if (userRoleCode != 0)
        {

            selectQuery = "SELECT SB.SUB_MENU_CODE,SB.MAIN_MENU_CODE,SB.SUB_MENU_NAME,SB.PAGE_PATH FROM WF_ADMIN_SUB_MENU SB " +
                        " INNER JOIN WF_ADMIN_USEROLE_PRIVILEGE T ON SB.MAIN_MENU_CODE=T.MAIN_MENU_CODE AND SB.SUB_MENU_CODE=T.SUB_MENU_CODE " +
                        " WHERE  T.USER_ROLE_CODE='" + userRoleCode + "' AND T.PRIVILEGE_GIVEN=1 " +
                " ORDER BY SB.SUB_MENU_NAME";

            cmd.CommandText = selectQuery;

            dr = cmd.ExecuteReader();


            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    drSubMenu = dtSubMenu.NewRow();
                    drSubMenu[0] = Convert.ToInt32(dr[0].ToString());
                    drSubMenu[1] = Convert.ToInt32(dr[1].ToString());
                    drSubMenu[2] = dr[2].ToString();
                    drSubMenu[3] = dr[3].ToString();
                    dtSubMenu.Rows.Add(drSubMenu);
                }
            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
            con.Dispose();
        }

        //

        return dtSubMenu;
    }


    private int getUserRoleCodeOfCurrentUser(string UserName)
    {
        int userRoleCode = 0;

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT USER_ROLE_CODE FROM WF_ADMIN_USERS WHERE STATUS=1 AND USER_CODE='" + UserName + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            userRoleCode = Convert.ToInt32(dr[0].ToString());
            Response.Cookies.Add(USERROLE);
            Response.Cookies["USERROLE"].Value = Convert.ToString(userRoleCode);

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return userRoleCode;

    }


    private string getWorkflowChoiceOfUser(string UserName)
    {
        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";
        selectQuery = "SELECT WORKFLOW_CHOICE FROM mrp_user_details WHERE  USER_CODE=:V_USER_CODE";

        OracleCommand cmd = new OracleCommand(selectQuery, con);


        cmd.Parameters.Add(new OracleParameter("V_USER_CODE", UserName));
        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            if (dr[0] != DBNull.Value)
            {
                returnVal = dr[0].ToString();
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;

    }





    protected void lnkBtnChangeWorkflow_Click(object sender, EventArgs e)
    {
        string choice = "";
        choice = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();

        if (choice == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            UserWorkflowChoice = System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString();
            Response.Cookies["WORKFLOW_CHOICE"].Value = UserWorkflowChoice;
        }
        else
        {
            UserWorkflowChoice = System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString();
            Response.Cookies["WORKFLOW_CHOICE"].Value = UserWorkflowChoice;
        }

        updateWorkflowChoice(UserName, UserWorkflowChoice);
        lblWorkflowChoice.Text = UserWorkflowChoice;
        Response.Redirect("~/MainPage.aspx");
    }



    private void updateWorkflowChoice(string userCode, string newChoice)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand cmd = new OracleCommand("UPDATE_WORKFLOW_CHOICE", con);
        cmd.CommandType = CommandType.StoredProcedure;


        cmd.Parameters.Add(new OracleParameter("V_USER_CODE", OracleType.VarChar));
        cmd.Parameters["V_USER_CODE"].Value = userCode;

        cmd.Parameters.Add(new OracleParameter("V_WORKFLOW_CHOICE", OracleType.VarChar));
        cmd.Parameters["V_WORKFLOW_CHOICE"].Value = newChoice;


        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
        }
        catch (OracleException err)
        {
            throw new ApplicationException("Data error.");
        }
        finally
        {
            con.Close();
        }

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

}
