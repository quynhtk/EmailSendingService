using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.ContentEmails
{
    public class ContentEmailDto : AuditedEntityDto<Guid>
    {
        public int Stt { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachment { get; set; }
        public bool Status { get; set; }
        public bool Featured { get; set; }
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Guid? EmailScheduleID { get; set; }
        public DateTime? Schedule { get; set; }
        public Guid? SenderEmailID { get; set; }
        public string SenderEmail { get; set; }
        public String[] BodyShow { get; set; }
        public string StatusSend { get; set; }
    }
}
