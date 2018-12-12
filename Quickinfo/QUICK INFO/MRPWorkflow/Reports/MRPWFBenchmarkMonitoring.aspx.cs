//******************************************
// Author            :Tharindu Athapattu
// Date              :19/06/2013
// Reviewed By       :
// Description       :MRP WF Benchmark Monitoring
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
using System.Data.SqlClient;
using System.Net;
using System.DirectoryServices;
using System.Net.Mail;
using System.IO;
using Telerik.Web.UI;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;

public partial class MRPWFBenchmarkMonitoring : System.Web.UI.Page
{
    PolicyInsuranceBenchmarkMonitoringBLL report = new PolicyInsuranceBenchmarkMonitoringBLL();
    StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                validatePageAuthentication();
                btnViewBmOutProposals.Visible = false;


                ddlReport.Items.Add("--Select Report--");
                ddlReport.Items.Add("Benchmark Monitering Report");
                //ddlReport.Items.Add("Benchmark Summary Report");
                //ddlReport.Items.Add("Benchmark Summary Report 2");
                //ddlReport.Items.Add("Benchmark Summary Report-Approved User");
                //ddlReport.Items.Add("Benchmark Summary Report 2-Approved User");
                ddlReport.Items.Add("Pending Received Report");
                ddlReport.Items.Add("Status Wise Proposals Report");
                ddlReport.Items.Add("PPI Report");
                ddlReport.Items.Add("Re Insurance Confirmation Report");
                ddlReport.Items.Add("Policy Scanning Report");
                ddlReport.Items.Add("Benchmark Summary - Pending Letters");
                ddlReport.Items.Add("Benchmark Summary - Pending Letters(Supervisors)");
                ddlReport.Items.Add("Benchmark Summary - Cover Notes");
                ddlReport.Items.Add("Benchmark Summary - Cover Notes(Supervisors)");
                ddlReport.Items.Add("Benchmark Summary - Policy Issuance");
                ddlReport.Items.Add("Benchmark Summary - Policy Issuance(Supervisors)");

                ddlReport.Items.Add("Branch Wise Pending Jobs-HNB");
                ddlReport.Items.Add("Branch Wise Pending Jobs-OB");
                ddlReport.Items.Add("All Pending Cleared");
                ddlReport.Items.Add("Medical Reimbursement Report");
                ddlReport.Items.Add("Labwise Medical Payment Summary");
                ddlReport.Items.Add("Mail Register Report - Pronto");
                ddlReport.Items.Add("Mail Register Report - Abans");


                ddlReport.Items.Add("Fast Track - Benchmark Summary");
                ddlReport.Items.Add("Fast Track - Detailed Report");

                ddlReport.Items.Add("Medical Payment Report");


                ddlReport.Items.Add("Quotation Summary Report");

                ddlReport.Items.Add("Job Allocation Report");

                ddlReport.Items.Add("Job Allocation Summary Report");

                ddlReport.Items.Add("RI Report");


                // ddlReport.Items.Add("Benchmark Summery-Assign");
                //ddlReport.Items.Add("Benchmark Summery-Approve");


                loadAssignUsers();
                loadApproveUsers();
                loadPendingClearedUsers();

                ddlPaymentMode.Items.Clear();
                ddlPaymentMode.Items.Add(new ListItem("Customer Paid", "Customer Paid"));
                ddlPaymentMode.Items.Add(new ListItem("Credit", "Credit"));


                lblPaymentMode.Visible = false;
                ddlPaymentMode.Visible = false;



