using log4net.Appender;
using System;
using System.Net.Mail;
using GATShipD365TS.App_Code;

namespace GATShipD365TS.App_Code
{
    public class ExtendedSmtpAppender : SmtpAppender
    {
        public bool IsBodyHtml { get; set; }
        public string m_replyTo { get; set; }
        public static bool isHighPriority { get; set; }

        #region SendMail Override

        protected override void SendEmail(string messageBody)
        {
            // .NET 2.0 has a new API for SMTP email System.Net.Mail
            // This API supports credentials and multiple hosts correctly.
            // The old API is deprecated.
            // Create and configure the smtp client

            //try
            //{

            //    SmtpClient smtpClient = new SmtpClient();
            //    if (!String.IsNullOrEmpty(SmtpHost))
            //    {
            //        smtpClient.Host = SmtpHost;
            //    }
            //    smtpClient.Port = Port;
            //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtpClient.EnableSsl = EnableSsl;

            //    if (Authentication == SmtpAuthentication.Basic)
            //    {
            //        // Perform basic authentication
            //        smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
            //    }
            //    else if (Authentication == SmtpAuthentication.Ntlm)
            //    {
            //        // Perform integrated authentication (NTLM)
            //        smtpClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            //    }

            //    using (MailMessage mailMessage = new MailMessage())
            //    {
            //        mailMessage.IsBodyHtml = IsBodyHtml;
            //        var payload = messageBody.Contains(":::") ? messageBody.Split(new string[] { ":::" }, StringSplitOptions.None)[1] : messageBody;
            //        var payloadData = payload.Split(new string[] { "\r\n<br> <br>" }, StringSplitOptions.None)[0];
            //        LogManager.Log.Info("MessageBody: " + messageBody);
            //        mailMessage.Body = messageBody.Contains(":::") ? messageBody.Split(new string[] { ":::" }, StringSplitOptions.None)[0] + "<br>"+ payload.Split(new string[] { "\r\n<br> <br>" }, StringSplitOptions.None)[1].Replace("\r\n<br> <br>","<br>") + " <br><br>" + payloadData + " <br><br>" + Config.footerError : messageBody;
            //        //mailMessage.BodyEncoding = BodyEncoding;
            //        mailMessage.From = new MailAddress(From);
            //        LogManager.Log.Info("Recipient - To.." + To);
            //        mailMessage.To.Add(To);
            //        LogManager.Log.Info("mailMessage - To.." + mailMessage.To.ToString());
            //        if (!String.IsNullOrEmpty(Cc))
            //        {
            //            mailMessage.CC.Add(Cc);
            //        }
            //        if (!String.IsNullOrEmpty(Bcc))
            //        {
            //            mailMessage.Bcc.Add(Bcc);
            //        }
            //        if (!String.IsNullOrEmpty(ReplyTo))
            //        {
            //            // .NET 4.0 warning CS0618: 'System.Net.Mail.MailMessage.ReplyTo' is obsolete:
            //            // 'ReplyTo is obsoleted for this type.  Please use ReplyToList instead which can accept multiple addresses. http://go.microsoft.com/fwlink/?linkid=14202'
            //            mailMessage.ReplyToList.Add(new MailAddress(m_replyTo));
            //        }

            //        LogManager.Log.Info("Verifying nomination number...");
            //        if (messageBody.IndexOf("Nomination Number:") > 1)
            //        {
            //            var nominationNumber = messageBody.Substring(messageBody.IndexOf("Nomination Number:")).Split(new string[] { "<br>" }, StringSplitOptions.None)[0].Split(':')[1];
            //            var entity = messageBody.Substring(messageBody.IndexOf("Entity:")).Split(new string[] { "<br>" }, StringSplitOptions.None)[0].Split(':')[1];
            //            var action = messageBody.Substring(messageBody.IndexOf("Action:")).Split(new string[] { "<br>" }, StringSplitOptions.None)[0].Split(':')[1];
            //            LogManager.Log.Info("Formatting email subject...");
            //            mailMessage.Subject = Subject + (" (" + nominationNumber + "-" + entity + "-" + action + ")").Replace("<b>","").Replace("</b>","");
            //            LogManager.Log.Info("Subject: " + mailMessage.Subject);
            //        }
            //        else
            //        {
            //            mailMessage.Subject = Subject;
            //        }

            //        //mailMessage.SubjectEncoding = m_subjectEncoding;

            //        if (isHighPriority)
            //        {
            //            mailMessage.Priority = MailPriority.High;
            //        }
            //        else
            //        {
            //            mailMessage.Priority = Priority;
            //        }

            //        // TODO: Consider using SendAsync to send the errorReferences without blocking. This would be a change in
            //        // behaviour compared to .NET 1.x. We would need a SendCompletedCallback to log errors.
            //        smtpClient.Send(mailMessage);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogManager.Log.Info("SmtpAppender Error: " + ex.Message + " " + ex.InnerException);
            //}
        }

        #endregion SendMail Override
    }
}
