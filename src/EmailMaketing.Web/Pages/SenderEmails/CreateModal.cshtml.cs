using AutoMapper.Internal.Mappers;
using EmailMaketing.ContentEmails;
using EmailMaketing.Customers;
using EmailMaketing.SenderEmails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace EmailMaketing.Web.Pages.SenderEmails
{
    public class CreateModalModel : EmailMaketingPageModel
    {
        private readonly SenderEmailAppService _senderEmailAppService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ContentEmailAppService _contentEmailAppService;
        private readonly ICurrentUser _currentUser;

        public CreateModalModel(
            SenderEmailAppService senderEmailAppService, 
            ICurrentUser currentUser, 
            ICustomerRepository customerRepository,
            ContentEmailAppService contentEmailAppService)
        {
            _senderEmailAppService = senderEmailAppService;
            _currentUser = currentUser;
            _customerRepository = customerRepository;
            _contentEmailAppService = contentEmailAppService;
        }

        public List<CreateUpdateSenderEmailDto> emailError = new List<CreateUpdateSenderEmailDto>();

        [BindProperty]
        public CreateSenderEmailViewModal SenderEmail { get; set; }

        public void OnGet()
        {
            SenderEmail = new CreateSenderEmailViewModal();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(_currentUser.UserName != "admin")
            {
                var userId = _currentUser.Id; //Lay userId hien tai
                var customer = await _customerRepository.FindAsync(x => x.UserID == userId);
                SenderEmail.CustomerID = customer.Id;
            }
            else
            {
                SenderEmail.CustomerID = null;
            }

            var emailExist = _contentEmailAppService.CheckEmailExist(SenderEmail.Email);
            var emailcheck = _contentEmailAppService.CheckAuthencation(SenderEmail.Email, SenderEmail.Password);
            var emailsenderExist = await _senderEmailAppService.CheckEmailExist(SenderEmail.Email);
            if (emailExist == "OK" && emailcheck == "Success" && emailsenderExist == false)
            {
                var senderemails = ObjectMapper.Map<CreateSenderEmailViewModal, CreateUpdateSenderEmailDto>(SenderEmail);
                await _senderEmailAppService.CreateAsync(senderemails);
            }
            else if (emailExist != "OK")
            {
                throw new UserFriendlyException(L["Email does not exist"]);
            }
            else if (emailcheck != "Success")
            {
                throw new UserFriendlyException(L["Email or Password does not have permission to send email"]);
            }
            else if (emailsenderExist == true)
            {
                throw new UserFriendlyException(L["Email already exists in the list"]);
            }

            return NoContent();
        }

        public class CreateSenderEmailViewModal
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            [HiddenInput]
            public Guid? CustomerID { get; set; }
        }
    }
}
