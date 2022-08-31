using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
namespace EmailMaketing.EmailSchedules
{
    public class EmailScheduleAppService : ApplicationService, IEmailScheduleAppService
    {
        private readonly IEmailScheduleRepository _emailScheduleRepository;

        public EmailScheduleAppService(IEmailScheduleRepository emailScheduleRepository )
        {
            _emailScheduleRepository = emailScheduleRepository;
        }

        public async Task<bool> ChangeStatusSendEmailAsync(Guid id)
        {
            var emailSchedule = await _emailScheduleRepository.GetAsync(id);
            emailSchedule.isSend = true;
            await _emailScheduleRepository.UpdateAsync(emailSchedule);
            return true;
        }

        public async Task<EmailScheduleDto> CreateAsync(CreateUpdateEmailSchedule input)
        {
            var emailSchedule = ObjectMapper.Map<CreateUpdateEmailSchedule, EmailSchedule>(input);
            await _emailScheduleRepository.InsertAsync(emailSchedule);
            return ObjectMapper.Map<EmailSchedule, EmailScheduleDto>(emailSchedule);
        }

        public async Task<EmailScheduleDto> GetEmailScheduleAsync(Guid id)
        {
            var emailSchedule = await _emailScheduleRepository.FindAsync(id);
            return ObjectMapper.Map<EmailSchedule, EmailScheduleDto>(emailSchedule);
        }

        public async Task<EmailScheduleDto> GetNewEmailScheduleAsync()
        {
            var emailSchedules = await _emailScheduleRepository.GetListAsync();
            var count = emailSchedules.Count - 1;
            var emailSchedule = emailSchedules[count];
            return ObjectMapper.Map<EmailSchedule, EmailScheduleDto>(emailSchedule);
        }
    }
}
