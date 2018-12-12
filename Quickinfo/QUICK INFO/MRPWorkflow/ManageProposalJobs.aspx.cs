//******************************************
// Author            :Tharindu Athapattu
// Date              :25/06/2013
// Reviewed By       :
// Description       : Create job numbers and assign to users Form
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
using System.Windows.Forms;

public partial class ManageProposalJobs : System.Web.UI.Page
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


            btnCancelJob.Attributes.Add("onClick", "if(confirm('Are you sure to cancel this Job?','MRP Workflow')){}else{return false}");

            btnRemoveFST.Attributes.Add("onClick", "if(confirm('Are you sure to remove the Fast Track of this Job?','MRP Workflow')){}else{return false}");



            ClearComponents();
            initializeValues();
            //loadJobSummary();
            Session.Remove("ProposalJobSetupMode");

            pnlProposalJobsGrid.Visible = false;
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

    protected void loadJobSummary()
    {
        try
        {
            grdJobSummary.DataSource = null;
            grdJobSummary.DataBind();

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            OracleDataAdapter da = new OracleDataAdapter();
            string sql = "";

            if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
            {
                sql = "SELECT * FROM MRP_WF_GET_PROP_SUMMARY";
            }
            else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
            {
                sql = "SELECT * FROM MCR_WF_GET_PROP_SUMMARY";
            }

            da.SelectCommand = new OracleCommand(sql, con);
            con.Open();


            OracleDataReader dr = da.SelectCommand.ExecuteReader();

            if (dr.HasRows)
            {
                grdJobSummary.DataSource = dr;
                grdJobSummary.DataBind();
            }

            con.Close();

        }
        catch (Exception ex)
        {

        }
    }



    protected void ddlBankType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankType.SelectedValue != "0")
        {
            loadBankName(ddlBankType.SelectedValue);
        }
    }

    protected void ddlBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankType.SelectedValue != "0" || ddlBankName.SelectedValue != "0")
        {
            loadBankBranch(ddlBankType.SelectedValue, ddlBankName.SelectedValue);
        }
    }
    private void loadAssignedTo()
    {
        ddlAssignedTo.Items.Clear();
        ddlAssignedTo.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchAssignedTo.Items.Clear();
        ddlSearchAssignedTo.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        string MRPUserCodes = System.Configuration.ConfigurationManager.AppSettings["MRPUserCodes"].ToString();


        selectQuery = "   SELECT T.USER_CODE,T.USER_NAME FROM WF_ADMIN_USERS T  " +
           " WHERE T.USER_ROLE_CODE IN (" + MRPUserCodes + ") AND T.STATUS=1 ORDER BY T.USER_NAME ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {

                ddlAssignedTo.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlSearchAssignedTo.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void loadBankData()
    {

        ddlBranch.Items.Clear();
        ddlBranch.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "SELECT  BANK_TYPE,  BANK_NAME,BRANCH_NAME FROM MRP_WF_BANKS ORDER BY BANK_TYPE ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {

                ddlBranch.Items.Add(new ListItem(dr[2].ToString(), dr[2].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    //New Development - 23/3/2016- Load Broker Details
    private void loadBrokerData()
    {
        ddlBrokerCode.Items.Clear();
        ddlBrokerCode.Items.Add(new ListItem("--- Select One ---", "0"));


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "SELECT  PRT.PTY_PARTY_CODE AGENT_CODE,  PTV.PVR_BUSINESS_NAME AGENT_NAME FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, T_STAKE_HOLDER_FUNCTION STF WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBrokerCode.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }



    private void loadBankTypes()
    {
        ddlBankType.Items.Clear();
        ddlBankType.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        selectQuery = "SELECT  BANK_TYPE_NAME FROM mrp_wf_bank_type ORDER BY BANK_TYPE_NAME ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBankType.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));


                if (dr[0].ToString() == "OTHER BANK")
                {
                    ddlBankName.Enabled = true;
                }
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }




    private void loadProposalModes()
    {
        ddlModeOfProposal.Items.Clear();
        ddlModeOfProposal.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "SELECT PROPOSAL_MODE_ID,PROPOSAL_MODE_NAME FROM MRP_WF_PROPOSAL_MODE ORDER BY PROPOSAL_MODE_NAME ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlModeOfProposal.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void loadBusinessChannels()
    {
        ddlBusinessChannel.Items.Clear();
        ddlBusinessChannel.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "SELECT BC_ID,BC_NAME FROM MRP_WF_BUSINESS_CHANNEL ORDER BY BC_NAME ASC";



        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBusinessChannel.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void loadBankName(string sBankType)
    {
        ddlBankName.Items.Clear();
        ddlBankName.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";

        selectQuery = "SELECT DISTINCT BANK_NAME FROM MRP_WF_BANKS " +
            " WHERE BANK_TYPE=:V_BANK_TYPE " +
            " ORDER BY BANK_NAME ASC";


        OracleCommand cmd = new OracleCommand(selectQuery, con);

        cmd.Parameters.Add(new OracleParameter("V_BANK_TYPE", sBankType));



        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBankName.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }



    private void loadBankBranch(string sBankType, string sBankName)
    {
        ddlBranch.Items.Clear();
        ddlBranch.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        selectQuery = "SELECT DISTINCT BRANCH_NAME FROM MRP_WF_BANKS " +
            " WHERE BANK_TYPE=:V_BANK_TYPE AND BANK_NAME=:V_BANK_NAME " +
            " ORDER BY BRANCH_NAME ASC";

        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_BANK_TYPE", sBankType));
        cmd.Parameters.Add(new OracleParameter("V_BANK_NAME", sBankName));



        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlBranch.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private string getBankCode(string sBankType, string sBankName, string sBranch)
    {
        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        selectQuery = "SELECT BANK_CODE FROM MRP_WF_BANKS " +
            " WHERE BANK_TYPE=:V_BANK_TYPE AND BANK_NAME=:V_BANK_NAME AND BRANCH_NAME=:V_BRANCH_NAME ";



        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_BANK_TYPE", sBankType));
        cmd.Parameters.Add(new OracleParameter("V_BANK_NAME", sBankName));
        cmd.Parameters.Add(new OracleParameter("V_BRANCH_NAME", sBranch));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            returnValue = dr[0].ToString();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
    }

    private string getMaxJobNo()
    {
        string returnValue = "0";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        string workflowType = "";

        workflowType = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();



        selectQuery = "select max(t.JOB_NO) from MRP_WF_PROPOSAL_JOBS t WHERE SUBSTR(t.JOB_NO,5,4)=to_char(SYSDATE,'YYYY') " +
            " AND WORKFLOW_TYPE=:V_WORKFLOW_TYPE";


        OracleCommand cmd = new OracleCommand(selectQuery, con);

        cmd.Parameters.Add(new OracleParameter("V_WORKFLOW_TYPE", workflowType));


        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            if (dr[0].ToString() != "")
            {
                returnValue = Right(dr[0].ToString(), 4);

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
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
        grdProposalJobs.DataSource = null;
        grdProposalJobs.DataBind();

        if ((txtSearchJobNumber.Text == "") && (txtSearchNIC1.Text == "") && (txtSearchNIC2.Text == "") && (ddlSearchAssignedTo.SelectedValue == "" || ddlSearchAssignedTo.SelectedValue == "0") && (txtfromDate.Text == "") && (txtToDate.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchJobNumber.Text != "")
        {

            SQL = "(LOWER(T.JOB_NO) LIKE '%" + txtSearchJobNumber.Text.ToLower() + "%') AND";
        }
        if (txtSearchNIC1.Text != "")
        {

            SQL = "(LOWER(T.NIC1) LIKE '%" + txtSearchNIC1.Text.ToLower() + "%') AND";
        }
        if (txtSearchNIC2.Text != "")
        {

            SQL = "(LOWER(T.NIC2) LIKE '%" + txtSearchNIC2.Text.ToLower() + "%') AND";
        }
        if (ddlSearchAssignedTo.SelectedValue != "" && ddlSearchAssignedTo.SelectedValue != "0")
        {
            SQL = SQL + "(T.ASSIGNED_USER_CODE = '" + ddlSearchAssignedTo.SelectedValue + "') AND";
        }

        if (txtfromDate.Text != "" && txtToDate.Text != "")
        {

            SQL = "t.created_date>to_date('" + txtfromDate.Text.ToLower() + "','DD/MM/YYYY') AND  t.created_date<to_date('" + txtToDate.Text.ToLower() + "','DD/MM/YYYY') AND";
        }

        SQL = SQL + " (T.WORKFLOW_TYPE = '" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "') AND";

        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";

        selectQuery = "   SELECT T.JOB_NO AS \"Job No\" ,T.PROPOSAL_NO AS \"Proposal No\",T.NIC1 AS \"NIC 1\" " +
            " ,T.NIC2 AS \"NIC 2\",T.BANK_CODE AS \"Bank Code\" ,T.ASSIGNED_USER_CODE, " +
            " U.USER_NAME  AS \"Assigned User\",T.CANCELLED,CASE WHEN (T.CANCELLED)=1 THEN 'Cancelled' ELSE '' END AS Status " +
            " ,t.created_date AS  \"Created Date\"" +
            ",(SELECT PTV.PVR_BUSINESS_NAME AGENT_NAME FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, T_STAKE_HOLDER_FUNCTION STF WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL AND PRT.PTY_PARTY_CODE = T.BROKER_CODE)BROKER_NAME" +
            " FROM MRP_WF_PROPOSAL_JOBS T  " +
            "INNER JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER_CODE=U.USER_CODE" +
            " WHERE (" + SQL + ") ORDER BY T.JOB_NO ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdProposalJobs.DataSource = myOleDbDataReader;
            grdProposalJobs.DataBind();

            pnlProposalJobsGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageProposalJobs.aspx");
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

    protected void btnCancelJob_Click(object sender, EventArgs e)
    {
        if (checkIsUserMRPSuperuser())
        {
            try
            {
                OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                conProcess.Open();

                string updateString = "";




                OracleCommand cmd = null;

                if (txtProposalNumber.Text == "")
                {
                    updateString = "UPDATE  MRP_WF_PROPOSAL_JOBS " +
                               " SET " +
                               " CANCELLED=1 " +
                               " WHERE JOB_NO=:V_JOB_NO";
                    cmd = new OracleCommand(updateString, conProcess);

                    cmd.Parameters.Add(new OracleParameter("V_JOB_NO", txtJobNumber.Text));
                }
                else
                {
                    updateString = "UPDATE  MRP_WF_PROPOSAL_JOBS " +
                              " SET " +
                              " CANCELLED=1, " +
                              " PROPOSAL_NO=:V_PROPOSAL_NO " +
                              " WHERE JOB_NO=:V_JOB_NO";
                    cmd = new OracleCommand(updateString, conProcess);

                    cmd.Parameters.Add(new OracleParameter("V_JOB_NO", txtJobNumber.Text));
                    cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", "C_" + txtProposalNumber.Text));
                }



                cmd.ExecuteNonQuery();
                conProcess.Close();

                if (txtProposalNumber.Text != "")
                {
                    CancelTheProposal(txtProposalNumber.Text);
                    UpdateTheCancelledProposalRelatedTables(txtProposalNumber.Text);
                }


                lblMsg.Text = "Job Successfully Cancelled.";
                Timer1.Enabled = true;

            }
            catch (Exception ee)
            {
                lblMsg.Text = "Error While Cancelling The Job.";
                Timer1.Enabled = true;
            }

            ClearComponents();
            SearchData();
        }
        else
        {
            lblMsg.Text = "This Status is only available for MRP Supervisors";
            Timer1.Enabled = true;
        }


    }



    private void CancelTheProposal(string sProposalNo)
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();


            string updateString = "";



            updateString = "UPDATE  MRP_WORKFLOW " +
           " SET " +
           " CANCELLED=1 , " +
           " PROPOSAL_NO=:V_NEW_PROPOSAL_NO " +
           " WHERE PROPOSAL_NO=:V_PROPOSAL_NO";


            OracleCommand cmd = new OracleCommand(updateString, conProcess);

            cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));
            cmd.Parameters.Add(new OracleParameter("V_NEW_PROPOSAL_NO", "C_" + sProposalNo));



            cmd.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ee)
        {

        }

    }






    private void UpdateTheCancelledProposalRelatedTables(string sProposalNo)
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            spProcess = new OracleCommand("UPDATE_CANCELLED_PROPOSAL");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = sProposalNo;


            spProcess.ExecuteNonQuery();
            conProcess.Close();


        }
        catch (Exception ex)
        {

        }

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }


    protected void btnRemoveFST_Click(object sender, EventArgs e)
    {
        if (checkIsFastTrackJob(txtJobNumber.Text))
        {


            string MRPManagerUserName = "";



            if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
            {
                MRPManagerUserName = System.Configuration.ConfigurationManager.AppSettings["MRPManagerUserName"].ToString();
            }
            else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
            {
                MRPManagerUserName = System.Configuration.ConfigurationManager.AppSettings["MCRManagerUserName"].ToString();
            }







            UserAuthentication userAuthentication = new UserAuthentication();
            string UserName = Context.User.Identity.Name;


            if (!MRPManagerUserName.Contains(Right(UserName, (UserName.Length) - 5)))
            {
                lblMsg.Text = "Removing Fast Track facility is only available for MRP Managers";
                Timer1.Enabled = true;
                chkIsFastTrack.Checked = true;
                return;
            }
            else
            {
                removeFromFST();
                sendFastTrackRemoveMailToUsers();
                ClearComponents();
                SearchData();

                lblMsg.Text = "Job successfully removed from Fast Track category";
                Timer1.Enabled = true;
            }


        }
        else
        {
            lblMsg.Text = "This job is not a Fast Track job";
            Timer1.Enabled = true;

        }
    }


    private void removeFromFST()
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();


            string updateString = "";
            updateString = "UPDATE  MRP_WF_PROPOSAL_JOBS " +
                       " SET " +
                           " IS_FAST_TRACK=0 " +
                       " WHERE JOB_NO=:V_JOB_NO";

            OracleCommand spProcess = new OracleCommand(updateString, conProcess);


            spProcess.Parameters.Add(new OracleParameter("V_JOB_NO", txtJobNumber.Text));

            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {



        if (txtNIC1.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter NIC 1";
            Timer1.Enabled = true;
            return;
        }
        else
        {
            Page page = HttpContext.Current.CurrentHandler as Page;

            if (checkIsNICIsBlacklisted(txtNIC1.Text))
            {

                string script = "<script type=\"text/javascript\">" +
      "alert('Entered NIC 1 is blacklisted')</script>";
                page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", script);



            }

            if (checkIsNICIsBlacklisted(txtNIC2.Text))
            {
                string script = "<script type=\"text/javascript\">" +
"alert('Entered NIC 2 is blacklisted')</script>";
                page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", script);


            }
        }



        if (ddlBankType.SelectedValue == "0" || ddlBankName.SelectedValue == "0" || ddlBranch.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select Bank Details";
            Timer1.Enabled = true;
            return;
        }
        if (ddlAssignedTo.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the Assigned Person";
            Timer1.Enabled = true;
            return;
        }



        if (txtProposalNumber.Text != "")
        {
            if (CheckProposalNoAlreadyExist(txtProposalNumber.Text))
            {
                lblMsg.Text = "Enetered Proposal Number Already Exists";
                Timer1.Enabled = true;
                return;
            }
        }

        if (txtProposalNumber.Text != "")
        {
            if (!CheckProposalNoExistInMRPSystem(txtProposalNumber.Text))
            {
                lblMsg.Text = "Enetered Proposal Number Doesn't Exists in MRP System";
                Timer1.Enabled = true;
                return;
            }
        }

        if (Session["ProposalJobSetupMode"].ToString() == "UPDATE")
        {
            if (txtProposalNumber.Text != "")
            {
                if (CheckProposalNoAlreadyExist(txtProposalNumber.Text))
                {
                    lblMsg.Text = "Enetered Proposal Number Already Exists";
                    Timer1.Enabled = true;
                    return;
                }
            }
        }

        if (txtProposalNumber.Text != "")
        {
            if (CheckProposalNoAvailable(txtProposalNumber.Text))
            {
                lblMsg.Text = "Enetered Proposal Number Not Available in MRP Workflow";
                Timer1.Enabled = true;
                return;
            }
        }
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;



            if (Session["ProposalJobSetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_PROPOSAL_JOBS");
                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_JOB_NO", OracleType.VarChar, 40).Value = txtJobNumber.Text;
                spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = txtProposalNumber.Text.Trim();
                spProcess.Parameters.Add("V_NIC1", OracleType.VarChar, 15).Value = txtNIC1.Text;
                spProcess.Parameters.Add("V_NIC2", OracleType.VarChar, 15).Value = txtNIC2.Text;
                spProcess.Parameters.Add("V_BANK_CODE", OracleType.VarChar, 50).Value = getBankCode(ddlBankType.SelectedValue, ddlBankName.SelectedValue, ddlBranch.SelectedValue);
                spProcess.Parameters.Add("V_ASSIGNED_USER_CODE", OracleType.VarChar, 100).Value = ddlAssignedTo.SelectedValue;
                spProcess.Parameters.Add("V_BROKER_CODE", OracleType.VarChar, 100).Value = ddlBrokerCode.SelectedValue;

                if (chkIsFastTrack.Checked == true)
                {
                    spProcess.Parameters.Add("V_IS_FAST_TRACK", OracleType.Number, 5).Value = 1;
                }
                else
                {
                    spProcess.Parameters.Add("V_IS_FAST_TRACK", OracleType.Number, 5).Value = 0;
                }


                if (chkIsFreeCoverLimitProposal.Checked == true)
                {
                    spProcess.Parameters.Add("V_IS_FREE_CVR_LIMIT_PRPSL", OracleType.Number, 1).Value = 1;
                }
                else
                {
                    spProcess.Parameters.Add("V_IS_FREE_CVR_LIMIT_PRPSL", OracleType.Number, 1).Value = 0;
                }



                spProcess.Parameters.Add("V_PROPOSAL_MODE_ID", OracleType.Number, 5).Value = ddlModeOfProposal.SelectedValue;

                spProcess.Parameters.Add("V_BUSINESS_CHNL_ID", OracleType.Number, 5).Value = ddlBusinessChannel.SelectedValue;
                spProcess.Parameters.Add("V_WORKFLOW_TYPE", OracleType.VarChar, 10).Value = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();

            }
            else if (Session["ProposalJobSetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_PROPOSAL_JOBS");
                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_JOB_NO", OracleType.VarChar, 40).Value = txtJobNumber.Text;
                spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = txtProposalNumber.Text.Trim();
                spProcess.Parameters.Add("V_NIC1", OracleType.VarChar, 15).Value = txtNIC1.Text;
                spProcess.Parameters.Add("V_NIC2", OracleType.VarChar, 15).Value = txtNIC2.Text;
                spProcess.Parameters.Add("V_BANK_CODE", OracleType.VarChar, 50).Value = getBankCode(ddlBankType.SelectedValue, ddlBankName.SelectedValue, ddlBranch.SelectedValue);
                spProcess.Parameters.Add("V_ASSIGNED_USER_CODE", OracleType.VarChar, 100).Value = ddlAssignedTo.SelectedValue;
                spProcess.Parameters.Add("V_BROKER_CODE", OracleType.VarChar, 100).Value = ddlBrokerCode.SelectedValue;

                if (chkIsFastTrack.Checked == true)
                {
                    spProcess.Parameters.Add("V_IS_FAST_TRACK", OracleType.Number, 5).Value = 1;
                }
                else
                {
                    spProcess.Parameters.Add("V_IS_FAST_TRACK", OracleType.Number, 5).Value = 0;
                }


                if (chkIsFreeCoverLimitProposal.Checked == true)
                {
                    spProcess.Parameters.Add("V_IS_FREE_CVR_LIMIT_PRPSL", OracleType.Number, 1).Value = 1;
                }
                else
                {
                    spProcess.Parameters.Add("V_IS_FREE_CVR_LIMIT_PRPSL", OracleType.Number, 1).Value = 0;
                }

                spProcess.Parameters.Add("V_PROPOSAL_MODE_ID", OracleType.Number, 5).Value = ddlModeOfProposal.SelectedValue;

                spProcess.Parameters.Add("V_BUSINESS_CHNL_ID", OracleType.Number, 5).Value = ddlBusinessChannel.SelectedValue;
                spProcess.Parameters.Add("V_WORKFLOW_TYPE", OracleType.VarChar, 10).Value = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();


            }

            spProcess.ExecuteNonQuery();
            conProcess.Close();


            if (chkIsFastTrack.Checked == true)
            {
                sendFastTrackMailToUsers();
            }
            else
            {
                sendJobAllocatedMailToUsers();
            }


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
        lblJobStatus.Text = "";

        txtJobNumber.Text = "";
        txtProposalNumber.Text = "";
        txtNIC1.Text = "";
        txtNIC2.Text = "";

        ddlBankType.Items.Clear();
        ddlBankName.Items.Clear();
        ddlBranch.Items.Clear();
        chkIsFastTrack.Checked = false;
        chkIsFreeCoverLimitProposal.Checked = false;
        ddlBrokerCode.Items.Clear();

        txtJobNumber.Enabled = false;
        txtProposalNumber.Enabled = false;
        txtNIC1.Enabled = false;
        txtNIC2.Enabled = false;

        ddlBankType.Enabled = false;
        ddlBankName.Enabled = false;
        ddlBranch.Enabled = false;
        ddlAssignedTo.Enabled = false;
        ddlBrokerCode.Enabled = false;
        ddlModeOfProposal.Enabled = false;
        ddlBusinessChannel.Enabled = false;
        chkIsFastTrack.Enabled = false;
        chkIsFreeCoverLimitProposal.Enabled = false;
        ddlBankType.Items.Clear();
        ddlBankType.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlBankName.Items.Clear();
        ddlBankName.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlBranch.Items.Clear();
        ddlBranch.Items.Add(new ListItem("--- Select One ---", "0"));




        ddlAssignedTo.SelectedValue = "0";
        ddlModeOfProposal.SelectedValue = "0";
        ddlBusinessChannel.SelectedValue = "0";

        ddlBrokerCode.Items.Clear();
        ddlBrokerCode.Items.Add(new ListItem("--- Select One ---", "0"));

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        btnCancelJob.Enabled = false;
        btnRefreshBanks.Enabled = false;
        btnRemoveFST.Enabled = false;
        //  btnCancel.Enabled = false;
    }

    private bool checkIsUserMRPSuperuser()
    {
        bool returnVal = false;

        string MRPSupervisoUserCode = "";


        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            MRPSupervisoUserCode = System.Configuration.ConfigurationManager.AppSettings["MRPSupervisoUserCode"].ToString();
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            MRPSupervisoUserCode = System.Configuration.ConfigurationManager.AppSettings["MCRSupervisoUserCode"].ToString();
        }


        UserAuthentication userAuthentication = new UserAuthentication();

        string UserName = Context.User.Identity.Name;

        if (userAuthentication.getUserRoleCodeOfCurrentUser(UserName) != Convert.ToInt32(MRPSupervisoUserCode))
        {
            returnVal = false;
        }
        else
        {
            returnVal = true;
        }
        return returnVal;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtJobNumber.Text = "";
        txtProposalNumber.Text = "";
        txtNIC1.Text = "";
        txtNIC2.Text = "";
        chkIsFastTrack.Checked = false;
        chkIsFreeCoverLimitProposal.Checked = false;
        txtJobNumber.Enabled = false;
        txtProposalNumber.Enabled = true;
        txtNIC1.Enabled = true;
        txtNIC2.Enabled = true;

        ddlBankType.Enabled = true;
        ddlBankName.Enabled = true;
        ddlBranch.Enabled = true;
        ddlAssignedTo.Enabled = true;
        ddlBrokerCode.Enabled = true;
        ddlModeOfProposal.Enabled = true;
        ddlBusinessChannel.Enabled = true;
        chkIsFastTrack.Enabled = true;
        chkIsFreeCoverLimitProposal.Enabled = true;
        btnSave.Enabled = true;
        btnRefreshBanks.Enabled = true;
        Session["ProposalJobSetupMode"] = "NEW";


        int maxValue = Convert.ToInt32(getMaxJobNo());
        int newValue = maxValue + 1;
        GenerateNumbers generateNewJobNumber = new GenerateNumbers();


        string newJobNo = "";

        newJobNo = generateNewJobNumber.GetNewNoPrefixYearValue(Request.Cookies["WORKFLOW_CHOICE"].Value.ToString(), 4, newValue.ToString());


        txtJobNumber.Text = newJobNo;


        ddlBankType.Items.Clear();
        ddlBankType.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlBankName.Items.Clear();
        ddlBankName.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlBranch.Items.Clear();
        ddlBranch.Items.Add(new ListItem("--- Select One ---", "0"));


        loadBankTypes();

    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {

        if (txtProposalNumber.Text != "")
        {
            if (chkIsFastTrack.Checked != true)
            {
                lblMsg.Text = "Job Details Cannot Edit After Updating the Proposal Number";
                Timer1.Enabled = true;
                return;
            }
            else
            {

                chkIsFastTrack.Enabled = true;
                btnSave.Enabled = true;

                Session["ProposalJobSetupMode"] = "UPDATE";
                return;

            }
        }



        if (txtNIC1.Text == "")
        {
            lblMsg.Text = "Please Select the NIC 1";
            return;
        }

        if (ddlAssignedTo.SelectedValue == "" || ddlAssignedTo.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the Assigned Person";
            Timer1.Enabled = true;
            return;
        }

        txtProposalNumber.Enabled = true;
        txtNIC1.Enabled = true;
        txtNIC2.Enabled = true;

        ddlBankType.Enabled = true;
        ddlBankName.Enabled = true;
        ddlBranch.Enabled = true;
        ddlAssignedTo.Enabled = true;
        ddlBrokerCode.Enabled = true;
        ddlModeOfProposal.Enabled = true;
        ddlBusinessChannel.Enabled = true;
        chkIsFastTrack.Enabled = true;
        chkIsFreeCoverLimitProposal.Enabled = true;
        btnSave.Enabled = true;
        btnRefreshBanks.Enabled = true;


        Session["ProposalJobSetupMode"] = "UPDATE";



    }

    protected void grdProposalJobs_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearComponents();

        txtJobNumber.Text = grdProposalJobs.SelectedRow.Cells[1].Text.Trim();
        loadProposalJobDetails(grdProposalJobs.SelectedRow.Cells[1].Text.Trim());

        loadBankTypes();
        loadBankDetails(grdProposalJobs.SelectedRow.Cells[5].Text.Trim());

        loadAssignedTo();
        ddlAssignedTo.SelectedValue = grdProposalJobs.SelectedRow.Cells[6].Text.Trim();

        loadBrokerData();
        ddlBrokerCode.SelectedItem.Text = grdProposalJobs.SelectedRow.Cells[11].Text;
        if (grdProposalJobs.SelectedRow.Cells[8].Text == "1")
        {
            lblJobStatus.Text = "Job Cancelled";
            return;
        }

        btnAlter.Enabled = true;
        btnCancelJob.Enabled = true;

        btnRemoveFST.Enabled = true;
    }


    private string loadProposalJobDetails(string sJobNo)
    {

        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        selectQuery = "   SELECT T.JOB_NO ," +
            "T.PROPOSAL_NO," +
            "T.NIC1," +
            "T.NIC2, " +
            "T.IS_FAST_TRACK," +
            "T.BANK_CODE," +
            "T.ASSIGNED_USER_CODE," +
            "U.USER_NAME, " +
            " (SELECT PTV.PVR_BUSINESS_NAME AGENT_NAME FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, " +
            " T_STAKE_HOLDER_FUNCTION STF WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND " +
            " PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND " +
            " PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL AND " +
            " PRT.PTY_PARTY_CODE = T.BROKER_CODE)BROKER_NAME, " +
            "T.PROPOSAL_MODE_ID, " +
            "T.BUSINESS_CHNL_ID, " +
            " T.IS_FREE_CVR_LIMIT_PRPSL " +
            " FROM MRP_WF_PROPOSAL_JOBS T  " +
          "INNER JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER_CODE=U.USER_CODE" +

          " WHERE JOB_NO=:V_JOB_NO";

        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_JOB_NO", sJobNo));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            txtProposalNumber.Text = dr[1].ToString();

            txtNIC1.Text = dr[2].ToString();
            txtNIC2.Text = dr[3].ToString();

            if (dr[4].ToString() == "1")
            {
                chkIsFastTrack.Checked = true;
            }
            else
            {
                chkIsFastTrack.Checked = false;

            }

            ddlModeOfProposal.SelectedValue = dr[9].ToString();
            ddlBusinessChannel.SelectedValue = dr[10].ToString();

            if (dr[11].ToString() == "1")
            {
                chkIsFreeCoverLimitProposal.Checked = true;
            }
            else
            {
                chkIsFreeCoverLimitProposal.Checked = false;

            }


        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
    }





    private string loadBankDetails(string sBankCode)
    {
        loadBankData();
        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        selectQuery = "SELECT BANK_TYPE,BANK_NAME,BRANCH_NAME FROM MRP_WF_BANKS " +
            " WHERE BANK_CODE=:V_BANK_CODE ";

        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_BANK_CODE", sBankCode));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            ddlBankType.ClearSelection();
            ddlBankType.Items.FindByValue(dr[0].ToString()).Selected = true;

            loadBankName(ddlBankType.SelectedValue);

            ddlBankName.ClearSelection();
            ddlBankName.Items.FindByValue(dr[1].ToString()).Selected = true;


            ddlBranch.ClearSelection();
            ddlBranch.Items.FindByValue(dr[2].ToString()).Selected = true;

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
    }

    protected void grdProposalJobs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;

        e.Row.Cells[8].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";

        loadBankTypes();
        loadAssignedTo();
        loadBrokerData();
        loadProposalModes();

        loadBusinessChannels();
    }


    private bool CheckProposalNoAlreadyExist(string sProposalNo)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";

        string workflowType = "";

        workflowType = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();



        selectQuery = "	SELECT PROPOSAL_NO FROM MRP_WF_PROPOSAL_JOBS " +
                     " WHERE PROPOSAL_NO=:V_PROPOSAL_NO AND  WORKFLOW_TYPE=:V_WORKFLOW_TYPE AND CANCELLED=0" +
                     " UNION " +
                    " SELECT PROPOSAL_NO FROM MRP_WORKFLOW " +
                    " WHERE PROPOSAL_NO=:V_PROPOSAL_NO AND  WORKFLOW_TYPE=:V_WORKFLOW_TYPE  AND CANCELLED=0";

        OracleCommand cmd = new OracleCommand(selectQuery, con);


        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));
        cmd.Parameters.Add(new OracleParameter("V_WORKFLOW_TYPE", workflowType));


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

    private bool CheckProposalNoAvailable(string sProposalNo)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";
        selectQuery = "	SELECT PROPOSAL_NO FROM MRP_WORKFLOW " +
                    " WHERE PROPOSAL_NO=:V_PROPOSAL_NO  AND CANCELLED=0";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));

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

    private bool CheckIsDefeDclined(string sNICNo)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";
        selectQuery = "	SELECT t.nic_no FROM WF_DIFERED_DECLINE_POLICIES t where t.nic_no=:V_NIC_NO";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_NIC_NO", sNICNo));

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


    private bool CheckProposalNoExistInMRPSystem(string sProposalNo)
    {
        bool returnVal = false;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;

        con.Open();


        String selectQuery = "";

        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            selectQuery = "SELECT " +
                           " ProposlReg.PropNo " +
                       " FROM ProposlReg  " +
                       " WHERE ProposlReg.PropNo=@PropNo";
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            selectQuery = "SELECT " +
                       " ProposlRegMicro.PropNo " +
                   " FROM ProposlRegMicro  " +
                   " WHERE ProposlRegMicro.PropNo=@PropNo";
        }




        SqlCommand cmd = new SqlCommand(selectQuery, con);
        cmd.Parameters.AddWithValue("@PropNo", sProposalNo);

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





    protected void btnRefreshBanks_Click(object sender, EventArgs e)
    {
        try
        {

            deleteBankData();
            UpdateBankDetailsFromMRP();
            UpdateCourierTypes();
            loadBankData();

        }
        catch (Exception ee)
        {


        }
    }





    private void UpdateBankDetailsFromMRP()
    {
        DataTable dt = new DataTable();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();

        con.Open();

        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "  SELECT  " +
                          " [BNKCode] AS 'BANK_CODE' " +
                          " ,[BNKkName] AS 'BANK_TYPE' " +
                          " ,CASE WHEN [BNKkName]='Other Bank' THEN [Otherbankname]  " +
                          " WHEN [BNKkName]='HNB' THEN [BNKkName]  " +
                          " WHEN [BNKkName]='Seeds' THEN [BNKkName]  " +
                          " END AS 'BANK_NAME' " +
                           ",[Branchname] AS 'BRANCH_NAME' " +
                           " FROM [MRP].[dbo].[Bank] " +
                           " WHERE [BNKCode] IS NOT NULL OR [BNKCode]<>''";
        cmd.CommandText = selectQuery;

        da.SelectCommand = cmd;


        da.Fill(dt);

        foreach (DataRow row in dt.Rows)
        {
            SaveBanksToMRPWFSystem(row);
        }


        cmd.Dispose();
        con.Close();
        con.Dispose();


    }

    private void deleteBankData()
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            string strQuery = "";

            strQuery = "DELETE FROM MRP_WF_BANKS";

            spProcess = new OracleCommand(strQuery, conProcess);

            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {

        }

    }
    private void UpdateCourierTypes()
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            string strQuery = "";

            strQuery = "update MRP_WF_BANKS b set b.courier_type=(select t.courier_type from  mrp_wf_banks_courier t where t.bank_code=b.bank_code)";

            spProcess = new OracleCommand(strQuery, conProcess);

            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {

        }

    }

    private void SaveBanksToMRPWFSystem(DataRow row)
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            spProcess = new OracleCommand("INSERT_MRP_WF_BANKS");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_BANK_CODE", OracleType.VarChar, 20).Value = row["BANK_CODE"].ToString();
            spProcess.Parameters.Add("V_BANK_TYPE", OracleType.VarChar, 40).Value = row["BANK_TYPE"].ToString();
            spProcess.Parameters.Add("V_BANK_NAME", OracleType.VarChar, 200).Value = row["BANK_NAME"].ToString();
            spProcess.Parameters.Add("V_BRANCH_NAME", OracleType.VarChar, 90).Value = row["BRANCH_NAME"].ToString();

            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {

        }

    }




    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
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

    protected void txtNIC1_TextChanged(object sender, EventArgs e)
    {
        if (txtNIC1.Text.Trim() != "")
        {
            ClientScriptManager CSM = Page.ClientScript;



            if (CheckIsDefeDclined(txtNIC1.Text))
            {
                string strconfirm = " alert('This Customer is Deferred or Declined.')";
                CSM.RegisterClientScriptBlock(this.GetType(), "Message", strconfirm, true);
                txtNIC1.Attributes.Add("style", "color:red");
            }
        }
    }


    protected void txtNIC2_TextChanged(object sender, EventArgs e)
    {
        if (txtNIC2.Text.Trim() != "")
        {
            ClientScriptManager CSM = Page.ClientScript;



            if (CheckIsDefeDclined(txtNIC2.Text))
            {
                string strconfirm = " alert('This Customer is Deferred or Declined.')";
                CSM.RegisterClientScriptBlock(this.GetType(), "Message", strconfirm, true);
                txtNIC2.Attributes.Add("style", "color:red");
            }
        }
    }
    protected void BtnCreateBanks_Click(object sender, EventArgs e)
    {
        string url = "MRPBankEmailDetails.aspx";
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.open('");
        sb.Append(url);
        sb.Append("');");
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());


    }

    private void sendFastTrackMailToUsers()
    {
        MRPWFMail mail = new MRPWFMail();

        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            mail.From_address = System.Configuration.ConfigurationManager.AppSettings["MRPEmailAddress"].ToString();
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            mail.From_address = System.Configuration.ConfigurationManager.AppSettings["MCREmailAddress"].ToString();
        }


        mail.To_address = GetUserAndSupervisorMailAddress(ddlAssignedTo.Text);
        mail.Cc_address = "tharindu.dilanka@hnbassurance.com";

        mail.Subject = "Fast Track Job Allocated";

        String BodyText;

        BodyText = "Fast Track Job(" + txtJobNumber.Text + ") Allocated to " + ddlAssignedTo.SelectedItem.ToString();

        mail.Body = BodyText;
        mail.sendMail();


    }

    private void sendJobAllocatedMailToUsers()
    {
        MRPWFMail mail = new MRPWFMail();

        String BodyText = "";

        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            mail.From_address = System.Configuration.ConfigurationManager.AppSettings["MRPEmailAddress"].ToString();
            mail.Subject = "MRP Job Allocated";
            BodyText = "MRP Job(" + txtJobNumber.Text + ") Allocated to " + ddlAssignedTo.SelectedItem.ToString();
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            mail.From_address = System.Configuration.ConfigurationManager.AppSettings["MCREmailAddress"].ToString();
            mail.Subject = "MCR Job Allocated";
            BodyText = "MCR Job(" + txtJobNumber.Text + ") Allocated to " + ddlAssignedTo.SelectedItem.ToString();
        }
        mail.To_address = GetUserAndSupervisorMailAddress(ddlAssignedTo.Text);
        mail.Cc_address = "tharindu.dilanka@hnbassurance.com";


        mail.Body = BodyText;
        mail.sendMail();


    }


    private void sendFastTrackRemoveMailToUsers()
    {
        MRPWFMail mail = new MRPWFMail();

        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            mail.From_address = System.Configuration.ConfigurationManager.AppSettings["MRPEmailAddress"].ToString();
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            mail.From_address = System.Configuration.ConfigurationManager.AppSettings["MCREmailAddress"].ToString();
        }

        mail.To_address = GetUserAndSupervisorMailAddress(ddlAssignedTo.Text);
        mail.Cc_address = "tharindu.dilanka@hnbassurance.com";

        mail.Subject = "Fast Track Category Removed from Job";

        String BodyText;

        BodyText = "Fast Track Category Removed from Job(" + txtJobNumber.Text + ") Allocated to " + ddlAssignedTo.SelectedItem.ToString();

        mail.Body = BodyText;
        mail.sendMail();


    }

    private string GetUserAndSupervisorMailAddress(String userCode)
    {
        String returnVal = "";
        string mrpManagerEmail = "";

        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            mrpManagerEmail = System.Configuration.ConfigurationManager.AppSettings["MRPManagerEmail"].ToString();
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            mrpManagerEmail = System.Configuration.ConfigurationManager.AppSettings["MCRManagerEmail"].ToString();
        }

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        try
        {
            con.Open();

            String selectQuery = "";



            selectQuery = "SELECT CASE WHEN u.email  IS NOT NULL THEN u.email ELSE '" + mrpManagerEmail + "' END ," +
                " CASE WHEN su.email  IS NOT NULL THEN su.email ELSE '" + mrpManagerEmail + "' END " +
                " from MRP_USER_DETAILS U " +
                " INNER JOIN  MRP_USER_DETAILS SU ON su.user_code=u.supervisor_user_code " +
                " INNER JOIN  MRP_USER_DETAILS SSU ON ssu.user_code=su.supervisor_user_code " +
                " WHERE u.user_code=:V_user_code";


            OracleCommand cmd = new OracleCommand(selectQuery, con);
            cmd.Parameters.Add(new OracleParameter("V_user_code", userCode));



            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    returnVal = dr[0].ToString() + "," + dr[1].ToString();
                }
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {

        }
        returnVal = returnVal.TrimEnd(',');
        return returnVal;

    }

    protected void chkIsFastTrack_CheckedChanged(object sender, EventArgs e)
    {
        if (checkIsFastTrackJob(txtJobNumber.Text))
        {
            if (chkIsFastTrack.Checked == false)
            {

                string MRPManagerUserName = "";




                if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
                {
                    MRPManagerUserName = System.Configuration.ConfigurationManager.AppSettings["MRPManagerUserName"].ToString();
                }
                else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
                {
                    MRPManagerUserName = System.Configuration.ConfigurationManager.AppSettings["MCRManagerUserName"].ToString();
                }


                UserAuthentication userAuthentication = new UserAuthentication();
                string UserName = Context.User.Identity.Name;


                if (!MRPManagerUserName.Contains(Right(UserName, (UserName.Length) - 5)))
                {
                    lblMsg.Text = "Removing Fast Track facility is only available for MRP Managers";
                    Timer1.Enabled = true;


                    chkIsFastTrack.Checked = true;


                    return;
                }

            }

        }
    }

    private bool checkIsFastTrackJob(string jobNo)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";
        selectQuery = "SELECT 	" +
                    " PJ.IS_FAST_TRACK   " +    //0
                     " FROM mrp_wf_proposal_jobs PJ  " +
                  " WHERE PJ.JOB_NO=:V_JOB_NO";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_JOB_NO", jobNo));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            if (dr[0].ToString() == "1")
            {
                returnVal = true;

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }


    private bool checkIsNICIsBlacklisted(string nic)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";
        selectQuery = "SELECT T.NIC_NO FROM MRP_WF_BLACKLISTED_NIC T WHERE T.NIC_NO=:V_NIC_NO AND T.IS_BLACKLISTED=1";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_NIC_NO", nic));

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

}

