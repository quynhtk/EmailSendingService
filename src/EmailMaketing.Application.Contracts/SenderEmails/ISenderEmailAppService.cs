using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EmailMaketing.SenderEmails
{
    public interface ISenderEmailAppService : IApplicationService
    {
        Task<List<SenderEmailDto>> GetListSenderAsync();
        Task<PagedResultDto<SenderEmailDto>> GetListAsync(GetSenderEmailInput input);
        Task<SenderEmailDto> GetSenderEmailAsync(Guid Id);
        Task<SenderEmailDto> CreateAsync(CreateUpdateSenderEmailDto input);
        Task<SenderEmailDto> UpdateAsync(Guid id, CreateUpdateSenderEmailDto input);       
        Task<bool> DeleteAsync(Guid id);
        Task<bool> CheckEmailExist(string email);
        Task<SenderEmailDto> SenderIsSendFalseAsync(Guid cusotmerId, string role);
        Task<bool> ChangeIsSendToFalseAsync(Guid cusotmerId, string role);

        Task<SenderEmailDto> SenderIsSendFalseAsync();
        Task<bool> ChangeIsSendToFalseAsync();
    }
}
