using MessageManager.Lib.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageManager.Lib.Services
{
    public interface ISendGridEmailService
    {
        void ProcessEmail(EmailModel emailModel);
    }
    public class SendGridEmailService : ISendGridEmailService
    {
        private readonly IConfiguration _configuration;
        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ProcessEmail(EmailModel emailModel)
        {
            try
            {
                var mailMessage = new SendGridMessage()
                {
                    From = new EmailAddress(emailModel.SenderEmail, emailModel.SenderDisplayName),
                    Subject = emailModel.Subject,
                    PlainTextContent = emailModel.Body,
                    HtmlContent = emailModel.Body
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
        private void SendMail(SendGridMessage mailMessage)
        {

            var apiKey = _configuration["SendGrid_ApiKey"];
            var client = new SendGridClient(apiKey);

            var response = client.SendEmailAsync(mailMessage).Result;
        }
        private void ProcessReceivers(EmailModel emailModel, SendGridMessage mailMessage)
        {
            //msg.AddTo(new EmailAddress("test@example.com", "Test User"));

            if (!string.IsNullOrEmpty(emailModel.RecipientEmails))
            {
                emailModel.RecipientEmails.Split(';', ',').ToList()
                .ForEach(email => mailMessage.AddTo(new EmailAddress(email.Trim())));

            }
            if (!string.IsNullOrEmpty(emailModel.Ccs))
            {
                emailModel.Ccs.Split(';', ',').ToList()
                .ForEach(email => mailMessage.AddCc(new EmailAddress(email.Trim())));
            }

            if (!string.IsNullOrEmpty(emailModel.Bccs))
            {
                emailModel.Bccs.Split(';', ',').ToList()
              .ForEach(email => mailMessage.AddBcc(new EmailAddress(email.Trim())));
            }

        }

        private void ProcessAttachments(EmailModel emailModel, SendGridMessage mailMessage)
        {

        }
    }
}
