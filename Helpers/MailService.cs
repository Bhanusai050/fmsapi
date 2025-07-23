using System;
using System.Net;
using System.Net.Mail;

namespace FmsAPI.Helpers
{
    public static class MailService
    {
        private static readonly string senderEmail = "zvenkatabhanusai2001@gmail.com"; 
        private static readonly string senderName = "Farm Management System";
        private static readonly string appPassword = "akrleltvevrpchnc";   

        public static void SendWelcomeEmail(string recipientEmail, string recipientName)
        {
            string subject = "Welcome to Farm Management System";
            string body = $"<h2>Hello {recipientName},</h2><p>Thank you for registering with us!</p><p>We’re happy to have you onboard.</p>";

            SendEmail(recipientEmail, subject, body);
        }

        public static void SendContactAcknowledgementEmail(string recipientEmail, string recipientName, string messageContent)
        {
            string subject = "Thanks for contacting Farm Management System";
            string body = $@"
        <h2>Hello {recipientName},</h2>
        <p>We have received your message:</p>
        <blockquote>{messageContent}</blockquote>
        <p>Our team will respond to you shortly.</p>
        <br/>
        <p>Regards,<br/>Farm Management System Team</p>";

            SendEmail(recipientEmail, subject, body);
        }


        public static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress(senderEmail, senderName);
                var toAddress = new MailAddress(toEmail);

                using (var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(senderEmail, appPassword)
                })
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }

    }
}
