//******************************************
// Author            : Tharindu Athapattu
// Date              : 30/03/2018
// Reviewed By       :
// Description       : BlacklistNIC Form
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
using Telerik.Web.UI;

public partial class BlacklistNIC : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            validatePageAuthentication();


            string InterVal = System.Configuration.ConfigurationManager.AppSettings["MessageClearAfter"].ToString();
            Timer1.Interval = Convert.ToInt32(InterVal);

            ClearComponents();
            initializeValues();

            Session.Remove("SetupMode");

            pnlSearchResultGrid.Visible = false;
        }


    }
    private void initializeValues()
    {
        lblError.Text = "";
        lblMsg.Text = "";


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


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchData();
        ClearComponents();
    }

    private void SearchData()
    {
        string SQL = "";
        lblError.Text = "";
        grdSearchResult.DataSource = null;
        grdSearchResult.DataBind();

        if ((txtSearchNIC.Text == ""))
        {
            lblError.Text = "Search text cannot be blank";
            return;
        }

        OracleConnection myOleDbConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());

        OracleCommand myOleDbCommand = new OracleCommand();

        myOleDbConnection.Open();

        myOleDbCommand.Connection = myOleDbConnection;


        if (txtSearchNIC.Text != "")
        {

            SQL = SQL + "(LOWER(T.NIC_NO) LIKE '%" + txtSearchNIC.Text.ToLower() + "%') AND";
        }

        SQL = SQL.Substring(0, SQL.Length - 3);




        String selectQuery = "";
        selectQuery = "   SELECT " +
                    " T.SEQ_ID,T.NIC_NO AS \"NIC\", T.REMARKS AS \"Remarks\"  " +
                    " FROM MRP_WF_BLACKLISTED_NIC T  " +
                      " WHERE (" + SQL + ") ORDER BY T.NIC_NO ASC";



        myOleDbCommand.CommandText = selectQuery;

        OracleDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
        if (myOleDbDataReader.HasRows == true)
        {
            DataTable dbTable = new DataTable();
            grdSearchResult.DataSource = myOleDbDataReader;
            grdSearchResult.DataBind();

            pnlSearchResultGrid.Visible = true;
        }
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("BlacklistNIC.aspx");
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearComponents();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtNIC.Text.Trim() == "")
        {
            lblMsg.Text = "Please Enter NIC";
            Timer1.Enabled = true;
            return;
        }



        if (Session["SetupMode"].ToString() == "NEW")
        {
            if (!validateDuplicates(txtNIC.Text))
            {
                lblMsg.Text = "Entered NIC already available";
                Timer1.Enabled = true;
                return;
            }
        }
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

            if (Session["SetupMode"].ToString() == "NEW")
            {
                spProcess = new OracleCommand("INSERT_MRP_WF_BLACKLISTED_NIC");
                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;
                spProcess.Parameters.Add("V_NIC_NO", OracleType.VarChar).Value = txtNIC.Text;

                if (chkIsBlacklisted.Checked == true)
                {
                    spProcess.Parameters.Add("V_IS_BLACKLISTED", OracleType.Number).Value = 1;
                }
                else
                {
                    spProcess.Parameters.Add("V_IS_BLACKLISTED", OracleType.Number).Value = 0;
                }
                spProcess.Parameters.Add("V_REMARKS", OracleType.VarChar).Value = txtRemarks.Text;

                spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = UserName;



            }
            else if (Session["SetupMode"].ToString() == "UPDATE")
            {
                spProcess = new OracleCommand("UPDATE_MRP_WF_BLACKLISTED_NIC");
                spProcess.CommandType = CommandType.StoredProcedure;
                spProcess.Connection = conProcess;

                spProcess.Parameters.Add("V_SEQ_ID", OracleType.VarChar).Value = txtSeqId.Text;
                spProcess.Parameters.Add("V_NIC_NO", OracleType.VarChar).Value = txtNIC.Text;


                if (chkIsBlacklisted.Checked == true)
                {
                    spProcess.Parameters.Add("V_IS_BLACKLISTED", OracleType.Number).Value = 1;
                }
                else
                {
                    spProcess.Parameters.Add("V_IS_BLACKLISTED", OracleType.Number).Value = 0;
                }
                spProcess.Parameters.Add("V_REMARKS", OracleType.VarChar).Value = txtRemarks.Text;

                spProcess.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = UserName;



            }





            spProcess.ExecuteNonQuery();
            conProcess.Close();

            ClearComponents();
            SearchData();
            lblMsg.Text = "Successfully saved";
            Timer1.Enabled = true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error while saving";
            Timer1.Enabled = true;
        }

    }

    private void ClearComponents()
    {
        txtSeqId.Enabled = false;
        txtNIC.Enabled = false;
        txtRemarks.Enabled = false;
        chkIsBlacklisted.Enabled = false;

        txtSeqId.Text = "";
        txtNIC.Text = "";
        txtRemarks.Text = "";
        chkIsBlacklisted.Checked = false;

        btnAddNew.Enabled = true;
        btnAlter.Enabled = false;
        btnSave.Enabled = false;

    }




    protected void btnAddNew_Click(object sender, EventArgs e)
    {

        txtNIC.Enabled = true;
        txtRemarks.Enabled = true;
        chkIsBlacklisted.Enabled = true;

        txtSeqId.Text = "";
        txtNIC.Text = "";
        txtRemarks.Text = "";
        chkIsBlacklisted.Checked = false;

        btnSave.Enabled = true;

        Session["SetupMode"] = "NEW";
    }


    protected void btnAlter_Click(object sender, EventArgs e)
    {
        if (txtSeqId.Text == "")
        {
            lblMsg.Text = "Please Select A NIC to alter";
            return;
        }

        txtNIC.Enabled = false;
        txtRemarks.Enabled = true;
        chkIsBlacklisted.Enabled = true;

        btnSave.Enabled = true;

        Session["SetupMode"] = "UPDATE";
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadDetails(grdSearchResult.SelectedRow.Cells[1].Text.Trim());


        btnAlter.Enabled = true;
    }

    private string loadDetails(string id)
    {

        string returnValue = "";

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;
        String selectQuery = "";

        selectQuery = "   SELECT  T.SEQ_ID ,T.NIC_NO, T.IS_BLACKLISTED,T.REMARKS FROM MRP_WF_BLACKLISTED_NIC T  " +
                      " WHERE T.SEQ_ID='" + id + "'";

        cmd.CommandText = selectQuery;

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();

            txtSeqId.Text = id;
            txtNIC.Text = dr[1].ToString();
            txtRemarks.Text = dr[3].ToString();

            if (dr[2].ToString() == "1")
            {
                chkIsBlacklisted.Checked = true;
            }
            else
            {
                chkIsBlacklisted.Checked = false;

            }
           

        }
        dr.Close();
        dr.Dispose();
        cmd.Dispose();
        con.Close();
        con.Dispose();


        return returnValue;
    }



    protected void grdSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
          e.Row.Cells[1].Visible = false;
    }

    private bool validateDuplicates(string id)
    {
        bool returnVal = true;
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
        OracleDataReader dr;

        con.Open();

        OracleCommand cmd = new OracleCommand();
        cmd.Connection = con;


        String selectQuery = "";
        selectQuery = "SELECT T.NIC_NO FROM MRP_WF_BLACKLISTED_NIC T WHERE T.NIC_NO='" + id + "'";


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

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        Timer1.Enabled = false;
    }

    public string Left(string text, int length)
    {
        return text.Substring(0, length);
    }

    public string Right(string text, int length)
    {
        return text.Substring(text.Length - length, length);
    }


}
