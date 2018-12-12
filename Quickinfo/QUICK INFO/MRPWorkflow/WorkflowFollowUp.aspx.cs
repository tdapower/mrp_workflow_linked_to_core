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


public partial class MRPWorkflow_WorkflowFollowUp : System.Web.UI.Page
{
    string ProposalNo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["ProposalNo"] != null)
        {
            if (Request.Params["ProposalNo"] != "")
            {
                ProposalNo = Request.Params["ProposalNo"].ToString();
                LoadFollowupData(Request.Params["ProposalNo"].ToString());
            }
        }
    }


    private void LoadFollowupData(string sProposalNo)
    {

        grdFollowUp.DataSource = null;
        grdFollowUp.DataBind();

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand myOleDbCommand = new OracleCommand();
        myOleDbConnection.Open();
        myOleDbCommand.Connection = myOleDbConnection;

        String selectQuery = "";
        //selectQuery = "SELECT "+
        //              "  FU.PROPOSAL_NO,"+
        //              "  FU.STATUS_CODE,"+
        //              "  ST.STATUS_NAME,"+
        //              "  FU.SYS_DATE,"+
        //              "  FU.REMARKS, "+
        //              "  U.USER_NAME " +
        //              "  FROM MRP_WORKFLOW_FOLLOWUP FU "+
        //              "  INNER JOIN MRP_WF_STATUSES ST ON FU.STATUS_CODE=ST.STATUS_CODE " +
        //              " LEFT JOIN WF_ADMIN_USERS U ON  FU.USER_NAME=U.USER_CODE " +
        //            " WHERE PROPOSAL_NO='" + sProposalNo + "' "+
        //            " ORDER BY  ST.ORDER_NO ASC";



        selectQuery = "  SELECT * FROM( " +
                            " SELECT  " +
                         " FU.PROPOSAL_NO, " +
                          " FU.STATUS_CODE, " +
                        " ST.STATUS_NAME, " +
                          " FU.SYS_DATE, " +
                          " FU.REMARKS,  " +
                        " U.USER_NAME  " +
                        " FROM MRP_WORKFLOW_FOLLOWUP FU  " +
                        " INNER JOIN MRP_WF_STATUSES ST ON FU.STATUS_CODE=ST.STATUS_CODE  " +
                       " LEFT JOIN WF_ADMIN_USERS U ON  FU.USER_NAME=U.USER_CODE  " +
                       " WHERE PROPOSAL_NO='" + sProposalNo + "'  " +
                       " UNION  " +
                       " select pj.proposal_no,0,'Job Assigned',pj.created_date,'','' from mrp_wf_proposal_jobs pj WHERE pj.PROPOSAL_NO='" + sProposalNo + "'  " +
                       " union " +
                       " select wf.proposal_no,0,'Fax Pending Cleared',wf.pending_com_date,'',U.USER_NAME   from mrp_workflow wf  LEFT JOIN WF_ADMIN_USERS U ON  wf.PENDING_COM_USER=U.USER_CODE  WHERE wf.PROPOSAL_NO='" + sProposalNo + "'   and  wf.pending_com_date is not null" +
                          " union " +
                       " select wf.proposal_no,0,'Original Pending Cleared',wf.ori_pend_cleared_date,'',U.USER_NAME   from mrp_workflow wf  LEFT JOIN WF_ADMIN_USERS U ON wf.ORI_PEND_CLEARED_USER=U.USER_CODE  WHERE wf.PROPOSAL_NO='" + sProposalNo + "'  and  wf.ori_pend_cleared_date is not null " +
                     " )  ORDER BY sys_date ";








        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdFollowUp.DataSource = myOleDbDataReader;
            grdFollowUp.DataBind();
        }
    }


    //protected void grdViewCustomers_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string customerID = grdViewCustomers.DataKeys[e.Row.RowIndex].Value.ToString();
    //        GridView grdViewOrdersOfCustomer = (GridView)e.Row.FindControl("grdViewOrdersOfCustomer");
    //        grdViewOrdersOfCustomer.DataSource = SelectData(
    //          "SELECT top 3 CustomerID, OrderID, OrderDate FROM Orders WHERE CustomerID='" +
    //          customerID + "'");
    //        grdViewOrdersOfCustomer.DataBind();
    //    }
    //}
    protected void grdFollowUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string status = grdFollowUp.DataKeys[e.Row.RowIndex].Values[1].ToString();


            GridView grdRemindersOfProposal = (GridView)e.Row.FindControl("grdReminders");

            if (status == "3")//Reminders-Pending and Medical
            {
                grdRemindersOfProposal.DataSource = SelectData("select t.reminder_no,t.sys_date from mrp_wf_reminder_log t where t.proposal_no='" + ProposalNo + "' and t.reminder_type=1 order by t.sys_date");
            }
            else if (status == "11")//Reminders-Cover note
            {
                grdRemindersOfProposal.DataSource = SelectData("select t.reminder_no,t.sys_date from mrp_wf_reminder_log t where t.proposal_no='" + ProposalNo + "' and t.reminder_type=2 order by t.sys_date");
            }
            else if (status == "26")//Reminder-Confirmation Letter
            {
                grdRemindersOfProposal.DataSource = SelectData("select t.reminder_no,t.sys_date from mrp_wf_reminder_log t where t.proposal_no='" + ProposalNo + "' and t.reminder_type=3 order by t.sys_date");
            }


            grdRemindersOfProposal.DataBind();


        }
    }
    private DataTable SelectData(string selectQuery)
    {

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());


        OracleDataAdapter da = new OracleDataAdapter();
        string sql = "";

        da.SelectCommand = new OracleCommand(selectQuery, myOleDbConnection);

        DataTable dt = new DataTable();

        try
        {
            myOleDbConnection.Open();
            dt.Load(da.SelectCommand.ExecuteReader());
            return dt;
        }
        catch (SqlException err)
        {
            throw new ApplicationException("Data error.");
        }
        finally
        {
            myOleDbConnection.Close();
        }

    }
}
