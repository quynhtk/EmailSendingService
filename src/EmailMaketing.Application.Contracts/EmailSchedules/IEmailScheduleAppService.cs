using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EmailMaketing.EmailSchedules
{
    public interface IEmailScheduleAppService : IApplicationService
    {
        Task<EmailScheduleDto> GetEmailScheduleAsync(Guid id);
        Task<EmailScheduleDto> CreateAsync(CreateUpdateEmailSchedule input);
        Task<EmailScheduleDto> GetNewEmailScheduleAsync();
        Task<bool> ChangeStatusSendEmailAsync(Guid id);
    }
}
