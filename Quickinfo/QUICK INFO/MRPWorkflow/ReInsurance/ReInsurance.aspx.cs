//******************************************
// Author            :Tharindu Athapattu
// Date              :08/05/2018
// Reviewed By       :
// Description       : ReInsurance
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

public partial class ReInsurance : System.Web.UI.Page
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

            Session.Remove("SetupMode");

            pnlSearchResult.Visible = false;
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
    private void loadDeparments()
    {
        ddlDepartment.Items.Clear();
        ddlDepartment.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchDepartment.Items.Clear();
        ddlSearchDepartment.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT DEPT_ID,DEPT_NAME  FROM MRP_WF_DEPARTMENT ORDER BY DEPT_ID ASC ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlDepartment.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlSearchDepartment.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }
    private void loadSendingReason()
    {
        ddlSendingReason.Items.Clear();
        ddlSendingReason.Items.Add(new ListItem("--- Select One ---", "0"));

        
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT SR_ID,SR_NAME  FROM MRP_WF_RE_INS_SENDING_REASON ORDER BY SR_NAME ASC ";



        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlSendingReason.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }


    private void loadRICompanies()
    {
        ddlReInsurer.Items.Clear();
        ddlReInsurer.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlSearchReInsurer.Items.Clear();
        ddlSearchReInsurer.Items.Add(new ListItem("--- Select One ---", "0"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT RE_INS_COMPANY_ID,RE_INS_COMPANY_NAME  FROM MRP_WF_RE_INS_COMPANY ORDER BY RE_INS_COMPANY_NAME ASC ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlReInsurer.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlSearchReInsurer.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

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

        ClearComponents();
        SearchData();


    }

    private void SearchData()
    {
        string SQL = "";
        lblError.Text = "";
        grdSearchResult.DataSource = null;
        grdSearchResult.DataBind();

        if ((txtSearchHNBARefNo.Text == "") && (txtSearchProposalNo.Text == "") &&
            (ddlSearchDepartment.SelectedValue == "" || ddlSearchDepartment.SelectedValue == "0") &&
            (ddlSearchReInsurer.SelectedValue == "" || ddlSearchReInsurer.SelectedValue == "0"))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchHNBARefNo.Text != "")
        {

            SQL = "(LOWER(T.HNBA_REF_NO) LIKE '%" + txtSearchHNBARefNo.Text.ToLower() + "%') AND";
        }
        if (txtSearchProposalNo.Text != "")
        {

            SQL = "(LOWER(T.PROPOSAL_NO) LIKE '%" + txtSearchProposalNo.Text.ToLower() + "%') AND";
        }


        if (ddlSearchDepartment.SelectedValue != "0")
        {
            SQL = SQL + "(T.DEPARTMENT_ID = '" + ddlSearchDepartment.SelectedValue + "') AND";

        }

        if (ddlSearchReInsurer.SelectedValue != "0")
        {
            SQL = SQL + "(T.RE_INSURER_ID = '" + ddlSearchReInsurer.SelectedValue + "') AND";

        }



        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.SEQ_ID ,R.RE_INS_COMPANY_NAME AS \"Re-Insurer\" ,T.HNBA_REF_NO AS \"Job No.\"  ,T.PROPOSAL_NO AS \"Proposal No.\" FROM MRP_WF_RE_INSURANCE T  " +
            " LEFT JOIN MRP_WF_RE_INS_COMPANY R ON T.RE_INSURER_ID=R.RE_INS_COMPANY_ID " +
                      " WHERE (" + SQL + ") ORDER BY T.SEQ_ID ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdSearchResult.DataSource = myOleDbDataReader;
            grdSearchResult.DataBind();

            pnlSearchResult.Visible = true;
        }
    }






    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReInsurance.aspx");
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
        if (ddlDepartment.SelectedValue == "" || ddlDepartment.SelectedValue == "0")
        {
            lblMsg.Text = "Please Select the Department";
            Timer1.Enabled = true;
            return;
        }
        if (txtProposalNo.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter Proposal No.";
            Timer1.Enabled = true;
            return;
        }
        //if (ddlReInsurer.SelectedValue == "" || ddlReInsurer.SelectedValue == "0")
        //{
        //    lblMsg.Text = "Please Select the Re-Insurance Company";
        //    Timer1.Enabled = true;
        //    return;
        //}



        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();

            OracleCommand spProcess = null;
            if (Session["SetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_RE_INSURANCE");
            }
            else if (Session["SetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_RE_INSURANCE");
            }

            String UserName = Context.User.Identity.Name;


            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }

            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            if (Session["SetupMode"].ToString() == "NEW")
            {
                spProcess.Parameters.Add("V_DEPARTMENT_ID", OracleType.Number).Value = ddlDepartment.SelectedValue;
                spProcess.Parameters.Add("V_SENDING_DATE", OracleType.DateTime).Value = txtDateOfSending.Text;

                spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = txtProposalNo.Text;
                spProcess.Parameters.Add("V_RE_INSURER_ID", OracleType.Number).Value = ddlReInsurer.SelectedValue;
                spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = UserName;

                spProcess.Parameters.Add("V_SENDING_REASON_ID", OracleType.Number).Value = ddlSendingReason.SelectedValue;

                spProcess.Parameters.Add("V_HNBA_REF_NO", OracleType.VarChar, 20).Direction = ParameterDirection.Output;
                spProcess.Parameters["V_HNBA_REF_NO"].Direction = ParameterDirection.Output;

                spProcess.Parameters.Add("V_SEQ_ID", OracleType.Number, 20).Direction = ParameterDirection.Output;
                spProcess.Parameters["V_SEQ_ID"].Direction = ParameterDirection.Output;


                spProcess.ExecuteNonQuery();


                txtSeqId.Text = Convert.ToString(spProcess.Parameters["V_SEQ_ID"].Value);
                txtHNBARefNo.Text = Convert.ToString(spProcess.Parameters["V_HNBA_REF_NO"].Value);


                ShowAlert("HNBA Ref. No. - " + txtHNBARefNo.Text);


            }
            else
            {
                spProcess.Parameters.Add("V_SEQ_ID", OracleType.Number).Value = Convert.ToInt32(txtSeqId.Text);
                spProcess.Parameters.Add("V_RI_REF_NO", OracleType.VarChar).Value = txtRIRefNo.Text;
                spProcess.Parameters.Add("V_RI_DECISION", OracleType.VarChar).Value = txtRIDecision.Text;
                spProcess.Parameters.Add("V_DATE_OF_DECISION", OracleType.DateTime).Value = txtDateOfDecision.Text;

                spProcess.Parameters.Add("V_RE_INSURER_ID", OracleType.Number).Value = ddlReInsurer.SelectedValue;
                spProcess.ExecuteNonQuery();

            }







            conProcess.Close();

            LockComponents();
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

    public void ShowAlert(string message)
    {
        // Cleans the message to allow single quotation marks 
        string cleanMessage = message.Replace("'", "\\'");
        string script = "<script type=\"text/javascript\">" +
            "alert('" + message + "');</script>";



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

        //        txtSearchHNBARefNo
        //ddlSearchDepartment
        //txtSearchProposalNo
        //ddlSearchReInsurer

        txtSeqId.Text = "";
        txtHNBARefNo.Text = "";
        ddlDepartment.SelectedValue = "0";
        ddlSendingReason.SelectedValue = "0";
        txtDateOfSending.Text = "";
        txtProposalNo.Text = "";
        txtLifeAssure1Name.Text = "";
        txtLifeAssure2Name.Text = "";
        txtLifeAssure1NIC.Text = "";
        txtLifeAssure2NIC.Text = "";
        txtPolicyNo.Text = "";
        txtSumInsured.Text = "";
        ddlReInsurer.SelectedValue = "0";
        txtRIRefNo.Text = "";
        txtRIDecision.Text = "";
        txtDateOfDecision.Text = "";



        txtSeqId.Enabled = false;
        txtHNBARefNo.Enabled = false;
        ddlDepartment.Enabled = false;
        ddlSendingReason.Enabled = false;
        txtDateOfSending.Enabled = false;
        txtProposalNo.Enabled = false;
        txtLifeAssure1Name.Enabled = false;
        txtLifeAssure2Name.Enabled = false;
        txtLifeAssure1NIC.Enabled = false;
        txtLifeAssure2NIC.Enabled = false;
        txtPolicyNo.Enabled = false;
        txtSumInsured.Enabled = false;
        ddlReInsurer.Enabled = false;
        txtRIRefNo.Enabled = false;
        txtRIDecision.Enabled = false;
        txtDateOfDecision.Enabled = false;



        btnLoadDataFromProposalNo.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //  btnCancel.Enabled = false;
    }


    private void LockComponents()
    {


        txtSeqId.Enabled = false;
        txtHNBARefNo.Enabled = false;
        ddlDepartment.Enabled = false;
        ddlSendingReason.Enabled = false;
        txtDateOfSending.Enabled = false;
        txtProposalNo.Enabled = false;
        txtLifeAssure1Name.Enabled = false;
        txtLifeAssure2Name.Enabled = false;
        txtLifeAssure1NIC.Enabled = false;
        txtLifeAssure2NIC.Enabled = false;
        txtPolicyNo.Enabled = false;
        txtSumInsured.Enabled = false;
        ddlReInsurer.Enabled = false;
        txtRIRefNo.Enabled = false;
        txtRIDecision.Enabled = false;
        txtDateOfDecision.Enabled = false;


        btnLoadDataFromProposalNo.Enabled = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;
        //  btnCancel.Enabled = false;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txtSeqId.Enabled = false;
        txtHNBARefNo.Enabled = false;
        ddlDepartment.Enabled = true;
        ddlSendingReason.Enabled = true;
        txtDateOfSending.Enabled = true;
        txtProposalNo.Enabled = true;
        txtLifeAssure1Name.Enabled = false;
        txtLifeAssure2Name.Enabled = false;
        txtLifeAssure1NIC.Enabled = false;
        txtLifeAssure2NIC.Enabled = false;
        txtPolicyNo.Enabled = false;
        txtSumInsured.Enabled = false;
        ddlReInsurer.Enabled = true;
        txtRIRefNo.Enabled = false;
        txtRIDecision.Enabled = false;
        txtDateOfDecision.Enabled = false;
        btnLoadDataFromProposalNo.Enabled = true;

        txtSeqId.Text = "";
        txtHNBARefNo.Text = "";
        ddlDepartment.SelectedValue = "0";
        ddlSendingReason.SelectedValue = "0";
        txtDateOfSending.Text = "";
        txtProposalNo.Text = "";
        txtLifeAssure1Name.Text = "";
        txtLifeAssure2Name.Text = "";
        txtLifeAssure1NIC.Text = "";
        txtLifeAssure2NIC.Text = "";
        txtPolicyNo.Text = "";
        txtSumInsured.Text = "";
        ddlReInsurer.SelectedValue = "0";
        txtRIRefNo.Text = "";
        txtRIDecision.Text = "";
        txtDateOfDecision.Text = "";



        btnSave.Enabled = true;

        Session["SetupMode"] = "NEW";
    }

    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtSeqId.Text == "")
        {
            lblMsg.Text = "Please Select A Job";
            return;
        }


        txtSeqId.Enabled = false;
        txtHNBARefNo.Enabled = false;
        ddlDepartment.Enabled = false;
        ddlSendingReason.Enabled = false;
        txtDateOfSending.Enabled = false;
        txtProposalNo.Enabled = false;
        txtLifeAssure1Name.Enabled = false;
        txtLifeAssure2Name.Enabled = false;
        txtLifeAssure1NIC.Enabled = false;
        txtLifeAssure2NIC.Enabled = false;
        txtPolicyNo.Enabled = false;
        txtSumInsured.Enabled = false;
        ddlReInsurer.Enabled = true;
        txtRIRefNo.Enabled = true;
        txtRIDecision.Enabled = true;
        txtDateOfDecision.Enabled = true;

        btnLoadDataFromProposalNo.Enabled = false;

        btnSave.Enabled = true;

        Session["SetupMode"] = "UPDATE";
    }




    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtSeqId.Text = grdSearchResult.SelectedRow.Cells[1].Text.Trim();


        if (ddlSearchDepartment.SelectedValue == "1")
        {

            loadMRPData(txtSeqId.Text);


        }
        else if (ddlSearchDepartment.SelectedValue == "2")
        {
            loadNewBusinessData(txtSeqId.Text);

        }
        btnAlter.Enabled = true;


    }



    private void loadMRPData(string seqId)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "  SELECT " +
                        " T.SEQ_ID, " +//0
                        " T.HNBA_REF_NO, " +//1
                        " T.DEPARTMENT_ID, " +//2
                        " T.SENDING_DATE, " +//3
                        " T.PROPOSAL_NO, " +//4
                        " T.RE_INSURER_ID, " +//5
                        " T.RI_REF_NO, " +//6
                        " T.RI_DECISION, " +//7
                        " T.DATE_OF_DECISION, " +//8
                        " W.LIFE_INSURED_1, " +//9
                        " W.LIFE_INSURED_2, " +//10
                        " W.NIC1, " +//11
                        " W.NIC2, " +//12
                        " W.POLICY_NO, " +//13
                        " W.SUM_INSURED, " +//14
                        " T.SENDING_REASON_ID " +//15
                        " FROM MRP_WF_RE_INSURANCE T " +
                        " INNER JOIN MRP_WORKFLOW W ON T.PROPOSAL_NO=W.PROPOSAL_NO " +

                      " WHERE T.SEQ_ID=" + seqId;


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();


        if (dr.HasRows)
        {
            dr.Read();


            txtSeqId.Text = dr[0].ToString();
            txtHNBARefNo.Text = dr[1].ToString();
            ddlDepartment.SelectedValue = dr[2].ToString();
            txtDateOfSending.Text = dr[3].ToString();
            txtProposalNo.Text = dr[4].ToString();
            txtLifeAssure1Name.Text = dr[9].ToString();
            txtLifeAssure2Name.Text = dr[10].ToString();
            txtLifeAssure1NIC.Text = dr[11].ToString();
            txtLifeAssure2NIC.Text = dr[12].ToString();
            txtPolicyNo.Text = dr[13].ToString();
            txtSumInsured.Text = dr[14].ToString();
            ddlReInsurer.SelectedValue = dr[5].ToString();
            txtRIRefNo.Text = dr[6].ToString();
            txtRIDecision.Text = dr[7].ToString();
            txtDateOfDecision.Text = dr[8].ToString();

            ddlSendingReason.SelectedValue = dr[15].ToString();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }



    private void loadNewBusinessData(string seqId)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "  SELECT " +
                        " T.SEQ_ID, " +//0
                        " T.HNBA_REF_NO, " +//1
                        " T.DEPARTMENT_ID, " +//2
                        " T.SENDING_DATE, " +//3
                        " T.PROPOSAL_NO, " +//4
                        " T.RE_INSURER_ID, " +//5
                        " T.RI_REF_NO, " +//6
                        " T.RI_DECISION, " +//7
                        " T.DATE_OF_DECISION, " +//8
                        " W.t_insured_name, " +//9
                        " W.t_cus_nic, " +//10
                        " W.t_pol_num, " +//11
                        " W.t_si, " +//12
                        " T.SENDING_REASON_ID " +//13
                        " FROM MRP_WF_RE_INSURANCE T " +
                        " INNER JOIN T_PREMIUM_DUE_REP_BI W ON T.PROPOSAL_NO=W.T_PROP_NUM " +

                      " WHERE T.SEQ_ID=" + seqId;


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();


        if (dr.HasRows)
        {
            dr.Read();


            txtSeqId.Text = dr[0].ToString();
            txtHNBARefNo.Text = dr[1].ToString();
            ddlDepartment.SelectedValue = dr[2].ToString();
            txtDateOfSending.Text = dr[3].ToString();
            txtProposalNo.Text = dr[4].ToString();

            ddlReInsurer.SelectedValue = dr[5].ToString();
            txtRIRefNo.Text = dr[6].ToString();
            txtRIDecision.Text = dr[7].ToString();
            txtDateOfDecision.Text = dr[8].ToString();

            txtLifeAssure1Name.Text = dr[9].ToString();
            txtLifeAssure2Name.Text = "";
            txtLifeAssure1NIC.Text = dr[10].ToString();
            txtLifeAssure2NIC.Text = "";
            txtPolicyNo.Text = dr[11].ToString();
            txtSumInsured.Text = dr[12].ToString();


            ddlSendingReason.SelectedValue = dr[13].ToString();
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }



    protected void grdSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";

        loadDeparments();
        loadRICompanies();
        loadSendingReason();
    }





    protected void btnLoadDataFromProposalNo_Click(object sender, EventArgs e)
    {
        if (txtProposalNo.Text == "")
        {
            lblMsg.Text = "Please enter a Proposal No.";
            return;
        }
        else
        {



            if (ddlDepartment.SelectedValue == "1")
            {

                loadMRPDataFromProposalNo(txtProposalNo.Text);

            }
            else if (ddlDepartment.SelectedValue == "2")
            {
                loadNewBusinessDataFromProposalNo(txtProposalNo.Text);

            }



        }





    }



    private void loadMRPDataFromProposalNo(string proposalNo)
    {
        string SQL = "";
        lblError.Text = "";


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;

        if (txtProposalNo.Text != "")
        {

            SQL = "(LOWER(T.PROPOSAL_NO) = '" + proposalNo.ToLower() + "') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);




        String selectQuery = "";
        selectQuery = "   SELECT " +
                     " T.LIFE_INSURED_1 ," + //0
                    " T.LIFE_INSURED_2 ," + //1
                    " T.NIC1 ," + //2
                    " T.NIC2 ," + //3
                    " T.POLICY_NO ," +      //4
                    " T.SUM_INSURED " +   //5
            " FROM MRP_WORKFLOW T  " +
                      " WHERE (" + SQL + ")";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();


        if (dr.HasRows)
        {
            dr.Read();

            txtLifeAssure1Name.Text = dr[0].ToString();
            txtLifeAssure2Name.Text = dr[1].ToString();
            txtLifeAssure1NIC.Text = dr[2].ToString();
            txtLifeAssure2NIC.Text = dr[3].ToString();
            txtPolicyNo.Text = dr[4].ToString();
            txtSumInsured.Text = dr[5].ToString();


        }
        else
        {
            lblMsg.Text = "Please enter a valid Proposal No.";

        }

        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }

    private void loadNewBusinessDataFromProposalNo(string proposalNo)
    {
        string SQL = "";
        lblError.Text = "";



        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;

        if (txtProposalNo.Text != "")
        {

            SQL = "(LOWER(T.t_prop_num) = '" + proposalNo.ToLower() + "') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);




        String selectQuery = "";
        selectQuery = "  select  " +
                    " t.t_insured_name, " + //0
                    " t.t_cus_nic, " + //1
                    " t.t_pol_num, " + //2
                    " t.t_si, " + //3
                    " t.t_prop_num " + //4
                     " from  " +
                    "  t_premium_due_rep_bi t  " +
                      " WHERE (" + SQL + ")";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();


        if (dr.HasRows)
        {
            dr.Read();

            txtLifeAssure1Name.Text = dr[0].ToString();
            txtLifeAssure2Name.Text = "";
            txtLifeAssure1NIC.Text = dr[1].ToString();
            txtLifeAssure2NIC.Text = "";
            txtPolicyNo.Text = dr[2].ToString();
            txtSumInsured.Text = dr[3].ToString();


        }
        else
        {
            lblMsg.Text = "Please enter a valid Proposal No.";

        }

        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }
    public string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }




}
