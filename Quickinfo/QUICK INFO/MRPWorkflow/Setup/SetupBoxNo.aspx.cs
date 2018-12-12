//******************************************
// Author            :Tharindu Athapattu
// Date              :11/04/2013
// Reviewed By       :
// Description       : Main Menu Setup Form
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

public partial class SetupBoxNo : System.Web.UI.Page
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

            Session.Remove("BoxNoSetupMode");

            pnlBoxNoGrid.Visible = false;
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
        grdBoxNos.DataSource = null;
        grdBoxNos.DataBind();

        if ((txtSearchBoxNo.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchBoxNo.Text != "")
        {

            SQL = "(LOWER(T.BOX_NO) LIKE '%" + txtSearchBoxNo.Text.ToLower() + "%') AND";
        }

        SQL = SQL + " (T.WORKFLOW_TYPE = '" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "') AND";
        
        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.BOX_NO_CODE ,T.BOX_NO AS \"Box Number\",T.DESCRIPTION AS \"Description\"  FROM MRP_WF_BOX_NOS T  " +
                      " WHERE (" + SQL + ") ORDER BY T.BOX_NO ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdBoxNos.DataSource = myOleDbDataReader;
            grdBoxNos.DataBind();

            pnlBoxNoGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("SetupBoxNo.aspx");
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

        if (txtBoxNo.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Box Number";
            Timer1.Enabled = true;
            return;
        }


        if (Session["BoxNoSetupMode"].ToString() == "NEW")
        {
            if (CheckMBoxNoAlreadyExist(txtBoxNo.Text))
            {
                lblMsg.Text = "Enetered Box Number Already Exists";
                Timer1.Enabled = true;
                return;
            }
        }

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["BoxNoSetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_BOX_NOS");
            }
            else if (Session["BoxNoSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_BOX_NOS");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            if (Session["BoxNoSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_BOX_NO_CODE", OracleType.Number, 5).Value = txtBoxNoCode.Text;
            }
            spProcess.Parameters.Add("V_BOX_NO", OracleType.VarChar, 100).Value = txtBoxNo.Text;
            spProcess.Parameters.Add("V_DESCRIPTION", OracleType.VarChar, 250).Value = txtDescription.Text;

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
        txtBoxNoCode.Text = "";
        txtBoxNo.Text = "";
        txtDescription.Text = "";

        txtBoxNoCode.Enabled = false;
        txtBoxNo.Enabled = false;
        txtDescription.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
      //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtBoxNoCode.Enabled = true;
        txtBoxNo.Enabled = true;
        txtDescription.Enabled = true;

        txtBoxNoCode.Text = "";
        txtBoxNo.Text = "";
        txtDescription.Text = "";

        btnSave.Enabled = true;

        Session["BoxNoSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtBoxNoCode.Text == "")
        {
            lblMsg.Text = "Please Select A Box Number";
            return;
        }


        txtBoxNo.Enabled = true;
        txtDescription.Enabled = true;

        btnSave.Enabled = true;

        Session["BoxNoSetupMode"] = "UPDATE";
    }

    protected void grdBoxNos_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtBoxNoCode.Text = grdBoxNos.SelectedRow.Cells[1].Text.Trim();
        txtBoxNo.Text = grdBoxNos.SelectedRow.Cells[2].Text.Trim();
        txtDescription.Text = grdBoxNos.SelectedRow.Cells[3].Text.Trim();


        btnAlter.Enabled = true;
    }

    protected void grdBoxNos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";
    }


    private bool CheckMBoxNoAlreadyExist(string sBoxNo)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";
        selectQuery = "SELECT BOX_NO FROM MRP_WF_BOX_NOS WHERE BOX_NO=:V_BOX_NO AND WORKFLOW_TYPE=:V_WORKFLOW_TYPE";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_BOX_NO", sBoxNo));
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
