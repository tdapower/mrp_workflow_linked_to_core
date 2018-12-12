//******************************************
// Author            :Tharindu Athapattu
// Date              :10/09/2015
// Reviewed By       :
// Description       : Pending Report  
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.OracleClient;
using System.IO;
using System.Drawing;

public partial class PendingReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            validatePageAuthentication();
            BindGrid();
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
    public DataTable getPendings()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ORAWF"].ToString();

        OracleConnection con = new OracleConnection(connectionString);
        OracleDataAdapter da = new OracleDataAdapter();

        String selectQuery = "";
        
        selectQuery = "select t.* from MRP_WF_GET_PENDINGS t WHERE t.\"System\"='" + Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() + "'  order by t.\"Job No\"";

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
            grdPendings.DataSource = null;
            grdPendings.DataBind();

            grdPendings.DataSource = getPendings();

            if (grdPendings.DataSource != null)
            {
                grdPendings.DataBind();
            }
        }
        catch (Exception ee)
        {

        }
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=Pendings.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            grdPendings.AllowPaging = false;
            this.BindGrid();

            grdPendings.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in grdPendings.HeaderRow.Cells)
            {
                cell.BackColor = grdPendings.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grdPendings.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grdPendings.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grdPendings.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grdPendings.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}
