using EmailMaketing.ContentEmails;
using EmailMaketing.Customers;
using Microsoft.AspNetCore.Http;
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

namespace EmailMaketing.Web.Pages.Customers
{
    public class CreateModalModel : EmailMaketingPageModel
    {
        private readonly ICustomerAppService _customerAppService;
        private readonly IdentityUserAppService _identityUserAppService;
        private readonly ContentEmailAppService _contentEmailAppService;
        private readonly IdentityRoleAppService _identityRoleAppService;

        [BindProperty]
        public CreateCustomerViewModal Customer { get; set; }
        [BindProperty]
        public IdentityUserCreateDto AppUser { get; set; }

        public List<SelectListItem> Roles { get; set; }
        [BindProperty]
        public IdentityUserUpdateRolesDto UpdateRole { get; set; }

        public CreateModalModel(ICustomerAppService customerAppService, IdentityUserAppService identityUserAppService,
           ContentEmailAppService contentEmailAppService, IdentityRoleAppService identityRoleAppService )
        {
            _customerAppService = customerAppService;
            _identityUserAppService = identityUserAppService;
            _contentEmailAppService = contentEmailAppService;
            _identityRoleAppService = identityRoleAppService;
        }
        public async void OnGet()
        {
            AppUser = new IdentityUserCreateDto();
            Customer = new CreateCustomerViewModal();
            UpdateRole = new IdentityUserUpdateRolesDto();
            var roles = await _identityRoleAppService.GetAllListAsync();

            Roles = roles.Items
                .Select(r => new SelectListItem(r.Name, r.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var pass = Request.Form["password"];
            Customer.Password = pass;
            var userExist = await _identityUserAppService.FindByEmailAsync(Customer.UserName);
            var emailExist = await _identityUserAppService.FindByEmailAsync(Customer.Email);
            if (userExist != null)
            {
                throw new UserFriendlyException(L["User Name is already exists"]);
            }
            if (emailExist != null)
            {
                throw new UserFriendlyException(L["Email is already exists"]);
            }
            AppUser.UserName = Customer.UserName;
            AppUser.Password = Customer.Password;
            AppUser.Email = Customer.Email;

            //create user
            await _identityUserAppService.CreateAsync(AppUser);
            var userId = await _identityUserAppService.FindByUsernameAsync(AppUser.UserName);
            Customer.UserID = userId.Id;

            // get all roles
            var listRoles = await _identityRoleAppService.GetAsync(Customer.RoleID);
            Customer.Type = listRoles.Name;
            // set value for IdentityUserUpdateRolesDto
            UpdateRole.RoleNames = new string[] { listRoles.Name};

            // Update Roles for user
            await _identityUserAppService.UpdateRolesAsync(userId.Id,UpdateRole);

            // create customer
            await _customerAppService.CreateAsync(
                    ObjectMapper.Map<CreateCustomerViewModal, CreateUpdateCustomer>(Customer)
                );
            return NoContent();
        }

        public class CreateCustomerViewModal
        {
            [HiddenInput]
            public Guid UserID { get; set; }
            [SelectItems(nameof(Roles))]
            [DisplayName("Roles")]
            public Guid RoleID { get; set; }
            [HiddenInput]
            public string Type { get; set; }
            [Required]
            [DisplayName("User Name")]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            [DisplayName("Full Name")]
            /*[RegularExpression("[a-zA-Z]Vs")]*/
            public string FullName { get; set; }
            [Required]
            [RegularExpression("[0-9]{10}")]
            [DisplayName("Phone Number")]
            public string PhoneNumber { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            /*public bool Status { get; set; }*/
        }

    }
}
