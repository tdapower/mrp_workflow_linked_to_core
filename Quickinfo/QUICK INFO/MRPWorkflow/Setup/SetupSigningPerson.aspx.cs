//******************************************
// Author            :Tharindu Athapattu
// Date              :09/05/2013
// Reviewed By       :
// Description       : Signing Person Setup Form
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

public partial class SetupSigningPerson : System.Web.UI.Page
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

            Session.Remove("SigningPersonSetupMode");

            pnlSigningPersonGrid.Visible = false;
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
        grdSigningPerson.DataSource = null;
        grdSigningPerson.DataBind();

        if ((txtSearchSigningPerson.Text == "") && (txtSearchDesignation.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchSigningPerson.Text != "")
        {

            SQL = "(LOWER(T.SIGN_PERSON_NAME) LIKE '%" + txtSearchSigningPerson.Text.ToLower() + "%') AND";
        }
        if (txtSearchDesignation.Text != "")
        {

            SQL = "(LOWER(T.DESIGNATION) LIKE '%" + txtSearchDesignation.Text.ToLower() + "%') AND";
        }



        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.SIGN_PERSON_CODE AS \"Signing Person Code\" ,T.SIGN_PERSON_NAME AS \"Name\"  ,T.DESIGNATION AS \"Designation\" FROM MRP_WF_SIGN_PERSON T  " +
                      " WHERE (" + SQL + ") ORDER BY T.SIGN_PERSON_NAME ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdSigningPerson.DataSource = myOleDbDataReader;
            grdSigningPerson.DataBind();

            pnlSigningPersonGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("SetupSigningPerson.aspx");
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

        if (txtSigningPerson.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Name of Person";
            Timer1.Enabled = true;
            return;
        }

        if (txtDesignation.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Designation";
            Timer1.Enabled = true;
            return;
        }

        if (Session["SigningPersonSetupMode"].ToString() == "NEW")
        {
            if (CheckPersonNameAlreadyExist(txtSigningPerson.Text, txtDesignation.Text))
            {
                lblMsg.Text = "Enetered Signing Person Already Exists";
                Timer1.Enabled = true;
                return;
            }
        }

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            if (Session["SigningPersonSetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_SIGN_PERSON");
            }
            else if (Session["SigningPersonSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_SIGN_PERSON");
            }


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            if (Session["SigningPersonSetupMode"].ToString() == "UPDATE")
            {
                spProcess.Parameters.Add("V_SIGN_PERSON_CODE", OracleType.Number, 5).Value = txtSigningPersonCode.Text;
            }
            spProcess.Parameters.Add("V_SIGN_PERSON_NAME", OracleType.VarChar, 250).Value = txtSigningPerson.Text;
            spProcess.Parameters.Add("V_DESIGNATION", OracleType.VarChar, 250).Value = txtDesignation.Text;


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
        txtSigningPersonCode.Text = "";
        txtSigningPerson.Text = "";
        txtDesignation.Text = "";

        txtSigningPersonCode.Enabled = false;
        txtSigningPerson.Enabled = false;
        txtDesignation.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtSigningPersonCode.Enabled = true;
        txtSigningPerson.Enabled = true;
        txtDesignation.Enabled = true;

        txtSigningPersonCode.Text = "";
        txtSigningPerson.Text = "";
        txtDesignation.Text = "";

        btnSave.Enabled = true;

        Session["SigningPersonSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtSigningPersonCode.Text == "")
        {
            lblMsg.Text = "Please Select A Person";
            return;
        }


        txtSigningPerson.Enabled = true;
        txtDesignation.Enabled = true;

        btnSave.Enabled = true;

        Session["SigningPersonSetupMode"] = "UPDATE";
    }

    protected void grdSigningPerson_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtSigningPersonCode.Text = grdSigningPerson.SelectedRow.Cells[1].Text.Trim();
        txtSigningPerson.Text = grdSigningPerson.SelectedRow.Cells[2].Text.Trim();
        txtDesignation.Text = grdSigningPerson.SelectedRow.Cells[3].Text.Trim();

        btnAlter.Enabled = true;
    }

    protected void grdSigningPerson_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";
    }


    private bool CheckPersonNameAlreadyExist(string sPersonName, string sDesignation)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT SIGN_PERSON_NAME FROM MRP_WF_SIGN_PERSON WHERE SIGN_PERSON_NAME='" + sPersonName + "' AND DESIGNATION='" + sDesignation + "'";

        cmd.CommandText = selectQuery;

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
