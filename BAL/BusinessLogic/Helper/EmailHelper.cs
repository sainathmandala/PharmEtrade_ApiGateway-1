using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BAL.BusinessLogic.Interface;
using Google.Protobuf;
using System.Diagnostics;

namespace BAL.BusinessLogic.Helper
{
    public class EmailHelper : IEmailHelper
    {
        private string _senderEmail = "";
        private string _Password = "";
        private string _hostName = "";
        private int _portNumber = 0;
        private string _defaultEmail = "";

        private readonly IConfiguration _configuration;
        public EmailHelper(IConfiguration configuration) { 
            _configuration = configuration;
            _senderEmail = configuration?.GetSection("EmailSettings")["SenderEmail"] ?? "";
            _defaultEmail = configuration?.GetSection("EmailSettings")["DefaultEmail"] ?? "";
            _Password = configuration?.GetSection("EmailSettings")["Password"] ?? "";
            _hostName = configuration?.GetSection("EmailSettings")["Host"] ?? "";
            _portNumber = string.IsNullOrEmpty(configuration?.GetSection("EmailSettings")["Port"] ?? "") ? 0 : Convert.ToInt32(configuration?.GetSection("EmailSettings")["Port"] ?? "");
        }

        public async Task SendEmail(string toMailAddress, string ccMailAddress, string mailSubject, string mailBody)
        {
            try
            {                
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                
                string emailAddress = _senderEmail;
                string password = _Password;
                message.From = new MailAddress(emailAddress);
                message.To.Add(toMailAddress);
                if (!string.IsNullOrEmpty(ccMailAddress))
                    message.CC.Add(ccMailAddress);
                else
                    message.CC.Add(_defaultEmail);
                message.Subject = mailSubject;
                message.IsBodyHtml = true; //to make message body as html
                message.Body = mailBody;
                smtp.Port = _portNumber;
                smtp.Host = _hostName;
                smtp.Credentials = new NetworkCredential(emailAddress, password); //new NetworkCredential("email address", "password");//
                smtp.EnableSsl = true;
                await Task.Run(() => smtp.Send(message));
                
                //await SendMailWithoutPassword();
            }
            catch (Exception)
            {

            }
        }
        public async Task SendMailWithoutPassword()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient();
            mail.To.Add("surendrababuvemuri@gmail.com");
            mail.From = new MailAddress("help@pharmetrade.com");
            mail.Subject = "Test without password";
            mail.IsBodyHtml = true;
            mail.Body = "Bingo";
            SmtpServer.Host = "smtpserver";
            SmtpServer.Port = 25;
            SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            try
            {
                await Task.Run(() => SmtpServer.Send(mail));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                if (ex.InnerException != null)
                    Debug.WriteLine("Exception Inner:   " + ex.InnerException);
            }
        }
        public async Task SendEmail(string toMailAddress, string ccMailAddress, string mailSubject, string mailBody, MemoryStream attachementStream)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                string emailAddress = _senderEmail;
                string password = _Password;
                message.From = new MailAddress(emailAddress);
                message.To.Add(toMailAddress);
                if (!string.IsNullOrEmpty(ccMailAddress))
                    message.CC.Add(ccMailAddress);
                else
                    message.CC.Add(_defaultEmail);
                message.Subject = mailSubject;
                message.Attachments.Add(new Attachment(attachementStream, "Invoice.pdf"));
                message.IsBodyHtml = true; //to make message body as html
                message.Body = mailBody;
                smtp.Port = _portNumber;
                smtp.Host = _hostName;
                smtp.Credentials = new NetworkCredential(emailAddress, password); //new NetworkCredential("email address", "password");//
                smtp.EnableSsl = true;
                await Task.Run(() => smtp.Send(message));
            }
            catch (Exception)
            {

            }
        }
    }
}
