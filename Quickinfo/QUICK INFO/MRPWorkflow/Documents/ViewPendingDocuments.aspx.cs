//******************************************
// Author            :Tharindu Athapattu
// Date              :14/05/2013
// Reviewed By       :
// Description       :View Pending Documents Form 
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
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Net;
using System.DirectoryServices;
using System.Net.Mail;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class ViewPendingDocuments : System.Web.UI.Page
{

    OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
    OracleConnection myConnectionUse = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());




    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);

            ClearComponents();
            initializeValues();

            if (Request.Params["ProposalNo"] != null)
            {
                if (Request.Params["ProposalNo"] != "")
                {
                    txtProposalNo.Text = Request.Params["ProposalNo"].ToString();

                    if (isAnyPendingsAddedBefore(txtProposalNo.Text) == true)
                    {
                        if (checkAllPendingsFaxCleared(txtProposalNo.Text) == true)
                        {
                            ddlDocumentType.SelectedValue = "original";
                            loadDataForProposalNo("original");
                        }
                        else
                        {
                            ddlDocumentType.SelectedValue = "fax";
                            loadDataForProposalNo("fax");
                        }
                    }
                    else
                    {
                        ddlDocumentType.SelectedValue = "fax";
                        loadDataForProposalNo("fax");

                    }


                    if (ddlDocumentType.SelectedValue == "fax")
                    {
                        btnSendPendingLetter.Visible = true;
                        btnSendOriginalsPendingLetter.Visible = false;
                    }
                    else if (ddlDocumentType.SelectedValue == "original")
                    {
                        btnSendPendingLetter.Visible = false;
                        btnSendOriginalsPendingLetter.Visible = true;
                    }
                }
            }


            if (Session["PendingActionMode"].ToString() == "VIEW")
            {
                btnSave.Enabled = false;
                tvPendingsLifeAssured1.Enabled = false;
                tvPendingsLifeAssured2.Enabled = false;

                lblNote.Text = "Pending Documents of MRP Workflow (View Mode)";
            }

            if (!isLifeInsured2Available(Request.Params["ProposalNo"].ToString()))
            {
                tvPendingsLifeAssured2.Enabled = false;
            }
        }


    }





    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewPendingDocuments.aspx");
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


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSendPendingLetter_Click(object sender, EventArgs e)
    {

        if (checkAllPendingsCleared() != true)
        {
            //sendPendingMail(txtProposalNo.Text);
            //if (!isPendingLetterSent(txtProposalNo.Text))
            //{
            //    updatePendingLetterSentDate();
            //}
        }
        else
        {
            lblMsg.Text = "There are no pendings available / Press save button before press send letter button.";
            Timer1.Enabled = true;
        }


    }


    protected void btnSendOriginalsPendingLetter_Click(object sender, EventArgs e)
    {
        if (checkAllPendingsCleared() != true)
        {
            sendPendingMail(txtProposalNo.Text);
            updatePendingCoverNoteSentDate();
        }
        else
        {
            lblMsg.Text = "There are no pendings available / Press save button before press send letter button.";
            Timer1.Enabled = true;
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlDocumentType.SelectedValue == "original")
        {
            if (checkAllPendingsFaxCleared(txtProposalNo.Text) == false)
            {
                lblMsg.Text = "All fax documents must be cleared before clearing originals.";
                // Timer1.Enabled = true;
                return;
            }
        }


        //for life assure 1
        int pending = 0;
        foreach (TreeNode node in tvPendingsLifeAssured1.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {
                pending = 0;
                if (cnode.Checked)
                {
                    pending = 1;
                }
                savePendings(txtProposalNo.Text, 1, Convert.ToInt32(cnode.Parent.Value.ToString()), Convert.ToInt32(cnode.Value.ToString()), pending, ddlDocumentType.SelectedValue);
            }
        }

        //for life assure 2
        pending = 0;
        foreach (TreeNode node in tvPendingsLifeAssured2.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {
                pending = 0;
                if (cnode.Checked)
                {
                    pending = 1;
                }
                savePendings(txtProposalNo.Text, 2, Convert.ToInt32(cnode.Parent.Value.ToString()), Convert.ToInt32(cnode.Value.ToString()), pending, ddlDocumentType.SelectedValue);
            }
        }
        // sendPendingMail(txtProposalNo.Text);



        //if (checkAllPendingsCleared() == true)
        if (!isPendingLetterSent(txtProposalNo.Text))
        {
            if (checkAllPendingsFaxCleared(txtProposalNo.Text) == true)
            {
                updatePendingCompletedDate();
                // sendPendingCompleteMail(txtProposalNo.Text, "fax");
            }
        }

        if (checkAllPendingsClearedWithOriginals(txtProposalNo.Text) == true)
        {
            updatePendingOriginalsCompletedDate();
            // sendPendingCompleteMail(txtProposalNo.Text, "original");
        }


        updatePendingCompletedDateAsLastPendingClearTime();

        //  ClearComponents();
        loadDataForProposalNo(ddlDocumentType.SelectedValue);


        lblMsg.Text = "Successfully Saved";
        Timer1.Enabled = true;


        //  Response.Redirect("ViewPendingDocuments.aspx");
    }



    private void savePendings(string proposalNo, int lifeAssure, int pendingCategoryCode, int pendingDocumentCode, int pending, string documentType)
    {

        try
        {
            String UserName = Context.User.Identity.Name;
            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }


            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            spProcess = new OracleCommand("COMPLETE_PENDING_DOCS");

            spProcess.CommandType = CommandType.StoredProcedure;
            spProcess.Connection = conProcess;
            spProcess.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 20).Value = proposalNo;
            spProcess.Parameters.Add("V_LIFE_ASSURE", OracleType.Number, 1).Value = lifeAssure;
            spProcess.Parameters.Add("V_DOC_CAT_CODE", OracleType.Number, 5).Value = pendingCategoryCode;
            spProcess.Parameters.Add("V_PENDING_DOC_CODE", OracleType.Number, 5).Value = pendingDocumentCode;
            spProcess.Parameters.Add("V_DOC_TYPE", OracleType.VarChar, 20).Value = documentType;
            spProcess.Parameters.Add("V_IS_PENDING", OracleType.Number, 1).Value = pending;
            spProcess.Parameters.Add("V_USER_NAME", OracleType.VarChar, 250).Value = UserName;

            spProcess.ExecuteNonQuery();
            conProcess.Close();

        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error While Saving";
            Timer1.Enabled = true;
        }

    }



    private bool checkAllPendingsCleared()
    {
        //for life assure 1
        int pending = 0;
        foreach (TreeNode node in tvPendingsLifeAssured1.Nodes)
        {

            foreach (TreeNode cnode in node.ChildNodes)
            {

                if (cnode.Checked)
                {
                    pending = 1;
                }

            }
        }

        //for life assure 2

        foreach (TreeNode node in tvPendingsLifeAssured2.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {

                if (cnode.Checked)
                {
                    pending = 1;
                }

            }
        }

        if (pending == 0)
        {
            return true;
        }
        else if (pending == 1)
        {
            return false;
        }
        return false;

    }
    private bool isPendingLetterSent(string sProposalNo)
    {


        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";



        selectQuery = "SELECT CASE WHEN PENDING_COM_DATE  IS NOT NULL THEN 1 ELSE 0 END  FROM MRP_WORKFLOW " +
                        " WHERE  PROPOSAL_NO='" + sProposalNo + "' ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            if (dr[0].ToString() == "1")
            {
                returnVal = true;
            }

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }


    private bool isAnyPendingsAddedBefore(string sProposalNo)
    {


        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";



        selectQuery = "SELECT PCD.proposal_no,PCD.pending_doc_code,PCD.IS_FAX_PENDING  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                        " WHERE  PCD.PROPOSAL_NO='" + sProposalNo + "' ";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            returnVal = true;
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }

    private bool checkAllPendingsFaxCleared(string sProposalNo)
    {


        bool returnVal = true;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";



        selectQuery = "SELECT PCD.proposal_no,PCD.pending_doc_code,PCD.IS_FAX_PENDING  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                        " WHERE  PCD.PROPOSAL_NO='" + sProposalNo + "'  AND PCD.IS_FAX_PENDING=1";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            returnVal = false;
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }

    private bool checkAllPendingsClearedWithOriginals(string sProposalNo)
    {


        bool returnVal = true;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        selectQuery = "SELECT PCD.proposal_no,PCD.pending_doc_code  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD " +
                        " WHERE  PCD.PROPOSAL_NO='" + sProposalNo + "' AND  (PCD.IS_FAX_PENDING=1 OR IS_ORIGINAL_PENDING=1)";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            returnVal = false;
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }
    //private void sendPendingMail(string sProposalNo)
    //{




    //    /////////////////////////To Get frequired data//////////////////////////////////////////////////

    //    String lifeAssured1 = "";
    //    String lifeAssured2 = "";
    //    String lifeAssureds = "";
    //    String bankName = "";
    //    String otherBankName = "";
    //    String branchName = "";
    //    String assuranceCode = "";
    //    String premiumWithCuurency = "";

    //    OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
    //    OracleDataReader dr;

    //    con.Open();

    //    OracleCommand cmd = new OracleCommand();
    //    cmd.Connection = con;
    //    String selectQuery = "";
    //    selectQuery = " SELECT " +
    //                     " MRP_WORKFLOW.PROPOSAL_NO," + //0
    //                     " MRP_WORKFLOW.LIFE_INSURED_1," +//1
    //                     " MRP_WORKFLOW.LIFE_INSURED_2," +//2
    //                     " MRP_WORKFLOW.BANK," + //3
    //                     " MRP_WORKFLOW.OTHER_BANK_NAME," +//4
    //                     " MRP_WORKFLOW.BRANCH_NAME," + //5
    //                     " MRP_WORKFLOW.As_Code, " +//6
    //                    " MRP_WORKFLOW.CURRENCY, " +//7
    //                    " MRP_WORKFLOW.PREMIUM " +//8
    //                     " FROM   HNBA_CRC.MRP_WORKFLOW MRP_WORKFLOW " +
    //                     " WHERE MRP_WORKFLOW.PROPOSAL_NO='" + sProposalNo + "'";

    //    cmd.CommandText = selectQuery;

    //    dr = cmd.ExecuteReader();
    //    if (dr.HasRows)
    //    {
    //        dr.Read();

    //        lifeAssured1 = dr[1].ToString();
    //        lifeAssured2 = dr[2].ToString();
    //        bankName = dr[3].ToString();
    //        otherBankName = dr[4].ToString();
    //        branchName = dr[5].ToString();
    //        assuranceCode = dr[6].ToString();

    //        premiumWithCuurency = dr[7].ToString() + ". " + dr[8].ToString();

    //        if (bankName != ("OTHER BANK"))
    //        {
    //            bankName = bankName;
    //        }
    //        else
    //        {
    //            bankName = otherBankName;
    //        }


    //        lifeAssureds = lifeAssured1;
    //        if (lifeAssured2 != "")
    //        {
    //            lifeAssureds = lifeAssured1 + " , " + lifeAssured2;
    //        }

    //    }
    //    dr.Close();
    //    dr.Dispose();
    //    cmd.Dispose();
    //    con.Close();
    //    con.Dispose();


    //    /////////////////////////////////////////////////////////////////////////////////////////////////


    //    //string TO = "tharindu.dilanka@hnbassurance.com";
    //    string TO = "asela.bandara@hnbassurance.com";

    //    MailMessage message = new MailMessage();


    //    string FromAdd = "asela.bandara@hnbassurance.com", FromName = "MRP Workflow";

    //    MailAddress from = new MailAddress(FromAdd, FromName);
    //    MailAddress to = new MailAddress(TO);

    //    message.To.Add(to);


    //    message.From = from;
    //    message.Subject = "MRP Proposal Pending Letter - " + sProposalNo;
    //    message.IsBodyHtml = true;
    //    string BodyText;


    //    BodyText = "<html>" +
    //                "<head>" +
    //                "<title>Pending Letter</title>" +
    //                "<style type=" + "text/css" + ">" +
    //                ".outer_table{" +
    //                "border:#309dcf solid 1px;" +
    //                "}" +
    //                ".outer_table_td{" +
    //                "background-color:#001D7B;" +
    //                "height:33px;" +
    //                "font-family: Tahoma;" +
    //                "font-size:14px;" +
    //                "font-size-adjust:none;" +
    //                "font-weight:bold;" +
    //                "color:#FFFFFF;" +
    //                "}" +
    //                ".outer_table_td1{" +
    //                "background-color:#309dcf;" +
    //                "height:33px;" +
    //                "font-family: Tahoma;" +
    //                "font-size:14px;" +
    //                "font-size-adjust:none;" +
    //                "font-weight:bold;" +
    //                "color:#FFFFFF;" +
    //                "}" +
    //                ".txt_normal{" +
    //                "font-family: Tahoma;" +
    //                "font-size:11px;" +
    //                "font-size-adjust:none;" +
    //                "color:#585858;" +
    //                "height:18px;" +
    //                "}" +
    //                ".inner_table_td_green{" +
    //                "border-bottom:#CCCCCC solid 1px;" +
    //                "background-color:#f3fde4;" +
    //                "font-family: Tahoma;" +
    //                "font-size:11px;" +
    //                "font-size-adjust:none;" +
    //                "color:#528c00;" +
    //                "width:200px;" +
    //                "height:18px;" +
    //                "text-indent:5px;" +
    //                "}" +
    //                ".inner_table_td_black{" +
    //                "border-bottom:#CCCCCC solid 1px;" +
    //                "background-color:#f3fde4; " +
    //                "font-family: Tahoma;" +
    //                "font-size:11px;" +
    //                "font-size-adjust:none;" +
    //                " color:#000000; " +
    //                "height:18px;" +
    //                "text-indent:5px;" +
    //                "}" +
    //                ".inner_table_td_white{" +
    //                "border-bottom:#CCCCCC solid 1px;" +
    //                "background-color:#f3fde4;" +
    //                "font-family: Tahoma;" +
    //                "font-size:11px;" +
    //                "font-size-adjust:none;" +
    //                "color:#FFFFFF;" +
    //                "height:18px;" +
    //                "text-indent:5px;" +
    //                "}" +
    //                ".inner_table_td_red{" +
    //                "border-bottom:#CCCCCC solid 1px;" +
    //                "background-color:#f3fde4;" +
    //                "font-family: Tahoma;" +
    //                "font-size:11px;" +
    //                "font-size-adjust:none;" +
    //                "color:#E10505;" +
    //                "height:18px;" +
    //                "text-indent:5px;" +
    //                "}" +
    //                ".inner_table_td_blue{" +
    //                "border-bottom:#CCCCCC solid 1px;" +
    //                "background-color:#f2fbff;" +
    //                "font-family: Tahoma;" +
    //                "font-size:11px;" +
    //                "font-size-adjust:none;" +
    //                "color:#0776a8;" +
    //                "height:18px;" +
    //                "width:1050px;" +
    //                "text-indent:10px;" +
    //                "}" +
    //                "</style>" +
    //                "</head>" +
    //                "<body>" +
    //                "<table width=850 border=0 cellspacing=0 cellpadding=0 class=outer_table>" +
    //                "  <tr>" +
    //                "    <td  class=outer_table_td width=10>&nbsp;</td>" +
    //                "    <td align=left valign=middle  class=outer_table_td>&nbsp; MRP Proposal Pending Letter - " + sProposalNo + "&nbsp; </td>" +
    //                "    <td align=left valign=middle  class=outer_table_td  width=10>&nbsp;</td>" +
    //                "  </tr>" +
    //                "  <tr>" +
    //                "    <td >&nbsp;</td>" +
    //                "    <td align=left valign=top >" +
    //                "    <table width=100% border=0 cellspacing=0 cellpadding=0>" +
    //                "      <tr>" +
    //                "        <td class=txt_normal></td>" +
    //                "     </tr>" +
    //                "     <tr>" +
    //                "       <td class=txt_normal></td>" +
    //                "     </tr>" +
    //                "     <tr>" +
    //                "       <td ></td>" +
    //                "     </tr>" +
    //                "   </table>" +
    //                "   <table width=850 border=0 cellspacing=2 cellpadding=3>" +
    //                "           <tr >" +
    //                "             <td colspan=2 class=txt_normal><strong>Proposal Pending Details</strong> " +
    //                "       </tr>" +
    //                "           <tr >" +
    //                "             <td class=inner_table_td_green>Proposal Number (MRP) </td>" +
    //                "             <td class=inner_table_td_blue>" + sProposalNo + "</td>" +
    //                "           </tr>" +
    //                "           <tr>" +
    //                "             <td class=inner_table_td_green>Name of Life Assured </td>" +
    //                "             <td class=inner_table_td_blue>" + lifeAssureds + "</td>" +
    //                "           </tr>" +
    //                "           <tr>" +
    //                "             <td class=inner_table_td_green>HNBA Branch </td>" +
    //                "             <td class=inner_table_td_blue>" + assuranceCode + "</td>" +
    //                "           </tr>" +
    //                "           <tr>" +
    //                "             <td class=inner_table_td_green>Pending Create Date </td>" +
    //                "             <td class=inner_table_td_blue>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td>" +
    //                "           </tr>" +
    //                "           <tr>" +
    //                "             <td class=inner_table_td_green>1st Reminder Date</td>" +
    //                "             <td class=inner_table_td_blue>" + DateTime.Now.AddDays(7).ToString("dd/MM/yyyy") + "</td>" +
    //                "           </tr>" +
    //                "	<tr>" +
    //                "      </table>" +
    //                "<br /> " +
    //                "<table width=850 border=0 cellspacing=1 cellpadding=3> " +
    //                "<tr> " +
    //                "		<td class=inner_table_td_black>While thanking you for the proposal for Life Assurance (MRP) made to us, we would like to inform you that the above proposal is pending for the following reason/s.</td> " +
    //                "	<tr> " +
    //                "	<tr> " +
    //                "		<td></td> " +
    //                " <tr> " +
    //                " <tr> " +
    //                "		<td class=inner_table_td_red>Please comply with the requirements on or before : " + DateTime.Now.AddDays(7).ToString("dd/MM/yyyy") + "</td> " +
    //                " <tr> " +
    //                "</table> " +
    //                "<br /> " +
    //                "<table width=850 border=0 cellspacing=1 cellpadding=3> " +
    //                "	<tr> " +
    //                "		<td colspan='2' class=inner_table_td_green style=width:200px !important; text-align:center !important;><strong>Requirements - </strong></td> " +
    //                "	</tr> " +
    //                "	<tr> " +
    //                "		<td class=inner_table_td_blue><strong>" + lifeAssured1 + "</strong></td>" +
    //                "		<td class=inner_table_td_blue><strong>" + lifeAssured2 + "</strong></td>" +
    //                "	</tr> " +
    //                "	<tr> " +
    //                "		<td class=inner_table_td_blue>&nbsp;&nbsp;  " + getPendingRequirementsList(sProposalNo, "1", premiumWithCuurency) + "</td>" +
    //                "		<td class=inner_table_td_blue>&nbsp;&nbsp;  " + getPendingRequirementsList(sProposalNo, "2", premiumWithCuurency) + "</td>" +
    //                "	</tr> " +
    //                "	<tr> " +
    //                "		<td colspan='2' class=inner_table_td_blue>" + "" + "</td> " +
    //                "	</tr> " +
    //                "</table> " +
    //                "</br> " +
    //                "</br> " +
    //                "</br> " +
    //                "   <table width=100% border=0 cellspacing=0 cellpadding=0> " +
    //                "<tr> " +
    //                " <td>&nbsp;</td> " +
    //                "</tr> " +
    //                "<tr> " +
    //                "  <td class=txt_normal ><p style=height:29px> " +
    //                " This is an auto generated email sent to you from the Life Workflow. Please do not reply to this email. " +
    //                "</p> " +
    //                "</td> " +
    //                "</tr> " +
    //                "<tr> " +
    //                " <td class=txt_normal>Regards,<br /> " +
    //                "Workflow Administrator</td> " +
    //                " </tr> " +
    //                "</table> " +
    //                "   </td> " +
    //                "  <td align=left valign=top ></td> " +
    //                "</tr> " +
    //                "<tr> " +
    //                " <td colspan=3 >&nbsp;</td> " +
    //                "</tr> " +
    //                "</table> " +
    //                " <p> " +
    //                "</p>  " +
    //                " </body> " +
    //                " </html>";

    //    message.Body = @BodyText;


    //    SmtpClient client = new SmtpClient("smtp2.hnbassurance.com");

    //    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("crc", "HNBA@customer");
    //    client.UseDefaultCredentials = false;
    //    client.Credentials = SMTPUserInfo;

    //    try
    //    {
    //        client.Send(message);

    //        lblMsg.Text = "Pending Letter Successfully Sent.";
    //        Timer1.Enabled = true;

    //    }
    //    catch (Exception ee)
    //    {
    //        lblMsg.Text = "Error while Sending Pending Letter.";
    //        Timer1.Enabled = true;
    //    }
    //}
    private string loadDesignationOfPerson(string userCode)
    {
        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "   SELECT T.DESIGNATION FROM MRP_WF_SIGN_PERSON T  " +
                        "  WHERE T.USER_CODE='" + userCode + "'";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            returnValue = dr[0].ToString();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
    }
    private string loadNameOfPerson(string userCode)
    {
        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "   SELECT T.SIGN_PERSON_NAME FROM MRP_WF_SIGN_PERSON T  " +
                     "  WHERE T.USER_CODE='" + userCode + "'";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();


            returnValue = dr[0].ToString();

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
    }
    private void sendPendingMail(string sProposalNo)
    {

        String UserName = Context.User.Identity.Name;
        if (Left(UserName, 4) == "HNBA")
        {
            UserName = Right(UserName, (UserName.Length) - 5);
        }
        else
        {
            UserName = Right(UserName, (UserName.Length) - 7);
        }


        ReportDocument crystalReport = new ReportDocument();
        crystalReport.Load(Server.MapPath("~/MRPWorkflow/Documents/letters/PENDING_DOCUMENTS_REQUESTING_LETTER.rpt"));
        //crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "HNBUAT", "");
        crystalReport.SetDatabaseLogon("hnba_crc", "HNBACRC", "RACPROD", "");


        crystalReport.SetParameterValue(0, sProposalNo);
        crystalReport.SetParameterValue("signPersonName", UserName);
        crystalReport.SetParameterValue("signPersonDisplayName", loadNameOfPerson(UserName));
        crystalReport.SetParameterValue("signPersonDesignation", loadDesignationOfPerson(UserName));
        // crystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Pending Letter");





        /////////////////////////To Get frequired data//////////////////////////////////////////////////

        String lifeAssured1 = "";
        String lifeAssured2 = "";
        String lifeAssureds = "";
        String bankName = "";
        String bankBranchName = "";
        String otherBankName = "";
        String branchName = "";
        String assuranceCode = "";
        String premiumWithCuurency = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = " SELECT " +
                         " MRP_WORKFLOW.PROPOSAL_NO," + //0
                         " MRP_WORKFLOW.LIFE_INSURED_1," +//1
                         " MRP_WORKFLOW.LIFE_INSURED_2," +//2
                         " MRP_WORKFLOW.BANK," + //3
                         " MRP_WORKFLOW.OTHER_BANK_NAME," +//4
                         " MRP_WORKFLOW.BRANCH_NAME," + //5
                         " MRP_WORKFLOW.As_Code, " +//6
                        " MRP_WORKFLOW.CURRENCY, " +//7
                        " MRP_WORKFLOW.PREMIUM " +//8
                         " FROM   HNBA_CRC.MRP_WORKFLOW MRP_WORKFLOW " +
                         " WHERE MRP_WORKFLOW.PROPOSAL_NO='" + sProposalNo + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            lifeAssured1 = dr[1].ToString();
            lifeAssured2 = dr[2].ToString();
            bankName = dr[3].ToString();
            otherBankName = dr[4].ToString();
            bankBranchName = dr[5].ToString();
            assuranceCode = dr[6].ToString();

            premiumWithCuurency = dr[7].ToString() + ". " + dr[8].ToString();

            if (bankName != ("OTHER BANK"))
            {
                bankName = bankName;
            }
            else
            {
                bankName = otherBankName;
            }


            lifeAssureds = lifeAssured1;
            if (lifeAssured2 != "")
            {
                lifeAssureds = lifeAssured1 + " , " + lifeAssured2;
            }

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        /////////////////////////////////////////////////////////////////////////////////////////////////


        MRPWFMail mail = new MRPWFMail();
        mail.From_address = "mrp.workflow@hnbassurance.com";
        mail.To_address = "tharindu.dilanka@hnbassurance.com";
        // mail.To_address = Get_Email_Addresses("to", sProposalNo);
        // mail.Cc_address = Get_Email_Addresses("cc", sProposalNo);
        mail.Bcc_address = "tharindu.dilanka@hnbassurance.com";
        mail.Subject = "MRP REMINDER";

        mail.Attachment = (new Attachment(crystalReport.ExportToStream(ExportFormatType.PortableDocFormat), "Pending_Letter.pdf"));


        String BodyText;


        BodyText = "<html>" +
                    "<head>" +
                    "<title>Pending Letter</title>" +
                    "<style type=" + "text/css" + ">" +
                    ".outer_table{" +
                    "border:#309dcf solid 1px;" +
                    "}" +
                    ".outer_table_td{" +
                    "background-color:#001D7B;" +
                    "height:33px;" +
                    "font-family: Tahoma;" +
                    "font-size:14px;" +
                    "font-size-adjust:none;" +
                    "font-weight:bold;" +
                    "color:#FFFFFF;" +
                    "}" +
                    ".outer_table_td1{" +
                    "background-color:#309dcf;" +
                    "height:33px;" +
                    "font-family: Tahoma;" +
                    "font-size:14px;" +
                    "font-size-adjust:none;" +
                    "font-weight:bold;" +
                    "color:#FFFFFF;" +
                    "}" +
                    ".txt_normal{" +
                    "font-family: Tahoma;" +
                    "font-size:11px;" +
                    "font-size-adjust:none;" +
                    "color:#585858;" +
                    "height:18px;" +
                    "}" +
                    ".inner_table_td_green{" +
                    "border-bottom:#CCCCCC solid 1px;" +
                    "background-color:#f3fde4;" +
                    "font-family: Tahoma;" +
                    "font-size:11px;" +
                    "font-size-adjust:none;" +
                    "color:#528c00;" +
                    "width:200px;" +
                    "height:18px;" +
                    "text-indent:5px;" +
                    "}" +
                    ".inner_table_td_black{" +
                    "border-bottom:#CCCCCC solid 1px;" +
                    "background-color:#f3fde4; " +
                    "font-family: Tahoma;" +
                    "font-size:11px;" +
                    "font-size-adjust:none;" +
                    " color:#000000; " +
                    "height:18px;" +
                    "text-indent:5px;" +
                    "}" +
                    ".inner_table_td_white{" +
                    "border-bottom:#CCCCCC solid 1px;" +
                    "background-color:#f3fde4;" +
                    "font-family: Tahoma;" +
                    "font-size:11px;" +
                    "font-size-adjust:none;" +
                    "color:#FFFFFF;" +
                    "height:18px;" +
                    "text-indent:5px;" +
                    "}" +
                    ".inner_table_td_red{" +
                    "border-bottom:#CCCCCC solid 1px;" +
                    "background-color:#f3fde4;" +
                    "font-family: Tahoma;" +
                    "font-size:11px;" +
                    "font-size-adjust:none;" +
                    "color:#E10505;" +
                    "height:18px;" +
                    "text-indent:5px;" +
                    "}" +
                    ".inner_table_td_black_bold{" +
                    "border-bottom:#CCCCCC solid 1px;" +
                    "background-color:#f3fde4;" +
                    "font-family: Tahoma;" +
                    "font-size:11px;" +
                    "font-size-adjust:none;" +
                    "color:#000000;" +
                    "height:18px;" +
                    "text-indent:5px;" +
                     "}" +
                    ".inner_table_td_blue{" +
                    "border-bottom:#CCCCCC solid 1px;" +
                    "background-color:#f2fbff;" +
                    "font-family: Tahoma;" +
                    "font-size:11px;" +
                    "font-size-adjust:none;" +
                    "color:#0776a8;" +
                    "height:18px;" +
                    "width:1050px;" +
                    "text-indent:10px;" +
                    "}" +
                    "</style>" +
                    "</head>" +
                    "<body>" +
                    "<table width=850 border=0 cellspacing=0 cellpadding=0 class=outer_table>" +
                    "  <tr>" +
                    "    <td  class=outer_table_td width=10>&nbsp;</td>" +
                    "    <td align=left valign=middle  class=outer_table_td>&nbsp; MRP Proposal Pending Letter - " + sProposalNo + "&nbsp; </td>" +
                    "    <td align=left valign=middle  class=outer_table_td  width=10>&nbsp;</td>" +
                    "  </tr>" +
                    "  <tr>" +
                    "    <td >&nbsp;</td>" +
                    "    <td align=left valign=top >" +
                    "    <table width=100% border=0 cellspacing=0 cellpadding=0>" +
                    "      <tr>" +
                    "        <td class=txt_normal></td>" +
                    "     </tr>" +
                    "     <tr>" +
                    "       <td class=txt_normal></td>" +
                    "     </tr>" +
                    "     <tr>" +
                    "       <td ></td>" +
                    "     </tr>" +
                    "   </table>" +
                    "   <table width=850 border=0 cellspacing=2 cellpadding=3>" +
                    "           <tr >" +
                    "             <td colspan=2 class=txt_normal><strong>Proposal Pending Details</strong> " +
                    "       </tr>" +
                    "           <tr >" +
                    "             <td class=inner_table_td_green>Proposal Number (MRP) </td>" +
                    "             <td class=inner_table_td_blue>" + sProposalNo + "</td>" +
                    "           </tr>" +
                    "           <tr>" +
                    "             <td class=inner_table_td_green>Name of Life Assured </td>" +
                    "             <td class=inner_table_td_blue>" + lifeAssureds + "</td>" +
                    "           </tr>" +
                    "           <tr>" +
                    "             <td class=inner_table_td_green>Bank / Financial Institute </td>" +
                    "             <td class=inner_table_td_blue>" + bankName + "</td>" +
                    "           </tr>" +
                    "           <tr>" +
                    "             <td class=inner_table_td_green>Bank Branch </td>" +
                    "             <td class=inner_table_td_blue>" + bankBranchName + "</td>" +
                    "           </tr>" +
                    "           <tr>" +
                    "             <td class=inner_table_td_green>HNBA Branch </td>" +
                    "             <td class=inner_table_td_blue>" + assuranceCode + "</td>" +
                    "           </tr>" +
                    "           <tr>" +
                    "             <td class=inner_table_td_green>Pending Create Date </td>" +
                    "             <td class=inner_table_td_blue>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td>" +
                    "           </tr>" +
                    "           <tr>" +
                    "             <td class=inner_table_td_green>1st Reminder Date</td>" +
                    "             <td class=inner_table_td_blue>" + DateTime.Now.AddDays(7).ToString("dd/MM/yyyy") + "</td>" +
                    "           </tr>" +
                    "	<tr>" +
                    "      </table>" +
                    "<br /> " +
                    "<table width=850 border=0 cellspacing=1 cellpadding=3> " +
                    "<tr> " +
                    "		<td class=inner_table_td_black>While thanking you for the proposal for Life Assurance (MRP) made to us, we would like to inform you that the above proposal is pending for the following reason/s.</td> " +
                    "	<tr> " +
                    "	<tr> " +
                    "		<td></td> " +
                    " <tr> " +
                    " <tr> " +
                    "		<td class=inner_table_td_black_bold>Please comply with the requirements on or before : " + DateTime.Now.AddDays(7).ToString("dd/MM/yyyy") + "</td> " +
                    " <tr> " +
                    "</table> " +
                    "<br /> " +
                    "<table width=850 border=0 cellspacing=1 cellpadding=3> " +
                    "	<tr> " +
                    "		<td colspan='2' class=inner_table_td_green style=width:200px !important; text-align:center !important;><strong>Requirements - </strong></td> " +
                    "	</tr> " +
                    "	<tr> " +
                    "		<td class=inner_table_td_blue><strong>" + lifeAssured1 + "</strong></td>" +
                    "		<td class=inner_table_td_blue><strong>" + lifeAssured2 + "</strong></td>" +
                    "	</tr> " +
                    "	<tr> " +
                    "		<td class=inner_table_td_blue>&nbsp;&nbsp;  " + getPendingRequirementsList(sProposalNo, "1", premiumWithCuurency) + "</td>" +
                    "		<td class=inner_table_td_blue>&nbsp;&nbsp;  " + getPendingRequirementsList(sProposalNo, "2", premiumWithCuurency) + "</td>" +
                    "	</tr> " +
                    "	<tr> " +
                    "		<td colspan='2' class=inner_table_td_blue>" + "" + "</td> " +
                    "	</tr> " +
                    "</table> " +
                    "</br> " +
                    "</br> " +
                    "</br> " +
                    "   <table width=100% border=0 cellspacing=0 cellpadding=0> " +
                    "<tr> " +
                    " <td>&nbsp;</td> " +
                    "</tr> " +
                    "<tr> " +
                    "  <td class=txt_normal ><p style=height:29px> " +
                    " This is an auto generated email sent to you from the Life Workflow. Please do not reply to this email. " +
                    "</p> " +
                    "</td> " +
                    "</tr> " +
                    "<tr> " +
                    " <td class=txt_normal>Regards,<br /> " +
                    "Workflow Administrator</td> " +
                    " </tr> " +
                    "</table> " +
                    "   </td> " +
                    "  <td align=left valign=top ></td> " +
                    "</tr> " +
                    "<tr> " +
                    " <td colspan=3 >&nbsp;</td> " +
                    "</tr> " +
                    "</table> " +
                    " <p> " +
                    "</p>  " +
                    " </body> " +
                    " </html>";



        try
        {
            mail.Body = BodyText;
            mail.sendMail();

            lblMsg.Text = "Pending Letter Successfully Sent.";
            Timer1.Enabled = true;

        }
        catch (Exception ee)
        {
            lblMsg.Text = "Error while Sending Pending Letter.";
            Timer1.Enabled = true;
        }
    }



    private string Get_Email_Addresses(String sEmailType, String sProposalNo)
    {
        String returnVal = "";
        string mrpManagerEmail = "";
        mrpManagerEmail = System.Configuration.ConfigurationManager.AppSettings["MRPManagerEmail"].ToString();

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand cmd = new OracleCommand();
        OracleDataReader dr;

        try
        {
            con.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;

            if (sEmailType == "to")
            {

                cmd.CommandText = "select  CASE   WHEN  t.hnb_email='NOT APP' THEN '" + mrpManagerEmail + "' WHEN t.hnb_email  IS NOT NULL THEN t.hnb_email ELSE '" + mrpManagerEmail + "' END " +
                                     " from mrp_wf_banks_email t  INNER JOIN mrp_workflow m on t.bank_code=m.agent_code  where  m.proposal_no='" + sProposalNo + "'";

            }
            else if (sEmailType == "cc")
            {
                cmd.CommandText = "select  CASE   WHEN  t.bancass_email='NOT APP' THEN '" + mrpManagerEmail + "' WHEN  t.bancass_email  IS NOT NULL THEN  t.bancass_email   ELSE '" + mrpManagerEmail + "' END," +
                                 " CASE WHEN   be.hnba_email='NOT APP'  THEN '" + mrpManagerEmail + "'  WHEN be.hnba_email  IS NOT NULL THEN  be.hnba_email ELSE '" + mrpManagerEmail + "' END  " +
                                 " from    mrp_workflow m  " +
                                 " LEFT JOIN mrp_wf_banks_email t on t.bank_code=m.agent_code " +
                                 " LEFT jOIN bancassurance_email be ON t.hnba_branch_code=be.hnb_code " +
                                 "  WHERE  m.proposal_no='" + sProposalNo + "'";

            }
            else if (sEmailType == "assignedUser")
            {
                cmd.CommandText = "SELECT MUD.EMAIL FROM MRP_WORKFLOW MW INNER JOIN MRP_USER_DETAILS MUD ON MW.ASSIGNED_USER=MUD.USER_CODE " +
                                 "  WHERE  MW.PROPOSAL_NO='" + sProposalNo + "'";

            }
            else if (sEmailType == "superUsers")
            {
                string MRPSupervisoUserCode = System.Configuration.ConfigurationManager.AppSettings["MRPSupervisoUserCode"].ToString();

                cmd.CommandText = " SELECT listagg(MUD.EMAIL, ',') within group (order by 1)  superusers FROM WF_ADMIN_USERS WAU " +
                                 " INNER JOIN MRP_USER_DETAILS MUD ON WAU.USER_CODE=MUD.USER_CODE " +
                                 " WHERE WAU.USER_ROLE_CODE=" + MRPSupervisoUserCode;

            }


            dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (sEmailType == "to")
                    {
                        returnVal = dr[0].ToString();
                    }
                    else if (sEmailType == "cc")
                    {
                        returnVal = dr[0].ToString() + "," + dr[1].ToString();
                    }
                    else if (sEmailType == "assignedUser")
                    {
                        returnVal = dr[0].ToString();
                    }
                    else if (sEmailType == "superUsers")
                    {
                        returnVal = dr[0].ToString();
                    }
                }
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {

        }

        if (sEmailType == "cc")
        {
            returnVal = returnVal + "," + Get_User_Email_Addresses(sProposalNo) + "," + txtEmailCcAddresses.Text;
        }


        return returnVal;
    }



    private string Get_User_Email_Addresses(String sProposalNo)
    {
        String returnVal = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand cmd = new OracleCommand();
        OracleDataReader dr;

        string mrpManagerEmail = "";
        mrpManagerEmail = System.Configuration.ConfigurationManager.AppSettings["MRPManagerEmail"].ToString();


        try
        {
            con.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;

            cmd.CommandText = "SELECT CASE WHEN u.email  IS NOT NULL THEN u.email ELSE '" + mrpManagerEmail + "' END ,CASE WHEN su.email  IS NOT NULL THEN su.email ELSE '" + mrpManagerEmail + "' END  from mrp_workflow m  " +
                    "INNER JOIN MRP_USER_DETAILS U ON m.assigned_user=u.user_code INNER JOIN  MRP_USER_DETAILS SU ON u.supervisor_user_code=Su.user_code WHERE m.proposal_no='" + sProposalNo + "'";

            dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    returnVal = dr[0].ToString() + "," + dr[1].ToString();
                }
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {

        }

        return returnVal;

    }

    private string getPendingRequirementsList(string sProposalNo, string lifeAssureNo, string sPremium)
    {

        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "  SELECT " +
                         " MRP_WF_PENDING_COMPLETE_DOCS.PROPOSAL_NO," + //0
                         " MRP_WF_PENDING_COMPLETE_DOCS.LIFE_ASSURE," +//1
                         " MRP_WF_PENDING_COMPLETE_DOCS.IS_FAX_PENDING," +//2
                         " MRP_WF_PENDING_DOCS.PENDING_DOC_NAME, " +//3
                         " MRP_WF_PENDING_DOCS.PENDING_DOC_CODE " +//4
                         " FROM   HNBA_CRC.MRP_WF_PENDING_COMPLETE_DOCS MRP_WF_PENDING_COMPLETE_DOCS  " +
                         " INNER JOIN HNBA_CRC.MRP_WF_PENDING_DOCS MRP_WF_PENDING_DOCS  " +
                         " ON MRP_WF_PENDING_COMPLETE_DOCS.PENDING_DOC_CODE=MRP_WF_PENDING_DOCS.PENDING_DOC_CODE " +
                         " WHERE  " +
                         "  MRP_WF_PENDING_COMPLETE_DOCS.PROPOSAL_NO='" + sProposalNo + "' AND " +
        " MRP_WF_PENDING_COMPLETE_DOCS.LIFE_ASSURE=" + lifeAssureNo + " AND  " +
        " (MRP_WF_PENDING_COMPLETE_DOCS.IS_FAX_PENDING=1 OR MRP_WF_PENDING_COMPLETE_DOCS.IS_ORIGINAL_PENDING=1 ) ";


        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                if (dr[4].ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPWFSinglePremiumPendingDocCode"].ToString())
                {
                    returnVal = returnVal + "<br />&nbsp;&nbsp;&nbsp;&nbsp; " + dr[3].ToString() + " of " + sPremium;
                }
                else
                {
                    returnVal = returnVal + "<br />&nbsp;&nbsp;&nbsp;&nbsp; " + dr[3].ToString();
                }
            }

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }


    private void ClearComponents()
    {
        txtProposalNo.Text = "";

        //for life assure 1
        foreach (TreeNode node in tvPendingsLifeAssured1.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {
                cnode.Checked = false;
            }
        }


        //for life assure 2
        foreach (TreeNode node in tvPendingsLifeAssured2.Nodes)
        {
            foreach (TreeNode cnode in node.ChildNodes)
            {
                cnode.Checked = false;
            }
        }



        // tvPendingsLifeAssured1.Enabled = false;

        // btnAlter.Enabled = false;
        // btnSave.Enabled = false;
    }



    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtProposalNo.Text == "")
        {
            lblMsg.Text = "Please Select A Proposal No";
            return;
        }

        tvPendingsLifeAssured1.Enabled = true;
        tvPendingsLifeAssured2.Enabled = true;

        btnSave.Enabled = true;


    }


    private void initializeValues()
    {
        lblMsg.Text = "";


        ddlDocumentType.Items.Clear();
        ddlDocumentType.Items.Add(new ListItem("--- Select One ---", "0"));
        ddlDocumentType.Items.Add(new ListItem("Fax", "fax"));
        ddlDocumentType.Items.Add(new ListItem("Original", "original"));
        ddlDocumentType.SelectedValue = "fax";

    }



    private void PopulatePendingsTreeview(TreeView treeView, int lifeAssure, string documentType)
    {
        try
        {
            DataTable dtPendingCategory = new DataTable();
            DataTable dtPendingDoc = new DataTable();
            dtPendingCategory = FillPendingCategoryTable(treeView);
            dtPendingDoc = FillPendingDocumentTable(treeView);
            DataSet ds = new DataSet();
            ds.Tables.Add(dtPendingCategory);
            ds.Tables.Add(dtPendingDoc);
            ds.Relations.Add("pendingDocuments", dtPendingCategory.Columns["DOC_CAT_CODE"], dtPendingDoc.Columns["DOC_CAT_CODE"]);

            if (ds.Tables[0].Rows.Count > 0)
            {
                treeView.Nodes.Clear();
                treeView.ShowCheckBoxes = TreeNodeTypes.Leaf;


                foreach (DataRow masterRow in ds.Tables[0].Rows)
                {
                    TreeNode masterNode = new TreeNode((string)masterRow["DOC_CAT_NAME"], Convert.ToString(masterRow["DOC_CAT_CODE"]));
                    treeView.Nodes.Add(masterNode);
                    masterNode.Collapse();
                    foreach (DataRow childRow in masterRow.GetChildRows("pendingDocuments"))
                    {
                        TreeNode childNode = new TreeNode((string)childRow["PENDING_DOC_NAME"], Convert.ToString(childRow["PENDING_DOC_CODE"]));
                        if (isPending(Convert.ToString(masterRow["DOC_CAT_CODE"]), Convert.ToString(childRow["PENDING_DOC_CODE"]), txtProposalNo.Text, lifeAssure, documentType))
                        {
                            childNode.Checked = true;
                            masterNode.Expand();
                            childNode.Text = "<font color='Red'>" + (string)childRow["PENDING_DOC_NAME"] + "</font>";
                        }
                        else
                        {

                            //if (Session["PendingActionMode"].ToString() == "ADD")
                            //{

                            //}
                            if (Session["PendingActionMode"].ToString() == "UPDATE")
                            {
                                childNode.Checked = false;
                                childNode.ShowCheckBox = false;
                            }
                        }
                        childNode.SelectAction = TreeNodeSelectAction.None;
                        masterNode.ChildNodes.Add(childNode);
                        childNode.Value = Convert.ToString(childRow["PENDING_DOC_CODE"]);

                    }
                }
                // treeView.CollapseAll();
            }
        }
        catch (Exception ex)
        {
            //throw new Exception("Unable to populate treeview" + ex.Message);
        }
    }

    private DataTable FillPendingCategoryTable(TreeView treeView)
    {
        DataTable dtPendingCategory = new DataTable();

        dtPendingCategory.Columns.Add("DOC_CAT_CODE", Type.GetType("System.Int32"));
        dtPendingCategory.Columns.Add("DOC_CAT_NAME", Type.GetType("System.String"));

        DataRow drPendingCategory;

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
        {
            selectQuery = "SELECT DOC_CAT_CODE,DOC_CAT_NAME FROM MRP_WF_PENDING_DOC_CATEGORY WHERE WORKFLOW_TYPE='MRP' ORDER BY ORDER_SEQ,DOC_CAT_NAME";
        }
        else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
        {
            selectQuery = "SELECT DOC_CAT_CODE,DOC_CAT_NAME FROM MRP_WF_PENDING_DOC_CATEGORY WHERE WORKFLOW_TYPE='MCR'  ORDER BY ORDER_SEQ,DOC_CAT_NAME";
        }






        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();

        treeView.Nodes.Clear();

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                drPendingCategory = dtPendingCategory.NewRow();
                drPendingCategory[0] = Convert.ToInt32(dr[0].ToString());
                drPendingCategory[1] = dr[1].ToString();
                dtPendingCategory.Rows.Add(drPendingCategory);

            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        ////

        return dtPendingCategory;
    }

    private DataTable FillPendingDocumentTable(TreeView treeView)
    {
        DataTable dtPendingDoc = new DataTable();

        dtPendingDoc.Columns.Add("PENDING_DOC_CODE", Type.GetType("System.Int32"));
        dtPendingDoc.Columns.Add("DOC_CAT_CODE", Type.GetType("System.Int32"));
        dtPendingDoc.Columns.Add("PENDING_DOC_NAME", Type.GetType("System.String"));

        DataRow drPendingDoc;


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        if (ddlDocumentType.SelectedValue == "fax")
        {



            if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
            {
                selectQuery = "SELECT D.PENDING_DOC_CODE,D.DOC_CAT_CODE,D.PENDING_DOC_NAME FROM MRP_WF_PENDING_DOCS D " +
                            " INNER JOIN MRP_WF_PENDING_DOC_CATEGORY DC ON D.DOC_CAT_CODE=DC.DOC_CAT_CODE " +    
                            " WHERE D.PENDING_DOC_CODE NOT IN(1,3) AND DC.WORKFLOW_TYPE='MRP'" + //Don't load Original  Proposal Form(1) and Single Premium(3)
                            " ORDER BY D.PENDING_DOC_CODE,D.PENDING_DOC_NAME";
            }
            else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
            {
                selectQuery = "SELECT D.PENDING_DOC_CODE,D.DOC_CAT_CODE,D.PENDING_DOC_NAME FROM MRP_WF_PENDING_DOCS D " +
                 " INNER JOIN MRP_WF_PENDING_DOC_CATEGORY DC ON D.DOC_CAT_CODE=DC.DOC_CAT_CODE " +
                 " WHERE D.PENDING_DOC_CODE NOT IN(1,3) AND DC.WORKFLOW_TYPE='MCR'" + //Don't load Original  Proposal Form(1) and Single Premium(3)
                 " ORDER BY D.PENDING_DOC_CODE,D.PENDING_DOC_NAME";
            }

        }
        else
        {
           

            if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MRPCode"].ToString())
            {
                selectQuery = "SELECT D.PENDING_DOC_CODE,D.DOC_CAT_CODE,D.PENDING_DOC_NAME FROM MRP_WF_PENDING_DOCS D " +
                 " INNER JOIN MRP_WF_PENDING_DOC_CATEGORY DC ON D.DOC_CAT_CODE=DC.DOC_CAT_CODE " +
                    " WHERE DC.WORKFLOW_TYPE='MRP'  ORDER BY D.PENDING_DOC_CODE,D.PENDING_DOC_NAME";

            }
            else if (Request.Cookies["WORKFLOW_CHOICE"].Value.ToString() == System.Configuration.ConfigurationManager.AppSettings["MCRCode"].ToString())
            {
                selectQuery = "SELECT D.PENDING_DOC_CODE,D.DOC_CAT_CODE,D.PENDING_DOC_NAME FROM MRP_WF_PENDING_DOCS D " +
               " INNER JOIN MRP_WF_PENDING_DOC_CATEGORY DC ON D.DOC_CAT_CODE=DC.DOC_CAT_CODE " +
                  " WHERE DC.WORKFLOW_TYPE='MCR'  ORDER BY D.PENDING_DOC_CODE,D.PENDING_DOC_NAME";
            }
        }

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();

        treeView.Nodes.Clear();

        if (dr.HasRows)
        {
            while (dr.Read())
            {

                drPendingDoc = dtPendingDoc.NewRow();
                drPendingDoc[0] = Convert.ToInt32(dr[0].ToString());
                drPendingDoc[1] = Convert.ToInt32(dr[1].ToString());
                drPendingDoc[2] = dr[2].ToString();
                dtPendingDoc.Rows.Add(drPendingDoc);
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return dtPendingDoc;
    }

    private bool isPending(string pendingCategoryCode, string pendingDocumentCode, string proposalNo, int lifeAssure, string documentType)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        if (documentType == "fax")
        {
            selectQuery = " SELECT (CASE WHEN T.IS_FAX_PENDING IS NULL THEN 0 ELSE T.IS_FAX_PENDING END)  " +
                           " FROM MRP_WF_PENDING_COMPLETE_DOCS T " +
                           " WHERE T.DOC_CAT_CODE='" + pendingCategoryCode + "' AND T.PENDING_DOC_CODE='" + pendingDocumentCode + "' AND T.PROPOSAL_NO='" + proposalNo + "' AND T.LIFE_ASSURE=" + lifeAssure;
        }
        else if (documentType == "original")
        {

            selectQuery = " SELECT (CASE WHEN T.IS_ORIGINAL_PENDING IS NULL THEN 0 ELSE T.IS_ORIGINAL_PENDING END)  " +
                        " FROM MRP_WF_PENDING_COMPLETE_DOCS T " +
                        " WHERE T.DOC_CAT_CODE='" + pendingCategoryCode + "' AND T.PENDING_DOC_CODE='" + pendingDocumentCode + "' AND T.PROPOSAL_NO='" + proposalNo + "' AND T.LIFE_ASSURE=" + lifeAssure;

        }



        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {

                if (dr[0].ToString() == "1")
                {
                    returnVal = true;
                }
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }




    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }


    private void loadDataForProposalNo(string documentType)
    {


        if (documentType == "fax")
        {
            //for life assure 1
            foreach (TreeNode node in tvPendingsLifeAssured1.Nodes)
            {
                foreach (TreeNode cnode in node.ChildNodes)
                {
                    cnode.Checked = false;
                }
            }
            PopulatePendingsTreeview(tvPendingsLifeAssured1, 1, documentType);

            //////////////////////////

            //for life assure 2
            foreach (TreeNode node in tvPendingsLifeAssured2.Nodes)
            {
                foreach (TreeNode cnode in node.ChildNodes)
                {
                    cnode.Checked = false;
                }
            }
            PopulatePendingsTreeview(tvPendingsLifeAssured2, 2, documentType);


            //////////////////////////
        }
        else if (documentType == "original")
        {
            //for life assure 1
            foreach (TreeNode node in tvPendingsLifeAssured1.Nodes)
            {
                foreach (TreeNode cnode in node.ChildNodes)
                {
                    cnode.Checked = false;
                }
            }
            PopulatePendingsTreeview(tvPendingsLifeAssured1, 1, documentType);
            //////////////////////////

            //for life assure 2
            foreach (TreeNode node in tvPendingsLifeAssured2.Nodes)
            {
                foreach (TreeNode cnode in node.ChildNodes)
                {
                    cnode.Checked = false;
                }
            }
            PopulatePendingsTreeview(tvPendingsLifeAssured2, 2, documentType);
            //////////////////////////
        }


        btnAlter.Enabled = true;
        //  tvPendingsLifeAssured1.Enabled = false;
    }

    private bool isLifeInsured2Available(string sProposalNo)
    {
        bool returnVal = false;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = "SELECT 	" +
                    " T.LIFE_INSURED_2 " +
                    " FROM MRP_WORKFLOW T " +
                  " WHERE T.PROPOSAL_NO='" + sProposalNo + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            if (dr[0].ToString() == "")
            {
                returnVal = false;
            }
            else
            {
                returnVal = true;
            }
        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return returnVal;
    }


    private void updatePendingCompletedDate()
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            String UserName = Context.User.Identity.Name;
            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }


            string updateString = "";
            updateString = "UPDATE  MRP_WORKFLOW " +
                       " SET " +
                           " PENDING_COM_DATE=SYSDATE, " +
                            " PENDING_COM_USER='" + UserName + "' " +
                       " WHERE PROPOSAL_NO='" + txtProposalNo.Text + "'";

            spProcess = new OracleCommand(updateString);

            spProcess.Connection = conProcess;



            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ee)
        {
            lblMsg.Text = "Error while updating pending completed date.";
            Timer1.Enabled = true;
        }
    }

    private void updatePendingCompletedDateAsLastPendingClearTime()
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            string updateString = "";

            String UserName = Context.User.Identity.Name;
            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }


            if (ddlDocumentType.SelectedValue == "fax")
            {

                updateString = "UPDATE  MRP_WORKFLOW " +
                           " SET " +
                               " PENDING_COM_DATE=(select max(t.sys_date)from mrp_wf_pend_cleared_docs_log t where t.proposal_no='" + txtProposalNo.Text + "' ) " +
                           " , PENDING_COM_USER='" + UserName + "' " +
                               " WHERE PROPOSAL_NO='" + txtProposalNo.Text + "'";
            }
            else
            {

                updateString = "UPDATE  MRP_WORKFLOW " +
                       " SET " +
                           " ORI_PEND_CLEARED_DATE=(select max(t.sys_date)from mrp_wf_pend_cleared_docs_log t where t.proposal_no='" + txtProposalNo.Text + "' ) " +
                       ",ORI_PEND_CLEARED_USER='" + UserName + "' " +
                           " WHERE PROPOSAL_NO='" + txtProposalNo.Text + "'";
            }
            spProcess = new OracleCommand(updateString);

            spProcess.Connection = conProcess;



            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ee)
        {
            lblMsg.Text = "Error while updating pending completed date.";
            Timer1.Enabled = true;
        }
    }
    private void updatePendingOriginalsCompletedDate()
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;

            String UserName = Context.User.Identity.Name;
            if (Left(UserName, 4) == "HNBA")
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            else
            {
                UserName = Right(UserName, (UserName.Length) - 7);
            }

            string updateString = "";
            updateString = "UPDATE  MRP_WORKFLOW " +
                       " SET " +
                           " ORI_PEND_CLEARED_DATE=SYSDATE, " +
                            " ORI_PEND_CLEARED_USER='" + UserName + "' " +
                       " WHERE PROPOSAL_NO='" + txtProposalNo.Text + "'";

            spProcess = new OracleCommand(updateString);

            spProcess.Connection = conProcess;



            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ee)
        {
            lblMsg.Text = "Error while updating pending completed date.";
            Timer1.Enabled = true;
        }
    }
    private void updatePendingLetterSentDate()
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            string updateString = "";
            updateString = "UPDATE  MRP_WORKFLOW " +
                       " SET " +
                       " PENDING_LETTER_SENT_DATE=SYSDATE " +
                       " WHERE PROPOSAL_NO='" + txtProposalNo.Text + "'";

            spProcess = new OracleCommand(updateString);

            spProcess.Connection = conProcess;



            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ee)
        {
            lblMsg.Text = "Error while updating pending completed date.";
            Timer1.Enabled = true;
        }
    }

    private void updatePendingCoverNoteSentDate()
    {
        try
        {
            OracleConnection conProcess = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conProcess.Open();
            OracleCommand spProcess = null;


            string updateString = "";
            updateString = "UPDATE  MRP_WORKFLOW " +
                       " SET " +
                       " PENDING_LETTER_SENT_DATE=SYSDATE " +
                       " WHERE PROPOSAL_NO='" + txtProposalNo.Text + "'";

            spProcess = new OracleCommand(updateString);

            spProcess.Connection = conProcess;



            spProcess.ExecuteNonQuery();
            conProcess.Close();
        }
        catch (Exception ee)
        {
            lblMsg.Text = "Error while updating pending completed date.";
            Timer1.Enabled = true;
        }
    }




    public string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }

    public string Mid(string text, int start, int end)
    {
        return text.Substring(start, end);
    }

    public string Mid(string text, int start)
    {
        return text.Substring(start, text.Length - start);
    }

    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDocumentType.SelectedValue != "0")
        {
            loadDataForProposalNo(ddlDocumentType.SelectedValue);

            if (ddlDocumentType.SelectedValue == "fax")
            {
                btnSendPendingLetter.Visible = true;
                btnSendOriginalsPendingLetter.Visible = false;
            }
            else if (ddlDocumentType.SelectedValue == "original")
            {
                btnSendPendingLetter.Visible = false;
                btnSendOriginalsPendingLetter.Visible = true;
            }
        }
    }



    protected void btnSendEmail_Click(object sender, EventArgs e)
    {

        try
        {

            CustomMRPWFMail mail = new CustomMRPWFMail();
            //  mail.From_address = "mrp.workflow@hnbassurance.com";


            String UserCode = Context.User.Identity.Name;
            if (Left(UserCode, 4) == "HNBA")
            {
                UserCode = Right(UserCode, (UserCode.Length) - 5);
            }
            else
            {
                UserCode = Right(UserCode, (UserCode.Length) - 7);



            }

            mail.From_address = getUserEmailAddress(UserCode);


            //mail.To_address = Get_Email_Addresses("to", txtProposalNo.Text);
            //mail.Cc_address = Get_Email_Addresses("cc", txtProposalNo.Text);
            //mail.Bcc_address = "tharindu.dilanka@hnbassurance.com";

            string bankType = getBankType(txtProposalNo.Text);
            if (bankType == "Other Bank")
            {
                mail.To_address = Get_Email_Addresses_For_OBs("to", txtProposalNo.Text);
                mail.Cc_address = Get_Email_Addresses_For_OBs("cc", txtProposalNo.Text);
                mail.Bcc_address = "tharindu.dilanka@hnbassurance.com";
            }
            else
            {
                mail.To_address = Get_Email_Addresses("to", txtProposalNo.Text);
                mail.Cc_address = Get_Email_Addresses("cc", txtProposalNo.Text);
                mail.Bcc_address = "tharindu.dilanka@hnbassurance.com";
            }



            MRPProposal mRPProposal = new MRPProposal();
            mRPProposal = getProposalDetails(txtProposalNo.Text);

            string mailSubject = "";
            mailSubject = "Pending requirements of ";
            if (mRPProposal.Life2Name != "")
            {
                mailSubject = mailSubject + " " + mRPProposal.Life1Name + "(" + mRPProposal.Life1NIC + "), " + mRPProposal.Life2Name + "(" + mRPProposal.Life2NIC + ") ";
            }
            else
            {
                mailSubject = mailSubject + " " + mRPProposal.Life1Name + "(" + mRPProposal.Life1NIC + ") ";
            }


            mail.Subject = mailSubject;


            String BodyText = "";
            // mail.Attachment = (new Attachment(crystalReport.ExportToStream(ExportFormatType.PortableDocFormat), "MRP Cancellation Letter.pdf"));

            BodyText = BodyText + "<html>" +
                        "<head>" +
                        "<title>Cancellation Letter</title>" +
                        "</head>" +
                        "<body>" +
                        " <p>Dear Sir/Madam,</p> " +
                     " <p><strong><u>Loan Protection Assurance Pending Requirement </u></strong></p> " +

                  " <p><strong>Proposal Number - " + mRPProposal.ProposalNo + "</strong></p> " +
                       " <p><strong>Life Assured 01 - " + mRPProposal.Life1Name + "</strong></p> " +

                       " <p><strong>Life Assured 01 NIC - " + mRPProposal.Life1NIC + "</strong></p> ";

            if (mRPProposal.Life2Name != "")
            {

                BodyText = BodyText + " <p><strong>Life Assured 02 - " + mRPProposal.Life2Name + "</strong></p> " +

                " <p><strong>Life Assured 02 NIC - " + mRPProposal.Life2NIC + "</strong></p> ";
            }


            BodyText = BodyText + " <p><strong>Name of lending institution - " + mRPProposal.BankName + "</strong></p> " +

          " <p><strong>Branch - " + mRPProposal.BankBranch + "</strong></p> " +

            " <p>Thank you for your proposal for loan protection life assurance plan.</p> " +

              " <p>As per the information disclosed in the proposal form, we would appreciate if you could send us the following requirements in order to proceed further.</p> " +
                " <p></p> " +

                 " <p><strong>" + mRPProposal.Life1Name + "</strong></p> " +
                getPendings(mRPProposal.ProposalNo, 1) +
                 " <p></p> ";

            if (mRPProposal.Life2Name != "")
            {


                string pendingsOfLife2 = "";
                pendingsOfLife2 = getPendings(mRPProposal.ProposalNo, 2);

                if (pendingsOfLife2 != "")
                {
                    BodyText = BodyText + " <p><strong>" + mRPProposal.Life2Name + "</strong></p> " +
                 pendingsOfLife2 +
                  " <p></p> ";

                }


            }


            BodyText = BodyText + txtCustomNote.Text.Replace("\n", "<br />") +

          " <p>Please comply with these requirements at your earliest convenience in order for us to provide you a speedy service. </p> " +

   " <p>This is a computer-generated document. Therefore, no signature is required.</p> " +
   " <p>You may contact the MRP department via Email: mrp.job@hnbassurance.com or via our direct number 0114793700 or Fax: 4677904 </p> " +
   " <p></p> " +

   " <p> " +
   "</p>  " +
   " </body> " +
   " </html>";

            mail.attachments = getPendingDocPDFs(mRPProposal.ProposalNo);


            if (fileUploadAdditionalAttachments.HasFile)
            {
                HttpFileCollection uploadedFiles = Request.Files;

                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile postedFile = uploadedFiles[i];
                    mail.Attachments.Add(new Attachment(postedFile.InputStream, postedFile.FileName));

                }



            }


            mail.Body = BodyText;
            mail.sendMail();


            ShowAlert("E-Mail successfully sent");

        }
        catch (Exception ss)
        {
            ShowAlert("Error while sending E-Mail");
        }



    }
    private string Get_Email_Addresses_For_OBs(String sEmailType, String sProposalNo)
    {
        String returnVal = "";
        string mrpManagerEmail = "";
        mrpManagerEmail = System.Configuration.ConfigurationManager.AppSettings["MRPManagerEmail"].ToString();



        String UserCode = Context.User.Identity.Name;
        if (Left(UserCode, 4) == "HNBA")
        {
            UserCode = Right(UserCode, (UserCode.Length) - 5);
        }
        else
        {
            UserCode = Right(UserCode, (UserCode.Length) - 7);
        }



        if (sEmailType == "to")
        {
            returnVal = Get_User_Email_Addresses(sProposalNo).TrimEnd(',') + "," + GetUserEmailAddress(UserCode).TrimEnd(',') + "," + Get_OB_Email_Address(sProposalNo).TrimEnd(',');
        }
        if (sEmailType == "cc")
        {
            string brokerAdrs = "";
            brokerAdrs = Get_Broker_Email_Addresses(sProposalNo).TrimEnd(',');

            if (brokerAdrs != "")
            {
                returnVal = Get_Branch_Email_Address(sProposalNo).TrimEnd(',') + "," + brokerAdrs + "," + txtEmailCcAddresses.Text;

            }
            else
            {
                returnVal = Get_Branch_Email_Address(sProposalNo).TrimEnd(',') + "," + txtEmailCcAddresses.Text;
            }




        }
        returnVal = returnVal.TrimEnd(',');

        return returnVal;
    }

    private string Get_Broker_Email_Addresses(String sProposalNo)
    {
        String returnVal = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand cmd = new OracleCommand();
        OracleDataReader dr;


        try
        {
            con.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;

            cmd.CommandText = " select b.broker_emails from MRP_WF_BROKERS b " +
                " inner join MRP_WF_PROPOSAL_JOBS j on b.broker_code=j.broker_code " +
                " WHERE j.proposal_no='" + sProposalNo + "'";



            dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    returnVal = dr[0].ToString();
                }
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {

        }

        return returnVal;

    }


    private string GetUserEmailAddress(String userCode)
    {
        String returnVal = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand cmd = new OracleCommand();
        OracleDataReader dr;


        try
        {
            con.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;

            cmd.CommandText = "select t.email from MRP_USER_DETAILS t where  t.user_code ='" + userCode + "'";

            dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    returnVal = dr[0].ToString();
                }
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {

        }

        return returnVal;

    }
    private string Get_Branch_Email_Address(string proposalNo)
    {
        String returnVal = "";
        string mrpManagerEmail = "";
        mrpManagerEmail = System.Configuration.ConfigurationManager.AppSettings["MRPManagerEmail"].ToString();

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand cmd = new OracleCommand();
        OracleDataReader dr;


        try
        {
            con.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;

            cmd.CommandText = "select t.hnba_email from mrp_wf_hnba_email t " +
                "   INNER JOIN mrp_workflow m on t.hnb_code=m.as_code  " +
                "where m.proposal_no='" + proposalNo + "'";




            dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (dr.HasRows)
            {
                dr.Read();

                returnVal = dr[0].ToString();

            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {

        }
        returnVal = returnVal.TrimEnd(',');
        return returnVal;

    }

    private string Get_OB_Email_Address(string proposalNo)
    {
        String returnVal = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleCommand cmd = new OracleCommand();
        OracleDataReader dr;


        try
        {
            con.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;

            cmd.CommandText = "select t.hnb_email from mrp_wf_banks_email t " +
                "   INNER JOIN mrp_workflow m on t.bank_code=m.agent_code  " +
               "where m.proposal_no='" + proposalNo + "'";

            dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (dr.HasRows)
            {
                dr.Read();

                returnVal = dr[0].ToString();

            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {

        }
        returnVal = returnVal.TrimEnd(',');
        return returnVal;

    }

    private string getBankType(string proposalNo)
    {

        if (proposalNo == "")
        {
            return "";
        }

        string returnVal = "";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        selectQuery = "SELECT BANK_TYPE FROM MRP_WF_BANKS t " +
            " INNER JOIN mrp_workflow m on t.bank_code=m.agent_code " +
            " WHERE m.proposal_no='" + proposalNo + "' ";


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



    public void ShowAlert(string message)
    {
        // Cleans the message to allow single quotation marks 
        string cleanMessage = message.Replace("'", "\\'");
        string script = "<script type=\"text/javascript\">" +
            "alert('" + message + "');</script>";



        // Gets the executing web page 
        Page page = HttpContext.Current.CurrentHandler as Page;

        // Checks if the handler is a Page and that the script isn't allready on the Page 
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", script);

        }
    }


    private string getUserEmailAddress(string userCode)
    {
        string userName = "";



        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        string MRPUserCodes = System.Configuration.ConfigurationManager.AppSettings["MRPUserCodes"].ToString();


        selectQuery = "   SELECT T.USER_EMAIL FROM WF_ADMIN_USERS T  " +
           " WHERE T.USER_CODE='" + userCode + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            userName = dr[0].ToString();


        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return userName;


    }

    private MRPProposal getProposalDetails(string proposalNo)
    {
        MRPProposal mRPProposal = new MRPProposal();


        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";
        selectQuery = " select t.proposal_no, " +//0
                        " t.life_insured_1, " +//1
                        " t.nic1, " +//2
                        " t.life_insured_2, " +//3
                        " t.nic2, " +//4
                        " case when t.bank='OTHER BANK' then t.other_bank_name else t.bank end as bank, " +//5
                        " t.branch_name " +//6
                        " from mrp_workflow t " +
                        " where t.proposal_no='" + proposalNo + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            mRPProposal.ProposalNo = dr[0].ToString();
            mRPProposal.Life1Name = dr[1].ToString();
            mRPProposal.Life1NIC = dr[2].ToString();
            mRPProposal.Life2Name = dr[3].ToString();
            mRPProposal.Life2NIC = dr[4].ToString();
            mRPProposal.BankName = dr[5].ToString();
            mRPProposal.BankBranch = dr[6].ToString();



        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return mRPProposal;
    }




    private void sendPendingCompleteMail(string sProposalNo, string pendingType)
    {


        MRPWFMail mail = new MRPWFMail();
        mail.From_address = "mrp.workflow@hnbassurance.com";
        mail.To_address = Get_Email_Addresses("assignedUser", sProposalNo);
        mail.Cc_address = Get_Email_Addresses("superUsers", sProposalNo);
        mail.Bcc_address = "tharindu.dilanka@hnbassurance.com";

        //mail.To_address = "anuranga@hnbassurance.com";
        //mail.Cc_address = "tdapower@yahoo.com";
        //mail.Bcc_address = "tdapower@yahoo.com";

        mail.Subject = "MRP Workflow Fax Pending clear (Proposal No :" + sProposalNo + ")";
        String BodyText;


        BodyText = "<html>" +
                    "<head>" +
                    "<title>Pending Letter</title>" +
                    "</head>" +
                    "<body>" +
                    "Proposal No : " + sProposalNo +
                    "<br />" +
                    "Dear User," +
                     "<br />";

        if (pendingType == "fax")
        {
            BodyText = BodyText + " All pending requirements of the above proposal are completed on " + System.DateTime.Today.Date.ToShortDateString() + ". Please issue the cover note. ";
        }
        else if (pendingType == "original")
        {
            BodyText = BodyText + " All pending requirements of the above proposal are completed on " + System.DateTime.Today.Date.ToShortDateString() + ". Please issue the policy. ";
        }


        BodyText = BodyText + "<br />" +
                    "<b>MRP Workflow System </b>" +
                   " </body> " +
                   " </html>";



        try
        {
            mail.Body = BodyText;
            mail.sendMail();


        }
        catch (Exception ee)
        {
            // lblMsg.Text = "Error while Sending Pending Letter.";
            // Timer1.Enabled = true;
        }
    }




    private string getPendings(string sProposalNo, int iLifeAssure)
    {

        string pendings = "<ul>";
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        selectQuery = "SELECT p.pending_doc_name,p.pending_doc_code  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD  " +
            " INNER JOIN MRP_WF_PENDING_DOCS p on PCD.pending_doc_code=p.pending_doc_code " +
            "  WHERE  PCD.PROPOSAL_NO='" + sProposalNo + "' AND PCD.life_assure='" + iLifeAssure + "' AND (PCD.IS_FAX_PENDING=1 OR IS_ORIGINAL_PENDING=1)";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                pendings = pendings + "<li>" + dr[0].ToString() + "</li>";


            }
        }

        pendings = pendings + "</ul>";

        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return pendings;
    }



    private AttachmentCollection getPendingDocPDFs(string sProposalNo)
    {

        MailMessage message = new MailMessage();

        AttachmentCollection attachmentCollection;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";


        selectQuery = "SELECT  p.document,p.document_name  FROM MRP_WF_PENDING_COMPLETE_DOCS PCD  " +
            " INNER JOIN MRP_WF_PENDING_DOCS p on PCD.pending_doc_code=p.pending_doc_code " +
            "  WHERE  PCD.PROPOSAL_NO='" + sProposalNo + "' AND  (PCD.IS_FAX_PENDING=1 OR IS_ORIGINAL_PENDING=1)";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();


        string fileName = "";
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                if (dr[0] is DBNull)
                {

                }
                else
                {
                    byte[] docData = (byte[])dr[0];
                    fileName = dr[1].ToString();

                    MemoryStream pdf = new MemoryStream(docData);
                    Attachment data = new Attachment(pdf, fileName);
                    // attachmentCollection.Add(data);

                    message.Attachments.Add(data);


                }
            }
        }


        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();

        return message.Attachments;
    }


    public class MRPProposal
    {
        public string _ProposalNo;
        public string _Life1Name;
        public string _Life1NIC;
        public string _Life2Name;
        public string _Life2NIC;
        public string _BankName;
        public string _BankBranch;


        public string ProposalNo
        {
            get { return _ProposalNo; }
            set { _ProposalNo = value; }
        }
        public string Life1Name
        {
            get { return _Life1Name; }
            set { _Life1Name = value; }
        }
        public string Life1NIC
        {
            get { return _Life1NIC; }
            set { _Life1NIC = value; }
        }
        public string Life2Name
        {
            get { return _Life2Name; }
            set { _Life2Name = value; }
        }
        public string Life2NIC
        {
            get { return _Life2NIC; }
            set { _Life2NIC = value; }
        }
        public string BankName
        {
            get { return _BankName; }
            set { _BankName = value; }
        }
        public string BankBranch
        {
            get { return _BankBranch; }
            set { _BankBranch = value; }
        }



    }



    public class MRPPendingDocs
    {
        public string _PendingDocCode;

        public string _PendingName;
        public byte[] _PendingDoc;
        public string _PendingDocName;


        public string PendingDocCode
        {
            get { return _PendingDocCode; }
            set { _PendingDocCode = value; }
        }

        public string PendingName
        {
            get { return _PendingName; }
            set { _PendingName = value; }
        }
        public byte[] PendingDoc
        {
            get { return _PendingDoc; }
            set { _PendingDoc = value; }
        }
        public string PendingDocName
        {
            get { return _PendingDocName; }
            set { _PendingDocName = value; }
        }
    }






}


