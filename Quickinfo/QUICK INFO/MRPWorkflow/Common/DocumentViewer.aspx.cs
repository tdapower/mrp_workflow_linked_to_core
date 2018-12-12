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
using Oracle.DataAccess.Types;

using Oracle.DataAccess.Client;

public partial class MRPWorkflow_Common_DocumentViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string proposalNo = "";
            string docId = "";

            if (Request.QueryString["ProposalNo"] == null || Request.QueryString["ProposalNo"] == "")
            {
                return;
            }
            if (Request.QueryString["ProposalNo"] != null)
            {
                proposalNo = Request.QueryString["ProposalNo"].ToString();
            }

            if (Request.QueryString["DocId"] != null)
            {
                docId = Request.QueryString["DocId"].ToString();
            }

            if (proposalNo != "" && docId != "")
            {
                loadDocument(proposalNo, docId);
            }
        }
    }

    private void loadDocument(string proposalNo, string docId)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        try
        {

            con.Open();
            OracleDataReader dr;


            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            String selectQuery = "";
            selectQuery = "SELECT DOCUMENT,DOC_NAME FROM MRP_WF_UPLOADED_DOCS  " +
                      " WHERE PROPOSAL_NO='" + proposalNo + "' AND DOC_SEQ_ID=" + docId;

            cmd.CommandText = selectQuery;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                if (dr["DOCUMENT"] != System.DBNull.Value)
                {
                    //  OracleBlob blob = dr.GetOracleBlob(0);
                    byte[] blob = (byte[])dr["DOCUMENT"];
                    Response.AddHeader("content-disposition", "inline;filename=" + dr[1].ToString() + "");
                    Response.AddHeader("content-length", blob.Length.ToString());


                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(blob);
                    Response.Flush();
                    // Response.End();
                }
            }

            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
        }
    }
}
