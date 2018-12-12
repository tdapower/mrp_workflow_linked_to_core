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
using System.IO;
using System.Data.OleDb;
//using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Net;
using System.DirectoryServices;
using System.Net.Mail;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;

public partial class DocumentViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["DocID"] != null)
        {
            if (Request.Params["DocID"] != "")
            {
                showDocument(Request.Params["DocID"].ToString());
            }
        }
    }

    private void showDocument(string iDocumentID)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT DOCUMENT FROM MRP_WORKFLOW_DOCUMENTS WHERE DOCUMENT_ID=" + iDocumentID;

        cmd.CommandText = selectQuery;
        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            OracleBlob blob = dr.GetOracleBlob(0);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(blob.Value);
            Response.End();
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();
    }
}
