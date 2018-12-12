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

public partial class CommonControls_ConfirmDialog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //lblMessage.Text = Session["ConfirmDialogLabelText"].ToString();
        //btn1.Text = Session["ConfirmDialogResultBtn1Text"].ToString();
        //btn2.Text = Session["ConfirmDialogResultBtn2Text"].ToString();
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        Session["ConfirmDialogResult"] = "Button1";
    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        Session["ConfirmDialogResult"] = "Button2";
    }
}
