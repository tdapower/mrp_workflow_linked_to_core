//******************************************
// Author            :Tharindu Athapattu
// Date              :11/04/2013
// Reviewed By       :
// Description       : Menu Privilege Assign To UserRoles Form 
//******************************************
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

public partial class MenuPrivilegeAssignToUserRoles : System.Web.UI.Page
{

    OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
    OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

  



    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            validatePageAuthentication();


            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);


            ClearComponents();
            initializeValues();


            //PopulateMenuTreeview();

            Session.Remove("UserRegMode");


        }


    }
    private void validatePageAuthentication()
    {
        if (Request.Params["pagecode"] != null)
        {
            if (Request.Params["pagecode"] != "")
            {
                UserAuthentication userAuthentication = new UserAuthentication();
                if (!userAuthentication.IsAuthorizeForThisPage(Context.User.Identity.Name, Request.Params["pagecode"].ToString()))
                {
                    Response.Redirect("~/NoPermission.aspx");
                }
            }
        }
    }

    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    SearchData();
    //    ClearComponents();
    //}

    //private void SearchData()
    //{
    //    lblError1.Text = "";
    //    grdUserRoles.DataSource = null;
    //    grdUserRoles.DataBind();

    //    if ((ddlSearchUserRole.SelectedValue.ToString() == "0"))
    //    {
    //        lblError1.Text = "Search text cannot be blank";
    //        return;
    //    }

    //    OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

    //    OracleCommand myOleDbCommand = new OracleCommand();

    //    myOleDbConnection.Open();

    //    myOleDbCommand.Connection = myOleDbConnection;


    //    if (ddlSearchUserRole.SelectedValue.ToString() != "")
    //    {

    //        SQL = "(T.USER_ROLE_CODE LIKE '%" + ddlSearchUserRole.SelectedValue.ToString() + "%')";
    //    }

    //    String selectQuery = "";
    //    //selectQuery = " SELECT T.USER_ROLE_CODE AS \"User Role Code\" ,T.USER_ROLE_NAME AS \"User Role Name\",T.DESCRIPTION AS \"Description\"  "+
    //    //            " ,(CASE WHEN UP.PRIVILEGE_GIVEN IS NULL THEN 0 ELSE UP.PRIVILEGE_GIVEN END) AS  \"PRIVILEGE_GIVEN\" "+
    //    //            " FROM WF_ADMIN_USER_ROLES T "+
    //    //            " LEFT JOIN WF_ADMIN_USEROLE_PRIVILEGE UP ON T.USER_ROLE_CODE=UP.USER_ROLE_CODE "+
    //    //            " WHERE (" + SQL + ") ORDER BY T.USER_ROLE_NAME ASC";

    //    selectQuery = " SELECT T.USER_ROLE_CODE AS \"User Role Code\" ,T.USER_ROLE_NAME AS \"User Role Name\",T.DESCRIPTION AS \"Description\"  " +
    //                   " FROM WF_ADMIN_USER_ROLES T " +
    //                   " WHERE (" + SQL + ") ORDER BY T.USER_ROLE_NAME ASC";

    //    myOleDbCommand.CommandText = selectQuery;

    //    OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
    //    if (myOleDbDataReader.HasRows == true)
    //    {
    //        DataTable dbTable = new DataTable();
    //        grdUserRoles.DataSource = myOleDbDataReader;
    //        grdUserRoles.DataBind();

    //        pnlUserRoleGrid.Visible = true;
    //    }
    //}




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuPrivilegeAssignToUserRoles.aspx");
    }

    public DirectoryEntry GetDirectoryObject()
    {
        DirectoryEntry oDE;
        oDE = new DirectoryEntry("LDAP://192.168.10.251");
        return oDE;
    }


    public DirectoryEntry GetLoginName(string EmployeeID)
    {
        DirectoryEntry de = GetDirectoryObject();
        DirectorySearcher deSearch = new DirectorySearcher();
        deSearch.SearchRoot = de;

        deSearch.Filter = "(&(objectClass=user)(EmployeeID=" + EmployeeID + "))";
        deSearch.SearchScope = SearchScope.Subtree;
        SearchResult results = deSearch.FindOne();


        if (!(results == null))
        {

            de = new DirectoryEntry(results.Path);
            Session["USER"] = de.Properties["SAMAccountName"][0].ToString();
            return de;
        }
        else
        {
            Session["USER"] = "";
            return null;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
       

        if (ddlSearchUserRole.SelectedValue == "" || ddlSearchUserRole.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the User Role";
            Timer1.Enabled = true;
            return;
        }



        int privilegeGiven = 0;
        foreach (TreeNode node in tvPrivileges.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {
                privilegeGiven = 0;
                if (cnode.Checked)
                {
                    privilegeGiven = 1;
                }
                savePrivileges(Convert.ToInt32(txtUserRoleCode.Text), Convert.ToInt32(cnode.Parent.Value.ToString()), Convert.ToInt32(cnode.Value.ToString()), privilegeGiven);
            }
        }

        ClearComponents();
        loadDataForUserRole();


        lblMsg.Text = "Successfully Saved";
        Timer1.Enabled = true;


        Response.Redirect("MenuPrivilegeAssignToUserRoles.aspx");
    }

    private void savePrivileges(int userRoleCode, int mainMenuCode, int subMenuCode, int privilegeGiven)
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            spProcess = new OracleCommand("ASSIGN_WF_ADMIN_USEROLE_P");

            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_USER_ROLE_CODE", OracleType.Number, 5).Value = userRoleCode;
            spProcess.Parameters.Add("V_MAIN_MENU_CODE", OracleType.Number, 5).Value = mainMenuCode;
            spProcess.Parameters.Add("V_SUB_MENU_CODE", OracleType.Number, 5).Value = subMenuCode;
            spProcess.Parameters.Add("V_PRIVILEGE_GIVEN", OracleType.Number, 5).Value = privilegeGiven;


            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error While Saving";
            Timer1.Enabled = true;
        }

    }

    private void ClearComponents()
    {
        txtUserRoleCode.Text = "";
        // ddlSearchUserRole.SelectedValue = "0";

        //ddlSearchUserRole.Enabled = false;

        foreach (TreeNode node in tvPrivileges.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {
                cnode.Checked = false;
            }
        }


        tvPrivileges.Enabled = false;

        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //btnCancel.Enabled = false;
    }



    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtUserRoleCode.Text == "")
        {
            lblMsg.Text = "Please Select An User Role";
            return;
        }

        //txtUserName.Enabled = true;
        //txtEPF.Enabled = true;
        //ddlUserRole.Enabled = true;

        tvPrivileges.Enabled = true;

        btnSave.Enabled = true;

        Session["UserRegMode"] = "UPDATE";
    }

    protected void grdUserRoles_SelectedIndexChanged(object sender, EventArgs e)
    {


    }




    private void initializeValues()
    {
        loadUserRoles();
        lblError1.Text = "";
        lblMsg.Text = "";
    }


    private void loadUserRoles()
    {

        ddlSearchUserRole.Items.Clear();
        ddlSearchUserRole.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT USER_ROLE_CODE,USER_ROLE_NAME FROM WF_ADMIN_USER_ROLES";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlSearchUserRole.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }



    private void PopulateMenuTreeview()
    {
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
                tvPrivileges.Nodes.Clear();
                tvPrivileges.ShowCheckBoxes = TreeNodeTypes.Leaf;

                foreach (DataRow masterRow in ds.Tables[0].Rows)
                {
                    TreeNode masterNode = new TreeNode((string)masterRow["MAIN_MENU_NAME"], Convert.ToString(masterRow["MAIN_MENU_CODE"]));
                    tvPrivileges.Nodes.Add(masterNode);

                    foreach (DataRow childRow in masterRow.GetChildRows("subMenus"))
                    {
                        TreeNode childNode = new TreeNode((string)childRow["SUB_MENU_NAME"], Convert.ToString(childRow["SUB_MENU_CODE"]));
                        if (isPrivilegeGiven(Convert.ToString(masterRow["MAIN_MENU_CODE"]), Convert.ToString(childRow["SUB_MENU_CODE"]), txtUserRoleCode.Text))
                        {
                            childNode.Checked = true;
                        }
                        childNode.SelectAction = TreeNodeSelectAction.None;
                        masterNode.ChildNodes.Add(childNode);
                        childNode.Value = Convert.ToString(childRow["SUB_MENU_CODE"]);

                    }
                }
                tvPrivileges.CollapseAll();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to populate treeview" + ex.Message);
        }
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

        tvPrivileges.Nodes.Clear();

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

        DataRow drSubMenu;

        ////

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT SUB_MENU_CODE,MAIN_MENU_CODE,SUB_MENU_NAME FROM WF_ADMIN_SUB_MENU ORDER BY SUB_MENU_CODE,SUB_MENU_NAME";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();

        tvPrivileges.Nodes.Clear();

        if (dr.HasRows)
        {
            while (dr.Read())
            {

                drSubMenu = dtSubMenu.NewRow();
                drSubMenu[0] = Convert.ToInt32(dr[0].ToString());
                drSubMenu[1] = Convert.ToInt32(dr[1].ToString());
                drSubMenu[2] = dr[2].ToString();
                dtSubMenu.Rows.Add(drSubMenu);
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        //

        return dtSubMenu;
    }

    private bool isPrivilegeGiven(string mainMenuCode, string subMenuCode, string userRoleCode)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = " SELECT (CASE WHEN T.PRIVILEGE_GIVEN IS NULL THEN 0 ELSE T.PRIVILEGE_GIVEN END)  " +
                       " FROM WF_ADMIN_USEROLE_PRIVILEGE T " +
                       " WHERE T.MAIN_MENU_CODE='" + mainMenuCode + "' AND T.SUB_MENU_CODE='" + subMenuCode + "' AND T.USER_ROLE_CODE='" + userRoleCode + "'";


        //selectQuery = " SELECT T.USER_ROLE_CODE AS \"User Role Code\" ,T.USER_ROLE_NAME AS \"User Role Name\",T.DESCRIPTION AS \"Description\"  " +
        //               " ,(CASE WHEN UP.PRIVILEGE_GIVEN IS NULL THEN 0 ELSE UP.PRIVILEGE_GIVEN END) AS  \"PRIVILEGE_GIVEN\" " +
        //               " FROM WF_ADMIN_USER_ROLES T " +
        //               " LEFT JOIN WF_ADMIN_USEROLE_PRIVILEGE UP ON T.USER_ROLE_CODE=UP.USER_ROLE_CODE " +
        //               " WHERE (" + SQL + ") ORDER BY T.USER_ROLE_NAME ASC";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {

                if (dr[0].ToString() == "1")
                {
                    returnVal = true;
                }
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }




    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }
    protected void ddlSearchUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearchUserRole.SelectedValue != "0")
        {

            loadDataForUserRole();
        }
    }


    private void loadDataForUserRole()
    {
        foreach (TreeNode node in tvPrivileges.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {
                cnode.Checked = false;
            }
        }


        txtUserRoleCode.Text = ddlSearchUserRole.SelectedValue.ToString();
        //ddlUserRole.SelectedValue = grdUserRoles.SelectedRow.Cells[1].Text.Trim();

        PopulateMenuTreeview();
        //ddlUserRole.SelectedValue = grdUsers.SelectedRow.Cells[1].Text.Trim();


        btnAlter.Enabled = true;
        tvPrivileges.Enabled = false;
    }
}
