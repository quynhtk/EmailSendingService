using EmailMaketing.Customers;
using EmailMaketing.SenderEmails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace EmailMaketing.Web.Pages.SenderEmails
{
    
    public class EditModalModel : EmailMaketingPageModel
    {
        private readonly ISenderEmailAppService _senderEmailAppService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUser _currentUser;
        public EditModalModel(
            ISenderEmailAppService senderEmailAppService,
            ICustomerRepository customerRepository,
            ICurrentUser currentUser)
        {
            _senderEmailAppService = senderEmailAppService;
            _customerRepository = customerRepository;
            _currentUser = currentUser;
        }

        [BindProperty]
        public EditSenderEmailViewModal SenderEmails { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            var senderemailDto = await _senderEmailAppService.GetSenderEmailAsync(id);
            SenderEmails = ObjectMapper.Map<SenderEmailDto, EditSenderEmailViewModal>(senderemailDto);

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (_currentUser.UserName != "admin")
            {
                var userId = _currentUser.Id; // Lay userId hien tai
                var customer = await _customerRepository.FindAsync(x => x.UserID == userId);
                SenderEmails.CustomerID = customer.Id;
            }
            else
            {
                SenderEmails.CustomerID = null;
            }
            await _senderEmailAppService.UpdateAsync(
                SenderEmails.Id,
                ObjectMapper.Map<EditSenderEmailViewModal, CreateUpdateSenderEmailDto>(SenderEmails));
            return NoContent();
        }
        public class EditSenderEmailViewModal
        {
            [HiddenInput]
            [BindProperty(SupportsGet = true)]
            public Guid Id { get; set; }
            [EmailAddress]
            public string Email { get; set; }
            public string Password { get; set; }
            [HiddenInput]
            public Guid? CustomerID { get; set; }
        }
    }
}
