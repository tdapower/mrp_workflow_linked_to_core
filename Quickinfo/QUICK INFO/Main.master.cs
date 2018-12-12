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
using System.Data.SqlClient;
using System.Text;

public partial class Main : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SqlConnection connMOI1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmdMOI1 = new SqlCommand();
            SqlDataReader drMOI1;

            connMOI1.Open();
            cmdMOI1.CommandType = CommandType.Text;
            cmdMOI1.Connection = connMOI1;
            cmdMOI1.CommandText = "SELECT NUMBER FROM TEL_CAP";
            drMOI1 = cmdMOI1.ExecuteReader();

            Session["MOI_NO"] = "";

            if (drMOI1.HasRows)
            {

            }

            cmdMOI1.Dispose();
            connMOI1.Close();
            connMOI1.Dispose();


            libClaim.Enabled = true;
            libClaimReports.Enabled = true;
            lnkClaimFinance.Enabled = true;
            lnkClaimGeneral.Enabled = true;

            if ((Session["USER_CAT"].ToString() == "CC_USER")||(Session["USER_CAT"].ToString() == "CC_MGR"))
            {
                LinkRequestList.Enabled = true;
            }
            else
            {
                LinkRequestList.Enabled = false;
            }

            if (Session["CLAIM_GENERATE"].ToString() != "YES")
            {
                libClaim.Enabled = false;
                libClaimReports.Enabled = false;
                lnkClaimFinance.Enabled = false;
                lnkClaimGeneral.Enabled = false;
            }
            else
            {

                if (Session["USER_TYPE"].ToString() == "ACC")
                {
                    lnkClaimFinance.Enabled = true;
                    lnkClaimGeneral.Enabled = false;
                    libClaim.Enabled = false;
                    libClaimReports.Enabled = true;
                }
                else
                {
                    lnkClaimFinance.Enabled = false;
                    lnkClaimGeneral.Enabled = true;
                    libClaim.Enabled = true;
                    libClaimReports.Enabled = true;
                }
            }


            if ((Session["USER_CAT"].ToString() != "IT_ADMIN") && (Session["USER_CAT"].ToString() != "IT_ASSIST"))
            {
                linkUpload.Enabled = false;
                linkUser.Enabled = false;
            }
            else
            {
                if ((Session["USER_CAT"].ToString() == "IT_ADMIN"))
                {
                    linkUpload.Enabled = true;
                    linkUser.Enabled = true;
                }
                else
                {
                    linkUpload.Enabled = false;
                    linkUser.Enabled = true;
                }
            }
            if ((Session["USER_CAT"].ToString() != "CC_USER") && (Session["USER_CAT"].ToString() != "CL_USER"))
            {
                linkReport.Enabled = false;
            }
            if (!IsPostBack)
            {
                switch (Convert.ToInt16(Session["LINK"]))
                {
                    case 1:
                        {
                            linkMotor.BackColor = Color.Gray;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;

                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;


                            break;
                        }
                    case 2:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Gray;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 3:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Gray;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;

                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 4:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Gray;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;


                            break;
                        }
                    case 5:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Gray;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 6:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Gray;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 7:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Gray;
                            linkDetails.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 8:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;
                            linkReport.BackColor = Color.Gray;
                            linkReport.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 9:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Gray;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 10:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Gray;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 11:
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Gray;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 12://CLAIM-GENERAL
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;

                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Gray;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;
                            
                            break;
                        }
                    case 13://CLAIM-FINANCE
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;

                            lnkClaimGeneral.BackColor = Color.Gray;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Gray;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 14://Request List
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;

                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;

                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;

                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;

                            LinkRequestList.BackColor = Color.Gray;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }

                    case 15://Photo Search
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;
                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;

                            LinkPhoto.BackColor = Color.Gray;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }
                    case 16://Photo Search
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;
                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;

                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Gray;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }

                    case 17://Photo Search
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;
                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;

                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Gray;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Navy;
                            linkTel.ForeColor = Color.White;

                            break;
                        }

                    case 18://TEL_VIEW
                        {
                            linkMotor.BackColor = Color.Navy;
                            linkMotor.ForeColor = Color.White;
                            linkAssessor.BackColor = Color.Navy;
                            linkAssessor.ForeColor = Color.White;
                            linkLife.BackColor = Color.Navy;
                            linkLife.ForeColor = Color.White;
                            linkUpload.BackColor = Color.Navy;
                            linkUpload.ForeColor = Color.White;
                            linkUser.BackColor = Color.Navy;
                            linkUser.ForeColor = Color.White;
                            lnkChgPass.BackColor = Color.Navy;
                            lnkChgPass.ForeColor = Color.White;
                            linkDetails.BackColor = Color.Navy;
                            linkDetails.ForeColor = Color.White;
                            linkReport.BackColor = Color.Navy;
                            linkReport.ForeColor = Color.White;
                            libLifeData.BackColor = Color.Navy;
                            libLifeData.ForeColor = Color.White;

                            libClaim.BackColor = Color.Navy;
                            libClaim.ForeColor = Color.White;
                            libClaimReports.BackColor = Color.Navy;
                            libClaimReports.ForeColor = Color.White;
                            lnkClaimGeneral.BackColor = Color.Navy;
                            lnkClaimGeneral.ForeColor = Color.White;
                            lnkClaimFinance.BackColor = Color.Navy;
                            lnkClaimFinance.ForeColor = Color.White;
                            LinkRequestList.BackColor = Color.Navy;
                            LinkRequestList.ForeColor = Color.White;
                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;

                            LinkPhoto.BackColor = Color.Navy;
                            LinkPhoto.ForeColor = Color.White;
                            linkNM_EMAIL.BackColor = Color.Navy;
                            linkNM_EMAIL.ForeColor = Color.White;
                            linkME.BackColor = Color.Navy;
                            linkME.ForeColor = Color.White;
                            linkTel.BackColor = Color.Gray;
                            linkTel.ForeColor = Color.White;


                            break;
                        }

                     default:
                        linkMotor.BackColor = Color.Gray;
                        linkMotor.ForeColor = Color.White;
                        linkLife.BackColor = Color.Navy;
                        linkLife.ForeColor = Color.White;
                        linkAssessor.BackColor = Color.Navy;
                        linkAssessor.ForeColor = Color.White;
                        linkUpload.BackColor = Color.Navy;
                        linkUpload.ForeColor = Color.White;
                        linkUser.BackColor = Color.Navy;
                        linkUser.ForeColor = Color.White;
                        lnkChgPass.BackColor = Color.Navy;
                        lnkChgPass.ForeColor = Color.White;
                        linkDetails.BackColor = Color.Navy;
                        linkDetails.ForeColor = Color.White;
                        linkReport.BackColor = Color.Navy;
                        linkReport.ForeColor = Color.White;
                        libLifeData.BackColor = Color.Navy;
                        libLifeData.ForeColor = Color.White;

                        libClaim.BackColor = Color.Navy;
                        libClaim.ForeColor = Color.White;

                        libClaimReports.BackColor = Color.Navy;
                        libClaimReports.ForeColor = Color.White;

                        lnkClaimGeneral.BackColor = Color.Navy;
                        lnkClaimGeneral.ForeColor = Color.White;
                        lnkClaimFinance.BackColor = Color.Navy;
                        lnkClaimFinance.ForeColor = Color.White;
                        LinkRequestList.BackColor = Color.Navy;
                        LinkRequestList.ForeColor = Color.White;
                        LinkPhoto.BackColor = Color.Navy;
                        LinkPhoto.ForeColor = Color.White;
                        linkNM_EMAIL.BackColor = Color.Navy;
                        linkNM_EMAIL.ForeColor = Color.White;
                        linkME.BackColor = Color.Navy;
                        linkME.ForeColor = Color.White;
                        linkTel.BackColor = Color.Navy;
                        linkTel.ForeColor = Color.White;


                        break;
                }
            }
        }
        catch (Exception)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
            //throw;
        }
    }
    protected void linkMotor_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 1;
        Response.Redirect("Motor.aspx");   
    }
    protected void linkAssessor_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 2;
        Response.Redirect("Assessors.aspx");
    }
    protected void linkUpload_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 3;
        Response.Redirect("dataUpload.aspx");
    }
    protected void linkLife_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 4;
        Response.Redirect("LifeData.aspx");
    }
    protected void linkUser_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 5;
        Response.Redirect("NewUser.aspx");
    }
    protected void linkLogout_Click(object sender, EventArgs e)
    {
        SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
        SqlCommand cmd_Update = new SqlCommand();
        conn_Log.Open();
        cmd_Update.CommandType = CommandType.StoredProcedure;
        cmd_Update.Connection = conn_Log;
        cmd_Update.CommandText = "SET_USER_STAT";
        cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
        cmd_Update.Parameters.AddWithValue("@FLAG", 2);
        cmd_Update.ExecuteScalar();
        cmd_Update.Dispose();
        conn_Log.Close();
        conn_Log.Dispose();
        Response.Cookies.Remove("USERID");
        
        Session["USER_ID"] = null;
        Session["LINK"] = null;
        Session["USER_CAT"] = null;
        Session["E_MAIL"] = null;
        Session["FLAG"] = null;
        Session["POLICY_NO"] = null;
        Session["VEHICLE_NO"] = null;
        Session["NAME_OF_INS"] = null;
        Session["FROM_DATE"] = null;
        Session["TO_DATE"] = null;
        Session["MAKE"] = null;
        Session["MODEL"] = null;
        Session["CONTACT_NO"] = null;
        Session["ACTION_TAKE"] = null;
        Session["ASSESSOR_NAME"] = null;
        Session["PALECE_OF_ACCIDENT"] = null;
        Session["DESCRIPTION"] = null;
        Session["COMMENT"] = null;
        Session["DATE_OF_ACCIDENT"] = null;
        Session["E_MAIL_FLAG"] = null;
        Session["CALL_DATE"] = null;
        Session["CALL_TIME"] = null;
        Session["INT_DATE"] = null;
        Session["INT_TIME"] = null;
        Session["COMP_DATE"] = null;
        Session["COMP_TIME"] = null;
        Session["SAVE_FLAG"] = null;
        Session["AGENT_CODE"] = null;
        Session["SelectPolicyID"] = null;
        Session["SelectAddress"] = null;
        Session["SelectBranchCode"] = null;
        Session["SelectInsuredName"] = null;
        Session["SelectPolicyNo"] = null;
        Response.Redirect("UserLogin.aspx");
    }
    protected void lnkChgPass_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();

            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 6;
        Response.Redirect("ChangePassword.aspx");
    }
    protected void linkDetails_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 7;
        Response.Redirect("Details.aspx");
    }

    protected void linkReport_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 8;
        Response.Redirect("Report_1.aspx");
    }
    protected void libLifeData_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
        {
            SqlConnection conn_Log = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
            SqlCommand cmd_Update = new SqlCommand();
            conn_Log.Open();
            cmd_Update.CommandType = CommandType.StoredProcedure;
            cmd_Update.Connection = conn_Log;
            cmd_Update.CommandText = "SET_USER_STAT";
            cmd_Update.Parameters.AddWithValue("@USER_ID", Request.Cookies["USERID"].Value);
            cmd_Update.Parameters.AddWithValue("@FLAG", 2);
            cmd_Update.ExecuteScalar();
            cmd_Update.Dispose();
            conn_Log.Close();
            conn_Log.Dispose();
            Response.Redirect("UserLogin.aspx");
        }
        Session["LINK"] = 9;
        Response.Redirect("LifeDetails.aspx");

    }
    protected void libClaim_Click(object sender, EventArgs e)
    {

        Session["LINK"] = 10;
        Response.Redirect("Claims.aspx");
    }
    protected void libClaimReports_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 11;
        Response.Redirect("ClaimReport.aspx");
    }

    protected void lnkClaimFinance_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 12;
        Response.Redirect("ClaimFinance.aspx");
    }
    protected void lnkClaimGeneral_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 13;
        Response.Redirect("ClaimGeneral.aspx");
    }
    protected void LinkRequestList_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 14;
        Response.Redirect("RequestList.aspx");
    }
    protected void LinkPhoto_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 15;
        //Response.Redirect("http://mobilepic.hnba.int/hnb/view_img.php");
        StringBuilder sb = new StringBuilder();
        sb.Append("<script>");
        sb.Append("window.open('http://quickinfo.hnbassurance.com', '','left=50, top=50, height=600, width= 900, status=no, resizable= yes, scrollbars= yes, toolbar= no,location= no, menubar= no');");
        sb.Append("</script>");
        Page.RegisterStartupScript("test", sb.ToString());

    }
    protected void linkNM_EMAIL_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 16;
        Response.Redirect("NM_Email.aspx");
    }
    protected void LinkME_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 17;
        Response.Redirect("ME_SMS_NEW.aspx");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
        //Session["CONTACT_NO"] = GridView1.SelectedRow.Cells[3].Text.Trim();


        SqlConnection connMOI1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());
        SqlCommand cmdMOI1 = new SqlCommand();
        SqlDataReader drMOI1;

        connMOI1.Open();
        cmdMOI1.CommandType = CommandType.Text;
        cmdMOI1.Connection = connMOI1;
        cmdMOI1.CommandText = "UPDATE TEL_CAP SET STATUS = 'NO' WHERE ID = '" + Session["CONTACT_NO"].ToString().Trim() + "' ";
        drMOI1 = cmdMOI1.ExecuteReader();

        cmdMOI1.Dispose();
        connMOI1.Close();
        connMOI1.Dispose();

        //Response.Redirect("Motor.aspx");

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("TEL_VIEW.aspx");
    }
    protected void LinkTEL_Click(object sender, EventArgs e)
    {
        Session["LINK"] = 18;
        Response.Redirect("TEL_MONITER.aspx");
    }
    protected void btnTel_Click(object sender, ImageClickEventArgs e)
    {
        Session["LINK"] = 18;
        Response.Redirect("TEL_MONITER.aspx");
    }
    protected void btnMotor_Click(object sender, ImageClickEventArgs e)
    {
        Session["LINK"] = 1;
        Response.Redirect("Motor.aspx");  
    }
    protected void btnLife_Click(object sender, ImageClickEventArgs e)
    {
        Session["LINK"] = 4;
        Response.Redirect("LifeData.aspx");
    }
}
