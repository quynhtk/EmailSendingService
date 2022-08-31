using Volo.Abp.BackgroundJobs;
using EmailMaketing.ContentEmails;
using EmailMaketing.Customers;
using EmailMaketing.Jobs;
using EmailMaketing.SenderEmails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using EmailMaketing.EmailSchedules;
using Volo.Abp;

namespace EmailMaketing.Web.Pages.ContentEmails
{
    public class SendEmailModalModel : EmailMaketingPageModel
    {
        private readonly ContentEmailAppService _contentEmailAppService;
        private readonly ICurrentUser _currentUser;
        private readonly SenderEmailAppService _senderEmailAppService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly EmailScheduleAppService _emailScheduleAppService;

        [BindProperty]
        public CreateContentEmailViewModal ContentEmail { get; set; }
        [BindProperty]
        public CreateUpdateEmailSchedule emailSchedule { get; set; }
        private List<string> listsfile = new List<string>();
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        public SendEmailModalModel(ContentEmailAppService contentEmailAppService,
            ICurrentUser currentUser,
            SenderEmailAppService senderEmailAppService,
            IHostingEnvironment hostingEnvironment,
            ICustomerRepository customerRepository,
            IBackgroundJobManager backgroundJobManager,
            EmailScheduleAppService emailScheduleAppService)
        {
            _contentEmailAppService = contentEmailAppService;
            _currentUser = currentUser;
            _senderEmailAppService = senderEmailAppService;
            _hostingEnvironment = hostingEnvironment;
            _customerRepository = customerRepository;
            _backgroundJobManager = backgroundJobManager;
            _emailScheduleAppService = emailScheduleAppService;
        }

        public async Task OnGetAsync()
        {
            ContentEmail = new CreateContentEmailViewModal();
            emailSchedule = new CreateUpdateEmailSchedule();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //get role
            var userId = _currentUser.Id;
            var userName = _currentUser.UserName;
            var customer = await _customerRepository.FindByCustomerWithUserIDAsync((Guid)userId);
            var dayNow = DateTime.Now;
            var timespan = ContentEmail.Day - dayNow;

            //get data to form va cat cac phan tu \r, \n
            var listEmailReceive = ContentEmail.RecipientEmail.ToString().Split(',');
            // tao bien de remove khoi arry
            string stringToRemove = "";
            // remove cac phan tu ""
            listEmailReceive = listEmailReceive.Where(val => val != stringToRemove).ToArray();                          
            var senderIsSendFalse = new SenderEmailDto();
            if (FileUpload != null)
            {
                //luu file vao thu muc wwwroot/FilesUpload
                var file = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/FilesUpload", FileUpload.FileName);
                listsfile.Add(file);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await FileUpload.CopyToAsync(fileStream);
                }
            }
            if (listEmailReceive.Length > 0 && listEmailReceive != null)
            {
                string htmlbody = "";
                var linesbody = ContentEmail.Body.ToString().Split('\r', '\n');
                foreach (var line in linesbody)
                {
                    if (line != "")
                    {
                        htmlbody += "<p>" + line + "</p>";
                    }

                }
                htmlbody += "<p style='display:none'>" + randomtext() + "</p>";
                var countEmailReceive = listEmailReceive.Length;
                for (int i = 0; i < countEmailReceive; i++)
                {
                    if (userName == "admin")
                    {
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync();
                    }
                    else
                    {
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync(customer.Id, customer.Type);
                    }
                    
                    if (senderIsSendFalse == null && userName == "admin")
                    {
                        await _senderEmailAppService.ChangeIsSendToFalseAsync();
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync();
                    }
                    else if (senderIsSendFalse == null)
                    {
                        await _senderEmailAppService.ChangeIsSendToFalseAsync(customer.Id, customer.Type);
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync(customer.Id, customer.Type);
                    }

                    if (listEmailReceive[i] != "")
                    {
                        await _contentEmailAppService.SendMailAsync(
                        listEmailReceive[i],
                        ContentEmail.Subject,
                        htmlbody,
                        senderIsSendFalse.Email,
                        ContentEmail.Name,
                        senderIsSendFalse.Password,
                        listsfile);
                        await CreateContentEmail(senderIsSendFalse, (Guid)userId,userName,0);
                    }

                }
            }


            return RedirectToAction("Index", "ContentEmails");
        }

