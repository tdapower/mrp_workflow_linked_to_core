//******************************************
// Author            :Tharindu Athapattu
// Date              :29/04/2013
// Reviewed By       :
// Description       : MRPWorkflow Form
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
using System.IO;
using MsgBox;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Net.Mail;

public partial class ViewMRPWorkflow : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            validatePageAuthentication();

            Session.Remove("ProposalNo");


            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);

            txtSumInsured.Attributes.Add("onkeyup", "jsValidateNum(this)");

            btnFollowup.Attributes.Add("onClick", "jsViewFollowup()");
         


            ClearComponents();
            LockComponents();
            initializeValues();

            hideAndShowComponents();

            Session.Remove("WorkflowMode");
            Session.Remove("PendingActionMode");

            pnlPolicyGrid.Visible = false;


            hid_Ticker.Value = new TimeSpan(1, 0, 0).ToString();
            tmrCountdown.Enabled = false;
            lblIsFastTrackJob.Visible = false;
            lit_Timer.Text = "";



        }

        if (Session["PendingActionMode"] == null)
        {
            Session["PendingActionMode"] = "VIEW";
        }
    }

    private void hideAndShowComponents()
    {


        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            lblCourierServiceLabel.Visible = true;
            lblCourierService.Visible = true;
            chkSkipToCertificateIssued.Visible = true;
            lblReInsurer.Visible = true;
            txtReInsurer.Visible = true;
            lblLifeAssured1RIRefNo.Visible = true;
            txtLifeAssured1RIRefNo.Visible = true;
            lblLifeAssured2RIRefNo.Visible = true;
            txtLifeAssured2RIRefNo.Visible = true;

            lblLifeAssured1HNBARefNo.Visible = true;
            txtLifeAssured1HNBARefNo.Visible = true;
            lblLifeAssured2HNBARefNo.Visible = true;
            txtLifeAssured2HNBARefNo.Visible = true;
            lblLoanAmount2.Visible = true;
            txtLoanAmount2.Visible = true;
            lblInterestRate2.Visible = true;
            txtInterestRate2.Visible = true;
            lblTerm2.Visible = true;
            txtTerm2.Visible = true;
            lblLoanAmount3.Visible = true;
            txtLoanAmount3.Visible = true;
            lblInterestRate3.Visible = true;
            txtInterestRate3.Visible = true;
            lblTerm3.Visible = true;
            txtTerm3.Visible = true;
            chkTPDMarketLimit.Visible = true;

        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            lblCourierServiceLabel.Visible = false;
            lblCourierService.Visible = false;
            chkSkipToCertificateIssued.Visible = false;
            lblReInsurer.Visible = false;
            txtReInsurer.Visible = false;
            lblLifeAssured1RIRefNo.Visible = false;
            txtLifeAssured1RIRefNo.Visible = false;
            lblLifeAssured2RIRefNo.Visible = false;
            txtLifeAssured2RIRefNo.Visible = false;

            lblLifeAssured1HNBARefNo.Visible = false;
            txtLifeAssured1HNBARefNo.Visible = false;
            lblLifeAssured2HNBARefNo.Visible = false;
            txtLifeAssured2HNBARefNo.Visible = false;
            lblLoanAmount2.Visible = false;
            txtLoanAmount2.Visible = false;
            lblInterestRate2.Visible = false;
            txtInterestRate2.Visible = false;
            lblTerm2.Visible = false;
            txtTerm2.Visible = false;
            lblLoanAmount3.Visible = false;
            txtLoanAmount3.Visible = false;
            lblInterestRate3.Visible = false;
            txtInterestRate3.Visible = false;
            lblTerm3.Visible = false;
            txtTerm3.Visible = false;
            chkTPDMarketLimit.Visible = false;

        }

        lblLifeassured1BeneficiaryName.Visible = false;
        txtLifeassured1BeneficiaryName.Visible = false;

        lblLifeassured1BeneficiaryNIC.Visible = false;
        txtLifeassured1BeneficiaryNIC.Visible = false;

        lblLifeassured1BeneficiaryAddress.Visible = false;
        txtLifeassured1BeneficiaryAddress.Visible = false;


        lblLifeassured2BeneficiaryName.Visible = false;
        txtLifeassured2BeneficiaryName.Visible = false;

        lblLifeassured2BeneficiaryNIC.Visible = false;
        txtLifeassured2BeneficiaryNIC.Visible = false;

        lblLifeassured2BeneficiaryAddress.Visible = false;
        txtLifeassured2BeneficiaryAddress.Visible = false;
    }


    private void CheckOtherPoliciesInMRPSystem(string sProposalNo)
    {
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        con.Open();


        OracleConnection conOR = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader drOR;
        OracleDataAdapter daOR = new OracleDataAdapter();
        OracleCommand cmdOR = new OracleCommand();
        conOR.Open();


        cmd.Connection = con;
        String selectQuery = "";



        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            selectQuery = "SELECT " +
                               " ProposlReg.NIC1, ProposlReg.NIC2" +
                               " FROM ProposlReg  " +
                               " WHERE ProposlReg.PropNo='" + sProposalNo + "'";
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            selectQuery = "SELECT " +
                              " P.NIC1, P.NIC2" +
                              " FROM ProposlRegMicro  P " +
                              " WHERE P.PropNo='" + sProposalNo + "'";
        }

        cmd.CommandText = selectQuery;

        da.SelectCommand = cmd;

        da.Fill(dt);

        if (dt.Rows.Count >= 1)
        {
            string NIC1 = dt.Rows[0]["NIC1"].ToString();
            string NIC2 = dt.Rows[0]["NIC2"].ToString();

            if (NIC1 != "")
            {



                if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
                {
                    selectQuery = "SELECT " +
                          " ProposlReg.PolicyNo POL_NO,Prostatus.prostatus POL_STATUS,'MRP' AS SYSTEM" +
                          " FROM ProposlReg , Prostatus" +
                          " WHERE ProposlReg.NIC1='" + NIC1 + "' AND ProposlReg.Prostatusid =Prostatus.prostatusid";
                }
                else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
                {
                    selectQuery = "SELECT " +
                           " P.PolicyNo POL_NO,Prostatus.prostatus POL_STATUS,'MCR' AS SYSTEM" +
                           " FROM ProposlRegMicro P, Prostatus" +
                           " WHERE P.NIC1='" + NIC1 + "' AND P.Prostatusid =Prostatus.prostatusid";
                }



                cmd.CommandText = selectQuery;

                da.SelectCommand = cmd;



                da.Fill(dt1);
            }

            if (NIC2 != "")
            {


                if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
                {
                    selectQuery = "SELECT " +
              " ProposlReg.PolicyNo POL_NO,Prostatus.prostatus POL_STATUS ,'MRP' AS SYSTEM" +
              " FROM ProposlReg , Prostatus" +
              " WHERE ProposlReg.NIC2='" + NIC2 + "' AND ProposlReg.Prostatusid =Prostatus.prostatusid";
                }
                else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
                {
                    selectQuery = "SELECT " +
               " ProposlRegMicro.PolicyNo POL_NO,Prostatus.prostatus POL_STATUS ,'MCR' AS SYSTEM" +
               " FROM ProposlRegMicro , Prostatus" +
               " WHERE ProposlRegMicro.NIC2='" + NIC2 + "' AND ProposlRegMicro.Prostatusid =Prostatus.prostatusid";
                }



                cmd.CommandText = selectQuery;

                da.SelectCommand = cmd;
                da.Fill(dt1);

            }

            /////////////////////////// Start - Before 2008 policies
            if (NIC1 != "")
            {
                selectQuery = "SELECT " +
                           " PrvPolicydata.PolicyNo POL_NO,PrvPolicydata.status POL_STATUS,'MRP' AS SYSTEM" +
                           " FROM PrvPolicydata " +
                           " WHERE PrvPolicydata.cusnic1='" + NIC1 + "' ";





                cmd.CommandText = selectQuery;

                da.SelectCommand = cmd;



                da.Fill(dt1);
            }

            if (NIC2 != "")
            {
                selectQuery = "SELECT " +
                   " PrvPolicydata.PolicyNo POL_NO,PrvPolicydata.status POL_STATUS ,'MRP' AS SYSTEM" +
                   " FROM PrvPolicydata " +
                   " WHERE PrvPolicydata.cusnic2='" + NIC2 + "' ";
                cmd.CommandText = selectQuery;

                da.SelectCommand = cmd;
                da.Fill(dt1);

            }


            /////////////////////////// End - Before 2008 policies


            cmdOR.Connection = conOR;
            String selectQueryOR = "";

            selectQueryOR = "SELECT " +
                                   " CRC_POLICY_LIFE.POL_NO POL_NO, CRC_POLICY_LIFE.POL_STAUS POL_STATUS ,'TCS' AS SYSTEM" +
                               " FROM CRC_POLICY_LIFE  " +
                               " WHERE CRC_POLICY_LIFE.POL_NIC='" + NIC1 + "' AND PRO_NO !='" + sProposalNo + "'";
            cmdOR.CommandText = selectQueryOR;

            daOR.SelectCommand = cmdOR;
            daOR.Fill(dt1);

            GrdOtherPolicies.DataSource = dt1;
            GrdOtherPolicies.DataBind();

        }

        cmd.Dispose();
        con.Close();
        con.Dispose();

        cmdOR.Dispose();
        conOR.Close();
        conOR.Dispose();


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

    private void initializeValues()
    {
        loadStatuses();
        loadAssignedTo();
        loadBrokerData();
        loadClauses();

        ddlMedical.Items.Clear();
        ddlMedical.Items.Add(new ListItem("--- Select One ---", "0"));
        ddlMedical.Items.Add(new ListItem("Local", "1"));
        ddlMedical.Items.Add(new ListItem("Medical", "2"));


        ddlMedicalNonMedical.Items.Clear();
        ddlMedicalNonMedical.Items.Add(new ListItem("--- Select One ---", "0"));
        ddlMedicalNonMedical.Items.Add(new ListItem("Medical", "1"));
        ddlMedicalNonMedical.Items.Add(new ListItem("Non-Medical", "2"));

        ddlSearchReminderStage.Items.Clear();
        ddlSearchReminderStage.Items.Add(new ListItem("", "0"));
        ddlSearchReminderStage.Items.Add(new ListItem("1", "1"));
        ddlSearchReminderStage.Items.Add(new ListItem("2", "2"));
        ddlSearchReminderStage.Items.Add(new ListItem("3", "3"));


        ddlCoverNoteValidityPeriod.Items.Clear();
        ddlCoverNoteValidityPeriod.Items.Add(new ListItem("7 days", "7 days"));
        ddlCoverNoteValidityPeriod.Items.Add(new ListItem("14 days", "14 days"));
        ddlCoverNoteValidityPeriod.Items.Add(new ListItem("30 days", "30 days"));




        lblError.Text = "";
        lblMsg.Text = "";


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

        selectQuery = "SELECT  PRT.PTY_PARTY_CODE AGENT_CODE,  PTV.PVR_BUSINESS_NAME AGENT_NAME " +
            " FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, T_STAKE_HOLDER_FUNCTION STF " +
            " WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND " +
            " PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND " +
            " PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL";

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

    private void enableDisableChkPPI()
    {
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
            chkPPI.Enabled = false;
        }
        else
        {
            chkPPI.Enabled = true;
        }
    }

    private void loadStatuses()
    {
        ddlStatus.Items.Clear();
        ddlStatus.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchStatus.Items.Clear();
        ddlSearchStatus.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT STATUS_CODE,STATUS_NAME FROM MRP_WF_STATUSES WHERE IS_ACTIVE=1 ORDER BY ORDER_NO ASC ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlStatus.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlSearchStatus.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }








    private bool AuthenticateMRPStaffForEditing()
    {
        bool isMRPUser = false;


        String UserName = Context.User.Identity.Name;


        if (Left(UserName, 4) == "HNBA")
        {
            UserName = Right(UserName, (UserName.Length) - 5);
        }
        else
        {
            UserName = Right(UserName, (UserName.Length) - 7);
        }

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        string MRPUserCodes = System.Configuration.ConfigurationManager.AppSettings["MRPUserCodes"].ToString();


        selectQuery = "   SELECT T.USER_ROLE_CODE FROM WF_ADMIN_USERS T  " +
           " WHERE T.USER_CODE='" + UserName + "' AND  T.USER_ROLE_CODE IN (" + MRPUserCodes + ") ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            isMRPUser = true;


        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return isMRPUser;
    }

    private string getUserName(string userCode)
    {
        string userName = "";



        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        string MRPUserCodes = System.Configuration.ConfigurationManager.AppSettings["MRPUserCodes"].ToString();


        selectQuery = "   SELECT T.USER_NAME FROM WF_ADMIN_USERS T  " +
           " WHERE T.USER_CODE=:V_USER_CODE";

        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_USER_CODE", userCode));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            userName = dr[0].ToString();


        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return userName;
    }

    private string loadDesignationOfPerson(string iSignPersonCode)
    {
        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        selectQuery = "   SELECT T.DESIGNATION FROM MRP_WF_SIGN_PERSON T  " +
                     "  WHERE T.USER_CODE=:V_USER_CODE";



        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_USER_CODE", iSignPersonCode));

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



    private void loadAssignedTo()
    {
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
           " WHERE T.USER_ROLE_CODE IN (" + MRPUserCodes + ") ORDER BY T.USER_NAME ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlSearchAssignedTo.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void loadClauses()
    {
        chkLstClauses.Items.Clear();
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        string MRPUserCodes = System.Configuration.ConfigurationManager.AppSettings["MRPUserCodes"].ToString();


        selectQuery = "   SELECT T.CLAUSE_ID,T.CLAUSE FROM MRP_WF_CLAUSE T  " +
           "   ORDER BY T.CLAUSE ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                chkLstClauses.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));


            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();




    }


    private void loadSelectedClauses(string proposalNo)
    {
        foreach (ListItem item in chkLstClauses.Items)
        {
            item.Selected = false;
        }



        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        string MRPUserCodes = System.Configuration.ConfigurationManager.AppSettings["MRPUserCodes"].ToString();


        selectQuery = "   SELECT T.CLAUSE_ID FROM MRP_WF_SELECTED_CLAUSES T  " +
           "   WHERE PROPOSAL_NO=:V_PROPOSAL_NO";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", proposalNo));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {

                foreach (ListItem item in chkLstClauses.Items)
                {

                    if ((item.Value == dr[0].ToString()))
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


  

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchData();
        ClearComponents();
        LockComponents();
    }


    private void SearchData()
    {
        lblError.Text = "";
        grdPolicies.DataSource = null;
        grdPolicies.DataBind();

        //if ((txtSearchPolicy.Text == "") && (txtSearchProposal.Text == "") && (txtSearchNIC1.Text == "") && (txtSearchNIC2.Text == ""))
        //{
        //    lblError.Text = "Search text cannot be blank";
        //    return;
        //}

        if ((txtSearchJobNumber.Text == "") && (txtSearchProposal.Text == "") && (txtSearchNIC1.Text == "") && (txtSearchNIC2.Text == "") &&
            (ddlSearchAssignedTo.SelectedValue == "" || ddlSearchAssignedTo.SelectedValue == "0") && (ddlSearchStatus.SelectedValue == "" || ddlSearchStatus.SelectedValue == "0") && chkSearchFaxCleared.Checked == false && chkSearchOriginalsCleared.Checked == false && (txtSearchfromDate.Text == "") && (txtSearchToDate.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }


        //SqlConnection myOleDbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());

        //SqlCommand myOleDbCommand = new SqlCommand();

        //myOleDbConnection.Open();

        //myOleDbCommand.Connection = myOleDbConnection;




        //if (txtSearchPolicy.Text != "")
        //{
        //    SQL = "(T.PolicyNo LIKE '%" + txtSearchPolicy.Text + "%') AND";
        //}
        //if (txtSearchProposal.Text != "")
        //{
        //    SQL = SQL + "(T.PropNo LIKE '%" + txtSearchProposal.Text + "%') AND";
        //}
        //if (txtSearchNIC1.Text != "")
        //{
        //    SQL = SQL + "(T.NIC1 LIKE '%" + txtSearchNIC1.Text + "%') AND";
        //}
        //if (txtSearchNIC2.Text != "")
        //{
        //    SQL = SQL + "(T.NIC2 LIKE '%" + txtSearchNIC2.Text + "%') AND";
        //}

        //SQL = SQL.Substring(0, SQL.Length - 3);


        //String selectQuery = "";
        //selectQuery = " SELECT T.PropNo AS \"Proposal No\",T.PolicyNo AS \"Policy No\",T.NIC1,T.NIC2 FROM ProposlReg T " +
        //    " WHERE (" + SQL + ") ORDER BY T.CurDate ASC";

        //myOleDbCommand.CommandText = selectQuery;

        //SqlDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        //if (myOleDbDataReader.HasRows == true)
        //{
        //    DataTable dbTable = new DataTable();
        //    grdPolicies.DataSource = myOleDbDataReader;
        //    grdPolicies.DataBind();

        //    pnlPolicyGrid.Visible = true;
        //}




        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;



        string SQL = "";
        //if (txtSearchPolicy.Text != "")
        //{
        //    SQL = "(T.PolicyNo LIKE '%" + txtSearchPolicy.Text + "%') AND";
        //}
        if (txtSearchJobNumber.Text != "")
        {
            SQL = "(LOWER(T.JOB_NO) LIKE '%" + txtSearchJobNumber.Text.ToLower() + "%') AND";
        }
        if (txtSearchProposal.Text != "")
        {
            SQL = SQL + "(LOWER(T.PROPOSAL_NO) LIKE '%" + txtSearchProposal.Text.ToLower() + "%') AND";
        }
        if (txtSearchNIC1.Text != "")
        {
            SQL = SQL + "(LOWER(T.NIC1) LIKE '%" + txtSearchNIC1.Text.ToLower() + "%') AND";
        }
        if (txtSearchNIC2.Text != "")
        {
            SQL = SQL + "(LOWER(T.NIC2) LIKE '%" + txtSearchNIC2.Text.ToLower() + "%') AND";
        }

        if (ddlSearchAssignedTo.SelectedValue != "" && ddlSearchAssignedTo.SelectedValue != "0")
        {

            //string MRPWorkflowPassedToUW = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowPassedToUW"].ToString();
            //string MRPWorkflowAcceptedStatuscode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowAcceptedStatuscode"].ToString();
            //string MRPWorkflowPaymentUpdatedStatuscode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowPaymentUpdatedStatuscode"].ToString();
            //string MRPWorkflowCertificateIssuedStatuscode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowCertificateIssuedStatuscode"].ToString();



            //if (ddlSearchStatus.SelectedValue == MRPWorkflowPassedToUW ||
            //                  ddlSearchStatus.SelectedValue == MRPWorkflowAcceptedStatuscode ||
            //                  ddlSearchStatus.SelectedValue == MRPWorkflowPaymentUpdatedStatuscode ||
            //                  ddlSearchStatus.SelectedValue == MRPWorkflowCertificateIssuedStatuscode)
            //{
            string MRPSupervisoUserCode = System.Configuration.ConfigurationManager.AppSettings["MRPSupervisoUserCode"].ToString();
            UserAuthentication userAuthentication = new UserAuthentication();


            if (userAuthentication.getUserRoleCodeOfCurrentUser(ddlSearchAssignedTo.SelectedValue) != Convert.ToInt32(MRPSupervisoUserCode))
            {

                SQL = SQL + "(FUP.USER_NAME = '" + ddlSearchAssignedTo.SelectedValue + "') AND";
            }
            else
            {
                SQL = SQL + "(T.ASSIGNED_USER_CODE = '" + ddlSearchAssignedTo.SelectedValue + "') AND";

            }


        }
        if (txtSearchfromDate.Text != "" && txtSearchToDate.Text != "")
        {

            SQL = "T.created_date>to_date('" + txtSearchfromDate.Text.ToLower() + "','DD/MM/YYYY') AND  T.created_date<to_date('" + txtSearchToDate.Text.ToLower() + "','DD/MM/YYYY') AND";
        }
        //if (ddlSearchStatus.SelectedValue != "" && ddlSearchStatus.SelectedValue != "0")
        //{

        //        SQL = SQL + "(WF.STATUS_CODE = '" + ddlSearchStatus.SelectedValue + "') AND";


        //}
        string MRPWorkflowCertificateIssuedStatuscode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowCertificateIssuedStatuscode"].ToString();

        string MRPWorkflowCoverNoteSent = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowCoverNoteSent"].ToString();



        //check the query part of MWS.ORDER_NO

        if (chkSearchFaxCleared.Checked == true)
        {
            SQL = SQL + " (SELECT COUNT(*)  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                        " WHERE  PCD.PROPOSAL_NO=WF.PROPOSAL_NO  AND PCD.IS_FAX_PENDING=0)>0 AND WFS.ORDER_NO<=(SELECT MM.ORDER_NO FROM MRP_WF_STATUSES MM WHERE MM.STATUS_CODE=" + MRPWorkflowCoverNoteSent + ") AND";
        }
        if (chkSearchOriginalsCleared.Checked == true)
        {
            SQL = SQL + " (SELECT COUNT(*)  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                       " WHERE  PCD.PROPOSAL_NO=WF.PROPOSAL_NO  AND PCD.IS_ORIGINAL_PENDING=0)>0 AND WFS.ORDER_NO<=(SELECT MM.ORDER_NO FROM MRP_WF_STATUSES MM WHERE MM.STATUS_CODE=" + MRPWorkflowCertificateIssuedStatuscode + ") AND";
        }
        if (chkSearchFaxCleared.Checked == true && chkSearchOriginalsCleared.Checked == true)
        {
            SQL = SQL + " (SELECT COUNT(*)  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                       " WHERE  PCD.PROPOSAL_NO=WF.PROPOSAL_NO  AND PCD.IS_FAX_PENDING=0 AND PCD.IS_ORIGINAL_PENDING=0)>0 AND";
        }



        if (ddlSearchStatus.SelectedValue != "" && ddlSearchStatus.SelectedValue != "0")
        {
            string ReminderType = "";

            string MRPWorkflowRemindersTypePendingReminderType = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowRemindersTypePendingReminderType"].ToString();
            string MRPWorkflowRemindersTypeCoverNoteReminderType = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowRemindersTypeCoverNoteReminderType"].ToString();
            string MRPWorkflowRemindersTypeConfirmationCoverReminderType = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowRemindersTypeConfirmationCoverReminderType"].ToString();


            string MRPWorkflowRemindersPendingMedicalStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowRemindersPendingMedicalStatusCode"].ToString();
            string MRPWorkflowRemindersCovernoteStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowRemindersCovernoteStatusCode"].ToString();
            string MRPWorkflowReminderConfirmationLetterStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowReminderConfirmationLetterStatusCode"].ToString();








            if (ddlSearchStatus.SelectedValue == MRPWorkflowRemindersPendingMedicalStatusCode)
            {
                ReminderType = MRPWorkflowRemindersTypePendingReminderType;
            }
            else if (ddlSearchStatus.SelectedValue == MRPWorkflowRemindersCovernoteStatusCode)
            {
                ReminderType = MRPWorkflowRemindersTypeCoverNoteReminderType;
            }
            else if (ddlSearchStatus.SelectedValue == MRPWorkflowReminderConfirmationLetterStatusCode)
            {
                ReminderType = MRPWorkflowRemindersTypeConfirmationCoverReminderType;
            }


            if (ddlSearchReminderStage.SelectedValue != "" && ddlSearchReminderStage.SelectedValue != "0")
            {
                SQL = SQL + "(WF.STATUS_CODE = '" + ddlSearchStatus.SelectedValue + "') AND ( (SELECT  MAX(REMINDER_NO) FROM MRP_WF_REMINDER_LOG WFRL WHERE WFRL.Proposal_No=WF.PROPOSAL_NO AND WFRL.Reminder_Type='" + ReminderType + "') = '" + ddlSearchReminderStage.SelectedValue + "' ) AND";

            }
            else
            {
                SQL = SQL + "(WF.STATUS_CODE = '" + ddlSearchStatus.SelectedValue + "') AND";

            }

        }





        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        //selectQuery = " SELECT T.JOB_NO AS \"Job No\",T.PROPOSAL_NO AS \"Proposal No\",T.NIC1,T.NIC2,T.ASSIGNED_USER_CODE,U.USER_NAME  AS \"Assigned User\",WFS.STATUS_NAME AS \"Status\" ,WF.STATUS_CODE FROM  MRP_WF_PROPOSAL_JOBS T  " +
        //              "INNER JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER_CODE=U.USER_CODE " +
        //              "LEFT JOIN MRP_WORKFLOW WF ON  T.JOB_NO=WF.JOB_NO " +
        //              "LEFT JOIN MRP_WF_STATUSES WFS ON  WF.STATUS_CODE=WFS.STATUS_CODE " +
        //    " WHERE (" + SQL + " AND T.PROPOSAL_NO IS NOT NULL AND (T.CANCELLED=0  OR T.CANCELLED IS NULL) AND (WF.CANCELLED=0 OR WF.CANCELLED IS NULL) ) ORDER BY T.JOB_NO ASC";


        //      selectQuery = " SELECT T.JOB_NO AS \"Job No\",T.PROPOSAL_NO AS \"Proposal No\",T.NIC1,T.NIC2,T.ASSIGNED_USER_CODE,U.USER_NAME  AS \"Assigned User\",WFS.STATUS_NAME AS \"Status\" ,WF.STATUS_CODE FROM  MRP_WF_PROPOSAL_JOBS T  " +
        //                    "INNER JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER_CODE=U.USER_CODE " +
        //                    "LEFT JOIN MRP_WORKFLOW WF ON  T.JOB_NO=WF.JOB_NO " +
        //                    "LEFT JOIN MRP_WF_STATUSES WFS ON  WF.STATUS_CODE=WFS.STATUS_CODE " +
        //"  LEFT JOIN MRP_WF_PROPOSAL_JOBS PJ ON WF.JOB_NO=PJ.JOB_NO " +
        //          " WHERE (" + SQL + " AND T.PROPOSAL_NO IS NOT NULL AND (T.CANCELLED=0  OR T.CANCELLED IS NULL) AND (WF.CANCELLED=0 OR WF.CANCELLED IS NULL) ) ORDER BY T.JOB_NO ASC";


        //selectQuery = " SELECT T.JOB_NO AS \"Job No\",T.PROPOSAL_NO AS \"Proposal No\",T.NIC1,T.NIC2,T.ASSIGNED_USER_CODE,U.USER_NAME  AS \"Assigned User\",WFS.STATUS_NAME AS \"Status\" ,WF.STATUS_CODE,(SELECT PTV.PVR_BUSINESS_NAME AGENT_NAME FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, T_STAKE_HOLDER_FUNCTION STF WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL AND PRT.PTY_PARTY_CODE = T.BROKER_CODE)BROKER_NAME FROM  MRP_WF_PROPOSAL_JOBS T  " +
        //              "INNER JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER_CODE = U.USER_CODE " +
        //              "LEFT JOIN MRP_WORKFLOW WF ON  T.JOB_NO=WF.JOB_NO " +
        //              "LEFT JOIN MRP_WF_STATUSES WFS ON  WF.STATUS_CODE = WFS.STATUS_CODE " +
        //            " LEFT JOIN MRP_WORKFLOW_FOLLOWUP FUP ON WF.PROPOSAL_NO = FUP.PROPOSAL_NO AND WF.STATUS_CODE = FUP.STATUS_CODE " +
        //    " WHERE (" + SQL + " AND T.PROPOSAL_NO IS NOT NULL AND (T.CANCELLED = 0  OR T.CANCELLED IS NULL) AND (WF.CANCELLED = 0 OR WF.CANCELLED IS NULL) ) ORDER BY T.JOB_NO ASC";

        string workflowType = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();

        selectQuery = " SELECT T.JOB_NO AS \"Job No\",T.PROPOSAL_NO AS \"Proposal No\",T.NIC1,T.NIC2,T.ASSIGNED_USER_CODE," +
            " U.USER_NAME  AS \"Assigned User\",WFS.STATUS_NAME AS \"Status\" ,WF.STATUS_CODE, " +
            " (SELECT PTV.PVR_BUSINESS_NAME AGENT_NAME FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, T_STAKE_HOLDER_FUNCTION STF " +
            " WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND " +
            " STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME " +
            " LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL AND PRT.PTY_PARTY_CODE = T.BROKER_CODE)BROKER_NAME " +
             " FROM  MRP_WF_PROPOSAL_JOBS T  " +
                      "INNER JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER_CODE = U.USER_CODE " +
                      "LEFT JOIN MRP_WORKFLOW WF ON  T.JOB_NO=WF.JOB_NO " +
                      "LEFT JOIN MRP_WF_STATUSES WFS ON  WF.STATUS_CODE = WFS.STATUS_CODE " +
                    " LEFT JOIN MRP_WORKFLOW_FOLLOWUP FUP ON WF.PROPOSAL_NO = FUP.PROPOSAL_NO AND WF.STATUS_CODE = FUP.STATUS_CODE " +
            " WHERE (" + SQL + " AND T.PROPOSAL_NO IS NOT NULL AND T.WORKFLOW_TYPE='" + workflowType + "') ORDER BY T.JOB_NO ASC";




        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdPolicies.DataSource = myOleDbDataReader;
            grdPolicies.DataBind();

            pnlPolicyGrid.Visible = true;
        }
    }








    private string GetStatusUpdatedDateInMRPSystem(string sProposalNo, string sFormId)
    {
        string returnVal = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;

        con.Open();

        String selectQuery = "";

        //In MRP DB payment update record is - 10	"Policy issued"
        //In MRP DB payment update record is - 12	"Add  New Payment"
        //In MRP DB payment update record is - 13	"Edit Payment"
        selectQuery = "SELECT  convert(varchar, Change_Date, 120) FROM Forms_Log WHERE  PropNo=@PropNo AND Formid=@Formid";



        SqlCommand cmd = new SqlCommand(selectQuery, con);
        cmd.Parameters.AddWithValue("@PropNo", sProposalNo);
        cmd.Parameters.AddWithValue("@Formid", sFormId);



        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            returnVal = Left(dr[0].ToString(), 19);
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;

    }



    protected void btnMsgOK_Click(object sender, EventArgs e)
    {
        //   updateStatusBySystem(System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowPaymentUpdatedStatuscode"].ToString());


    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("MRPWorkflow.aspx");
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


    protected void btnFollowup_Click(object sender, EventArgs e)
    {

    }






    private string getPendingRequirementsList(string sProposalNo, string lifeAssureNo, string sPremium)
    {

        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";
        selectQuery = "  SELECT " +
                         " MRP_WF_PENDING_COMPLETE_DOCS.PROPOSAL_NO," + //0
                         " MRP_WF_PENDING_COMPLETE_DOCS.LIFE_ASSURE," +//1
                         " MRP_WF_PENDING_COMPLETE_DOCS.IS_FAX_PENDING," +//2
                         " MRP_WF_PENDING_DOCS.PENDING_DOC_NAME, " +//3
                         " MRP_WF_PENDING_DOCS.PENDING_DOC_CODE " +//4
                         " FROM   HNBA_CRC.MRP_WF_PENDING_COMPLETE_DOCS MRP_WF_PENDING_COMPLETE_DOCS  " +
                         " INNER JOIN HNBA_CRC.MRP_WF_PENDING_DOCS MRP_WF_PENDING_DOCS  " +
                         " ON MRP_WF_PENDING_COMPLETE_DOCS.PENDING_DOC_CODE=MRP_WF_PENDING_DOCS.PENDING_DOC_CODE " +
                         " WHERE  " +
                         "  MRP_WF_PENDING_COMPLETE_DOCS.PROPOSAL_NO=:V_PROPOSAL_NO AND " +
        " MRP_WF_PENDING_COMPLETE_DOCS.LIFE_ASSURE=:V_LIFE_ASSURE_NO AND  " +
        " (MRP_WF_PENDING_COMPLETE_DOCS.IS_FAX_PENDING=1 OR MRP_WF_PENDING_COMPLETE_DOCS.IS_ORIGINAL_PENDING=1 ) ";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));

        cmd.Parameters.Add(new OracleParameter("V_LIFE_ASSURE_NO", lifeAssureNo));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                if (dr[4].ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPWFSinglePremiumPendingDocCode"].ToString())
                {
                    returnVal = returnVal + "<br />&nbsp;&nbsp;&nbsp;&nbsp; " + dr[3].ToString() + " of " + sPremium;
                }
                else
                {
                    returnVal = returnVal + "<br />&nbsp;&nbsp;&nbsp;&nbsp; " + dr[3].ToString();
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


    private string getNewPolicyNumber(string sProposalNo)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;
        string NewPolicyNumber = "";
        con.Open();

        String selectQuery = "";
        selectQuery = "SELECT " +
                       " ProposlReg.PolicyNo " +        //0
                      " FROM ProposlReg  " +
                      " WHERE ProposlReg.PropNo=@PropNo";


        SqlCommand cmd = new SqlCommand(selectQuery, con);
        cmd.Parameters.AddWithValue("@PropNo", sProposalNo);

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            NewPolicyNumber = dr[0].ToString();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return NewPolicyNumber;
    }


    private string getApprovedPerson(string sProposalNo)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;
        string ApprovedPerson = "";
        con.Open();

        String selectQuery = "";
        selectQuery = "SELECT  rtrim(UserID)   FROM Forms_Log   WHERE PropNo=@PropNo " +
                      " AND Formid=10";



        SqlCommand cmd = new SqlCommand(selectQuery, con);
        cmd.Parameters.AddWithValue("@PropNo", sProposalNo);

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            ApprovedPerson = dr[0].ToString();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
        return ApprovedPerson;
    }




    private bool CheckProposaIslInWorkflow(string sProposalNo)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";
        selectQuery = "SELECT PROPOSAL_NO   FROM MRP_WORKFLOW WHERE PROPOSAL_NO=:V_PROPOSAL_NO";


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




    public void Show(string message)
    {
        // Cleans the message to allow single quotation marks 
        string cleanMessage = message.Replace("'", "\\'");
        string script = "<script type=\"text/javascript\">" +
            "var hiddenField = document.getElementById(('<%= txtProposal.ClientID %>'));" +
        "if(confirm('Are you sure to cancel this proposal?','MRP Workflow'))" +
        "{alert('ok clicked');session('tda')='aaa';}" +
        "else{alert('cancel clicked');}</script>";



        // Gets the executing web page 
        Page page = HttpContext.Current.CurrentHandler as Page;

        // Checks if the handler is a Page and that the script isn't allready on the Page 
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", script);

        }
    }


    private void ClearComponents()
    {
        txtJobNumber.Text = "";
        txtPolicy.Text = "";
        txtProposal.Text = "";
        txtSumInsured.Text = "";
        txtLifeInsured1.Text = "";
        txtLifeInsured2.Text = "";
        txtBank.Text = "";
        txtAgentCode.Text = "";
        txtBranch.Text = "";
        txtRemarks.Text = "";

        txtAssignedUser.Text = "";
        txtApprovedUser.Text = "";

        txtCurrency.Text = "";


        txtReInsurer.Text = "";
        txtLifeAssured1HNBARefNo.Text = "";
        txtLifeAssured2HNBARefNo.Text = "";
        txtLifeAssured1RIRefNo.Text = "";
        txtLifeAssured2RIRefNo.Text = "";

        txtCommencementDate.Text = "";

        txtFullTerm.Text = "";

        txtFixedInterestTerm.Text = "";
        txtAdditionToAWPLR.Text = "";
        txtInterestRateType.Text = "";



        ddlBrokerCode.SelectedValue = "0";

        ddlStatus.SelectedValue = "0";
        ddlMedicalNonMedical.SelectedValue = "0";
        ddlMedical.SelectedValue = "0";
        ddlCoverNoteValidityPeriod.SelectedValue = "7 days";

        chkLifeassured1coversNaturalOrAccDeath.Checked = false;
        chkLifeassured1coversTPD.Checked = false;
        chkLifeassured2coversNaturalOrAccDeath.Checked = false;
        chkLifeassured2coversTPD.Checked = false;

        chkTPDMarketLimit.Checked = false;

        chkPPI.Checked = false;
        chkScanned.Checked = false;
        chkSkipToCertificateIssued.Checked = false;


        chkExcessPremiumReimbursementDone.Checked = false;
        chkMedicalReimbursementDone.Checked = false;



        hid_Ticker.Value = new TimeSpan(0, 0, 0).ToString();
        Timer1.Enabled = false;

    }

    private void LockComponents()
    {

        txtPolicy.Enabled = false;
        txtProposal.Enabled = false;
        txtSumInsured.Enabled = false;
        txtLifeInsured1.Enabled = false;
        txtLifeInsured2.Enabled = false;

        txtApprovedUser.Enabled = false;
        txtAssignedUser.Enabled = false;

        txtNIC1.Enabled = false;
        txtNIC2.Enabled = false;
        txtBank.Enabled = false;
        txtAgentCode.Enabled = false;
        txtAssuranceCode.Enabled = false;
        txtBranch.Enabled = false;
        txtRemarks.Enabled = false;

        txtAge1.Enabled = false;
        txtAge2.Enabled = false;

        txtReInsurer.Enabled = false;
        txtLifeAssured1HNBARefNo.Enabled = false;
        txtLifeAssured2HNBARefNo.Enabled = false;
        txtLifeAssured1RIRefNo.Enabled = false;
        txtLifeAssured2RIRefNo.Enabled = false;

        txtInterestRate1.Enabled = false;

        txtCommencementDate.Enabled = false;

        ddlStatus.Enabled = false;
        ddlMedical.Enabled = false;
        ddlMedicalNonMedical.Enabled = false;
        ddlCoverNoteValidityPeriod.Enabled = false;

        ddlBrokerCode.Enabled = false;

        chkLifeassured1coversNaturalOrAccDeath.Enabled = false;
        chkLifeassured1coversTPD.Enabled = false;
        chkLifeassured2coversNaturalOrAccDeath.Enabled = false;
        chkLifeassured2coversTPD.Enabled = false;



        chkPPI.Enabled = false;
        chkScanned.Enabled = false;
        chkSkipToCertificateIssued.Enabled = false;

        chkExcessPremiumReimbursementDone.Enabled = false;
        chkMedicalReimbursementDone.Enabled = false;

        chkLifeassured1BeneficiaryCover.Enabled = false;
        txtLifeassured1BeneficiaryName.Enabled = false;
        txtLifeassured1BeneficiaryNIC.Enabled = false;
        txtLifeassured1BeneficiaryAddress.Enabled = false;


        chkLifeassured1BeneficiaryCover.Enabled = false;
        txtLifeassured2BeneficiaryName.Enabled = false;
        txtLifeassured2BeneficiaryNIC.Enabled = false;
        txtLifeassured2BeneficiaryAddress.Enabled = false;

        btnFollowup.Enabled = false;



        chkLstClauses.Enabled = false;
        chkTPDMarketLimit.Enabled = false;
    }




    private string getBankType()
    {
        string agetnCode = "";
        agetnCode = txtAgentCode.Text;
        if (agetnCode == "")
        {
            return "";
        }

        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";


        selectQuery = "SELECT BANK_TYPE FROM MRP_WF_BANKS " +
            " WHERE BANK_CODE=:V_BANK_CODE ";



        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_BANK_CODE", agetnCode));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {

            dr.Read();
            returnVal = dr[0].ToString();
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }




    private string getServerDateTime()
    {


        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        selectQuery = "select   to_char(sysdate,'RRRR-MM-DD hh24:mi:ss') from dual";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {

            dr.Read();
            returnVal = dr[0].ToString();
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {

        if (txtProposal.Text == "")
        {
            lblMsg.Text = "Please Select A Proposal";
            Timer1.Enabled = true;
            return;
        }

        //updateFromMRPSystem(txtProposal.Text);//To update the data from MRP System regularly



        if (txtPolicy.Text == "")
        {
            lblMsg.Text = "Please Select A Policy";
            Timer1.Enabled = true;
            return;
        }


        if (ddlStatus.SelectedValue == System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowCancelledStatuscode"].ToString())
        {
            lblMsg.Text = "This workflow has been cancelled, And not allowed to modify.";
            Timer1.Enabled = true;
            return;
        }





        if (CheckProposaIslInWorkflow(txtProposal.Text))
        {
            ddlStatus.Enabled = true;
            ddlBrokerCode.Enabled = true;
        }
        else
        {
            ddlStatus.Enabled = false;
        }

        if (IsExcessPremiumVoucherGenerated(txtProposal.Text))
        {
            chkExcessPremiumReimbursementDone.Enabled = true;
        }
        else
        {
            chkExcessPremiumReimbursementDone.Enabled = false;
        }

        if (IsMedicalReimbursed(txtProposal.Text))
        {
            chkMedicalReimbursementDone.Enabled = true;
        }
        else
        {
            chkMedicalReimbursementDone.Enabled = false;
        }

        if (ddlStatus.SelectedValue == "1")
        {
            ddlMedicalNonMedical.Enabled = true;//Medical Or Non Medical can be choose only at processing state
            ddlMedical.Enabled = true;
            chkSkipToCertificateIssued.Enabled = true;
        }
        ddlCoverNoteValidityPeriod.Enabled = true;


        Session["PendingActionMode"] = "ADD";

        chkLifeassured1coversNaturalOrAccDeath.Enabled = true;
        chkLifeassured1coversTPD.Enabled = true;
        chkLifeassured2coversNaturalOrAccDeath.Enabled = true;
        chkLifeassured2coversTPD.Enabled = true;



        chkPPI.Enabled = true;
        chkScanned.Enabled = true;
        txtRemarks.Enabled = true;


        txtLifeAssured1HNBARefNo.Enabled = true;
        txtLifeAssured2HNBARefNo.Enabled = true;
        txtLifeAssured1RIRefNo.Enabled = true;
        txtLifeAssured2RIRefNo.Enabled = true;


        txtInterestRate1.Enabled = false;
        chkLstClauses.Enabled = true;
        chkTPDMarketLimit.Enabled = true;


        chkLifeassured1BeneficiaryCover.Enabled = true;
        txtLifeassured1BeneficiaryName.Enabled = true;
        txtLifeassured1BeneficiaryNIC.Enabled = true;
        txtLifeassured1BeneficiaryAddress.Enabled = true;


        chkLifeassured1BeneficiaryCover.Enabled = true;
        txtLifeassured2BeneficiaryName.Enabled = true;
        txtLifeassured2BeneficiaryNIC.Enabled = true;
        txtLifeassured2BeneficiaryAddress.Enabled = true;


        btnFollowup.Enabled = true;

        //enableDisableChkPPI();

        //string MRPSupervisoUserCode = System.Configuration.ConfigurationManager.AppSettings["MRPSupervisoUserCode"].ToString();
        //UserAuthentication userAuthentication = new UserAuthentication();
        //if (userAuthentication.getUserRoleCodeOfCurrentUser(Context.User.Identity.Name) != Convert.ToInt32(MRPSupervisoUserCode))
        //{
        //    chkPPI.Enabled = false;
        //}
    }
    protected void ddlSearchStatus_SelectedIndexChange(object sender, EventArgs e)
    {


        string MRPWorkflowRemindersPendingMedicalStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowRemindersPendingMedicalStatusCode"].ToString();
        string MRPWorkflowReminderConfirmationLetterStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowReminderConfirmationLetterStatusCode"].ToString();
        string MRPWorkflowRemindersCovernoteStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowRemindersCovernoteStatusCode"].ToString();

        if (ddlSearchStatus.SelectedValue.ToString() == MRPWorkflowRemindersPendingMedicalStatusCode || ddlSearchStatus.SelectedValue.ToString() == MRPWorkflowReminderConfirmationLetterStatusCode || ddlSearchStatus.SelectedValue.ToString() == MRPWorkflowRemindersCovernoteStatusCode)
        {
            ddlSearchReminderStage.Visible = true;
        }
        else
        {
            ddlSearchReminderStage.Visible = false;
        }
    }

    protected void grdPolicies_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearComponents();

        string ProposalNo = "";
        string StatusCode = "";

        Session.Remove("ProposalNo");

        ProposalNo = grdPolicies.SelectedRow.Cells[2].Text.Trim();
        StatusCode = grdPolicies.SelectedRow.Cells[8].Text.Trim();


        Session["ProposalNo"] = ProposalNo;



        loadSelectedClauses(ProposalNo);

        CheckOtherPoliciesInMRPSystem(grdPolicies.SelectedRow.Cells[2].Text.Trim());

        if (CheckProposaIslInWorkflow(grdPolicies.SelectedRow.Cells[2].Text.Trim()))
        {






            loadPolicyDetailsFromWorkflow(grdPolicies.SelectedRow.Cells[2].Text.Trim());

            string intRateWording = "";
            if (txtInterestRateType.Text == "Fixed")
            {
                intRateWording = txtInterestRate1.Text + "%";
            }
            else if (txtInterestRateType.Text == "Variable")
            {

                if ((Convert.ToInt32(txtFullTerm.Text) / 12) != Convert.ToInt32(txtFixedInterestTerm.Text))
                {
                    if (Convert.ToInt32(txtFixedInterestTerm.Text) == 0)
                    {
                        intRateWording = "AWPLR %2b " + txtAdditionToAWPLR.Text + "%"; //%2b is for "+"
                    }
                    else if (Convert.ToInt32(txtFixedInterestTerm.Text) > 0)
                    {
                        intRateWording = txtInterestRate1.Text + "% up to " + txtFixedInterestTerm.Text + " Years thereafter AWPLR %2b " + txtAdditionToAWPLR.Text + "%";
                    }

                }

            }





            LockComponents();
            SearchData();






            if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
            {
                loadMRPPolicyDetailsFromInitial(grdPolicies.SelectedRow.Cells[2].Text.Trim());


            }
            else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
            {
                loadMCRPolicyDetailsFromInitial(grdPolicies.SelectedRow.Cells[2].Text.Trim());


            }



            txtJobNumber.Text = grdPolicies.SelectedRow.Cells[1].Text;


            txtAssignedUserCode.Text = grdPolicies.SelectedRow.Cells[5].Text;
            txtAssignedUser.Text = grdPolicies.SelectedRow.Cells[6].Text;

        }




        loadFastTrackDetails(txtJobNumber.Text);







        LockComponents();

        Session["PendingActionMode"] = "VIEW";


        btnFollowup.Enabled = true;
    }

    private void loadMRPPolicyDetailsFromInitial(string sProposalNo)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;

        con.Open();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT " +
                       " ProposlReg.PolicyNo " +        //0
                      " ,ProposlReg.PropNo " +           //1
                      " ,CustomerInfo.LoanAmt1+CustomerInfo.LoanAmt2  +CustomerInfo.LoanAmt3 AS 'SUM_INSURED' " +      //2
                      " ,ProposlReg.Life1 " +            //3
                      " ,ProposlReg.Life2 " +            //4
                      " ,ProposlReg.NIC1 " +            //5
                      " ,ProposlReg.NIC2 " +            //6
                      " ,ProposlReg.Bank " +             //7
                      " ,ProposlReg.Otherbankname " +        //8
                      " ,ProposlReg.BNKCode AS 'AGENTCODE' " +//9
                      " ,ProposlReg.Branchname " +           //10
                      " ,ProposlReg.CurDate " +             //11
                      " ,PolicyInfo.PremiumFee  " +      //12
                      " ,CustomerInfo.LoanAmt1 " +            //13
                      " ,CustomerInfo.LoanAmt2 " +            //14
                      " ,CustomerInfo.LoanAmt3 " +            //15
                      " ,CustomerInfo.Inst1 " +            //16
                      " ,CustomerInfo.Inst2 " +            //17
                      " ,CustomerInfo.Inst3 " +            //18
                      " ,ProposlReg.Ascode " +            //19
                      " ,CustomerInfo.Age1 " +            //20
                      " ,CustomerInfo.Age2 " +            //21
                      " ,CustomerInfo.Term1 " +            //22
                      " ,CustomerInfo.Term2 " +            //23
                      " ,CustomerInfo.Term3 " +            //24
                      " ,PolicyInfo.Currency  " +      //25
                      " ,PolicyInfo.Recompany  " +      //26
                      " ,PolicyInfo.DateOfComm  " +      //27
                  " FROM ProposlReg  " +
                  " INNER JOIN PolicyInfo ON PolicyInfo.PolicyNo=ProposlReg.PolicyNo " +
                  " INNER JOIN CustomerInfo ON CustomerInfo.PropNo=ProposlReg.PropNo " +
                  " WHERE ProposlReg.PropNo='" + sProposalNo + "'";




        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            txtPolicy.Text = dr[0].ToString();
            txtProposal.Text = dr[1].ToString();
            txtSumInsured.Text = dr[2].ToString();
            txtLifeInsured1.Text = dr[3].ToString();
            txtLifeInsured2.Text = dr[4].ToString();

            txtNIC1.Text = dr[5].ToString();
            txtNIC2.Text = dr[6].ToString();

            txtBankType.Text = dr[7].ToString();
            if (dr[7].ToString() != ("OTHER BANK"))
            {
                txtBank.Text = dr[7].ToString();
            }
            else
            {
                txtBank.Text = dr[8].ToString();
            }


            txtAgentCode.Text = dr[9].ToString();
            txtBranch.Text = dr[10].ToString();

            txtCurDate.Text = dr[11].ToString();

            ddlStatus.SelectedValue = "1";
            ddlMedicalNonMedical.SelectedValue = "1";
            ddlCoverNoteValidityPeriod.SelectedValue = "7 days";

            txtPremium.Text = dr[12].ToString();

            txtLoanAmount1.Text = dr[13].ToString();
            txtLoanAmount2.Text = dr[14].ToString();
            txtLoanAmount3.Text = dr[15].ToString();
            txtInterestRate1.Text = dr[16].ToString();
            txtInterestRate2.Text = dr[17].ToString();
            txtInterestRate3.Text = dr[18].ToString();

            txtAssuranceCode.Text = dr[19].ToString();

            txtAge1.Text = dr[20].ToString();
            txtAge2.Text = dr[21].ToString();

            txtTerm1.Text = dr[22].ToString();
            txtTerm2.Text = dr[23].ToString();
            txtTerm3.Text = dr[24].ToString();

            txtCurrency.Text = dr[25].ToString();
            txtReInsurer.Text = dr[26].ToString();

            txtCommencementDate.Text = dr[27].ToString();
            //txtSumInsured1.Text=

            ////////
            String UserName = Context.User.Identity.Name;
            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }
            txtAssignedUser.Text = UserName;



            /////////////
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void loadMCRPolicyDetailsFromInitial(string sProposalNo)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;

        con.Open();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT " +
                       " PRM.PolicyNo " +        //0
                      " ,PRM.PropNo " +           //1
                      " ,CIM.LoanAmt1+CIM.LoanAmt2  +CIM.LoanAmt3 AS 'SUM_INSURED' " +      //2
                      " ,PRM.Life1 " +            //3
                      " ,PRM.Life2 " +            //4
                      " ,PRM.NIC1 " +            //5
                      " ,PRM.NIC2 " +            //6
                      " ,PRM.Bank " +             //7
                      " ,PRM.Otherbankname " +        //8
                      " ,PRM.BNKCode AS 'AGENTCODE' " +//9
                      " ,PRM.Branchname " +           //10
                      " ,PRM.CurDate " +             //11
                      " ,PIM.PremiumFee  " +      //12
                      " ,CIM.LoanAmt1 " +            //13
                      " ,CIM.LoanAmt2 " +            //14
                      " ,CIM.LoanAmt3 " +            //15
                      " ,CIM.Inst1 " +            //16
                      " ,CIM.Inst2 " +            //17
                      " ,CIM.Inst3 " +            //18
                      " ,PRM.Ascode " +            //19
                      " ,CIM.Age1 " +            //20
                      " ,CIM.Age2 " +            //21
                      " ,CIM.Term1 " +            //22
                      " ,CIM.Term2 " +            //23
                      " ,CIM.Term3 " +            //24
                      " ,PIM.Currency  " +      //25
                      " ,PIM.DateOfComm  " +      //26
                  " FROM ProposlRegMicro  PRM " +
                  " INNER JOIN PolicyInfoMicro PIM ON PIM.PolicyNo=PRM.PolicyNo " +
                  " INNER JOIN CustomerInfoMicro CIM ON CIM.PropNo=PRM.PropNo " +
                  " WHERE PRM.PropNo='" + sProposalNo + "'";




        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            txtPolicy.Text = dr[0].ToString();
            txtProposal.Text = dr[1].ToString();
            txtSumInsured.Text = dr[2].ToString();
            txtLifeInsured1.Text = dr[3].ToString();
            txtLifeInsured2.Text = dr[4].ToString();

            txtNIC1.Text = dr[5].ToString();
            txtNIC2.Text = dr[6].ToString();

            txtBankType.Text = dr[7].ToString();
            if (dr[7].ToString() != ("OTHER BANK"))
            {
                txtBank.Text = dr[7].ToString();
            }
            else
            {
                txtBank.Text = dr[8].ToString();
            }


            txtAgentCode.Text = dr[9].ToString();
            txtBranch.Text = dr[10].ToString();

            txtCurDate.Text = dr[11].ToString();

            ddlStatus.SelectedValue = "1";
            ddlMedicalNonMedical.SelectedValue = "1";
            ddlCoverNoteValidityPeriod.SelectedValue = "7 days";

            txtPremium.Text = dr[12].ToString();

            txtLoanAmount1.Text = dr[13].ToString();
            txtLoanAmount2.Text = dr[14].ToString();
            txtLoanAmount3.Text = dr[15].ToString();
            txtInterestRate1.Text = dr[16].ToString();
            txtInterestRate2.Text = dr[17].ToString();
            txtInterestRate3.Text = dr[18].ToString();

            txtAssuranceCode.Text = dr[19].ToString();

            txtAge1.Text = dr[20].ToString();
            txtAge2.Text = dr[21].ToString();

            txtTerm1.Text = dr[22].ToString();
            txtTerm2.Text = dr[23].ToString();
            txtTerm3.Text = dr[24].ToString();

            txtCurrency.Text = dr[25].ToString();
            txtReInsurer.Text = "";

            txtCommencementDate.Text = dr[26].ToString();
            //txtSumInsured1.Text=

            ////////
            String UserName = Context.User.Identity.Name;
            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }
            txtAssignedUser.Text = UserName;



            /////////////
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }




    private void loadPolicyDetailsFromWorkflow(string sProposalNo)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT 	" +
                    " T.PROPOSAL_NO ," +    //0
                    " T.POLICY_NO ," +      //1
                    " T.STATUS_CODE ," +    //2
                    " T.SUM_INSURED," +   //3
                    " T.LIFE_INSURED_1 ," + //4
                    " T.LIFE_INSURED_2 ," + //5
                    " T.NIC1 ," + //6
                    " T.NIC2 ," + //7
                    " T.BANK  ," +          //8
                    " T.OTHER_BANK_NAME ," +//9
                    " T.AGENT_CODE ," +     //10
                    " T.BRANCH_NAME," +     //11
                    " T.CUR_DATE ," +       //12
                    " T.MEDICAL_CODE, " +       //13
                    " T.IS_MEDICAL, " +       //14
                    " T.IS_LIFEASSU1_NATORACCDEATH ," +//15
                    " T.IS_LIFEASSU1_TPD ," +     //16
                    " T.IS_LIFEASSU2_NATORACCDEATH," +     //17
                    " T.IS_LIFEASSU2_TPD ," +       //18
                    " IS_PPI, " +       //19
                    " IS_SCANNED, " +       //20
                    " T.PREMIUM ," +    //21
                    " T.LOANAMT1 ," +    //22
                    " T.LOANAMT2 ," +    //23
                    " T.LOANAMT3 ," +    //24
                    " T.INST1 ," +    //25
                    " T.INST2 ," +    //26
                    " T.INST3, " +    //27
                    " T.IS_SKIPPED_STATUSES, " +    //28
                    " T.AS_CODE, " +    //29
                    " T.LIFE_INSURED_1_AGE ," +    //30
                    " T.LIFE_INSURED_2_AGE, " +    //31
                    " U.USER_NAME ," +    //32
                    " T.APPROVED_USER, " +    //33
                    " T.TERM1, " +    //34
                    " T.TERM2, " +    //35
                    " T.TERM3, " +    //36
                    " T.JOB_NO, " +    //37
                    " T.ASSIGNED_USER, " +    //38
                    " T.CURRENCY, " +    //39
                    " T.RE_INSURER, " +    //40
                    " T.LIFE_INS_1_HNBA_REF_NO, " +    //41
                    " T.LIFE_INS_2_HNBA_REF_NO, " +    //42
                    " T.LIFE_INS_1_RI_REF_NO, " +    //43
                    " T.LIFE_INS_2_RI_REF_NO, " +    //44
                   " T.COMMENCEMENT_DATE, " +    //45
                    " WB.COURIER_TYPE " +    //46
                     ",(SELECT PRT.PTY_PARTY_CODE AGENT_NAME FROM T_PARTY PRT, T_PARTY_VERSION PTV,T_PARTY_FUNCTION PTF, T_STAKE_HOLDER_FUNCTION STF WHERE PRT.PTY_PARTY_ID=PTV.PVR_PTY_PARTY_ID AND PTV.PVR_EFFECTIVE_END_DATE IS NULL AND PTF.PFY_PTY_PARTY_ID=PRT.PTY_PARTY_ID AND STF.SHR_STAKE_HOLDER_FN_ID=PTF.PFY_SHR_STAKE_HOLDER_FN_ID AND PTF.PFY_EFFECTIVE_END_DATE IS NULL AND STF.SHR_STAKE_HOLDER_FN_NAME LIKE 'Broker%' AND PTV.PVR_BUSINESS_NAME  IS NOT NULL AND PRT.PTY_PARTY_CODE = PJ.BROKER_CODE)BROKER_NAME, " +//47
                     " T.IS_TPD_M_LIMIT, " +    //48
                    " T.FULL_TERM, " +    //49
                    " PJ.IS_FAST_TRACK ,   " +    //50
                    " PJ.CREATED_DATE,    " +    //51
                     " T.TERM_OF_FIXED_INT, " +    //52
                    " T.ADDITION_TO_AWPLR, " +    //53
                    " T.INTEREST_TYPE, " +    //54
                    " T.CVR_NOTE_VAL_PERIOD, " +    //55
                    " T.IS_LIFE1_BENEF_CVR, " +    //56
                    " T.LIFE1_BENEF_NAME, " +    //57
                    " T.LIFE1_BENEF_NIC, " +    //58
                    " T.LIFE1_BENEF_ADDRESS, " +    //59
                    " T.IS_LIFE2_BENEF_CVR, " +    //60
                     " T.LIFE2_BENEF_NAME, " +    //61
                    " T.LIFE2_BENEF_NIC, " +    //62
                     " T.LIFE2_BENEF_ADDRESS " +    //63
                     " FROM MRP_WORKFLOW T " +
                    " LEFT JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER=U.USER_CODE " +
                     " LEFT JOIN mrp_wf_proposal_jobs PJ ON  T.JOB_NO=PJ.JOB_NO " +
                     " LEFT JOIN MRP_WF_BANKS WB ON  PJ.BANK_CODE=WB.BANK_CODE " +
                  " WHERE T.PROPOSAL_NO='" + sProposalNo + "'";



        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            txtProposal.Text = dr[0].ToString();
            txtPolicy.Text = dr[1].ToString();
            ddlStatus.SelectedValue = dr[2].ToString();
            txtSumInsured.Text = dr[3].ToString();
            txtLifeInsured1.Text = dr[4].ToString();
            txtLifeInsured2.Text = dr[5].ToString();
            txtNIC1.Text = dr[6].ToString();
            txtNIC2.Text = dr[7].ToString();
            txtBankType.Text = dr[8].ToString();
            if (dr[8].ToString() != ("OTHER BANK"))
            {
                txtBank.Text = dr[8].ToString();
            }
            else
            {
                txtBank.Text = dr[9].ToString();
            }


            txtAgentCode.Text = dr[10].ToString();
            txtBranch.Text = dr[11].ToString();

            txtCurDate.Text = dr[12].ToString();

            ddlMedical.SelectedValue = dr[13].ToString();
            ddlMedicalNonMedical.SelectedValue = dr[14].ToString();


            //New Development
            if (dr[47].ToString() != "")
            {
                ddlBrokerCode.SelectedValue = dr[47].ToString();
            }

            txtPremium.Text = dr[21].ToString();

            txtLoanAmount1.Text = dr[22].ToString();
            txtLoanAmount2.Text = dr[23].ToString();
            txtLoanAmount3.Text = dr[24].ToString();
            txtInterestRate1.Text = dr[25].ToString();
            txtInterestRate2.Text = dr[26].ToString();
            txtInterestRate3.Text = dr[27].ToString();

            txtAssuranceCode.Text = dr[29].ToString();

            txtAge1.Text = dr[30].ToString();
            txtAge2.Text = dr[31].ToString();

            txtAssignedUser.Text = dr[32].ToString();
            txtAssignedUserCode.Text = dr[38].ToString();

            txtApprovedUser.Text = dr[33].ToString();

            txtTerm1.Text = dr[34].ToString();
            txtTerm2.Text = dr[35].ToString();
            txtTerm3.Text = dr[36].ToString();

            txtJobNumber.Text = dr[37].ToString();

            txtCurrency.Text = dr[39].ToString();


            txtReInsurer.Text = dr[40].ToString();
            txtLifeAssured1HNBARefNo.Text = dr[41].ToString();
            txtLifeAssured2HNBARefNo.Text = dr[42].ToString();
            txtLifeAssured1RIRefNo.Text = dr[43].ToString();
            txtLifeAssured2RIRefNo.Text = dr[44].ToString();

            txtCommencementDate.Text = dr[45].ToString().Remove(10);

            lblCourierService.Text = dr[46].ToString();

            txtFullTerm.Text = dr[49].ToString();


            txtFixedInterestTerm.Text = dr[52].ToString();
            txtAdditionToAWPLR.Text = dr[53].ToString();
            txtInterestRateType.Text = dr[54].ToString();


            if (dr[55].ToString() != "")
            {
                ddlCoverNoteValidityPeriod.SelectedValue = dr[55].ToString();
            }

            if (dr[15].ToString() == "1")
            {
                chkLifeassured1coversNaturalOrAccDeath.Checked = true;
            }
            else
            {
                chkLifeassured1coversNaturalOrAccDeath.Checked = false;
            }

            if (dr[16].ToString() == "1")
            {
                chkLifeassured1coversTPD.Checked = true;
            }
            else
            {
                chkLifeassured1coversTPD.Checked = false;
            }

            if (dr[17].ToString() == "1")
            {
                chkLifeassured2coversNaturalOrAccDeath.Checked = true;
            }
            else
            {
                chkLifeassured2coversNaturalOrAccDeath.Checked = false;
            }

            if (dr[18].ToString() == "1")
            {
                chkLifeassured2coversTPD.Checked = true;
            }
            else
            {
                chkLifeassured2coversTPD.Checked = false;
            }

            if (dr[19].ToString() == "1")
            {
                chkPPI.Checked = true;
            }
            else
            {
                chkPPI.Checked = false;
            }

            if (dr[20].ToString() == "1")
            {
                chkScanned.Checked = true;
            }
            else
            {
                chkScanned.Checked = false;
            }
            if (dr[28].ToString() == "1")
            {
                chkSkipToCertificateIssued.Checked = true;
            }
            else
            {
                chkSkipToCertificateIssued.Checked = false;
            }


            if (dr[48].ToString() == "1")
            {
                chkTPDMarketLimit.Checked = true;
            }
            else
            {
                chkTPDMarketLimit.Checked = false;
            }


            if (dr[56].ToString() == "1")
            {
                chkLifeassured1BeneficiaryCover.Checked = true;
            }
            else
            {
                chkLifeassured1BeneficiaryCover.Checked = false;
            }
            txtLifeassured1BeneficiaryName.Text = dr[57].ToString();
            txtLifeassured1BeneficiaryNIC.Text = dr[58].ToString();
            txtLifeassured1BeneficiaryAddress.Text = dr[59].ToString();


            if (dr[60].ToString() == "1")
            {
                chkLifeassured2BeneficiaryCover.Checked = true;
            }
            else
            {
                chkLifeassured2BeneficiaryCover.Checked = false;
            }
            txtLifeassured2BeneficiaryName.Text = dr[61].ToString();
            txtLifeassured2BeneficiaryNIC.Text = dr[62].ToString();
            txtLifeassured2BeneficiaryAddress.Text = dr[63].ToString();





            if (chkLifeassured1BeneficiaryCover.Checked)
            {
                lblLifeassured1BeneficiaryName.Visible = true;
                txtLifeassured1BeneficiaryName.Visible = true;

                lblLifeassured1BeneficiaryNIC.Visible = true;
                txtLifeassured1BeneficiaryNIC.Visible = true;

                lblLifeassured1BeneficiaryAddress.Visible = true;
                txtLifeassured1BeneficiaryAddress.Visible = true;
            }
            else
            {
                lblLifeassured1BeneficiaryName.Visible = false;
                txtLifeassured1BeneficiaryName.Visible = false;

                lblLifeassured1BeneficiaryNIC.Visible = false;
                txtLifeassured1BeneficiaryNIC.Visible = false;

                lblLifeassured1BeneficiaryAddress.Visible = false;
                txtLifeassured1BeneficiaryAddress.Visible = false;
            }





            if (chkLifeassured2BeneficiaryCover.Checked)
            {
                //lblLifeassured2BeneficiaryName.Visible = true;
                //txtLifeassured2BeneficiaryName.Visible = true;

                //lblLifeassured2BeneficiaryNIC.Visible = true;
                //txtLifeassured2BeneficiaryNIC.Visible = true;

                //lblLifeassured2BeneficiaryAddress.Visible = true;
                //txtLifeassured2BeneficiaryAddress.Visible = true;
            }
            else
            {


                lblLifeassured2BeneficiaryName.Visible = false;
                txtLifeassured2BeneficiaryName.Visible = false;

                lblLifeassured2BeneficiaryNIC.Visible = false;
                txtLifeassured2BeneficiaryNIC.Visible = false;

                lblLifeassured2BeneficiaryAddress.Visible = false;
                txtLifeassured2BeneficiaryAddress.Visible = false;
            }




            if (dr[50].ToString() == "1")
            {

                lblIsFastTrackJob.Visible = true;
                lblIsFastTrackJob.Text = "Fast Track Job";


                ////////Fast Track Timer
                // System.DateTime dt1 = DateTime.Parse(System.DateTime.Now.ToString());
                // System.DateTime dt2 = DateTime.Parse(dr[51].ToString());
                //TimeSpan t = (dt2.Subtract(dt1));

                // hid_Ticker.Value = t.Hours.ToString() + ":" + t.Minutes.ToString() + ":" + t.Seconds.ToString();
                // hid_Ticker.Value = new TimeSpan(t.Hours.ToString(), t.Minutes.ToString(), t.Seconds.ToString());

                // hid_Ticker.Value = new TimeSpan(1, 0, 0).ToString();

                //  hid_Ticker.Value = TimeSpan.Parse(new TimeSpan(1, 0, 0).ToString()).Subtract(t).ToString();


                //if (hid_Ticker.Value != new TimeSpan(0, 0, 0).ToString())
                //{

                //    hid_Ticker.Value = TimeSpan.Parse(hid_Ticker.Value).Subtract(new TimeSpan(0, 0, 1)).ToString();
                //    lit_Timer.Text = "<font size=10 color=red>" + hid_Ticker.Value.ToString() + "</font>";
                //}



                tmrCountdown.Enabled = true;
                ////////Fast Track Timer
            }
            else
            {
                lblIsFastTrackJob.Text = "";


                lblIsFastTrackJob.Visible = false;
                lit_Timer.Text = "";
                tmrCountdown.Enabled = false;
            }

            validateNIC();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void validateNIC()
    {
        Page page = HttpContext.Current.CurrentHandler as Page;

        if (checkIsNICIsBlacklisted(txtNIC1.Text))
        {

            string script = "<script type=\"text/javascript\">" +
  "alert('NIC 1 is blacklisted')</script>";
            page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", script);


        }


        if (checkIsNICIsBlacklisted(txtNIC2.Text))
        {

            string script = "<script type=\"text/javascript\">" +
  "alert('NIC 2 is blacklisted')</script>";
            page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", script);


        }
    }
    private bool checkIsNICIsBlacklisted(string nic)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;


        String selectQuery = "";
        selectQuery = "SELECT T.NIC_NO FROM MRP_WF_BLACKLISTED_NIC T WHERE T.NIC_NO='" + nic + "' AND T.IS_BLACKLISTED=1";


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
    private void loadFastTrackDetails(string jobNo)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT 	" +
                    " PJ.IS_FAST_TRACK ,   " +    //0
                    " PJ.CREATED_DATE,    " +    //1
                    " SYSDATE    " +    //2
                     " FROM mrp_wf_proposal_jobs PJ  " +
                  " WHERE PJ.JOB_NO='" + jobNo + "'";



        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            if (dr[0].ToString() == "1")
            {
                lblIsFastTrackJob.Visible = true;
                lblIsFastTrackJob.Text = "Fast Track Job";

                ////////Fast Track Timer
                System.DateTime dt1 = DateTime.Parse(dr[2].ToString());
                System.DateTime dt2 = DateTime.Parse(dr[1].ToString());
                TimeSpan t = (dt1.Subtract(dt2));

                //hid_Ticker.Value = t.Hours.ToString() + ":" + t.Minutes.ToString() + ":" + t.Seconds.ToString();


                hid_Ticker.Value = TimeSpan.Parse(new TimeSpan(1, 0, 0).ToString()).Subtract(t).ToString();

                tmrCountdown.Enabled = true;
                ////////Fast Track Timer
            }
            else
            {
                lblIsFastTrackJob.Text = "";


                lblIsFastTrackJob.Visible = false;
                lit_Timer.Text = "";
                tmrCountdown.Enabled = false;
            }



        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }



    protected void grdPolicies_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[8].Visible = false;
    }



    private bool validatePrerequisites(string newStatusCode)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();



        string currentStatusCode = getCurrentStatus(txtProposal.Text);

        String selectQuery = "";
        selectQuery = "SELECT T.PREREQUISITES FROM MRP_WF_STATUSES T WHERE T.STATUS_CODE=:V_STATUS_CODE  AND IS_ACTIVE=1";

        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_STATUS_CODE", newStatusCode));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();



            string[] statuseCodes = dr[0].ToString().Split(',');
            foreach (string status in statuseCodes)
            {

                if (!status.Contains("/"))
                {
                    if (!checkPrerequisiteCompleted(status))
                    {
                        returnVal = false;
                        ddlStatus.SelectedValue = currentStatusCode;
                        break;
                    }
                    else
                    {
                        returnVal = true;
                    }
                }
                else
                {//check whether status is group of statuses
                    bool isPrereqCompleted = false;
                    string[] orStatuseCodes = status.Split('/');//to hold the statuses in multiple prerequisites 
                    //split the group of status codes from "or" word

                    foreach (string sts in orStatuseCodes)
                    {
                        if (checkPrerequisiteCompleted(sts.Trim()))
                        {
                            isPrereqCompleted = true;
                        }
                    }

                    if (isPrereqCompleted == false)
                    {
                        returnVal = false;
                    }
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

    private bool checkPrerequisiteCompleted(string sStatusCode)
    {


        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        string currentStatusCode = getCurrentStatus(txtProposal.Text);

        selectQuery = "SELECT T.STATUS_CODE FROM MRP_WORKFLOW_FOLLOWUP T WHERE T.PROPOSAL_NO='" + txtProposal.Text + "' AND T.STATUS_CODE=" + sStatusCode;

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

    private bool validateForDisallowPreviousStatus(string newStatusCode)
    {
        if (newStatusCode == "")
        {
            return false;
        }

        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        string currentStatusCode = getCurrentStatus(txtProposal.Text);

        selectQuery = "SELECT CASE WHEN (SELECT T.ORDER_NO FROM MRP_WF_STATUSES T WHERE T.STATUS_CODE=" + newStatusCode + " AND IS_ACTIVE=1)<(SELECT T.ORDER_NO FROM MRP_WF_STATUSES T WHERE T.STATUS_CODE=" + currentStatusCode + " AND IS_ACTIVE=1) THEN 'FAIL' ELSE 'PASS' END " +
                    " FROM DUAL";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            if (dr[0].ToString() == "FAIL")
            {
                returnVal = false;
                ddlStatus.SelectedValue = currentStatusCode;

            }
            else
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



    private bool IsExcessPremiumVoucherGenerated(string sProposalNo)
    {


        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";

        string ExcessPremiumVoucherGeneratedStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowExcessPremiumVoucherGeneratedStatusCode"].ToString();


        selectQuery = "SELECT t.PROPOSAL_NO FROM MRP_WORKFLOW_FOLLOWUP t " +
            " WHERE t.PROPOSAL_NO=:V_PROPOSAL_NO AND t.Status_Code=:V_STATUS_CODE";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));
        cmd.Parameters.Add(new OracleParameter("V_STATUS_CODE", ExcessPremiumVoucherGeneratedStatusCode));

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

    private bool IsMedicalReimbursed(string sProposalNo)
    {


        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        string MedicalReimbursementStatusCode = System.Configuration.ConfigurationManager.AppSettings["MRPWorkflowMedicalReimbursementStatusCode"].ToString();

        selectQuery = "SELECT t.PROPOSAL_NO FROM MRP_WORKFLOW_FOLLOWUP t " +
            " WHERE t.PROPOSAL_NO=:V_PROPOSAL_NO AND t.Status_Code=:V_STATUS_CODE";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));
        cmd.Parameters.Add(new OracleParameter("V_STATUS_CODE", MedicalReimbursementStatusCode));

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


    private bool checkAllOriginalPendingsCleared(string sProposalNo)
    {


        bool returnVal = true;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";

        string currentStatusCode = getCurrentStatus(txtProposal.Text);

        selectQuery = "SELECT PCD.proposal_no,PCD.pending_doc_code,PCD.IS_ORIGINAL_PENDING  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                        " WHERE  PCD.PROPOSAL_NO=:V_PROPOSAL_NO  AND  PCD.IS_ORIGINAL_PENDING=1";

        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            returnVal = false;
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }
    protected void btnOpenDashboard_Click(object sender, EventArgs e)
    {
        UserAuthentication userAuthentication = new UserAuthentication();

        string UserName = Context.User.Identity.Name;

        UserName = Right(UserName, (UserName.Length) - 5);

        //Session["USER_ID"] = UserName;
        //Session["PAGE_ID"] = 1;

        //string url = "http://192.168.10.13:9199/MRP_Dashboard/MRP_Home.aspx";
        //StringBuilder sb = new StringBuilder();
        //sb.Append("<script type = 'text/javascript'>");
        //sb.Append("window.open('");
        //sb.Append(url);
        //sb.Append("');");
        //sb.Append("</script>");
        //ClientScript.RegisterStartupScript(this.GetType(),
        //        "script", sb.ToString());




        HttpCookie USER_ID = new HttpCookie("DASHBOARDAPP_USER_ID");
        Response.Cookies.Add(USER_ID);
        Response.Cookies["DASHBOARDAPP_USER_ID"].Value = UserName;//"gayani.alwis";//"udaya.piyasena";//"rebecca.fernando";//"sahan.511";//"shamali.sanjeewani";//"shanika.amarasinghe";//"dinesh.udawatta";//"shanika.amarasinghe";

        HttpCookie PAGE_ID = new HttpCookie("DASHBOARDAPP_PAGE_ID");
        Response.Cookies.Add(PAGE_ID);
        Response.Cookies["DASHBOARDAPP_PAGE_ID"].Value = "1";

        string url = "http://192.168.10.54:82/Dashboard_App/Index.aspx";
        //"http://192.168.10.89:82/Dashboard_App/Index.aspx";
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.open('");
        sb.Append(url);
        sb.Append("');");
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());
    }

    private bool checkAllPendingsCleared(string sProposalNo)
    {


        bool returnVal = true;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";

        string currentStatusCode = getCurrentStatus(txtProposal.Text);

        selectQuery = "SELECT PCD.proposal_no FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                        " WHERE  PCD.PROPOSAL_NO=:V_PROPOSAL_NO AND  (PCD.IS_FAX_PENDING=1   OR  PCD.IS_ORIGINAL_PENDING=1) ";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            returnVal = false;
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }



    private string getCurrentStatus(string sProposalNo)
    {
        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();


        String selectQuery = "";
        selectQuery = "SELECT 	" +
                    " T.STATUS_CODE " +
                    " FROM MRP_WORKFLOW T " +
                  " WHERE T.PROPOSAL_NO=:V_PROPOSAL_NO";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", sProposalNo));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            returnVal = dr[0].ToString();
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }

    private int getOrderOfStatus(string sStatusCode)
    {
        int returnVal = 0;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        String selectQuery = "";
        selectQuery = "SELECT T.ORDER_NO FROM MRP_WF_STATUSES T WHERE T.STATUS_CODE=:V_STATUS_CODE";


        OracleCommand cmd = new OracleCommand(selectQuery, con);
        cmd.Parameters.Add(new OracleParameter("V_STATUS_CODE", sStatusCode));

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            returnVal = Convert.ToInt32(dr[0].ToString());
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









    protected void tmrCountdown_Tick(object sender, EventArgs e)
    {
        if (hid_Ticker.Value.Contains("-"))
        {
            return;
        }

        if (hid_Ticker.Value != new TimeSpan(0, 0, 0).ToString())
        {

            hid_Ticker.Value = TimeSpan.Parse(hid_Ticker.Value).Subtract(new TimeSpan(0, 0, 1)).ToString();
            lit_Timer.Text = "<font size=10 color=red>" + hid_Ticker.Value.ToString() + "</font>";
        }
    }

}
