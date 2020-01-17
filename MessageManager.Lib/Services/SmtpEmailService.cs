using MessageManager.Lib.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace MessageManager.Lib.Services
{
    public interface ISmtpEmailService
    {
        void ProcessEmail(EmailModel emailModel);
    }
    public class SmtpEmailService : ISmtpEmailService
    {
        private readonly IConfiguration _configuration;
        public SmtpEmailService(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public void ProcessEmail(EmailModel emailModel)
        {
            try
            {

                var mailMessage = new MailMessage
                {
                    Body = emailModel.Body,
                    From = new MailAddress(emailModel.SenderEmail, emailModel.SenderDisplayName),
                    Subject = emailModel.Subject,
                    IsBodyHtml = emailModel.IsHtmlContent,
                    Priority = MailPriority.High
                };

                //mailMessage.ReplyToList.Add("");
                ProcessReceivers(emailModel, mailMessage);
                ProcessAttachments(emailModel, mailMessage);

                SendMail(mailMessage);
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void SendMail(MailMessage mailMessage)
        {
            var smtpClientName = _configuration["SMTP_Client"];
            var userName = _configuration["SMTP_UserName"];
            var password = _configuration["SMTP_Password"];
            var port = _configuration["SMTP_Port"];

            var smtpClient = new SmtpClient(smtpClientName)
            {
                Credentials = new System.Net.NetworkCredential(userName, password),
                EnableSsl = true,
                Port = Int32.Parse(port)
            };

            smtpClient.Send(mailMessage);
        }
        private void ProcessReceivers(EmailModel emailModel, MailMessage mailMessage)
        {

            if (!string.IsNullOrEmpty(emailModel.RecipientEmails))
            {
                emailModel.RecipientEmails.Split(';', ',').ToList()
                .ForEach(email => mailMessage.To.Add(new MailAddress(email.Trim())));

            }
            if (!string.IsNullOrEmpty(emailModel.Ccs))
            {
                emailModel.Ccs.Split(';', ',').ToList()
                .ForEach(email => mailMessage.CC.Add(new MailAddress(email.Trim())));

            }
            if (!string.IsNullOrEmpty(emailModel.Bccs))
            {
                emailModel.Bccs.Split(';', ',').ToList()
              .ForEach(email => mailMessage.Bcc.Add(new MailAddress(email.Trim())));
            }

        }

        private void ProcessAttachments(EmailModel emailModel, MailMessage mailMessage)
        {

        }
    }
}
