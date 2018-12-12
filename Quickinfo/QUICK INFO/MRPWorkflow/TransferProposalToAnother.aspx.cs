//******************************************
// Author            :Tharindu Athapattu
// Date              :01/08/2013
// Reviewed By       :
// Description       : Transfer Proposal To Another USer
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

public partial class TransferProposalToAnother : System.Web.UI.Page
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



            pnlProposalsGrid.Visible = false;
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


                string MRPManagerUserName = System.Configuration.ConfigurationManager.AppSettings["MRPManagerUserName"].ToString();

                string UserName = Context.User.Identity.Name;


                if (!MRPManagerUserName.Contains(Right(UserName, (UserName.Length) - 5)))
                {
                    lblMsg.Text = "Reassigning facility is only available for MRP Managers";
                    Timer1.Enabled = true;
                    return;
                }
            }
        }
    }
    private void loadAssignedTo()
    {
        ddlSearchAssignedTo.Items.Clear();
        ddlSearchAssignedTo.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlAssignedTo.Items.Clear();
        ddlAssignedTo.Items.Add(new ListItem("--- Select One ---", "0"));

        ddlTransferTo.Items.Clear();
        ddlTransferTo.Items.Add(new ListItem("--- Select One ---", "0"));

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
                ddlAssignedTo.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                ddlTransferTo.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

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
    }

    private void SearchData()
    {
        string SQL = "";
        lblError.Text = "";
        grdProposals.DataSource = null;
        grdProposals.DataBind();

        if ((txtSearchJobNumber.Text == "") && (txtSearchProposal.Text == "") && (ddlSearchAssignedTo.SelectedValue == "" || ddlSearchAssignedTo.SelectedValue == "0"))
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
        if (txtSearchProposal.Text != "")
        {
            SQL = SQL + "(LOWER(T.PROPOSAL_NO) LIKE '%" + txtSearchProposal.Text.ToLower() + "%') AND";
        }
        if (ddlSearchAssignedTo.SelectedValue != "" && ddlSearchAssignedTo.SelectedValue != "0")
        {
            SQL = SQL + "(T.ASSIGNED_USER_CODE = '" + ddlSearchAssignedTo.SelectedValue + "') AND";
        }

        SQL = SQL + " (T.WORKFLOW_TYPE = '" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "') AND";

        SQL = SQL.Substring(0, SQL.Length - 3);


        String selectQuery = "";
        selectQuery = "   SELECT T.JOB_NO AS \"Job No\",T.PROPOSAL_NO AS \"Proposal No\",T.ASSIGNED_USER_CODE,U.USER_NAME  AS \"Assigned User\"  FROM   MRP_WF_PROPOSAL_JOBS  T  " +
             "INNER JOIN WF_ADMIN_USERS U ON  T.ASSIGNED_USER_CODE=U.USER_CODE " +
             "INNER JOIN MRP_WORKFLOW F ON  T.PROPOSAL_NO=F.PROPOSAL_NO " +
            " WHERE (" + SQL + " AND F.STATUS_CODE NOT IN (7,8,17,22,23)) ORDER BY T.JOB_NO ASC";

        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdProposals.DataSource = myOleDbDataReader;
            grdProposals.DataBind();

            pnlProposalsGrid.Visible = true;
        }
    }




    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("TransferProposalToAnother.aspx");
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtProposal.Text.Trim() == "" || txtJobNumber.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter the Proposal Number / Job Number";
            Timer1.Enabled = true;
            return;
        }

        try
        {
            String UserName = Context.User.Identity.Name;


            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }

            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            spProcess = new OracleCommand("TRANSFER_ASSIGNED_TO");

            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;

            spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = txtProposal.Text;
            spProcess.Parameters.Add("V_JOB_NO", OracleType.VarChar, 40).Value = txtJobNumber.Text;

            spProcess.Parameters.Add("V_ASSIGNED_USER_CODE", OracleType.VarChar, 100).Value = ddlAssignedTo.SelectedValue;
            spProcess.Parameters.Add("V_TRANSFERED_USER_CODE", OracleType.VarChar, 100).Value = ddlTransferTo.SelectedValue;

            spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar, 100).Value = UserName;


            spProcess.ExecuteNonQuery();
            conProcess.Close();


            addCommentToBlackboard(txtProposal.Text, ddlTransferTo.SelectedItem.Text, UserName);

            ClearComponents();
            SearchData();

            sendJobAllocatedMailToUsers();

            lblMsg.Text = "Successfully saved";
            Timer1.Enabled = true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }

    }


    private void addCommentToBlackboard(string proposalNo, string reassignedUserName, string assignedBy)
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            spProcess = new OracleCommand("INSERT_MRP_WF_BLACKBOARD_CMT");


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = proposalNo;
            spProcess.Parameters.Add("V_USER_COMMENT", OracleType.VarChar).Value = "Job Re-assigned to " + reassignedUserName;
            spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = assignedBy;


            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {

        }
    }
    private void ClearComponents()
    {
        txtJobNumber.Text = "";
        txtProposal.Text = "";
        ddlAssignedTo.SelectedValue = "0";
        ddlTransferTo.SelectedValue = "0";

        txtJobNumber.Enabled = false;
        txtProposal.Enabled = false;
        ddlAssignedTo.Enabled = false;
        ddlTransferTo.Enabled = false;


        btnAlter.Enabled = false;
        btnSave.Enabled = false;

    }



    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtProposal.Text == "")
        {
            lblMsg.Text = "Please Select A Proposal";
            return;
        }



        ddlTransferTo.Enabled = true;

        btnSave.Enabled = true;
    }

    protected void grdProposals_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtJobNumber.Text = grdProposals.SelectedRow.Cells[1].Text.Trim();
        txtProposal.Text = grdProposals.SelectedRow.Cells[2].Text.Trim();
        ddlAssignedTo.SelectedValue = grdProposals.SelectedRow.Cells[3].Text.Trim();


        btnAlter.Enabled = true;
    }

    protected void grdProposals_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[3].Visible = false;
    }


    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";

        loadAssignedTo();
    }

    private void sendJobAllocatedMailToUsers()
    {
        MRPWFMail mail = new MRPWFMail();
        mail.From_address = "mrp.workflow@hnbassurance.com";
        // mail.To_address = "tharindu.dilanka@hnbassurance.com";
        mail.To_address = GetUserAndSupervisorMailAddress(ddlTransferTo.Text);
        mail.Cc_address = "tharindu.dilanka@hnbassurance.com";
        String BodyText = "" ;

        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            mail.Subject = "MRP Job Re-Assigned";
            BodyText = "MRP Job(" + txtJobNumber.Text + ")  Re-Assigned to " + ddlTransferTo.SelectedItem.ToString();
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            mail.Subject = "MCR Job Re-Assigned";
            BodyText = "MCR Job(" + txtJobNumber.Text + ")  Re-Assigned to " + ddlTransferTo.SelectedItem.ToString();

        }

        mail.Body = BodyText;
        mail.sendMail();


    }

    private string GetUserAndSupervisorMailAddress(String userCode)
    {
        String returnVal = "";
        string mrpManagerEmail = "";
        mrpManagerEmail = System.Configuration.ConfigurationManager.AppSettings["MRPManagerEmail"].ToString();

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleDataReader dr;
        String selectQuery = "";

        try
        {
            con.Open();

            selectQuery = "SELECT CASE WHEN u.email  IS NOT NULL THEN u.email ELSE '" + mrpManagerEmail + "' END ," +
                " CASE WHEN su.email  IS NOT NULL THEN su.email ELSE '" + mrpManagerEmail + "' END " +
                " from MRP_USER_DETAILS U " +
                " INNER JOIN  MRP_USER_DETAILS SU ON su.user_code=u.supervisor_user_code " +
                " INNER JOIN  MRP_USER_DETAILS SSU ON ssu.user_code=su.supervisor_user_code " +
                " WHERE u.user_code=:V_USER_CODE";



            OracleCommand cmd = new OracleCommand(selectQuery, con);
            cmd.Parameters.Add(new OracleParameter("V_USER_CODE", userCode));


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
}
