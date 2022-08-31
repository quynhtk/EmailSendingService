using EmailMaketing.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace EmailMaketing.Web.Pages.Customers
{
    public class ResetPasswordModel : EmailMaketingPageModel
    {
        private readonly ICustomerAppService _customerAppService;

        [BindProperty]
        public ResetPasswordCustomerViewModal Customer { get; set; }
        public ResetPasswordModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var customerDto = await _customerAppService.GetCustomerAsync(id);
            Customer = ObjectMapper.Map<CustomerDto, ResetPasswordCustomerViewModal>(customerDto);

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var pass = Request.Form["password"];
            Customer.Password = pass;
            await _customerAppService.ReSetPasswordAsync(Customer.Id, Customer.Password);
            return NoContent();
        }
        public class ResetPasswordCustomerViewModal
        {
            [HiddenInput]
            [BindProperty(SupportsGet = true)]
            public Guid Id { get; set; }
            [HiddenInput]
            public Guid UserID { get; set; }
            public string Password { get; set; }
        }
    }
}
