using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EmailMaketing.EmailSchedules
{
    public class EmailSchedule : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public DateTime Schedule { get; set; }
        public bool isSend { get; set; }
    }
}
