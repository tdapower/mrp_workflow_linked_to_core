using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for Mail
/// </summary>
public class Mail
{
    private string from_address;

    public string From_address
    {
        get { return from_address; }
        set { from_address = value; }
    }
    private string to_address;

    public string To_address
    {
        get { return to_address; }
        set { to_address = value; }
    }
    private string to_address2;

    public string To_address2
    {
        get { return to_address2; }
        set { to_address2 = value; }
    }
    private string to_address_sender;

    public string To_address_sender
    {
        get { return to_address_sender; }
        set { to_address_sender = value; }
    }

    private string display_name;

    public string Display_name
    {
        get { return display_name; }
        set { display_name = value; }
    }
    private string subject;
 
    public string Subject
    {
        get { return subject; }
        set { subject = value; }
    }
    private string body;

    public string Body
    {
        get { return body; }
        set { body = value; }
    }
    private Attachment attachment;

    public Attachment Attachment
    {
        get { return attachment; }
        set { attachment = value; }
    }
    private Attachment attachment1;

    public Attachment Attachment1
    {
        get { return attachment1; }
        set { attachment1 = value; }
    }
	public Mail()
	{
		//
		// TODO: Add constructor logic here
		//
        
	}

    public void sendMail()
    {
        try
        {
            //The From address (Email ID)
            string str_from_address = From_address; 

            //The Display Name
            string str_name = Display_name;

            //The To address (Email ID)
            string str_to_address = To_address;

            string str_to_address_sender =  To_address_sender;

            //Create MailMessage Object
            MailMessage email_msg = new MailMessage();

            //Specifying From,Sender & Reply to address
            email_msg.From = new MailAddress(str_from_address, str_name);
            email_msg.Sender = new MailAddress(str_from_address, str_name);
            email_msg.ReplyTo = new MailAddress(str_from_address, str_name);
            
            
            //The To Email id
            email_msg.To.Add(str_to_address);
            email_msg.To.Add(str_to_address_sender);
            email_msg.To.Add("suranga.kulawardana@hnbassurance.com");

            email_msg.Subject = Subject;//Subject of email

            email_msg.Body = @Body;
            
            email_msg.IsBodyHtml = true;
            email_msg.Attachments.Add(Attachment);
            email_msg.Attachments.Add(Attachment1);
                //Create an object for SmtpClient class
            SmtpClient mail_client = new SmtpClient();

            //Providing Credentials (Username & password)
            //NetworkCredential network_cdr = new NetworkCredential();
            //network_cdr.UserName = str_from_address;
            //network_cdr.Password = "xxxxx";

            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("crc", "HNBA@customer");
            mail_client.UseDefaultCredentials = false;
            //client.Credentials = SMTPUserInfo;

            mail_client.Credentials = SMTPUserInfo;

            //Specify the SMTP Port
            //mail_client.Port = 587;

            //Specify the name/IP address of Host
            mail_client.Host = "smtp2.hnbassurance.com";

            //Uses Secure Sockets Layer(SSL) to encrypt the connection
            //mail_client.EnableSsl = true;

            //Now Send the message
            mail_client.Send(email_msg);

            //MessageBox.Show("Email Sent Successfully");
        }
        catch (Exception ex)
        {
            //Some error occured
            //MessageBox.Show(ex.Message.ToString());
        }
    }

    public void sendMail1()
    {
        try
        {
            //The From address (Email ID)
            string str_from_address = From_address;

            //The Display Name
            string str_name = Display_name;

            //The To address (Email ID)
            string str_to_address = To_address;

            string str_to_address_sender = To_address_sender; 
            //Create MailMessage Object
            MailMessage email_msg = new MailMessage();

            //Specifying From,Sender & Reply to address
            email_msg.From = new MailAddress(str_from_address, str_name);
            email_msg.Sender = new MailAddress(str_from_address, str_name);
            email_msg.ReplyTo = new MailAddress(str_from_address, str_name);

            //The To Email id
            email_msg.To.Add(str_to_address);
            email_msg.To.Add(str_to_address_sender);
            email_msg.To.Add("suranga.kulawardana@hnbassurance.com");

            email_msg.Subject = Subject;//Subject of email

            email_msg.Body = @Body;

            email_msg.IsBodyHtml = true;
            email_msg.Attachments.Add(Attachment);
            //email_msg.Attachments.Add(Attachment1);
            //Create an object for SmtpClient class
            SmtpClient mail_client = new SmtpClient();

            //Providing Credentials (Username & password)
            //NetworkCredential network_cdr = new NetworkCredential();
            //network_cdr.UserName = str_from_address;
            //network_cdr.Password = "xxxxx";

            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("crc", "HNBA@customer");
            mail_client.UseDefaultCredentials = false;
            //client.Credentials = SMTPUserInfo;

            mail_client.Credentials = SMTPUserInfo;

            //Specify the SMTP Port
            //mail_client.Port = 587;

            //Specify the name/IP address of Host
            mail_client.Host = "smtp2.hnbassurance.com";

            //Uses Secure Sockets Layer(SSL) to encrypt the connection
            //mail_client.EnableSsl = true;

            //Now Send the message
            mail_client.Send(email_msg);

            //MessageBox.Show("Email Sent Successfully");
        }
        catch (Exception ex)
        {
            //Some error occured
            //MessageBox.Show(ex.Message.ToString());
        }
    }

