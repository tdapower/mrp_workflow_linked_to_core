//******************************************
// Author            :Tharindu Athapattu
// Date              :12/06/2013
// Reviewed By       :
// Description       : Add scanned proposals to box
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

public partial class AddProposalsToBox : System.Web.UI.Page
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

            Session.Remove("AssignToBoxSetupMode");

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

        if ((txtSearchBoxNo.Text == "") && (txtSearchPolicy.Text == ""))
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

        if (txtSearchPolicy.Text != "")
        {

            SQL = " '" + txtSearchPolicy.Text.ToLower() + "'  IN (SELECT LOWER(POLICY_NO) FROM MRP_WF_BOX_PROPOSALS WHERE BOX_NO_CODE= T.BOX_NO_CODE )  AND";
        }

        SQL = SQL + " (T.WORKFLOW_TYPE = '" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "') AND";

        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.BOX_NO_CODE ,T.BOX_NO AS \"Box Number\",T.DESCRIPTION AS \"Description\"  FROM MRP_WF_BOX_NOS T  " +
            " WHERE (" + SQL + ")   ORDER BY T.BOX_NO ASC";

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
        Response.Redirect("AddProposalsToBox.aspx");
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

        if (Session["AssignToBoxSetupMode"].ToString() == "UPDATE")
        {
            deletePreviousData(txtBoxNoCode.Text);
        }

        try
        {
            for (int i = 0; i < lstSelected.Items.Count; i++)
            {
                saveSelectedProposals(lstSelected.Items[i].Value, lstSelected.Items[i].Text);
            }
            lblMsg.Text = "Successfully saved";
            Timer1.Enabled = true;
            // ClearComponents();
            SearchData();
            LockComponents();

        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }

    }


    private void LockComponents()
    {

        txtBoxNoCode.Enabled = false;
        txtBoxNo.Enabled = false;
        txtDescription.Enabled = false;

        lstSelected.Enabled = false;
        lstUnselected.Enabled = false;


        btnAlter.Enabled = false;
        btnSave.Enabled = false;

        btnAdd.Enabled = false;
        btnRemove.Enabled = false;

    }


    private void saveSelectedProposals(string sProposalNo, string sPolicyNo)
    {

        OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        conProcess.Open();
        OracleCommand spProcess = null;


        spProcess = new OracleCommand("INSERT_MRP_WF_BOX_PROPOSALS");



        spProcess.CommandType = CommandType.StoredProcedure;
        spProcess.Connection = conProcess;

        spProcess.Parameters.Add("V_BOX_NO_CODE", OracleType.Number, 5).Value = txtBoxNoCode.Text;
        spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = sProposalNo;
        spProcess.Parameters.Add("V_POLICY_NO", OracleType.VarChar, 20).Value = sPolicyNo;


        spProcess.ExecuteNonQuery();
        conProcess.Close();



    }


    private void deletePreviousData(string sBoxCodeNo)
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            string strQuery = "";

            strQuery = "DELETE FROM MRP_WF_BOX_PROPOSALS WHERE BOX_NO_CODE=:V_BOX_NO_CODE" ;

            spProcess = new OracleCommand(strQuery, conProcess);

            spProcess.Parameters.Add(new OracleParameter("V_BOX_NO_CODE", sBoxCodeNo));



            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {

        }

    }

    private void ClearComponents()
    {
        txtBoxNoCode.Text = "";
        txtBoxNo.Text = "";
        txtDescription.Text = "";

        lstSelected.Items.Clear();
        lstUnselected.Items.Clear();

        txtBoxNoCode.Enabled = false;
        txtBoxNo.Enabled = false;
        txtDescription.Enabled = false;

        lstSelected.Enabled = false;
        lstUnselected.Enabled = false;


        btnAlter.Enabled = false;
        btnSave.Enabled = false;

        btnAdd.Enabled = false;
        btnRemove.Enabled = false;
        //  btnCancel.Enabled = false;
    }



    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtBoxNoCode.Text == "")
        {
            lblMsg.Text = "Please Select A Box Number";
            return;
        }

        btnAdd.Enabled = true;
        btnRemove.Enabled = true;
        btnSave.Enabled = true;


        lstSelected.Enabled = true;
        lstUnselected.Enabled = true;



        Session["AssignToBoxSetupMode"] = "UPDATE";
    }

    protected void grdBoxNos_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtBoxNoCode.Text = grdBoxNos.SelectedRow.Cells[1].Text.Trim();
        txtBoxNo.Text = grdBoxNos.SelectedRow.Cells[2].Text.Trim();
        txtDescription.Text = grdBoxNos.SelectedRow.Cells[3].Text.Trim();

        PopulateUnSelectedProposals(txtBoxNoCode.Text);
        PopulateSelectedProposals(txtBoxNoCode.Text);

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




    private void PopulateUnSelectedProposals(string iBoxNoCode)
    {

        lstUnselected.DataSource = null;
        lstUnselected.Items.Clear();
        lstUnselected.Items.Add(new ListItem("----------Select from the list----------", "0"));

        string sql = "SELECT WF.PROPOSAL_NO,WF.POLICY_NO  FROM MRP_WORKFLOW WF WHERE WF.IS_SCANNED=1 AND "+
            " (WF.STATUS_CODE!=23) AND "+
            " WF.WORKFLOW_TYPE = '" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "' AND " +
            " WF.PROPOSAL_NO NOT IN ( " +
                        " SELECT BP.PROPOSAL_NO  FROM MRP_WF_BOX_PROPOSALS BP) " +
                        " ORDER BY WF.PROPOSAL_NO";

        lstUnselected.DataSource = GetProposals(sql);
        lstUnselected.DataTextField = "POLICY_NO";
        lstUnselected.DataValueField = "PROPOSAL_NO";
        lstUnselected.DataBind();
    }

    private void PopulateSelectedProposals(string iBoxNoCode)
    {

        lstSelected.DataSource = null;
        lstSelected.Items.Clear();
        lstSelected.Items.Add(new ListItem("----------Select from the list----------", "0"));


        string sql = "SELECT BP.PROPOSAL_NO,BP.POLICY_NO   FROM MRP_WF_BOX_PROPOSALS BP WHERE BP.BOX_NO_CODE= " + iBoxNoCode + " " +
                   " ORDER BY BP.PROPOSAL_NO";

        lstSelected.DataSource = GetProposals(sql);
        lstSelected.DataTextField = "POLICY_NO";
        lstSelected.DataValueField = "PROPOSAL_NO";
        lstSelected.DataBind();
    }

    protected DataTable GetProposals(string selectString)
    {
        OracleDataAdapter adapter = new OracleDataAdapter(selectString, ConfigurationManager.ConnectionStrings["ORAWF"].ConnectionString);
        DataTable result = new DataTable();
        adapter.Fill(result);
        return result;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstUnselected.Items.Count; i++)
        {
            if (lstUnselected.Items[i].Selected)
            {
                if (lstSelected.Items.FindByValue(lstUnselected.Items[i].Value) == null)
                {
                    lstSelected.Items.Add(new ListItem(lstUnselected.Items[i].Text, lstUnselected.Items[i].Value));
                }
            }
        }
        for (int i = lstUnselected.Items.Count - 1; i >= 0; i--)
        {
            if (lstUnselected.Items[i].Selected)
            {
                lstUnselected.Items.Remove(lstUnselected.Items[i]);
            }
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstSelected.Items.Count; i++)
        {
            if (lstSelected.Items[i].Selected)
            {
                if (lstUnselected.Items.FindByValue(lstSelected.Items[i].Value) == null)
                {
                    lstUnselected.Items.Add(new ListItem(lstSelected.Items[i].Text, lstSelected.Items[i].Value));
                }
            }
        }

        for (int i = lstSelected.Items.Count - 1; i >= 0; i--)
        {
            if (lstSelected.Items[i].Selected)
            {
                lstSelected.Items.Remove(lstSelected.Items[i]);
            }
        }


    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }
}
