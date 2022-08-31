using EmailMaketing.ContentEmails;
using EmailMaketing.SenderEmails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace EmailMaketing.Customers
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IdentityUserAppService _identityUserAppService;
        private readonly ISenderEmailAppService _senderEmailAppService;
        private readonly ContentEmailAppService _contentEmailAppService;

        public CustomerAppService(ICustomerRepository customerRepository,
            IdentityUserAppService identityUserAppService,
            ISenderEmailAppService senderEmailAppService,
            ContentEmailAppService contentEmailAppService)
        {
            _customerRepository = customerRepository;
            _identityUserAppService = identityUserAppService;
            _senderEmailAppService = senderEmailAppService;
            _contentEmailAppService = contentEmailAppService;
        }

        public async Task ChangeStatus(Guid Id)
        {
            var identityUser = new IdentityUserUpdateDto();
            var customer = await _customerRepository.FindAsync(Id);

            customer.Status = !customer.Status;
            await _customerRepository.UpdateAsync(customer);
            var user = await _identityUserAppService.GetAsync(customer.UserID);

            user.IsActive = !user.IsActive;
            identityUser.UserName = user.UserName;
            identityUser.Email = user.Email;
            identityUser.IsActive = user.IsActive;
            await _identityUserAppService.UpdateAsync(customer.UserID, identityUser);
        }

        public async Task<CustomerDto> CreateAsync(CreateUpdateCustomer input)
        {
            var customer = ObjectMapper.Map<CreateUpdateCustomer, Customer>(input);
            await _customerRepository.InsertAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task<string> DeleteAsync(Guid id)
        {
            var customer = await _customerRepository.FindAsync(id);
            var senderEmails = await _senderEmailAppService.GetListSenderAsync();
            var contenEmails = await _contentEmailAppService.GetListsEmailAsync();
            foreach (var item in contenEmails)
            {
                if (item.CustomerID == customer.Id)
                {
                    return "Customer have data with Content";
                }
            }
            foreach (var item in senderEmails)
            {
                if (item.CustomerID == customer.Id)
                {
                    return "Customer have data with Sender Email";
                }
            }
            await _identityUserAppService.DeleteAsync(customer.UserID);
            await _customerRepository.DeleteAsync(customer);
            return "Ok";
        }

        public async Task<CustomerDto> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.FindAsync(id);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task<ListResultDto<GetCustomerTypeLookup>> GetCustomerTypeLookupAsync()
        {
            var customers = await _customerRepository.GetListAsync();
            var customerLookupDto = ObjectMapper.Map<List<Customer>, List<GetCustomerTypeLookup>>(customers);
            return new ListResultDto<GetCustomerTypeLookup>(
                    customerLookupDto
                );
        }

        public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerInput input)
        {
            if (input.Sorting.IsNullOrEmpty())
            {
                input.Sorting = nameof(Customer.CreationTime);
            }
            var customers = await _customerRepository.GetListAsync(
                    input.SkipCount,
                    input.MaxResultCount,
                    input.Sorting,
                    input.Filter
                );

            var customerDtos = ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers);
            var stt = 1;
            
            foreach (var item in customerDtos)
            {
                item.Stt = stt++;
                
            }
            
            var totalCount = await _customerRepository.GetCountAsync();
            return new PagedResultDto<CustomerDto>(
                    totalCount,
                    customerDtos
                );
        }

        public async Task<CustomerDto> ReSetPasswordAsync(Guid id, string password)
        {
            var identityUpdateDto = new IdentityUserUpdateDto();
            var customer = await _customerRepository.FindAsync(id);
            var user = await _identityUserAppService.FindByUsernameAsync(customer.UserName);
            if (user != null)
            {
                identityUpdateDto.UserName = customer.UserName;
                identityUpdateDto.Email = customer.Email;
                identityUpdateDto.Password = password;
                identityUpdateDto.IsActive = user.IsActive;
                await _identityUserAppService.UpdateAsync(user.Id, identityUpdateDto);
            }
            else
            {
                throw new UserFriendlyException(L["Not Found"]);
            }
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomer input)
        {
            var identityUpdateUserDto = new IdentityUserUpdateDto();
            var customer = await _customerRepository.FindAsync(id);
            var user = await _identityUserAppService.FindByUsernameAsync(customer.UserName);
            if (user != null)
            {
                customer.UserName = input.UserName;
                customer.Email = input.Email;
                customer.PhoneNumber = input.PhoneNumber;
                customer.FullName = input.FullName;
                customer.UserID = input.UserID;
                customer.Type = input.Type;
                await _customerRepository.UpdateAsync(customer);


                identityUpdateUserDto.UserName = input.UserName;
                identityUpdateUserDto.Password = input.Password;
                identityUpdateUserDto.Email = input.Email;
                identityUpdateUserDto.IsActive = user.IsActive;
                await _identityUserAppService.UpdateAsync(customer.UserID, identityUpdateUserDto);
            }
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }
    }
}
