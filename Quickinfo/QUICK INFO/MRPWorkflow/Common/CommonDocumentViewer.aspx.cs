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

public partial class MRPWorkflow_Common_CommonDocumentViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string documentType = "";
            string docId = "";


            if (Request.QueryString["DocumentType"] == null || Request.QueryString["DocumentType"] == "")
            {
                return;
            }
            else
            {
                documentType = Request.QueryString["DocumentType"].ToString();
            }


            if (Request.QueryString["DocId"] != null)
            {
                docId = Request.QueryString["DocId"].ToString();
            }


            if (documentType == "PendingDocs")
            {
                loadPendingDocument(docId);
            }


           
        }
    }

    private void loadPendingDocument(string docId)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        try
        {

            con.Open();
            OracleDataReader dr;


            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            String selectQuery = "";
            selectQuery = "SELECT DOCUMENT FROM MRP_WF_PENDING_DOCS  " +
                      " WHERE PENDING_DOC_CODE=" + docId;

            cmd.CommandText = selectQuery;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                if (dr["DOCUMENT"] != System.DBNull.Value)
                {
                    //  OracleBlob blob = dr.GetOracleBlob(0);
                    byte[] blob = (byte[])dr["DOCUMENT"];
                    // Response.AddHeader("content-disposition", "inline;filename=" + dr[1].ToString() + "");
                    Response.AddHeader("content-length", blob.Length.ToString());


                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(blob);
                    Response.Flush();
                    // Response.End();
                }
                else
                {
                    Response.Write("<b><font size='7'>No documents available...");
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
