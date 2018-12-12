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
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlClient;

public class sms
{

    SqlConnection connDetalis = new SqlConnection(ConfigurationManager.ConnectionStrings["ONTHESPOT"].ToString());

	public string sendsms(string NUM, string MSG)
	{
        NUM = NUM.Replace("-", "");
        if (NUM.Length == 10)
        {
            NUM = NUM.Substring(1, 9);
            NUM = "94" + NUM;
        }
        if (NUM.Length == 9)
        {
            //NUM = NUM.Substring(1, 9);
            NUM = "94" + NUM;
        }

        //string url = "https://groupsms.etisalat.lk/sendsmsmultimask.php?USER=hnb&PWD=hnb@https&MASK=HNBA&NUM=" + NUM.Replace("-", "").Replace("/", "") + "&MSG= " + MSG + " ";
        string url = "http://220.247.223.51:8081/sendsms?username=hnb_sms&password=sfa$658&from=HNBA&to=" + NUM.Replace("-", "").Replace("/", "") + "&msg= " + MSG + "&msg_ref_num=14";
        //string url = "https://203.189.191.209/sendsms.php?USER=hnb&PWD=hnb@https&NUM=" + NUM.Replace("-", "").Replace("/", "") + "&MSG= " + MSG + " ";

        HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(string.Format(url));

        webReq.Method = "GET";

        WebProxy myProxy = new WebProxy("http://192.168.140.140:8080", true);
        myProxy.Credentials = new NetworkCredential("spadmin", "Titan#7891", "HNBA");
        WebRequest.DefaultWebProxy = myProxy;

        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

        HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse();

        Stream answer = webResponse.GetResponseStream();

        return "x";

	}

}