                ddlBranch.DataValueField = "ASSURANCE_CODE";
                ddlBranch.DataTextField = "ASSURANCE_CODE";
                DataTable Branch = report.GetBranch();
                ddlBranch.DataSource = Branch;
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem("ALL", "ALL"));


                string hour = "";
                for (int i = 1; i <= 12; i++)
                {
                    if (i < 10)
                        hour = "0" + i.ToString();
                    else
                        hour = i.ToString();


                    ddlToHour.Items.Add(new ListItem(hour, hour));
                    ddlFromHour.Items.Add(new ListItem(hour, hour));

                }


                string minute = "";
                for (int i = 0; i < 60; i++)
                {
                    if (i < 10)
                        minute = "0" + i.ToString();
                    else
                        minute = i.ToString();

                    ddlToMin.Items.Add(new ListItem(minute, minute));
                    ddlFromMin.Items.Add(new ListItem(minute, minute));
                }

                ddlToAMPM.Items.Add(new ListItem("AM", "AM"));
                ddlToAMPM.Items.Add(new ListItem("PM", "PM"));
                ddlFromAMPM.Items.Add(new ListItem("AM", "AM"));
                ddlFromAMPM.Items.Add(new ListItem("PM", "PM"));

            }

            if (ddlReport.SelectedItem.Text == "Benchmark Summary - Pending Letters" || ddlReport.SelectedItem.Text == "Benchmark Summary - Pending Letters(Supervisors)"
                        || ddlReport.SelectedItem.Text == "Benchmark Summary - Cover Notes" || ddlReport.SelectedItem.Text == "Benchmark Summary - Cover Notes(Supervisors)"
                        || ddlReport.SelectedItem.Text == "Benchmark Summary - Policy Issuance" || ddlReport.SelectedItem.Text == "Benchmark Summary - Policy Issuance(Supervisors)")
            {
                btnViewBmOutProposals.Visible = true;
            }
            else
            {
                btnViewBmOutProposals.Visible = false;
            }



        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
            return;

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

    private void loadAssignUsers()
    {
        ddlAssinedUser.Items.Clear();
        ddlAssinedUser.Items.Add(new ListItem("ALL", "ALL"));

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
                ddlAssinedUser.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }
    private void loadPendingClearedUsers()
    {
        ddlPendingClearedUser.Items.Clear();
        ddlPendingClearedUser.Items.Add(new ListItem("ALL", "ALL"));

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        selectQuery = " select DISTINCT PCL.User_Name,USR.USER_NAME from MRP_WF_PEND_CLEARED_DOCS_LOG PCL  " +
                    " INNER JOIN WF_ADMIN_USERS USR ON PCL.USER_NAME=USR.USER_CODE ORDER BY USR.USER_NAME";
        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlPendingClearedUser.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }
    private void loadApproveUsers()
    {
        ddlApprovedUser.Items.Clear();
        ddlApprovedUser.Items.Add(new ListItem("ALL", "ALL"));

        //OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        //OracleDataReader dr;

        //con.Open();

        //OracleCommand cmd = new OracleCommand();
        //cmd.Connection = con;
        //String selectQuery = "";
        //selectQuery = "SELECT DISTINCT  APPROVED_USER FROM MRP_WORKFLOW WHERE  CANCELLED=0 ORDER BY APPROVED_USER ASC ";

        //cmd.CommandText = selectQuery;

        //dr = cmd.ExecuteReader();
        //if (dr.HasRows)
        //{
        //    while (dr.Read())
        //    {
        //        ddlApprovedUser.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
        //    }
        //}
        //dr.Close();
        //dr.Dispose();
        //cmd.Dispose();
        //con.Close();
        //con.Dispose();


        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MRPWORKFLOW"].ToString());
        SqlDataReader dr;

        con.Open();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT DISTINCT UserID   FROM Forms_Log   WHERE  Formid=8";



        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddlApprovedUser.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();



    }


    protected void btnViewBmOutProposals_Click(object sender, EventArgs e)
    {
        sb.Append("<script>");
        sb.Append("window.open('BMOutProposals.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
        sb.Append("</script>");
        Page.RegisterStartupScript("test", sb.ToString());
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {

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



    protected void btn1ViewReport_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            //  report.DeleteReport();



            if (ddlReport.SelectedItem.Text == "Benchmark Monitering Report")
            {
                //Date
                if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex == 0))
                {
                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;


                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE1", "ALL", "ALL", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                    //Date, Assigned User
                else if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex == 0))
                {
                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;


                    GetLoginName(ddlAssinedUser.SelectedValue);
                    string AssighnedUserName = Session["USER"].ToString();
                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE2", AssighnedUserName, "ALL", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                //Date, Approved User
                else if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex != 0))
                {

                    string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;


                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE3", "ALL", ApprovedUserName, txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                //Assigned User
                else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex == 0))
                {

                    // GetLoginName(ddlAssinedUser.SelectedValue);
                    string AssighnedUserName = ddlAssinedUser.SelectedValue.ToString();

                    string reportDescription = "";
                    reportDescription = "";

                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE4", AssighnedUserName, "ALL", DateTime.Today.ToString().ToString(), DateTime.Today.ToString());
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                //Assigned User, Approve User
                else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex != 0))
                {

                    //GetLoginName(ddlAssinedUser.SelectedValue);
                    string AssighnedUserName = ddlAssinedUser.SelectedValue.ToString();
                    //GetLoginName(ddlApprovedUser.SelectedValue);
                    string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

                    string reportDescription = "";
                    reportDescription = "";

                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE5", AssighnedUserName, ApprovedUserName, DateTime.Today.ToString(), DateTime.Today.ToString());
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                //Approve User
                else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex != 0))
                {
                    string reportDescription = "";
                    reportDescription = "";
                    string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE6", "ALL", ApprovedUserName, DateTime.Today.ToString(), DateTime.Today.ToString());
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                    //Default(No date,All Assighned User, All Approved User)
                else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex == 0))
                {
                    string reportDescription = "";
                    reportDescription = "";

                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE7", "ALL", "ALL", DateTime.Today.ToString(), DateTime.Today.ToString());
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                //Date , Assigned User, Approved User
                else if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex != 0))
                {
                    // GetLoginName(ddlAssinedUser.SelectedValue);
                    string AssighnedUserName = ddlAssinedUser.SelectedValue;
                    string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE8", AssighnedUserName, ApprovedUserName, txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }
                else
                {
                    lblError.Text = "Invalid...!";
                }
            }
            else if (ddlReport.SelectedItem.Text == "Benchmark Summary Report")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReport(txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport2.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }
            ///////////////////////////////////////////////////////////
            else if (ddlReport.SelectedItem.Text == "Benchmark Summary - Pending Letters")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Pending Letters Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("PendingLetters", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport2.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }


            else if (ddlReport.SelectedItem.Text == "Benchmark Summary - Pending Letters(Supervisors)")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Pending Letters Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("PendingLettersSupervisor", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport3.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }
            else if (ddlReport.SelectedItem.Text == "Benchmark Summary - Cover Notes")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Cover Notes Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("CoverNote", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport2.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }
            else if (ddlReport.SelectedItem.Text == "Benchmark Summary - Cover Notes(Supervisors)")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Cover Notes Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("CoverNoteSupervisor", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport3.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }
            else if (ddlReport.SelectedItem.Text == "Benchmark Summary - Policy Issuance")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Policy Issuance Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("PolicyIssue", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport2.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }
            else if (ddlReport.SelectedItem.Text == "Benchmark Summary - Policy Issuance(Supervisors)")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Policy Issuance Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("PolicyIssueSupervisor", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport3.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }

            else if (ddlReport.SelectedItem.Text == "All Pending Cleared")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "All Pending Cleared Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("PendingComplete", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('PendingComplete/CompletedPendings.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }


            /////////////////////////////


            else if (ddlReport.SelectedItem.Text == "Benchmark Summary Report 2")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReport4(txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkSummaryReport4.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }


            else if (ddlReport.SelectedItem.Text == "Benchmark Summary Report-Approved User")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReport2(txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkMonitoringReport3.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }

            else if (ddlReport.SelectedItem.Text == "Benchmark Summary Report 2-Approved User")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReport3(txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('BenchMarkSummaryReport3.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }

            else if (ddlReport.SelectedItem.Text == "Quotation Summary Report")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {

                    DataTable report1 = report.SelectMRPSQuotationSummaryReport(txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('MRPSystemQuotationReport/MRPSystemQuotationReport.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }


            else if (ddlReport.SelectedItem.Text == "Job Allocation Report")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != "") || (ddlAssinedUser.SelectedIndex == 0))
                {

                    string AssighnedUserName = ddlAssinedUser.SelectedItem.Text;

                    sb.Append("<script>");
                    sb.Append("window.open('JobAllocationReport/JobAllocationReport.aspx?user=" + AssighnedUserName + "&fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    //sb.Append("window.open('JobAllocationReport/JobAllocationReport.aspx'?user=" + ddlAssinedUser.SelectedValue.ToString() + "&fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());


                }

            }


            else if (ddlReport.SelectedItem.Text == "Pending Received Report")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    DateTime convertedFromDate = DateTime.ParseExact(txtfromDate.Text, "dd/MM/yyyy", null);
                    DateTime convertedToDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                    string AssignedUserName = ddlAssinedUser.SelectedValue.ToString();
                    string PendClearedUser = ddlPendingClearedUser.SelectedValue.ToString();



                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + "";

                    sb.Append("<script>");
                    sb.Append("window.open('ReceivedPendings/ReceivedPendings.aspx?fromDateTime=" + convertedFromDate.ToString() + "&toDateTime=" + convertedToDate.ToString() + "&assignedUser=" + AssignedUserName + "&pendClearedUser=" + PendClearedUser + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }
                else
                {
                    lblError.Text = "Select Date Range...";
                }

            }

            else if (ddlReport.SelectedItem.Text == "Branch Wise Pending Jobs-HNB")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    DateTime convertedFromDate = DateTime.ParseExact(txtfromDate.Text, "dd/MM/yyyy", null);
                    DateTime convertedToDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + " ";

                    sb.Append("<script>");
                    sb.Append("window.open('BranchWisePendingJobs/BranchWisePendingJobs.aspx?reportDescription=" + reportDescription + "&fromDateTime=" + convertedFromDate.ToString() + "&toDateTime=" + convertedToDate.ToString() + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }
                else
                {
                    lblError.Text = "Select Date Range...";
                }

            }
            else if (ddlReport.SelectedItem.Text == "Branch Wise Pending Jobs-OB")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    string AssignedUserName = ddlAssinedUser.SelectedValue.ToString();
                    string PendClearedUser = ddlPendingClearedUser.SelectedValue.ToString();


                    string fromDateTime = "";
                    string toDateTime = "";

                    fromDateTime = txtfromDate.Text + " " + ddlFromHour.SelectedValue + ":" + ddlFromMin.SelectedValue + ":00" + " " + ddlFromAMPM.SelectedValue;
                    toDateTime = txtToDate.Text + " " + ddlToHour.SelectedValue + ":" + ddlToMin.SelectedValue + ":00" + " " + ddlToAMPM.SelectedValue;


                    DateTime convertedFromDate = DateTime.ParseExact(fromDateTime, "dd/MM/yyyy h:m:s tt", null);
                    //DateTime convertedFromDate = Convert.ToDateTime(fromDateTime).ToString("MM/dd/yyyy hh:MM:tt:ss");


                    DateTime convertedToDate = DateTime.ParseExact(toDateTime, "dd/MM/yyyy h:m:s tt", null);
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + " of all users";

                    sb.Append("<script>");
                    sb.Append("window.open('BranchWisePendingJobsOB/BranchWisePendingJobs.aspx?reportDescription=" + reportDescription + "&fromDateTime=" + convertedFromDate.ToString() + "&toDateTime=" + convertedToDate.ToString() + "&AssignedUser=" + AssignedUserName + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }
                else
                {
                    lblError.Text = "Select Date Range...";
                }

            }
            ///


            else if (ddlReport.SelectedItem.Text == "Job Allocation Summary Report")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    DateTime convertedFromDate = DateTime.ParseExact(txtfromDate.Text, "dd/MM/yyyy", null);
                    DateTime convertedToDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                    DataTable report1 = report.SelectMRPWFBenchmarkJobAlloSummary(convertedFromDate, convertedToDate);
                   
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + " ";

                    sb.Append("<script>");
                    sb.Append("window.open('JobAllocationReport/JobAllocationSummaryReport.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                  
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }
                else
                {
                    lblError.Text = "Select Date Range...";
                }

            }

            else if (ddlReport.SelectedItem.Text == "Status Wise Proposals Report")
            {
                string reportDescription = "";
                //Date
                if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex == 0))
                {
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + " of all users";

                    DataTable report1 = report.SelectMRPWFStatusWiseProposalsReport("CASE1", "ALL", "ALL", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('StatusWiseProposals/StatusWiseProposals.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

                    //Date, Assigned User
                else if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex != 0))
                {
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + " of " + ddlAssinedUser.SelectedItem.ToString();

                    DataTable report1 = report.SelectMRPWFStatusWiseProposalsReport("CASE2", ddlAssinedUser.SelectedValue, "ALL", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('StatusWiseProposals/StatusWiseProposals.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }



                //Assigned User
                else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex != 0))
                {
                    reportDescription = "Report of " + ddlAssinedUser.SelectedItem.ToString();

                    DataTable report1 = report.SelectMRPWFStatusWiseProposalsReport("CASE3", ddlAssinedUser.SelectedValue, "ALL", DateTime.Today.ToString(), DateTime.Today.ToString());
                    sb.Append("<script>");
                    sb.Append("window.open('StatusWiseProposals/StatusWiseProposals.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }


                    //Default(No date,All Assighned User)
                else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex == 0))
                {
                    reportDescription = "Report of all users";
                    DataTable report1 = report.SelectMRPWFStatusWiseProposalsReport("CASE4", "ALL", "ALL", DateTime.Today.ToString(), DateTime.Today.ToString());
                    sb.Append("<script>");
                    sb.Append("window.open('StatusWiseProposals/StatusWiseProposals.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }


                else
                {
                    lblError.Text = "Invalid...!";
                }
            }
            else if (ddlReport.SelectedItem.Text == "PPI Report")
            {
                string reportDescription = "";
                //Date
                if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex == 0))
                {
                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + " of all users";

                   // DataTable report1 = report.SelectMRPWFStatusWiseProposalsReport("CASE1", "ALL", "ALL", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('PPI/PPIReport.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }






                    //Default(No date,All Assighned User)
                else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex == 0))
                {
                    reportDescription = "Report of all users";
                    DataTable report1 = report.SelectMRPWFStatusWiseProposalsReport("CASE4", "ALL", "ALL", DateTime.Today.ToString(), DateTime.Today.ToString());
                    sb.Append("<script>");
                    sb.Append("window.open('PPI/PPIReport.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }


                else
                {
                    lblError.Text = "Invalid...!";
                }
            }
            else if (ddlReport.SelectedItem.Text == "Policy Scanning Report")
            {

                sb.Append("<script>");
                sb.Append("window.open('PolicyScanningReport/PolicyScanningReport.aspx?fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());


            }
            else if (ddlReport.SelectedItem.Text == "Mail Register Report - Pronto")
            {

                sb.Append("<script>");
                sb.Append("window.open('MailedProposals/MailedProposalsPronto.aspx?user=" + ddlAssinedUser.SelectedValue.ToString() + "&userName=" + ddlAssinedUser.SelectedItem.ToString() + "&fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());


            }
            else if (ddlReport.SelectedItem.Text == "Mail Register Report - Abans")
            {

                sb.Append("<script>");
                sb.Append("window.open('MailedProposals/MailedProposals.aspx?user=" + ddlAssinedUser.SelectedValue.ToString() + "&userName=" + ddlAssinedUser.SelectedItem.ToString() + "&fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());


            }


            else if (ddlReport.SelectedItem.Text == "Medical Reimbursement Report")
            {

                sb.Append("<script>");
                sb.Append("window.open('MedicalReimbursementReport/MedicalReimbursementReport.aspx?fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());

            }


            else if (ddlReport.SelectedItem.Text == "Medical Payment Report")
            {

                sb.Append("<script>");
                sb.Append("window.open('MedicalPaymentReport/MedicalPaymentReport.aspx?fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "&paymentMode=" + ddlPaymentMode.SelectedValue.ToString() + " ', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());

            }



            else if (ddlReport.SelectedItem.Text == "Labwise Medical Payment Summary")
            {

                sb.Append("<script>");
                sb.Append("window.open('LabwiseMedicalPaymentSummary/LabwiseMedicalPaymentSummary.aspx?fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());

            }
            else if (ddlReport.SelectedItem.Text == "RI Report")
            {

                sb.Append("<script>");
                sb.Append("window.open('../ReInsurance/Reports/ReInsuranceReport.aspx?fromDateTime=" + txtfromDate.Text + "&toDateTime=" + txtToDate.Text + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());

            }



            else if (ddlReport.SelectedItem.Text == "Re Insurance Confirmation Report")
            {


                //Date
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    string fromDateTime = "";
                    string toDateTime = "";

                    fromDateTime = txtfromDate.Text + " " + ddlFromHour.SelectedValue + ":" + ddlFromMin.SelectedValue + ":00" + " " + ddlFromAMPM.SelectedValue;
                    toDateTime = txtToDate.Text + " " + ddlToHour.SelectedValue + ":" + ddlToMin.SelectedValue + ":00" + " " + ddlToAMPM.SelectedValue;


                    DateTime convertedFromDate = DateTime.ParseExact(fromDateTime, "dd/MM/yyyy h:m:s tt", null);


                    DateTime convertedToDate = DateTime.ParseExact(toDateTime, "dd/MM/yyyy h:m:s tt", null);



                    reportDescription = "Report from " + txtfromDate.Text + " to " + txtToDate.Text + " of all users";

                    sb.Append("<script>");
                    sb.Append("window.open('ReInsuranceConfirmationReport/ReInsuranceConfirmationReport.aspx?reportDescription=" + reportDescription + "&fromDateTime=" + convertedFromDate.ToString() + "&toDateTime=" + convertedToDate.ToString() + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }

            else if (ddlReport.SelectedItem.Text == "Fast Track - Benchmark Summary")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("FSTBenchMark", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('FSTBenchMarkMonitoringReport.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }

            else if (ddlReport.SelectedItem.Text == "Fast Track - Detailed Report")
            {
                if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
                {
                    string reportDescription = "";
                    reportDescription = "from " + txtfromDate.Text + " to " + txtToDate.Text;

                    DataTable report1 = report.SelectMRPWFBenchmarkSummaryReports("FSTBenchMarkDaily", txtfromDate.Text, txtToDate.Text);
                    sb.Append("<script>");
                    sb.Append("window.open('FSTBenchMarkDailyMonitoringReport.aspx?reportDescription=" + reportDescription + "', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                    sb.Append("</script>");
                    Page.RegisterStartupScript("test", sb.ToString());
                }

            }


            else
            {

                lblError.Text = "Select Report...";

            }

            if (ddlReport.SelectedItem.Text == "Benchmark Summary - Pending Letters" || ddlReport.SelectedItem.Text == "Benchmark Summary - Pending Letters(Supervisors)"
             || ddlReport.SelectedItem.Text == "Benchmark Summary - Cover Notes" || ddlReport.SelectedItem.Text == "Benchmark Summary - Cover Notes(Supervisors)"
             || ddlReport.SelectedItem.Text == "Benchmark Summary - Policy Issuance" || ddlReport.SelectedItem.Text == "Benchmark Summary - Policy Issuance(Supervisors)")
            {
                btnViewBmOutProposals.Visible = true;
            }
            else
            {
                btnViewBmOutProposals.Visible = false;
            }
        }
        catch (Exception ex)
        {
            //lblError.Text = ex.Message;
            lblError.Text = "Error while loading report";

            lblError.Visible = true;
            return;

        }


    }

    //protected void btn1ViewReport_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblError.Text = "";
    //        report.DeleteReport();

    //        if (ddlReport.SelectedItem.Text == "Banchmark Monitering Report")
    //        {
    //            //Date
    //            if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex == 0))
    //            {

    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE1", "ALL", "ALL", txtfromDate.Text, txtToDate.Text);
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //                //Date, Assigned User
    //            else if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex == 0))
    //            {

    //                GetLoginName(ddlAssinedUser.SelectedValue);
    //                string AssighnedUserName = Session["USER"].ToString();
    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE2", AssighnedUserName, "ALL", txtfromDate.Text, txtToDate.Text);
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //            //Date, Approved User
    //            else if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex != 0))
    //            {

    //                string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE3", "ALL", ApprovedUserName, txtfromDate.Text, txtToDate.Text);
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //            //Assigned User
    //            else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex == 0))
    //            {

    //                GetLoginName(ddlAssinedUser.SelectedValue);
    //                string AssighnedUserName = Session["USER"].ToString();

    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE4", AssighnedUserName, "ALL", DateTime.Today.ToString(), DateTime.Today.ToString());
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //            //Assigned User, Approve User
    //            else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex != 0))
    //            {

    //                GetLoginName(ddlAssinedUser.SelectedValue);
    //                string AssighnedUserName = Session["USER"].ToString();
    //                //GetLoginName(ddlApprovedUser.SelectedValue);
    //                string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE5", AssighnedUserName, ApprovedUserName, DateTime.Today.ToString(), DateTime.Today.ToString());
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //            //Approve User
    //            else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex != 0))
    //            {

    //                string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE6", "ALL", ApprovedUserName, DateTime.Today.ToString(), DateTime.Today.ToString());
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //                //Default(No date,All Assighned User, All Approved User)
    //            else if ((txtfromDate.Text == "") && (txtToDate.Text == "") && (ddlAssinedUser.SelectedIndex == 0) && (ddlApprovedUser.SelectedIndex == 0))
    //            {

    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE7", "ALL", "ALL", DateTime.Today.ToString(), DateTime.Today.ToString());
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //            //Date , Assigned User, Approved User
    //            else if ((txtfromDate.Text != "") && (txtToDate.Text != "") && (ddlAssinedUser.SelectedIndex != 0) && (ddlApprovedUser.SelectedIndex != 0))
    //            {
    //                GetLoginName(ddlAssinedUser.SelectedValue);
    //                string AssighnedUserName = Session["USER"].ToString();
    //                string ApprovedUserName = ddlApprovedUser.SelectedValue.ToString();

    //                DataTable report1 = report.SelectMRPWFBenchmarkReport("CASE8", AssighnedUserName, ApprovedUserName, txtfromDate.Text, txtToDate.Text);
    //                sb.Append("<script>");
    //                sb.Append("window.open('MRPWFBenchmarkMonitoringRPT1.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }
    //            else
    //            {
    //                lblError.Text = "Invalid...!";
    //            }
    //        }
    //        else if (ddlReport.SelectedItem.Text == "Banchmark Summery-Assign")
    //        {

    //            //Date
    //            if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
    //            {

    //                DataTable report1 = report.SelectReport2( txtfromDate.Text, txtToDate.Text);
    //                sb.Append("<script>");
    //                sb.Append("window.open('BenchMarkMonitoringReport2.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //            else
    //            {
    //                lblError.Text = "Invalid...!";
    //            }

    //        }
    //        else if (ddlReport.SelectedItem.Text == "Banchmark Summery-Approve")
    //        {

    //            //Date
    //            if ((txtfromDate.Text != "") && (txtToDate.Text != ""))
    //            {

    //                DataTable report1 = report.SelectReport4(txtfromDate.Text, txtToDate.Text);
    //                sb.Append("<script>");
    //                sb.Append("window.open('BenchMarkMonitoringReport2.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
    //                sb.Append("</script>");
    //                Page.RegisterStartupScript("test", sb.ToString());
    //            }

    //            else
    //            {
    //                lblError.Text = "Invalid...!";
    //            }

    //        }
    //        else
    //        {

    //            lblError.Text = "Select Report...";

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblError.Text = ex.Message;
    //        lblError.Visible = true;
    //        return;

    //    }


    //}



    protected void BtnView2_Click1(object sender, EventArgs e)
    {
        try
        {

            lblError.Text = "";
            report.DeleteReport();
            //Policy No
            if ((txtPolicyNo.Text != "") && (txtPropNo.Text == "") && (ddlBranch.SelectedIndex == 0))
            {

                DataTable report1 = report.SelectReport3("CASE1", txtPolicyNo.Text, "", "ALL");
                sb.Append("<script>");
                sb.Append("window.open('BenchMarkMonitoringReport3.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());

            }

            //Policy No,Proposal No
            else if ((txtPolicyNo.Text != "") && (txtPropNo.Text != "") && (ddlBranch.SelectedIndex == 0))
            {
                DataTable report1 = report.SelectReport3("CASE2", txtPolicyNo.Text, txtPropNo.Text, "ALL");
                sb.Append("<script>");
                sb.Append("window.open('BenchMarkMonitoringReport3.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());


            }
            //Proposal No
            else if ((txtPolicyNo.Text == "") && (txtPropNo.Text != "") && (ddlBranch.SelectedIndex == 0))
            {
                DataTable report1 = report.SelectReport3("CASE3", "", txtPropNo.Text, "ALL");
                sb.Append("<script>");
                sb.Append("window.open('BenchMarkMonitoringReport3.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());


            }
            //Branch
            else if ((txtPolicyNo.Text == "") && (txtPropNo.Text == "") && (ddlBranch.SelectedIndex != 0))
            {
                string branch = ddlBranch.SelectedValue.ToString();
                DataTable report1 = report.SelectReport3("CASE4", "", "", branch);
                sb.Append("<script>");
                sb.Append("window.open('BenchMarkMonitoringReport3.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());


            }
            //None
            else if ((txtPolicyNo.Text == "") && (txtPropNo.Text == "") && (ddlBranch.SelectedIndex == 0))
            {

                DataTable report1 = report.SelectReport3("CASE5", "", "", "ALL");
                sb.Append("<script>");
                sb.Append("window.open('BenchMarkMonitoringReport3.aspx', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
                sb.Append("</script>");
                Page.RegisterStartupScript("test", sb.ToString());
            }
            else
            {
                lblError.Text = "Invalid...!";
            }

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
            return;

        }
    }



    protected void ddlReport_SelectedIndexChanged1(object sender, EventArgs e)
    {

        if (ddlReport.SelectedItem.Text == "Pending Received Report")
        {
            ddlFromHour.Enabled = true;
            ddlFromMin.Enabled = true;
            ddlToHour.Enabled = true;
            ddlToMin.Enabled = true;
            ddlFromAMPM.Enabled = true;
            ddlToAMPM.Enabled = true;
            ddlPendingClearedUser.Visible = true;
            lblPendingClearedUser.Visible = true;
        }
        else if (ddlReport.SelectedItem.Text == "Medical Payment Report")
        {
            lblPaymentMode.Visible = true;
            ddlPaymentMode.Visible = true;
        }
        else
        {
            ddlFromHour.Enabled = false;
            ddlFromMin.Enabled = false;
            ddlToHour.Enabled = false;
            ddlToMin.Enabled = false;
            ddlFromAMPM.Enabled = false;
            ddlToAMPM.Enabled = false;
            ddlPendingClearedUser.Visible = false;
            lblPendingClearedUser.Visible = false;

            lblPaymentMode.Visible = false;
            ddlPaymentMode.Visible = false;
        }





    }
}
