using EmailMaketing.ContentEmails;
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
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace EmailMaketing.Web.Pages.ContentEmails
{
    public class CreateModalModel : EmailMaketingPageModel
    {
        private readonly ContentEmailAppService _contentEmailAppService;

        [BindProperty]
        public CreateContentViewModal ContentEmail { get; set; }
        public List<SelectListItem> SenderEmails { get; set; }

        public CreateModalModel(ContentEmailAppService contentEmailAppService)
        {
            _contentEmailAppService = contentEmailAppService;
        }
        
        public async Task OnGetAsync()
        {
            ContentEmail = new CreateContentViewModal();
            var senderLookup = await _contentEmailAppService.GetSenderLookupAsync();
            SenderEmails = senderLookup.Items
                .Select(s => new SelectListItem(s.email, s.Id.ToString()))
                .ToList();
        }

        public class CreateContentViewModal
        {
            public string Name { get; set; }
            [Required]
            public string Subject { get; set; }
            [Required]
            [TextArea(Rows = 10)]
            public string Body { get; set; }
            [Required]
            [TextArea(Rows = 3)]
            [DisplayName("Recipient Email")]
            public string RecipientEmail { get; set; }
            [DisplayName("File")]
            public string Attachment { get; set; }
            public bool Status { get; set; }
            public bool Featured { get; set; }
            public Guid CustomerID { get; set; }
            public DateTime Schedule { get; set; }

            [Required]
            [SelectItems(nameof(SenderEmails))]
            [DisplayName("Sender Email")]
            public Guid SenderEmailId { get; set; }
        }
    }
}
