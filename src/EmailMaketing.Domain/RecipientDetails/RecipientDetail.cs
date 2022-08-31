using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EmailMaketing.RecipientDetails
{
    public class RecipientDetail : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string RecipientEmail { get; set; }
        public bool Status { get; set; }
        public Guid ContentEmailID { get; set; }
        public Guid SenderEmailID { get; set; }

    }
}
