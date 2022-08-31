using EmailMaketing.ContentEmails;
using EmailMaketing.Customers;
using EmailMaketing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace EmailMaketing.SenderEmails
{
    //[Authorize(EmailMaketingPermissions.SenderEmails.Default)]
    public class SenderEmailAppService : ApplicationService, ISenderEmailAppService
    {
        private readonly ISenderEmailRepository _senderEmailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IContentEmailRepository _contentEmailRepository;

        public SenderEmailAppService(
            ISenderEmailRepository senderEmailRepository,
            ICustomerRepository customerRepository,
            IIdentityUserRepository identityUserRepository,
            ICurrentUser currentUser,
            IContentEmailRepository contentEmailRepository)
        {
            _senderEmailRepository = senderEmailRepository;
            _customerRepository = customerRepository;
            _identityUserRepository = identityUserRepository;
            _currentUser = currentUser;
            _contentEmailRepository = contentEmailRepository;
        }

        public async Task<SenderEmailDto> CreateAsync(CreateUpdateSenderEmailDto input)
        {
            var SenderEmail = ObjectMapper.Map<CreateUpdateSenderEmailDto, SenderEmail>(input);
            await _senderEmailRepository.InsertAsync(SenderEmail);
            return ObjectMapper.Map<SenderEmail, SenderEmailDto>(SenderEmail);
        }

        public async Task<List<SenderEmailDto>> CreateManyAsync(List<CreateUpdateSenderEmailDto> senders)
        {
            var SenderEmails = ObjectMapper.Map<List<CreateUpdateSenderEmailDto>, List<SenderEmail>>(senders);
            await _senderEmailRepository.InsertManyAsync(SenderEmails);
            var senderEmailDeletes = await _senderEmailRepository.GetListAsync();
            return ObjectMapper.Map<List<SenderEmail>, List<SenderEmailDto>>(SenderEmails);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sender = await _senderEmailRepository.FindAsync(id);
            var userId = _currentUser.Id;
            var userAdmin = await _identityUserRepository.FindAsync((Guid)userId);
            if (userAdmin.UserName == "admin")
            {
                var contenEmail = await _contentEmailRepository.FindByIdSenderEmailAsync(id);
                if (contenEmail != null)
                {
                    return false;
                }
                await _senderEmailRepository.DeleteAsync(sender);
                return true;
            }
            if (sender.CustomerID != null)
            {
                var customer = await _customerRepository.FindAsync(x => x.UserID == userId);
                if (sender.CustomerID == customer.Id)
                {
                    var contenEmail = await _contentEmailRepository.FindByIdSenderEmailAsync(id);
                    if (contenEmail != null)
                    {
                        return false;
                    }
                    await _senderEmailRepository.DeleteAsync(sender);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<List<SenderEmailDto>> GetListSenderAsync()
        {
            var sender = await _senderEmailRepository.GetListAsync();
            return ObjectMapper.Map<List<SenderEmail>, List<SenderEmailDto>>(sender);
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            var emailExist = await _senderEmailRepository.FindByEmailAsync(email);
            if (emailExist != null)
            {
                return true;
            }
            return false;
        }

        public async Task<PagedResultDto<SenderEmailDto>> GetListAsync(GetSenderEmailInput input)
        {
            var userid = _currentUser.Id;
            var username = _currentUser.UserName;
            if(input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(SenderEmail.Email);
            }
            var senderEmail = await _senderEmailRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter);
            var listSenderEmail = new List<SenderEmailDto>();
            var senderEmailDtos = ObjectMapper.Map<List<SenderEmail>, List<SenderEmailDto>>(senderEmail);
            var totalcount = await _senderEmailRepository.GetCountAsync();
            if(username != "admin")
            {
                var customer = await _customerRepository.FindByCustomerWithUserIDAsync((Guid)userid);
                listSenderEmail = senderEmailDtos.Where(c => c.CustomerID == customer.Id).ToList();
            }
            else
            {
                listSenderEmail = senderEmailDtos;
            }
            var stt = 1;
            foreach (var item in listSenderEmail)
            {
                item.Stt = stt;
                stt++;
                if(item.CustomerID != null)
                {
                    item.CustomerName = await GetCustomerNameAsync(item, username, (Guid)item.CustomerID);
                }
            }
            return new PagedResultDto<SenderEmailDto>(
                totalcount,
                listSenderEmail
                );
        }

        private async Task<string> GetCustomerNameAsync(SenderEmailDto senderEmailDto, string username, Guid id)
        {
            if (username == "admin")
            {
                var userId = _currentUser.Id;
                if (userId == id)
                {
                    return username;
                }
                else
                {
                    var customerUserAdmin = await _customerRepository.FindByCustomerWithIDAsync(id);
                    return customerUserAdmin.FullName;
                }
            }
            var customerUserNoAdmin = await _customerRepository.FindByCustomerWithIDAsync(id);
            return customerUserNoAdmin.FullName;
        }

        /*public async Task<PagedResultDto<SenderWithNavigationDto>> GetListWithNavigationAsync(GetSenderEmailInput input)
        {
            //Set a default sorting, if not provided
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(SenderEmail.Email);
            }
            var senderemail = await _senderEmailRepository.GetListWithNavigationAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter);
            //Convert to DTOs
            var senderWithNavigationDtos = ObjectMapper.Map<List<SenderWithNavigation>, List<SenderWithNavigationDto>>(senderemail);
            var stt = 1;
            foreach (var item in senderWithNavigationDtos)
            {
                item.Stt = stt;
                stt++;
            }
            //Get the total count with another query (required for the paging)
            var totalcount = await _senderEmailRepository.GetCountAsync();

            return new PagedResultDto<SenderWithNavigationDto>
            {
                TotalCount = totalcount,
                Items = senderWithNavigationDtos
            };
        }*/

        public async Task<SenderEmailDto> GetSenderEmailAsync(Guid Id)
        {
            var senderemail = await _senderEmailRepository.FindAsync(Id);
            return ObjectMapper.Map<SenderEmail, SenderEmailDto>(senderemail);
        }

        public async Task<SenderEmailDto> UpdateAsync(Guid id, CreateUpdateSenderEmailDto input)
        {
            var items = await _senderEmailRepository.FindAsync(id);
            items.Email = input.Email;
            items.Password = input.Password;
            items.CustomerID = input.CustomerID;
            await _senderEmailRepository.UpdateAsync(items);
            return ObjectMapper.Map<SenderEmail, SenderEmailDto>(items);
        }

        //get sender with IsSend = false
        public async Task<SenderEmailDto> SenderIsSendFalseAsync(Guid cusotmerId, string role)
        {
            var senders = await _senderEmailRepository.GetListAsync();
            if (role == "Private")
            {
               senders = senders.Where(s => s.CustomerID == cusotmerId).ToList();
            }
            else
            {
                senders = senders.Where(s => s.CustomerID == null).ToList();
            }

            foreach (var sender in senders)
            {
                if (sender.IsSend == false)
                {
                    sender.IsSend = true;
                    await _senderEmailRepository.UpdateAsync(sender);
                    var senderdto = ObjectMapper.Map<SenderEmail, SenderEmailDto>(sender);
                    return senderdto;
                }
            }
            return null;
        }

        //change all sender with IsSend = true to IsSend = false
        public async Task<bool> ChangeIsSendToFalseAsync(Guid cusotmerId, string role)
        {
            var senders = await _senderEmailRepository.GetListAsync();
            if (role == "Private")
            {
                senders = senders.Where(s => s.CustomerID == cusotmerId).ToList();
            }
            else
            {
                senders = senders.Where(s => s.CustomerID == null).ToList();
            }
            foreach (var sender in senders)
            {
                sender.IsSend = false;
                await _senderEmailRepository.UpdateAsync(sender);
            }
            return true;
        }

        public async Task<SenderEmailDto> SenderIsSendFalseAsync()
        {
            var senders = await _senderEmailRepository.GetListAsync();
            senders = senders.Where(s => s.CustomerID == null).ToList();

            foreach (var sender in senders)
            {
                if (sender.IsSend == false)
                {
                    sender.IsSend = true;
                    await _senderEmailRepository.UpdateAsync(sender);
                    var senderdto = ObjectMapper.Map<SenderEmail, SenderEmailDto>(sender);
                    return senderdto;
                }
            }
            return null;
        }

        public async Task<bool> ChangeIsSendToFalseAsync()
        {

            var senders = await _senderEmailRepository.GetListAsync();
            senders = senders.Where(s => s.CustomerID == null).ToList();
            foreach (var sender in senders)
            {
                sender.IsSend = false;
                await _senderEmailRepository.UpdateAsync(sender);
            }
            return true;
        }
    }
}
