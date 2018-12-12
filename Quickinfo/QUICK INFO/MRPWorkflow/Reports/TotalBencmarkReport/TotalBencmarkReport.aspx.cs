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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.OracleClient;

public partial class TotalBencmarkReport : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           validatePageAuthentication();

            loadYears();



            grdPendingsBenchmark.DataSource = null;
            grdPendingsBenchmark.DataBind();

            grdPolicyBenchmark.DataSource = null;
            grdPolicyBenchmark.DataBind();


            grdCoverBenchmark.DataSource = null;
            grdCoverBenchmark.DataBind();

            grdSummary.DataSource = null;
            grdSummary.DataBind();
        }

    }


    private void loadYears()
    {

        for (int a = 2000; a < 2026; a++)
        {
            ddlYears.Items.Add(new ListItem(a.ToString(), a.ToString()));
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

    private void executePendingSP(string year)
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            spProcess = new OracleCommand("MRP_WF_BENCH_SUMPEND_SP");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_YEAR", OracleType.Number).Value = year;
            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ex)
        {
        }
    }

    private void executePolicySP(string year)
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            spProcess = new OracleCommand("MRP_WF_BENCH_SUMPOL_SP");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_YEAR", OracleType.Number).Value = year;
            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ex)
        {
        }
    }

    private void executeCoverSP(string year)
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            spProcess = new OracleCommand("MRP_WF_BENCH_SUMCOVER_SP");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_YEAR", OracleType.Number).Value = year;
            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ex)
        {
        }
    }

    private void executeSummarySP()
    {

        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            spProcess = new OracleCommand("MRP_WF_BENCH_SUMMARY_SP");
            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ex)
        {
        }
    }


    public DataTable getResult(string reportType)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ORACONN"].ToString();

        OracleConnection con = new OracleConnection(connectionString);
        OracleDataAdapter da = new OracleDataAdapter();

        String selectQuery = "";

        selectQuery = "select " +
                    " u.user_name AS \"Name\" , "+
                    " ROUND(JAN_PERCERNTAGE,2)     AS \"January Achievement %\" ,   " +
                      " JAN_RATING     AS \"January Rate\" , " +
                    " ROUND(FEB_PERCERNTAGE,2)       AS \"February Achievement %\"   ,   " +
                    "   FEB_RATING       AS \"February Rate\"   , " +
                    " ROUND(MAR_PERCERNTAGE,2)     AS \"March Achievement %\"     ,   " +
                      " MAR_RATING       AS \"March Rate\"   , " +
                    " ROUND(APR_PERCERNTAGE,2)      AS \"April Achievement %\"    ,   " +
                    "   APR_RATING        AS \"April Rate\"  , " +
                    " ROUND(MAY_PERCERNTAGE,2)      AS \"May Achievement %\"    ,   " +
                    "   MAY_RATING        AS \"May Rate\"  , " +
                    " ROUND(JUN_PERCERNTAGE,2)      AS \"June Achievement %\"    ,   " +
                    "   JUN_RATING       AS \"June Rate\"   , " +
                    " ROUND(JUL_PERCERNTAGE,2)      AS \"July Achievement %\"    ,   " +
                    "   JUL_RATING        AS \"July Rate\"  , " +
                    " ROUND(AUG_PERCERNTAGE,2)     AS \"August Achievement %\"     ,   " +
                    "   AUG_RATING        AS \"August Rate\"  , " +
                    " ROUND(SEP_PERCERNTAGE,2)      AS \"September Achievement %\"    ,   " +
                    "   SEP_RATING        AS \"September Rate\"  , " +
                    " ROUND(OCT_PERCERNTAGE,2)       AS \"October Achievement %\"   ,   " +
                    "   OCT_RATING       AS \"October Rate\"   , " +
                    " ROUND(NOV_PERCERNTAGE,2)       AS \"November Achievement %\"   ,   " +
                    "   NOV_RATING        AS \"November Rate\"  , " +
                    " ROUND(DEC_PERCERNTAGE,2)      AS \"December Achievement %\"    ,   " +
                    "   DEC_RATING         AS \"December Rate\" , " +
                    "   ROUND(SUMMARY_PERCERNTAGE,2)      AS \"Achievement %\"    ,   " +
                        " SUMMARY_RATING      AS \"Rate\"    	 " +
                     " from mrp_wf_bench_summary t  " +
                    " inner join wf_admin_users u on t.user_code=u.user_code  " +
                    " where t.report_type='" + reportType + "' " +
                    " order by u.user_name ";

        DataTable dt = new DataTable();
        da.SelectCommand = new OracleCommand(selectQuery, con);

        try
        {
            con.Open();
            dt.Load(da.SelectCommand.ExecuteReader());
            return dt;
        }
        catch (OracleException err)
        {
            throw new ApplicationException("Data error.");
        }
        finally
        {
            con.Close();
        }
    }


    private void BindGrid()
    {
        try
        {
            grdPendingsBenchmark.DataSource = null;
            grdPendingsBenchmark.DataBind();

            grdPendingsBenchmark.DataSource = getResult("PENDING");

            if (grdPendingsBenchmark.DataSource != null)
            {
                grdPendingsBenchmark.DataBind();
            }



            grdPolicyBenchmark.DataSource = null;
            grdPolicyBenchmark.DataBind();

            grdPolicyBenchmark.DataSource = getResult("POLICY");

            if (grdPolicyBenchmark.DataSource != null)
            {
                grdPolicyBenchmark.DataBind();
            }


            grdCoverBenchmark.DataSource = null;
            grdCoverBenchmark.DataBind();

            grdCoverBenchmark.DataSource = getResult("COVER");

            if (grdCoverBenchmark.DataSource != null)
            {
                grdCoverBenchmark.DataBind();
            }


            grdSummary.DataSource = null;
            grdSummary.DataBind();

            grdSummary.DataSource = getResult("SUMMARY");

            if (grdSummary.DataSource != null)
            {
                grdSummary.DataBind();
            }
        }
        catch (Exception ee)
        {

        }
    }




    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        string year = "";

        year = ddlYears.SelectedValue.ToString();



        executePendingSP(year);
        executePolicySP(year);
        executeCoverSP(year);
        executeSummarySP();

        BindGrid();

    }
}
