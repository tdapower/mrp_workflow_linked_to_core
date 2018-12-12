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
using System.Data.SqlClient;
using System.Data.OracleClient;
using Microsoft.VisualBasic;

public partial class MRPDetails : System.Web.UI.Page
{
    SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MRP"].ToString());

    OracleConnection myOraConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
    string meassgae = "";
    string msgresult = "";
    string Table = "";
    string sql = "";
    string Product = "";
    string fullpolicyno = "";
    DateTime FromDate;
    DateTime ToDate;
    double policyfee = 0;
    double Premium = 0;
    string Year = "";
    string PolicyNo = "";
    string CashAccount = "";
    string DCNUMBER = "";
    int DCLEN = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboSearchStatus.Text = "-";
            cboPolicyYear.Text = "2012";
            txtSeaPol_no.Text = "";
            txtSeaProposalNo.Text = "";
            pnlmessage.Visible = false;
        }
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdNewBAC.Rows.Count == 0)
            {
                lblError.Text = "Record not found";
                return;
            }

            if (cboReason.Text == "-")
            {
                lblError.Text = "Reason cannot be blank";
                return;
            }

            if (cboPolicyYear.Text == "-")
            {
                lblError.Text = "Policy Year cannot be blank";
                return;
            }

            Session["SAVEDATE"] = GetServerDate();
            DateTime CurrentDate = Convert.ToDateTime((Session["SAVEDATE"].ToString().Remove(10)));

            foreach (GridViewRow gr in grdNewBAC.Rows)
            {
                CheckBox cb = (CheckBox)gr.Cells[0].FindControl("myCheckBox");
                if (cb.Checked)
                {
                    Session["PolicyNo"]  = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;

                    OracleConnection myPolicy = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());

                    OracleCommand mycomPolicy = new OracleCommand();

                    myPolicy.Open();

                    mycomPolicy.Connection = myPolicy;

                    mycomPolicy.CommandText = "SELECT T.PROPOSAL_NO FROM WF_BAC_PREMIUM_DATA T WHERE T.POLICY_NO = '" + Session["PolicyNo"].ToString() + "'";

                    OracleDataReader mydataPolicy = mycomPolicy.ExecuteReader();
                    if (mydataPolicy.HasRows == true)
                    {
                        meassgae = "You have already posted a payment for policy no (" + Session["PolicyNo"].ToString() + ") .Do you want post another payment for this policy.";
                        lblMessage.Text = meassgae.ToString();
                        pnlmessage.Visible = true;
                        return;
                    }
                     
                    myPolicy.Close();


                    Year = cboPolicyYear.SelectedValue;

                    lblError.Text = "";

                    if (cboReason.SelectedValue == "NEW PROPOSAL")
                    {
                        OracleConnection myOleDb = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
                        OracleCommand myOleDbCommand = new OracleCommand();
                        myOleDb.Open();
                        myOleDbCommand.Connection = myOleDb;
                        myOleDbCommand.CommandText = " SELECT P.PARAM_VALUE FROM WF_PARRAM P " +
                                                       " WHERE P.PARAM_CODE = 'POLICY_FEE_MRP'" +
                                                       " AND P.PARAM_DESCRIP = '" + Year.ToString() + "'";


                        OracleDataReader myOleDbData = myOleDbCommand.ExecuteReader();
                        if (myOleDbData.HasRows == true)
                        {
                            while (myOleDbData.Read())
                            {
                                policyfee = Convert.ToDouble(myOleDbData[0].ToString());
                            }
                        }

                        Premium = Convert.ToDouble(grdNewBAC.Rows[gr.RowIndex].Cells[3].Text);

                        Premium = Premium - policyfee;

                        myOleDb.Close();
                    }
                    else
                    {
                        Premium = Convert.ToDouble(grdNewBAC.Rows[gr.RowIndex].Cells[3].Text);
                        policyfee = 0;
                    }

                    CashAccount = grdNewBAC.Rows[gr.RowIndex].Cells[6].Text;
                    fullpolicyno = "";
                    fullpolicyno = grdNewBAC.Rows[gr.RowIndex].Cells[4].Text + grdNewBAC.Rows[gr.RowIndex].Cells[1].Text + grdNewBAC.Rows[gr.RowIndex].Cells[8].Text;
                    //DCLEN = DCNUMBER.Length;
                    DCNUMBER = Right(CashAccount.ToString(),(CashAccount.Length) - 2);
                    CashAccount = Left(CashAccount, 2);


                    myOraConnection.Open();
                    OracleCommand spinsert = new OracleCommand("INSERT_WF_BAC_PREMIUM_DATA");
                    spinsert.CommandType = CommandType.StoredProcedure;
                    spinsert.Connection = myOraConnection;
                    spinsert.Parameters.Add("V_POLICY_ID", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;
                    spinsert.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;
                    spinsert.Parameters.Add("V_POLICY_NO", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;
                    spinsert.Parameters.Add("V_CUSTOMER_NAME", OracleType.VarChar, 200).Value = grdNewBAC.Rows[gr.RowIndex].Cells[2].Text;
                    spinsert.Parameters.Add("V_CUSTOMER_ADDRESS", OracleType.VarChar, 500).Value = DBNull.Value;
                    spinsert.Parameters.Add("V_PRODUCT_TYPE", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[7].Text;
                    spinsert.Parameters.Add("V_POLICY_TERM", OracleType.VarChar, 50).Value = DBNull.Value;
                    spinsert.Parameters.Add("V_POLICY_MODE", OracleType.VarChar, 50).Value = DBNull.Value;
                    spinsert.Parameters.Add("V_AGENT_CODE", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[4].Text;
                    spinsert.Parameters.Add("V_USER_CODE", OracleType.VarChar, 50).Value = Request.Cookies["USERID"].Value;
                    spinsert.Parameters.Add("V_PROPOSAL_DATE", OracleType.DateTime).Value = CurrentDate.ToString();
                    spinsert.Parameters.Add("V_IBT_DATE", OracleType.DateTime).Value = grdNewBAC.Rows[gr.RowIndex].Cells[5].Text;
                    spinsert.Parameters.Add("V_REASON", OracleType.VarChar, 50).Value = cboReason.SelectedValue;
                    spinsert.Parameters.Add("V_PAID_AMT", OracleType.Number, 15).Value = Premium.ToString();
                    spinsert.Parameters.Add("V_POLICY_FEE", OracleType.Number, 15).Value = policyfee.ToString();
                    spinsert.Parameters.Add("V_CREATED_USER", OracleType.VarChar, 50).Value = Request.Cookies["USERID"].Value;
                    spinsert.Parameters.Add("V_TEMP_1", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[8].Text;
                    spinsert.Parameters.Add("V_TEMP_2", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[7].Text;
                    spinsert.Parameters.Add("V_TEMP_3", OracleType.VarChar, 50).Value = fullpolicyno.ToString().Replace("/", "");
                    spinsert.Parameters.Add("V_TEMP_4", OracleType.VarChar, 50).Value = DCNUMBER.ToString();
                    spinsert.Parameters.Add("V_TEMP_5", OracleType.Number, 15).Value = 0;
                    spinsert.Parameters.Add("V_MAN_PROPOSAL_NO", OracleType.VarChar, 200).Value = "";
                    spinsert.Parameters.Add("V_BANK_CODE", OracleType.VarChar, 50).Value = CashAccount.ToString();
                    spinsert.Parameters.Add("V_MRP_BANK_CODE", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[9].Text;
                    
                    spinsert.ExecuteNonQuery();
                    myOraConnection.Close();
                }
            }

            grdNewBAC.DataSource = "";
            grdNewBAC.DataBind();
            cboPolicyYear.Text = "2012";
            cboReason.Text = "-";
            lblError.Text = "Record Successfully saved.";
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            return;
        }


    }
    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        if ((txtSeaPol_no.Text == "") && (txtSeaProposalNo.Text == "") && (cboSearchStatus.Text == "-"))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        if (cboSearchStatus.Text == "-")
        {
            lblError.Text = "Product cannot be blank";
            return;
        }

        switch (cboSearchStatus.SelectedValue)
        {
            case "MRP":
                Table = "Chequedetail C,BANK BAN ";
                Product = "'MRP' AS PRODUCT,BAN.Branchname,C.BNKCode";
                break;
            case "MCR":
                Table = "ChequedetailMicro C,BANK BAN ";
                Product = "'MCR' AS PRODUCT,BAN.Branchname,C.BNKCode";
                break;
        }

        FromDate = Convert.ToDateTime(txtSeaPol_no.Text.Trim());
        string From_Date1 = FromDate.ToString("MM/dd/yyyy");
        ToDate = Convert.ToDateTime(txtSeaProposalNo.Text.Trim());
        string To_Date1 = ToDate.ToString("MM/dd/yyyy");

        grdNewBAC.DataSource = "";
        grdNewBAC.DataBind();

        SqlCommand cmdSearch = new SqlCommand();

        myConnection.Open();

        SqlDataReader drSearch;
        cmdSearch.CommandType = CommandType.Text;
        cmdSearch.Connection = myConnection;

        sql = "SELECT REPLACE(C.PropNo,'/','') AS PROPOSALNO,UPPER(C.Name) AS CLIENTNAME,C.Premiumpolicyfee AS PREMIUM,UPPER(SUBSTRING(C.Code,1,4)) AS HNBCODE, " +
                " C.Ibtdate AS IBTDATE,UPPER(REPLACE(REPLACE(C.ACCOUNT,' ',''),'-','')) AS CASHACCOUNT," + Product.ToString() +
                " FROM  " +
                Table.ToString() +
                " WHERE " +
                " BAN.BNKCode = C.Code " +
                " AND Convert(datetime,C.ChqDate,103) >= '" + From_Date1.ToString() + "'" +
                " AND Convert(datetime,C.ChqDate,103) <= '" + To_Date1.ToString() + "'" +
                " AND Convert(datetime,C.Ibtdate,103) <> ''" +
                " ORDER BY Convert(datetime,C.ChqDate,103) ASC";

        cmdSearch.CommandText = sql.ToString();
        drSearch = cmdSearch.ExecuteReader();

        if (drSearch.HasRows)
        {
            grdNewBAC.DataSource = drSearch;
            grdNewBAC.DataBind();
            grdNewBAC.Dispose();
        }

        myConnection.Close();

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("MRPIBTConfirm.aspx");
    }

    public string GetServerDate()
    {
        string ServerDate = "";
        OracleConnection connDetalis = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
        OracleCommand cmdDetails = new OracleCommand();
        connDetalis.Open();

        OracleCommand cmdGetDocNo = new OracleCommand();
        OracleDataReader drcmdGetDocNo;

        cmdGetDocNo.CommandType = CommandType.Text;
        cmdGetDocNo.Connection = connDetalis;
        cmdGetDocNo.CommandText = "SELECT TO_DATE(SYSDATE,'dd/mm/RRRR') FROM DUAL";

        drcmdGetDocNo = cmdGetDocNo.ExecuteReader();

        if (drcmdGetDocNo.HasRows)
        {
            while (drcmdGetDocNo.Read())
            {
                ServerDate = drcmdGetDocNo[0].ToString();
            }
        }
        return ServerDate;
    }


    protected void btnmsgYes_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in grdNewBAC.Rows)
        {
            CheckBox cb = (CheckBox)gr.Cells[0].FindControl("myCheckBox");
            if (cb.Checked)
            {
                PolicyNo = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;

                Year = cboPolicyYear.SelectedValue;

                lblError.Text = "";

                if (cboReason.SelectedValue == "NEW PROPOSAL")
                {
                    Session["PolicyNo"] = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;


                    OracleConnection myOleDb = new OracleConnection(ConfigurationManager.ConnectionStrings["ORACONN"].ToString());
                    OracleCommand myOleDbCommand = new OracleCommand();
                    myOleDb.Open();
                    myOleDbCommand.Connection = myOleDb;
                    myOleDbCommand.CommandText = " SELECT P.PARAM_VALUE FROM WF_PARRAM P " +
                                                   " WHERE P.PARAM_CODE = 'POLICY_FEE_MRP'" +
                                                   " AND P.PARAM_DESCRIP = '" + Year.ToString() + "'";


                    OracleDataReader myOleDbData = myOleDbCommand.ExecuteReader();
                    if (myOleDbData.HasRows == true)
                    {
                        while (myOleDbData.Read())
                        {
                            policyfee = Convert.ToDouble(myOleDbData[0].ToString());
                        }
                    }

                    Premium = Convert.ToDouble(grdNewBAC.Rows[gr.RowIndex].Cells[3].Text);

                    Premium = Premium - policyfee;

                    myOleDb.Close();
                }
                else
                {
                    Premium = Convert.ToDouble(grdNewBAC.Rows[gr.RowIndex].Cells[3].Text);
                    policyfee = 0;
                }
                DateTime CurrentDate = Convert.ToDateTime((Session["SAVEDATE"].ToString().Remove(10)));
                CashAccount = grdNewBAC.Rows[gr.RowIndex].Cells[6].Text;
                fullpolicyno = "";
                fullpolicyno = grdNewBAC.Rows[gr.RowIndex].Cells[4].Text + grdNewBAC.Rows[gr.RowIndex].Cells[1].Text + grdNewBAC.Rows[gr.RowIndex].Cells[8].Text;
                DCNUMBER = Right(CashAccount.ToString(), (CashAccount.Length) - 2);
                CashAccount = Left(CashAccount, 2);

                myOraConnection.Open();
                OracleCommand spinsert = new OracleCommand("INSERT_WF_BAC_PREMIUM_DATA");
                spinsert.CommandType = CommandType.StoredProcedure;
                spinsert.Connection = myOraConnection;
                spinsert.Parameters.Add("V_POLICY_ID", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;
                spinsert.Parameters.Add("V_PROPOSAL_NO", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;
                spinsert.Parameters.Add("V_POLICY_NO", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[1].Text;
                spinsert.Parameters.Add("V_CUSTOMER_NAME", OracleType.VarChar, 200).Value = grdNewBAC.Rows[gr.RowIndex].Cells[2].Text;
                spinsert.Parameters.Add("V_CUSTOMER_ADDRESS", OracleType.VarChar, 500).Value = DBNull.Value;
                spinsert.Parameters.Add("V_PRODUCT_TYPE", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[7].Text;
                spinsert.Parameters.Add("V_POLICY_TERM", OracleType.VarChar, 50).Value = DBNull.Value;
                spinsert.Parameters.Add("V_POLICY_MODE", OracleType.VarChar, 50).Value = DBNull.Value;
                spinsert.Parameters.Add("V_AGENT_CODE", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[4].Text;
                spinsert.Parameters.Add("V_USER_CODE", OracleType.VarChar, 50).Value = Request.Cookies["USERID"].Value;
                spinsert.Parameters.Add("V_PROPOSAL_DATE", OracleType.DateTime).Value = CurrentDate.ToString();
                spinsert.Parameters.Add("V_IBT_DATE", OracleType.DateTime).Value = grdNewBAC.Rows[gr.RowIndex].Cells[5].Text;
                spinsert.Parameters.Add("V_REASON", OracleType.VarChar, 50).Value = cboReason.SelectedValue;
                spinsert.Parameters.Add("V_PAID_AMT", OracleType.Number, 15).Value = Premium.ToString();
                spinsert.Parameters.Add("V_POLICY_FEE", OracleType.Number, 15).Value = policyfee.ToString();
                spinsert.Parameters.Add("V_CREATED_USER", OracleType.VarChar, 50).Value = Request.Cookies["USERID"].Value;
                spinsert.Parameters.Add("V_TEMP_1", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[8].Text;
                spinsert.Parameters.Add("V_TEMP_2", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[7].Text;
                spinsert.Parameters.Add("V_TEMP_3", OracleType.VarChar, 50).Value = fullpolicyno.ToString().Replace("/", "");
                spinsert.Parameters.Add("V_TEMP_4", OracleType.VarChar, 50).Value = DCNUMBER.ToString();
                spinsert.Parameters.Add("V_TEMP_5", OracleType.Number, 15).Value = 0;
                spinsert.Parameters.Add("V_MAN_PROPOSAL_NO", OracleType.VarChar, 200).Value = "";
                spinsert.Parameters.Add("V_BANK_CODE", OracleType.VarChar, 50).Value = CashAccount.ToString();
                spinsert.Parameters.Add("V_MRP_BANK_CODE", OracleType.VarChar, 50).Value = grdNewBAC.Rows[gr.RowIndex].Cells[9].Text;

                spinsert.ExecuteNonQuery();
                myOraConnection.Close();
            }
        }
        grdNewBAC.DataSource = "";
        grdNewBAC.DataBind();
        cboPolicyYear.Text = "2012";
        cboReason.Text = "-";
        lblError.Text = "Record Successfully saved.";
        pnlmessage.Visible = false;
    }
    protected void btnmsgNo_Click(object sender, EventArgs e)
    {
        meassgae = "You have already posted a payment for policy no (" + Session["PolicyNo"].ToString() + ") .Do you want post another payment for this policy.";
        lblError.Text = meassgae.ToString();
        pnlmessage.Visible = false;
        grdNewBAC.DataSource = "";
        grdNewBAC.DataBind();
    }

    public static string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public static string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }

    public static string Mid(string text, int start, int end)
    {
        return text.Substring(start, end);
    }

    public static string Mid(string text, int start)
    {
        return text.Substring(start, text.Length - start);
    }

    protected void grdNewBAC_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grdNewBAC_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
}

