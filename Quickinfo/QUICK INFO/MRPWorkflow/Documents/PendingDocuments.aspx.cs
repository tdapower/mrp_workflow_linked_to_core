//******************************************
// Author            :Tharindu Athapattu
// Date              :13/05/2013
// Reviewed By       :
// Description       :Pending Documents Setup Form
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

public partial class PendingDocuments : System.Web.UI.Page
{

    OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
    OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());





    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);

            ClearComponents();
            initializeValues();

            Session.Remove("PendingDocSetupMode");

            pnlPendingDocumentGrid.Visible = false;
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
        grdPendingDocument.DataSource = null;
        grdPendingDocument.DataBind();

        if ((txtSearchPendingDocument.Text == "") && (ddlSearchPendingDocCategory.SelectedValue.ToString() == "0"))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchPendingDocument.Text != "")
        {

            SQL = "(LOWER(T.PENDING_DOC_NAME) LIKE '%" + txtSearchPendingDocument.Text.ToLower() + "%') AND";
        }


        if (ddlSearchPendingDocCategory.SelectedValue.ToString() != "0")
        {

            SQL = SQL + "(T.DOC_CAT_CODE LIKE '%" + ddlSearchPendingDocCategory.SelectedValue.ToString() + "%') AND";
        }

        SQL = SQL + " (PDC.WORKFLOW_TYPE = '" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "') AND";


        SQL = SQL.Substring(0, SQL.Length - 3);

        String selectQuery = "";
        selectQuery = "  SELECT T.DOC_CAT_CODE AS \"Pending Category Code\" ,PDC.DOC_CAT_NAME AS \"Pending Category\", " +
                      " T.PENDING_DOC_CODE AS \"Pending Document Code\", T.PENDING_DOC_NAME AS \"Pending Document\"   " +
                      " FROM MRP_WF_PENDING_DOCS T " +
                      " INNER JOIN MRP_WF_PENDING_DOC_CATEGORY PDC ON T.DOC_CAT_CODE=PDC.DOC_CAT_CODE" +
                      " WHERE (" + SQL + ") ORDER BY PDC.DOC_CAT_NAME,T.PENDING_DOC_NAME ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdPendingDocument.DataSource = myOleDbDataReader;
            grdPendingDocument.DataBind();

            pnlPendingDocumentGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingDocuments.aspx");
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


        if (ddlPendingDocCategory.SelectedValue == "" || ddlPendingDocCategory.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the Pending Document Category";
            Timer1.Enabled = true;
            return;
        }

        if (txtPendingDocument.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Pending Document";
            Timer1.Enabled = true;
            return;
        }


        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;




            if (Session["PendingDocSetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_PENDING_DOCS");
                spProcess.Parameters.Add("V_DOC_CAT_CODE", OracleType.Number, 5).Value = ddlPendingDocCategory.SelectedValue.ToString();
                spProcess.Parameters.Add("V_PENDING_DOC_NAME", OracleType.VarChar, 100).Value = txtPendingDocument.Text;



                spProcess.Parameters.Add("V_PENDING_DOC_CODE", OracleType.Number, 20).Direction = ParameterDirection.Output;
                spProcess.Parameters["V_PENDING_DOC_CODE"].Direction = ParameterDirection.Output;

                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;



                spProcess.ExecuteNonQuery();

                txtPendingDocumentCode.Text = Convert.ToString(spProcess.Parameters["V_PENDING_DOC_CODE"].Value);
            }
            else if (Session["PendingDocSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_PENDING_DOCS");

                spProcess.Parameters.Add("V_PENDING_DOC_CODE", OracleType.Number, 5).Value = txtPendingDocumentCode.Text;
                spProcess.Parameters.Add("V_DOC_CAT_CODE", OracleType.Number, 5).Value = ddlPendingDocCategory.SelectedValue.ToString();
                spProcess.Parameters.Add("V_PENDING_DOC_NAME", OracleType.VarChar, 100).Value = txtPendingDocument.Text;

                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;



                spProcess.ExecuteNonQuery();
            }

            conProcess.Close();


            uploadDocument(txtPendingDocumentCode.Text);



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



    private void uploadDocument(string docCode)
    {

        try
        {
            if (!fileUploadDocument.HasFile)
            {
                return;
            }
            int intlength = fileUploadDocument.PostedFile.ContentLength;
            Byte[] byteData = new Byte[intlength];
            fileUploadDocument.PostedFile.InputStream.Read(byteData, 0, intlength);




            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            OracleParameter blobParameterDoc = new OracleParameter();

            string updateString = "";
            updateString = "UPDATE  MRP_WF_PENDING_DOCS " +
                       " SET " +
                           " DOCUMENT=:doc," +
                            " DOCUMENT_NAME='" + fileUploadDocument.FileName + "' " +
                       " WHERE PENDING_DOC_CODE=" + docCode + "";

            blobParameterDoc.ParameterName = "doc";
            blobParameterDoc.Direction = ParameterDirection.Input;

            blobParameterDoc.Value = byteData;


            spProcess = new OracleCommand(updateString, conProcess);
            spProcess.Parameters.Add(blobParameterDoc);


            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {

        }
    }

    private void ClearComponents()
    {
        ddlPendingDocCategory.SelectedValue = "0";
        txtPendingDocumentCode.Text = "";
        txtPendingDocument.Text = "";


        ddlPendingDocCategory.Enabled = false;
        txtPendingDocumentCode.Enabled = false;
        txtPendingDocument.Enabled = false;

        fileUploadDocument.Enabled = false;
        btnViewDocument.Visible = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {

        ddlPendingDocCategory.Enabled = true;
        txtPendingDocumentCode.Enabled = true;
        txtPendingDocument.Enabled = true;


        ddlPendingDocCategory.SelectedValue = "0";
        txtPendingDocumentCode.Text = "";
        txtPendingDocument.Text = "";

        fileUploadDocument.Enabled = true;
        btnViewDocument.Visible = false;

        btnSave.Enabled = true;

        Session["PendingDocSetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtPendingDocumentCode.Text == "")
        {
            lblMsg.Text = "Please Select A Document";
            return;
        }

        ddlPendingDocCategory.Enabled = true;
        //txtPendingDocumentCode.Enabled = true;
        txtPendingDocument.Enabled = true;


        fileUploadDocument.Enabled = true;
        btnViewDocument.Visible = true;

        btnSave.Enabled = true;

        Session["PendingDocSetupMode"] = "UPDATE";
    }

    protected void grdPendingDocument_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlPendingDocCategory.SelectedValue = grdPendingDocument.SelectedRow.Cells[1].Text.Trim();
        txtPendingDocumentCode.Text = grdPendingDocument.SelectedRow.Cells[3].Text.Trim();
        txtPendingDocument.Text = grdPendingDocument.SelectedRow.Cells[4].Text.Trim();

        btnAlter.Enabled = true;

        btnViewDocument.Visible = true;
        //btnViewDocument.Attributes.Add("onClick", "window.open(../Common/CommonDocumentViewer.aspx?DocumentType=PendingDocs&DocId=" + txtPendingDocumentCode.Text+")");
        btnViewDocument.NavigateUrl = "../Common/CommonDocumentViewer.aspx?DocumentType=PendingDocs&DocId=" + txtPendingDocumentCode.Text;


    }

    protected void grdPendingDocument_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;

    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";

        ddlPendingDocCategory.Items.Clear();
        ddlPendingDocCategory.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchPendingDocCategory.Items.Clear();
        ddlSearchPendingDocCategory.Items.Add(new ListItem("--- Select One ---", "0"));

        loadDocs();
    }



    private void loadDocs()
    {
        ddlPendingDocCategory.Items.Clear();
        ddlPendingDocCategory.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchPendingDocCategory.Items.Clear();
        ddlSearchPendingDocCategory.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";
        selectQuery = " SELECT T.DOC_CAT_CODE,T.DOC_CAT_NAME   FROM MRP_WF_PENDING_DOC_CATEGORY T WHERE T.WORKFLOW_TYPE=:V_WORKFLOW_TYPE ORDER BY T.DOC_CAT_NAME";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_WORKFLOW_TYPE", Request.Cookies["WORKFLOW_CHOICE"].Value.ToString()));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlPendingDocCategory.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlSearchPendingDocCategory.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }
}