    public void sendMailFinal()
    {
        try
        {
            //The From address (Email ID)
            string str_from_address = From_address;

            //The Display Name
            string str_name = Display_name;

            //The To address (Email ID)
            string str_to_address = To_address;
            string str_to_address2 = To_address2;

            string str_to_address_sender = To_address_sender;
            //Create MailMessage Object
            MailMessage email_msg = new MailMessage();

            //Specifying From,Sender & Reply to address
            email_msg.From = new MailAddress(str_from_address, str_name);
            email_msg.Sender = new MailAddress(str_from_address, str_name);
            email_msg.ReplyTo = new MailAddress(str_from_address, str_name);

            //The To Email id
            email_msg.To.Add(str_to_address);
            email_msg.To.Add(str_to_address2);
            email_msg.To.Add(str_to_address_sender);
            email_msg.To.Add("suranga.kulawardana@hnbassurance.com");

            email_msg.Subject = Subject;//Subject of email

            email_msg.Body = @Body;

            email_msg.IsBodyHtml = true;
            email_msg.Attachments.Add(Attachment);
            //email_msg.Attachments.Add(Attachment1);
            //Create an object for SmtpClient class
            SmtpClient mail_client = new SmtpClient();

            //Providing Credentials (Username & password)
            //NetworkCredential network_cdr = new NetworkCredential();
            //network_cdr.UserName = str_from_address;
            //network_cdr.Password = "xxxxx";

            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("crc", "HNBA@customer");
            mail_client.UseDefaultCredentials = false;
            //client.Credentials = SMTPUserInfo;

            mail_client.Credentials = SMTPUserInfo;

            //Specify the SMTP Port
            //mail_client.Port = 587;

            //Specify the name/IP address of Host
            mail_client.Host = "smtp2.hnbassurance.com";

            //Uses Secure Sockets Layer(SSL) to encrypt the connection
            //mail_client.EnableSsl = true;

            //Now Send the message
            mail_client.Send(email_msg);

            //MessageBox.Show("Email Sent Successfully");
        }
        catch (Exception ex)
        {
            //Some error occured
            //MessageBox.Show(ex.Message.ToString());
        }
    }

    public void sendMail_Info()
    {
        try
        {
            //The From address (Email ID)
            string str_from_address = From_address;

            //The Display Name
            string str_name = Display_name;

            //The To address (Email ID)
            string str_to_address = To_address;

            string str_to_address_sender = To_address_sender;
            //Create MailMessage Object
            MailMessage email_msg = new MailMessage();

            //Specifying From,Sender & Reply to address
            email_msg.From = new MailAddress(str_from_address, str_name);
            email_msg.Sender = new MailAddress(str_from_address, str_name);
            email_msg.ReplyTo = new MailAddress(str_from_address, str_name);

            //The To Email id
            email_msg.To.Add(str_to_address);
            email_msg.To.Add(str_to_address_sender);
            email_msg.To.Add("suranga.kulawardana@hnbassurance.com");

            email_msg.Subject = Subject;//Subject of email

            email_msg.Body = @Body;

            email_msg.IsBodyHtml = true;
            //email_msg.Attachments.Add(Attachment);
            //email_msg.Attachments.Add(Attachment1);
            //Create an object for SmtpClient class
            SmtpClient mail_client = new SmtpClient();

            //Providing Credentials (Username & password)
            //NetworkCredential network_cdr = new NetworkCredential();
            //network_cdr.UserName = str_from_address;
            //network_cdr.Password = "xxxxx";

            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("crc", "HNBA@customer");
            mail_client.UseDefaultCredentials = false;
            //client.Credentials = SMTPUserInfo;

            mail_client.Credentials = SMTPUserInfo;

            //Specify the SMTP Port
            //mail_client.Port = 587;

            //Specify the name/IP address of Host
            mail_client.Host = "smtp2.hnbassurance.com";

            //Uses Secure Sockets Layer(SSL) to encrypt the connection
            //mail_client.EnableSsl = true;

            //Now Send the message
            mail_client.Send(email_msg);

            //MessageBox.Show("Email Sent Successfully");
        }
        catch (Exception ex)
        {
            //Some error occured
            //MessageBox.Show(ex.Message.ToString());
        }
    }

    public void sendMail(string _From_address, string _To_address, string _Subject, string _Body)
    {
        try
        {
            //The From address (Email ID)
            string str_from_address = _From_address;

            //The Display Name
            string str_name = _Subject;

            //The To address (Email ID)
            string str_to_address = _To_address;

            //Create MailMessage Object
            MailMessage email_msg = new MailMessage();

            //Specifying From,Sender & Reply to address
            email_msg.From = new MailAddress(str_from_address, str_name);
            email_msg.Sender = new MailAddress(str_from_address, str_name);
            email_msg.ReplyTo = new MailAddress(str_from_address, str_name);


            //The To Email id
            email_msg.To.Add(str_to_address);
            email_msg.To.Add("suranga.kulawardana@hnbassurance.com");

            email_msg.Subject = _Subject;//Subject of email

            email_msg.Body = @_Body;

            email_msg.IsBodyHtml = true;

            //Create an object for SmtpClient class
            SmtpClient mail_client = new SmtpClient();

            //Providing Credentials (Username & password)
            //NetworkCredential network_cdr = new NetworkCredential();
            //network_cdr.UserName = str_from_address;
            //network_cdr.Password = "xxxxx";

            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("crc", "HNBA@customer");
            mail_client.UseDefaultCredentials = false;
            //client.Credentials = SMTPUserInfo;

            mail_client.Credentials = SMTPUserInfo;

            //Specify the SMTP Port
            //mail_client.Port = 587;

            //Specify the name/IP address of Host
            mail_client.Host = "smtp2.hnbassurance.com";

            //Uses Secure Sockets Layer(SSL) to encrypt the connection
            //mail_client.EnableSsl = true;

            //Now Send the message
            mail_client.Send(email_msg);

            //MessageBox.Show("Email Sent Successfully");
        }
        catch (Exception ex)
        {
            //Some error occured
            //MessageBox.Show(ex.Message.ToString());
        }
    }
}
