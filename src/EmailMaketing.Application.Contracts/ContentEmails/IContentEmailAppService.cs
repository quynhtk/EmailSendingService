using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EmailMaketing.ContentEmails
{
    public interface IContentEmailAppService : IApplicationService
    {
        Task<ContentEmailDto> CreateAsync(CreateUpdateContentEmailDto input);
        Task<List<ContentEmailDto>> GetListsEmailAsync(Guid id);
        Task<List<ContentEmailDto>> GetListsEmailAsync();
        Task<ContentEmailDto> GetEmailAsync(Guid id);

        Task<bool> DeleteAsync(Guid id);
        Task<ContentEmailDto> UpdateDataAsync(Guid id, CreateUpdateContentEmailDto input);
        //Task<ContentEmailDto> UpdateAsync(Guid)
        Task<int> SendMailAsync(string to, string  subject, string body, string emailaddress, string name, string pass, List<string> listfile);
        string CheckEmailExist(string addressEmail);

        string CheckAuthencation(string addressEmail, string pass);

        Task<ListResultDto<SenderLookup>> GetSenderLookupAsync();
        Task<PagedResultDto<ContentEmailDto>> GetListAsync(GetContentEmailInput input);
    }
}
 