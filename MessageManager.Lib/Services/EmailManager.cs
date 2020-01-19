using MessageManager.Lib.Models;
using MessageManager.Lib.Utilities;
using Microsoft.Extensions.Configuration;
using System;


namespace MessageManager.Lib.Services
{
    public interface IEmailManager
    {
        void SendPasswordResetEmail(string userName, string userEmail, string callBackUrl);
        void SendAccountConfirmationEmail(string userName, string userEmail, string confirmEmailCallBackUrl);
        void SendAccountConfirmationEmail(string userName, string userEmail, string confirmEmailCallBackUrl, string completeProfileCallBackUrl);
    }
    public class EmailManager : IEmailManager
    {
        private readonly ISmtpEmailService _smtpEmailService;
        private readonly ISendGridEmailService _sendGridEmailService;
        private readonly IEmailHelper _emailHelper;
        private readonly IConfiguration _configuration;
        public EmailManager(IEmailHelper emailHelper, IConfiguration configuration, ISmtpEmailService smtpEmailService,
               ISendGridEmailService sendGridEmailService)
        {
            _emailHelper = emailHelper;
            _configuration = configuration;
            _smtpEmailService = smtpEmailService;
            _sendGridEmailService = sendGridEmailService;
        }
        public void SendAccountConfirmationEmail(string receiverName, string receiverEmail, string callBackUrl)
        {
            var emailBody = _emailHelper.GetEmailTemplate("EmailConfirmation.html");

            var logoUrl = _emailHelper.GetCompanyLogoUrl("course-1.jpg");


            emailBody = emailBody.Replace("{receiverName}", receiverName) //replacing the required things

           .Replace("{companyLogo}", logoUrl)

           .Replace("[verificationUrl]", callBackUrl)

           .Replace("{receiverName}", receiverName);

            var emailModel = new EmailModel
            {
                Body = emailBody,
                RecipientEmails = receiverEmail,
                SenderDisplayName = _configuration["DisplayName"],
                SenderEmail = _configuration["SenderEmail"],
                Subject = "Account Confirmation",
                AttachmentFiles = null,
                Bccs = "",
                Ccs = "",
                IsHtmlContent = true
            };


            ValidateAndProcessEmail(emailModel);
        }

        public void SendAccountConfirmationEmail(string receiverName, string receiverEmail, string confirmEmailCallBackUrl, string completeProfileCallBackUrl)
        {
            var emailBody = _emailHelper.GetEmailTemplate("ClientEmailConfirmation.html");

            var logoUrl = _emailHelper.GetCompanyLogoUrl("course-1.jpg");


            emailBody = emailBody.Replace("{receiverName}", receiverName) //replacing the required things

           .Replace("{companyLogo}", logoUrl)

           .Replace("{verificationUrl}", confirmEmailCallBackUrl)

            .Replace("{completeMyProfileUrl}", completeProfileCallBackUrl)

           .Replace("{receiverName}", receiverName);

            var emailModel = new EmailModel
            {
                Body = emailBody,
                RecipientEmails = receiverEmail,
                SenderDisplayName = _configuration["DisplayName"],
                SenderEmail = _configuration["SenderEmail"],
                Subject = "Account Confirmation",
                AttachmentFiles = null,
                Bccs = "",
                Ccs = "",
                IsHtmlContent = true
            };


            ValidateAndProcessEmail(emailModel);
        }
        public void SendPasswordResetEmail(string receiverName, string receiverEmail, string callBackUrl)
        {
            var emailBody = _emailHelper.GetEmailTemplate("PasswordResetEmail.html");

            var logoUrl = _emailHelper.GetCompanyLogoUrl("course-1.jpg");

            emailBody = emailBody.Replace("{receiverName}", receiverName) //replacing the required things

           .Replace("{companyLogo}", logoUrl)

           .Replace("{link}", callBackUrl)

           .Replace("{receiverName}", receiverName);

            var emailModel = new EmailModel
            {
                Body = emailBody,
                RecipientEmails = receiverEmail,
                SenderDisplayName = _configuration["DisplayName"],
                SenderEmail = _configuration["SenderEmail"],
                Subject = "Reset Password",
                AttachmentFiles = null,
                Bccs = "",
                Ccs = "",
                IsHtmlContent = true
            };


            ValidateAndProcessEmail(emailModel);
        }
        private void ValidateAndProcessEmail(EmailModel emailModel)
        {
            if (string.IsNullOrEmpty(emailModel.RecipientEmails))
                throw new Exception("Recipient email address is required!");

            if (string.IsNullOrEmpty(emailModel.Subject))
                throw new Exception("Subject is required!");

            if (string.IsNullOrEmpty(emailModel.SenderDisplayName))
                throw new Exception("Display name is required!");

            if (string.IsNullOrEmpty(emailModel.Body))
                throw new Exception("Email body is required!");

            if (string.IsNullOrEmpty(emailModel.SenderEmail))
                throw new Exception("Sender email address is required!");

            _smtpEmailService.ProcessEmail(emailModel);
            // _sendGridEmailService.ProcessEmail(emailModel);
        }

    }
}
