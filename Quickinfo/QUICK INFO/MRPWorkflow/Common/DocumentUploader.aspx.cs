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
using System.IO;

public partial class MRPWorkflow_Common_DocumentUploader : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string proposalNo = "";
            if (Session["ProposalNo"] == null)
            {
                return;
            }

            proposalNo = Session["ProposalNo"].ToString();
            if (Request.QueryString["tid"] != null)
            {

                int MaxImageNoOfProposal = 0;
                MaxImageNoOfProposal = Convert.ToInt32(getMaxImageNoOfProposal(proposalNo));




                foreach (string s in Request.Files)
                {
                    MaxImageNoOfProposal = MaxImageNoOfProposal + 1;
                    HttpPostedFile file = Request.Files[s];



                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes(file.ContentLength);

                    string fileName=new FileInfo(file.FileName).Name.ToString();
                    saveDocument(proposalNo, MaxImageNoOfProposal, binData, fileName);


                }


            }
        }
    }
    private void saveDocument(string proposalNo, int imageId, byte[] doc, string fileName)
    {


        try
        {


            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;
            int AppCode = 0;
            string strQuery = "";



            OracleParameter blobParameterDocument = new OracleParameter();


            strQuery = "INSERT INTO MRP_WF_UPLOADED_DOCS(PROPOSAL_NO,DOC_SEQ_ID,DOC_NAME, DOCUMENT) VALUES (";
            strQuery += "'" + proposalNo + "', ";
            strQuery += "" + imageId + ", ";
            strQuery += "'" + fileName + "', ";
            strQuery += ":doc)";

            blobParameterDocument.ParameterName = "doc";
            blobParameterDocument.Direction = ParameterDirection.Input;


            blobParameterDocument.Value = doc;



            spProcess = new OracleCommand(strQuery, conProcess);
            spProcess.Parameters.Add(blobParameterDocument);


            spProcess.ExecuteNonQuery();
            conProcess.Close();
            conProcess.Dispose();
        }
        catch (Exception ex)
        {

        }


    }

    private string getMaxImageNoOfProposal(string sProposalNo)
    {
        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT 	" +
                    " CASE WHEN MAX(T.DOC_SEQ_ID)  IS NULL THEN 0 ELSE TO_NUMBER((MAX(T.DOC_SEQ_ID))) END " +
                    " FROM MRP_WF_UPLOADED_DOCS T " +
                  " WHERE T.PROPOSAL_NO='" + sProposalNo + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            returnVal = dr[0].ToString();
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }
}
