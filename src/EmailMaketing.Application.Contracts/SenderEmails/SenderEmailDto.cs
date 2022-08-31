using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.SenderEmails
{
    public class SenderEmailDto : AuditedEntityDto<Guid>
    {
        public int Stt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsSend { get; set; }
    }
}
