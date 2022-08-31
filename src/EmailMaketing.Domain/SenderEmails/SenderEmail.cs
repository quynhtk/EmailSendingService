using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EmailMaketing.SenderEmails
{
    public class SenderEmail : AuditedAggregateRoot<Guid>,IMultiTenant
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsSend { get; set; }

        public Guid? TenantId { get; set; }
    }
}
