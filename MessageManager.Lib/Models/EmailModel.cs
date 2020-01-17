using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManager.Lib.Models
{
    public class EmailModel
    {
        public EmailModel()
        {
            AttachmentFiles = new List<EmailAttachmentModel>();
        }
        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsHtmlContent { get; set; }

        public string RecipientEmails { get; set; }

        public string Ccs { get; set; }

        public string Bccs { get; set; }

        public string SenderEmail { get; set; }

        public string SenderDisplayName { get; set; }

        public List<EmailAttachmentModel> AttachmentFiles { get; set; }
    }

    public class EmailAttachmentModel
    {
        public string FileName { get; set; }

        public byte AttachmentData { get; set; }


    }
}
