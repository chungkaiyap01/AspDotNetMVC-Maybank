using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;

namespace Maybank.Web
{
    public class GlobalVariable
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalVariable()
        {
            WebApiClient.BaseAddress = new Uri("http://localhost:49962/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static bool PreventIntruder(object CustomerID)
        {
            try
            {
                if (string.IsNullOrEmpty(CustomerID.ToString()))
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static void SendEmail(string RecipientEmail, string Subject, string Content)
        {
            //Configuring webMail class to send emails

            //gmail smtp server
            WebMail.SmtpServer = "smtp-mail.outlook.com";

            //gmail port to send emails
            WebMail.SmtpPort = 587;

            WebMail.SmtpUseDefaultCredentials = true;

            //sending emails with secure protocol
            WebMail.EnableSsl = true;

            //Email ID used to send emails from application
            WebMail.UserName = "";
            WebMail.Password = "";

            //Sender email address
            WebMail.From = "";

            //Send email 
            WebMail.Send(to: RecipientEmail, subject: Subject, body: Content, isBodyHtml: true);
        }

    }
}