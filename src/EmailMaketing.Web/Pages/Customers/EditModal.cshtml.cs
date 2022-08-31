using EmailMaketing.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;

namespace EmailMaketing.Web.Pages.Customers
{
    
    public class EditModalModel : EmailMaketingPageModel
    {
        private readonly CustomerAppService _customerAppService;
        private readonly IdentityRoleAppService _identityRoleAppService;
        private readonly IdentityUserAppService _identityUserAppService;

        [BindProperty]
        public EditCustomerViewModal Customer { get; set; }
        public EditModalModel(CustomerAppService customerAppService, IdentityRoleAppService identityRoleAppService, IdentityUserAppService identityUserAppService)
        {
            _customerAppService = customerAppService;
            _identityRoleAppService = identityRoleAppService;
            _identityUserAppService = identityUserAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var customerDto = await _customerAppService.GetCustomerAsync(id);
            Customer = ObjectMapper.Map<CustomerDto, EditCustomerViewModal>(customerDto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _customerAppService.UpdateAsync(
                    Customer.Id,
                    ObjectMapper.Map<EditCustomerViewModal, CreateUpdateCustomer>(Customer)
                );
            return NoContent();
        }
        public class EditCustomerViewModal
        {
            [HiddenInput]
            [BindProperty(SupportsGet = true)]
            public Guid Id { get; set; }
            [HiddenInput]
            public Guid UserID { get; set; }
            [HiddenInput]
            public string Password { get; set; }
            [DisplayName("User Name")]
            public string UserName { get; set; }
            [DisplayName("Full Name")]
            public string FullName { get; set; }
            [RegularExpression("[0-9]{10}")]
            [DisplayName("Phone Number")]
            public string PhoneNumber { get; set; }
            [EmailAddress]
            public string Email { get; set; }
            [HiddenInput]
            public string Type { get; set; }
        }
    }
}
