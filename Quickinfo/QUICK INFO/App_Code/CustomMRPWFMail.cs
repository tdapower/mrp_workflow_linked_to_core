using System;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for Mail
/// </summary>
public class CustomMRPWFMail
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

    private string cc_address;

    public string Cc_address
    {
        get { return cc_address; }
        set { cc_address = value; }
    }

    private string bcc_address;

    public string Bcc_address
    {
        get { return bcc_address; }
        set { bcc_address = value; }
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

    public AttachmentCollection attachments;
    public AttachmentCollection Attachments
    {
        get { return attachments; }
        set { attachments = value; }
    }






    public CustomMRPWFMail()
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

            //The CC address (Email ID)
            string str_cc_address = Cc_address;

            //The BCC address (Email ID)
            string str_bcc_address = Bcc_address;



            //Create MailMessage Object
            MailMessage email_msg = new MailMessage();

            //Specifying From,Sender & Reply to address
            email_msg.From = new MailAddress(str_from_address, str_name);
            email_msg.Sender = new MailAddress(str_from_address, str_name);
            email_msg.ReplyTo = new MailAddress(str_from_address, str_name);


            //The To Email id
            email_msg.To.Add(str_to_address);

            if (str_cc_address != null && str_cc_address != "")
            {
                email_msg.CC.Add(str_cc_address);
            }

            if (str_bcc_address != null && str_bcc_address != "")
            {
                email_msg.Bcc.Add(str_bcc_address);
            }


            email_msg.Subject = Subject;//Subject of email

            email_msg.Body = @Body;

            email_msg.IsBodyHtml = true;

            if (Attachment != null)
            {
                email_msg.Attachments.Add(Attachment);
            }

            foreach (Attachment a in attachments)
            {
                email_msg.Attachments.Add(a);
            }


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
