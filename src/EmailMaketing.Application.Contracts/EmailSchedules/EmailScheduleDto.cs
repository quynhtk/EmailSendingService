using System;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.EmailSchedules
{
    public class EmailScheduleDto : EntityDto<Guid>
    {
        public DateTime Schedule { get; set; }
        public bool isSend { get; set; }
    }
}
