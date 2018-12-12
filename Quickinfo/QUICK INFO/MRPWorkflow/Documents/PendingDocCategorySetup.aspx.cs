//******************************************
// Author            :Tharindu Athapattu
// Date              :13/05/2013
// Reviewed By       :
// Description       :Pending Document Category Setup Form
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

public partial class PendingDocCategorySetup : System.Web.UI.Page
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

            Session.Remove("DocCategorySetupMode");

            pnlDocCategoryGrid.Visible = false;
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchData();
        ClearComponents();
    }

    private void SearchData()
    {
        string SQL = "";
        lblError.Text = "";
        grdDocCategory.DataSource = null;
        grdDocCategory.DataBind();

        if ((txtSearchDocCategory.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchDocCategory.Text != "")
        {

            SQL = "(LOWER(T.DOC_CAT_NAME) LIKE '%" + txtSearchDocCategory.Text.ToLower() + "%') AND";
        }

        SQL = SQL + " (T.WORKFLOW_TYPE = '" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "') AND";


        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.DOC_CAT_CODE AS \"Document Category Code\" ,T.DOC_CAT_NAME AS \"Document Category\" FROM MRP_WF_PENDING_DOC_CATEGORY T  " +
                      " WHERE (" + SQL + ") ORDER BY T.DOC_CAT_NAME ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdDocCategory.DataSource = myOleDbDataReader;
            grdDocCategory.DataBind();

            pnlDocCategoryGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingDocCategorySetup.aspx");
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

        if (txtDocCategory.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Document Category";
            Timer1.Enabled = true;
            return;
        }



        if (Session["DocCategorySetupMode"].ToString() == "NEW")
        {
            if (CheckDocCategoryAlreadyExist(txtDocCategory.Text))
            {
                lblMsg.Text = "Enetered Document Category Already Exists";
                Timer1.Enabled = true;
                return;
            }
        }

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["DocCategorySetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_PENDING_DOC_CAT");
            }
            else if (Session["DocCategorySetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_PENDING_DOC_CAT");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            if (Session["DocCategorySetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_DOC_CAT_CODE", OracleType.Number, 5).Value = txtDocCategoryCode.Text;
            }
            spProcess.Parameters.Add("V_DOC_CAT_NAME", OracleType.VarChar, 100).Value = txtDocCategory.Text;
            spProcess.Parameters.Add("V_WORKFLOW_TYPE", OracleType.VarChar, 10).Value = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();
    


            spProcess.ExecuteNonQuery();
            conProcess.Close();

            ClearComponents();
            SearchData();
            lblMsg.Text = "Successfully saved";
            Timer1.Enabled = true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }

    }

    private void ClearComponents()
    {
        txtDocCategoryCode.Text = "";
        txtDocCategory.Text = "";

        txtDocCategoryCode.Enabled = false;
        txtDocCategory.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtDocCategoryCode.Enabled = true;
        txtDocCategory.Enabled = true;

        txtDocCategoryCode.Text = "";
        txtDocCategory.Text = "";

        btnSave.Enabled = true;

        Session["DocCategorySetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtDocCategoryCode.Text == "")
        {
            lblMsg.Text = "Please Select A Document Category";
            return;
        }


        txtDocCategory.Enabled = true;

        btnSave.Enabled = true;

        Session["DocCategorySetupMode"] = "UPDATE";
    }

    protected void grdDocCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtDocCategoryCode.Text = grdDocCategory.SelectedRow.Cells[1].Text.Trim();
        txtDocCategory.Text = grdDocCategory.SelectedRow.Cells[2].Text.Trim();


        btnAlter.Enabled = true;
    }

    protected void grdDocCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";
    }


    private bool CheckDocCategoryAlreadyExist(string DocCategory)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";
        selectQuery = "SELECT DOC_CAT_NAME FROM MRP_WF_PENDING_DOC_CATEGORY WHERE DOC_CAT_NAME=:V_DOC_CAT_NAME AND WORKFLOW_TYPE=:V_WORKFLOW_TYPE ";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_DOC_CAT_NAME", DocCategory));
        cmd.Parameters.Add(new OracleParameter("V_WORKFLOW_TYPE", Request.Cookies["WORKFLOW_CHOICE"].Value.ToString()));


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


    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }
}
