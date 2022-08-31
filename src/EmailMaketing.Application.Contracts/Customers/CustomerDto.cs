using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.Customers
{
    public class CustomerDto : AuditedEntityDto<Guid>
    {
        public int Stt { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
    }
}
