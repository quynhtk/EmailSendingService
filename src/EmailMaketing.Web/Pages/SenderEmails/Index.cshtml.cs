using Abp.UI;
using ClosedXML.Excel;
using EmailMaketing.ContentEmails;
using EmailMaketing.Customers;
using EmailMaketing.SenderEmails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace EmailMaketing.Web.Pages.SenderEmails
{
    public class IndexModel : EmailMaketingPageModel
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerRepository _customerRepository;
        private readonly SenderEmailAppService _senderEmailAppService;
        private readonly ContentEmailAppService _contentEmailAppService;

        List<CreateUpdateSenderEmailDto> senderEmail = new List<CreateUpdateSenderEmailDto>();
        public List<CreateUpdateSenderEmailDto> emailError = new List<CreateUpdateSenderEmailDto>();
        [BindProperty]
        public List<SenderEmailDto> SenderEmails { get; set; }
        

        public IndexModel(ICurrentUser currentUser, ICustomerRepository customerRepository,
            SenderEmailAppService senderEmailAppService,
            ContentEmailAppService contentEmailAppService)
        {
            _currentUser = currentUser;
            _customerRepository = customerRepository;
            _senderEmailAppService = senderEmailAppService;
            _contentEmailAppService = contentEmailAppService;
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            var memoryStream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users Sheet");

                worksheet.Column(1).Width = 30;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 30;

                var email = worksheet.Cell(1, 1);
                var password = worksheet.Cell(1, 2);

                email.Value = "Email";
                email.Style.Font.Bold = true;
                password.Value = "Password";
                password.Style.Font.Bold = true;
                workbook.SaveAs(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task OnPostImportAsync(IFormFile excel)
        {
            using (var workbook = new XLWorkbook(excel.OpenReadStream()))
            {
                var worksheet = workbook.Worksheet("Users Sheet");
                var count = 0;
            
                foreach (var row in worksheet.Rows())
                {
                    var email = row.Cell(1).Value.ToString();
                    var pass = row.Cell(2).Value.ToString();
                    var emailExist = _contentEmailAppService.CheckEmailExist(email);
                    var emailcheck = _contentEmailAppService.CheckAuthencation(email, pass);
                    count += 1;
                    var userId = _currentUser.Id; //Lay userId hien tai
                    var customer = await _customerRepository.FindAsync(x => x.UserID == userId);
            
                    var emailsenderExist = await _senderEmailAppService.CheckEmailExist(email);
            
                    if (count > 1)
                    {
                        if (emailExist == "OK" && emailcheck == "Success" && emailsenderExist == false)
                        {
                            if (_currentUser.UserName != "admin")
                            {
                                senderEmail.Add(new CreateUpdateSenderEmailDto()
                                {
                                    Email = email,
                                    Password = pass,
                                    CustomerID = customer.Id
                                });
                            }
                            else
                            {
                                senderEmail.Add(new CreateUpdateSenderEmailDto()
                                {
                                    Email = email,
                                    Password = pass
                                });
                            }
                        }
                        else
                        {
                            emailError.Add(new CreateUpdateSenderEmailDto() { Email = email });
                        }
                    }
                }
            }
            
            if (senderEmail.Count > 0)
            {
                await _senderEmailAppService.CreateManyAsync(senderEmail);
            }
            /*return RedirectToAction("Index", "SenderEmails");*/
        }
    }
}
