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

public partial class MRPWorkflow_Common_DocumentList : System.Web.UI.Page
{
    string proposalNo = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["ProposalNo"] == null || Request.QueryString["ProposalNo"] == "")
            {
                return;
            }
            if (Request.QueryString["ProposalNo"] != null)
            {
                proposalNo = Request.QueryString["ProposalNo"].ToString();
                txtProposalNo.Text = proposalNo;
            }

            if (proposalNo != "")
            {

                loadUploadedDocumentsToGrid(proposalNo);
            }


        }
    }
    protected void grdUploadedDocs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //   e.Row.Cells[1].Visible = false;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string DocId = e.Row.Cells[2].Text;

            (e.Row.FindControl("irm2") as HtmlControl).Attributes.Add("src", "DocumentViewer.aspx?ProposalNo=" + proposalNo + "&DocId=" + DocId);

            LinkButton btnDel = e.Row.FindControl("lnkBtnDeleteDocument") as LinkButton;
            btnDel.Attributes.Add("onClick", "if(confirm('Are you sure to Delete this Document?','MRP Workflow')){}else{return false}");

        }

    }

    protected void grdOnlineMRPUploadedDocs_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string DocURL = e.Row.Cells[2].Text;

            (e.Row.FindControl("irm2") as HtmlControl).Attributes.Add("src", DocURL);


        }

    }



    private void loadUploadedDocumentsToGrid(string proposalNo)
    {

        grdUploadedDocs.DataSource = null;
        grdUploadedDocs.DataBind();

        DataTable docList = new DataTable();
        docList = GetDocList(proposalNo);
        grdUploadedDocs.DataSource = docList;


        if (grdUploadedDocs.DataSource != null)
        {
            grdUploadedDocs.DataBind();
        }


        grdOnlineMRPUploadedDocs.DataSource = null;
        grdOnlineMRPUploadedDocs.DataBind();

        DataTable onlineMRPDocList = new DataTable();
        onlineMRPDocList = GetOnlineMRPUploadedDocList(proposalNo);
        grdOnlineMRPUploadedDocs.DataSource = onlineMRPDocList;


        if (grdOnlineMRPUploadedDocs.DataSource != null)
        {
            grdOnlineMRPUploadedDocs.DataBind();
        }






    }

    public DataTable GetDocList(string proposalNo)
    {

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataAdapter da = new OracleDataAdapter();
        string sql = "";

        sql = " select t.DOC_SEQ_ID as \"Document Id\",  " +
                " t.DOC_NAME as \"Document Name\"  " +
                 " from MRP_WF_UPLOADED_DOCS t  " +
                " where t.PROPOSAL_NO=:V_PROPOSAL_NO  " +
                " order by t.DOC_SEQ_ID asc  ";




        OracleCommand cmd = new OracleCommand(sql, con);

        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", proposalNo));


        DataTable dt = new DataTable();

        try
        {
            con.Open();
            dt.Load(cmd.ExecuteReader());
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





    protected void lnkBtnDeleteDocument_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;

        string docId = grdUploadedDocs.Rows[rowID].Cells[2].Text;

        deleteDocument(docId);

        loadUploadedDocumentsToGrid(txtProposalNo.Text);
    }


    private void deleteDocument(string docId)
    {
        if (txtProposalNo.Text == "" && docId == null)
        {
            return;
        }
        try
        {

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            OracleDataAdapter da = new OracleDataAdapter();
            string sql = "";
            sql = "DELETE  FROM MRP_WF_UPLOADED_DOCS WHERE PROPOSAL_NO='" + txtProposalNo.Text + "' AND DOC_SEQ_ID=" + docId;
            da.DeleteCommand = new OracleCommand(sql, con);
            con.Open();



            da.DeleteCommand.ExecuteNonQuery();


            con.Close();

        }
        catch (Exception ex)
        {

        }


    }



    public DataTable GetOnlineMRPUploadedDocList(string proposalNo)
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataAdapter da = new OracleDataAdapter();
        string sql = "";

        string MRP_DOC_UPLOAD_LOCATION_URL = System.Configuration.ConfigurationManager.AppSettings["MRP_DOC_UPLOAD_LOCATION_URL"].ToString();

        sql = "select dt.doc_type_name as \"Document\" ,'" + MRP_DOC_UPLOAD_LOCATION_URL + "'||t.doc_url as \"Path\" from MRPS_MAIN m " +
                " inner join mrps_uploaded_doc t on m.seq_id=t.main_seq_id " +
                " inner join mrps_uploaded_doc_type dt on t.doc_type_id = dt.doc_type_id " +
                " where m.proposal_no=:V_PROPOSAL_NO  ";

        OracleCommand cmd = new OracleCommand(sql, con);

        cmd.Parameters.Add(new OracleParameter("V_PROPOSAL_NO", proposalNo));

       
        DataTable dt = new DataTable();

        try
        {
            con.Open();
            dt.Load(cmd.ExecuteReader());
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




}