        private async Task CreateContentEmail(SenderEmailDto senderEmailDto, Guid userId, string userName, int timespan)
        {
            if (userName == "admin")
            {
                ContentEmail.CustomerID = (Guid)userId;
            }
            else
            {
                var customer = await _customerRepository.FindByCustomerWithUserIDAsync((Guid)userId);
                ContentEmail.CustomerID = customer.Id;
            }

            ContentEmail.SenderEmailID = senderEmailDto.Id;
            ContentEmail.Subject = ContentEmail.Subject;
            ContentEmail.Body = ContentEmail.Body;
            if (timespan == 1)
            {
                emailSchedule.Schedule = ContentEmail.Day;
                emailSchedule.isSend = false;
                await _emailScheduleAppService.CreateAsync(emailSchedule);
                var emailNew = await _emailScheduleAppService.GetNewEmailScheduleAsync();
                ContentEmail.EmailScheduleID = emailNew.Id;
            }
            var contentDto = ObjectMapper.Map<CreateContentEmailViewModal, CreateUpdateContentEmailDto>(ContentEmail);
            await _contentEmailAppService.CreateAsync(contentDto);
        }
        private string randomtext()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[30];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        public async Task<IActionResult> OnPostScheduleJobAsync()
        {
            //get role
            var userId = _currentUser.Id;
            var userName = _currentUser.UserName;
            var customer = await _customerRepository.FindByCustomerWithUserIDAsync((Guid)userId);
            var dayNow = DateTime.Now;
            var timeInForm = Request.Form["dateschedule"];
            ContentEmail.Day = Convert.ToDateTime(timeInForm);
            /*var timespan = ContentEmail.Day - dayNow;*/
            var timespan = ContentEmail.Day - dayNow;
            //get data to form va cat cac phan tu \r, \n
            var listEmailReceive = ContentEmail.RecipientEmail.ToString().Split(',');
            // tao bien de remove khoi arry
            string stringToRemove = "";
            // remove cac phan tu ""
            listEmailReceive = listEmailReceive.Where(val => val != stringToRemove).ToArray();
            var senderIsSendFalse = new SenderEmailDto();
            if (FileUpload != null)
            {
                //luu file vao thu muc wwwroot/FilesUpload
                var file = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/FilesUpload", FileUpload.FileName);
                listsfile.Add(file);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await FileUpload.CopyToAsync(fileStream);
                }
            }
            if (listEmailReceive.Length > 0 && listEmailReceive != null)
            {
                string htmlbody = "";
                var linesbody = ContentEmail.Body.ToString().Split('\r', '\n');
                foreach (var line in linesbody)
                {
                    if (line != "")
                    {
                        htmlbody += "<p>" + line + "</p>";
                    }

                }
                htmlbody += "<p style='display:none'>" + randomtext() + "</p>";
                var countEmailReceive = listEmailReceive.Length;
                for (int i = 0; i < countEmailReceive; i++)
                {
                    if (userName == "admin")
                    {
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync();
                    }
                    else
                    {
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync(customer.Id, customer.Type);
                    }

                    if (senderIsSendFalse == null && userName == "admin")
                    {
                        await _senderEmailAppService.ChangeIsSendToFalseAsync();
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync();
                    }
                    else if (senderIsSendFalse == null)
                    {
                        await _senderEmailAppService.ChangeIsSendToFalseAsync(customer.Id, customer.Type);
                        senderIsSendFalse = await _senderEmailAppService.SenderIsSendFalseAsync(customer.Id, customer.Type);
                    }

                    if (listEmailReceive[i] != "")
                    {

                        var senderJob = new SendEmailArgs()
                        {
                            To = listEmailReceive[i],
                            Subject = ContentEmail.Subject,
                            Body = htmlbody,
                            EmailAddress = senderIsSendFalse.Email,
                            Name = ContentEmail.Name,
                            Password = senderIsSendFalse.Password,
                            File = listsfile
                        };
                        await _backgroundJobManager.EnqueueAsync(senderJob, BackgroundJobPriority.High, timespan);
                        await CreateContentEmail(senderIsSendFalse, (Guid)userId, userName, 1);
                    }

                }
            }
            return RedirectToAction("Index", "ContentEmails");

        }
       
        public class CreateContentEmailViewModal
        {
            [DisplayName("Sender Name")]
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
            public List<string> Attachment { get; set; }
            public bool Status { get; set; }
            public bool Featured { get; set; }
            public Guid CustomerID { get; set; }
            public Guid? EmailScheduleID { get; set; }
            public DateTime Schedule { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime Day { get; set; } = DateTime.Now;
            public Guid SenderEmailID { get; set; }
        }
    }
}
