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
using System.Data.OracleClient;

public partial class MRPWorkflow_Blackboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.Params["ProposalNo"] != null)
            {
                if (Request.Params["ProposalNo"] != "")
                {
                    txtProposalNo.Text = Request.Params["ProposalNo"].ToString();
                    loadPreviousComments(txtProposalNo.Text);
                }
            }

            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);



        }

    }

    private void loadPreviousComments(string proposalNo)
    {
        ltrlOldComments.Text = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "SELECT t.PROPOSAL_NO  ,  t.SEQ_ID  ,  t.USER_COMMENT  ,  t.SYS_DATE ,u.user_name     FROM  MRP_WF_BLACKBOARD_COMMENT t " +
                        " INNER JOIN wf_admin_users u on t.user_code=u.user_code " +
                        " WHERE t.PROPOSAL_NO= '" + proposalNo + "' " +
                     "  ORDER BY t.SYS_DATE ASC";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ltrlOldComments.Text = ltrlOldComments.Text + " <p> <b>" + dr[4].ToString() + " (" + dr[3].ToString() + ")</b> </br> " + dr[2].ToString() + "</p>";
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }



    protected void btnSaveComment_Click(object sender, EventArgs e)
    {


        if (txtNewComment.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter comment to save";
            Timer1.Enabled = true;
            return;
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



        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            spProcess = new OracleCommand("INSERT_MRP_WF_BLACKBOARD_CMT");


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = txtProposalNo.Text.Trim();
            spProcess.Parameters.Add("V_USER_COMMENT", OracleType.VarChar).Value = txtNewComment.Text.Trim();
            spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = UserName;

            spProcess.Parameters.Add("V_WORKFLOW_TYPE", OracleType.VarChar).Value = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();

            spProcess.ExecuteNonQuery();
            conProcess.Close();

            loadPreviousComments(txtProposalNo.Text);
            txtNewComment.Text = "";
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }

    }

    protected void btnSaveReminder_Click(object sender, EventArgs e)
    {


        if (txtReminderText.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter reminder to save";
            Timer1.Enabled = true;
            return;
        }

        if (txtReminderDate.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter reminder date to save";
            Timer1.Enabled = true;
            return;
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

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;



            spProcess = new OracleCommand("INSERT_MRP_WF_CUSTOM_REMINDER");


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = txtProposalNo.Text.Trim();
            spProcess.Parameters.Add("V_REMINDER_TEXT", OracleType.VarChar).Value = txtReminderText.Text.Trim();
            spProcess.Parameters.Add("V_REMINDER_DATE", OracleType.DateTime).Value = txtReminderDate.Text;


            spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = UserName;

            spProcess.Parameters.Add("V_WORKFLOW_TYPE", OracleType.VarChar).Value = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();

            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }



        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            spProcess = new OracleCommand("INSERT_MRP_WF_BLACKBOARD_CMT");


            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar).Value = txtProposalNo.Text.Trim();
            spProcess.Parameters.Add("V_USER_COMMENT", OracleType.VarChar).Value = "Remind " + txtReminderText.Text.Trim() + " on " + txtReminderDate.Text;
            spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = UserName;

            spProcess.Parameters.Add("V_WORKFLOW_TYPE", OracleType.VarChar).Value = Request.Cookies["WORKFLOW_CHOICE"].Value.ToString();

            spProcess.ExecuteNonQuery();
            conProcess.Close();


            loadPreviousComments(txtProposalNo.Text);


            txtReminderText.Text = "";
            txtReminderDate.Text = "";
 
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
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
}
