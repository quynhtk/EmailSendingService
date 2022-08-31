using EmailMaketing.ContentEmails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using EmailMaketing.Jobs;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Volo.Abp.Users;
using Volo.Abp.BackgroundJobs;

namespace EmailMaketing.Web.Pages.ContentEmails
{
    public class FormContentEmailModel : PageModel
    {
        private readonly ContentEmailAppService _ContentEmailAppService;
        private readonly RegistrationMailService _RegistrationMailService;
        private readonly IHostingEnvironment _environment;
        private readonly ICurrentUser _currentUser;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public List<ContentEmailDto> ContentEmail { get; set; }
        public ContentEmailDto SelectEmail { get; set; }
        private List<string> listsfile = new List<string>();
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        public FormContentEmailModel(ContentEmailAppService contentEmailAppService, RegistrationMailService registrationMailService,
            IHostingEnvironment environment, ICurrentUser currentUser, IBackgroundJobManager backgroundJobManager)
        {
            _ContentEmailAppService = contentEmailAppService;
            _RegistrationMailService = registrationMailService;
            _environment = environment;
            _currentUser = currentUser;
            _backgroundJobManager = backgroundJobManager;
        }
        private static string foderfileUser = "";
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
        public async Task OnGetAsync()
        {
            Guid? Idcustomer = _currentUser.Id;
            foderfileUser = Idcustomer.ToString().Split("-")[0]; // Your code goes here
            var directory = Path.Combine(_environment.ContentRootPath, "wwwroot/FilesUpload", foderfileUser);
            try
            {
                if (!Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {

            }
            ContentEmail = await _ContentEmailAppService.GetListsEmailAsync((Guid)Idcustomer);
        }
        public async Task<JsonResult> OnGetIndex()
        {
            Guid? Idcustomer = _currentUser.Id;
            ContentEmail = await _ContentEmailAppService.GetListsEmailAsync((Guid)Idcustomer);
            SelectEmail = ContentEmail.FirstOrDefault();
            return new JsonResult(SelectEmail);
        }

        public async Task<IActionResult> OnPostSendPreview()
        {
            if (Request.Form.Files.Count > 0)
            {
                var files = Request.Form.Files;
                try
                {
                    foreach (var FileUpload in files)
                    {
                        var file = Path.Combine(_environment.ContentRootPath, "wwwroot/TempFile", FileUpload.FileName);
                        listsfile.Add(file);
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await FileUpload.CopyToAsync(fileStream);
                        }
                    }
                }
                catch (Exception ex) { }
            }
            string htmlbody = "";
            var linesbody = Request.Form["body"].ToString().Split("/");
            foreach (var line in linesbody)
            {
                htmlbody += "<p>" + line + "</p>";
            }

            //  await _RegistrationMailService.RegisterAsync("asdfsfasdf");
            await _ContentEmailAppService.SendMailAsync(Request.Form["email"].ToString(), Request.Form["subject"], htmlbody, "Henrydao0810@gmail.com", "Tran Van Dao", "leuzxdmiwryorxxi", listsfile);
            listsfile.Clear();
            var path = Path.Combine(_environment.ContentRootPath, "wwwroot/TempFile");
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            return new JsonResult("OK");
        }
        public async Task<IActionResult> OnPostAddEmail(string id)
        {
            Guid? Idcustomer = _currentUser.Id;
            var Data = new CreateUpdateContentEmailDto();
            Data.Subject = "No Subject";
            Data.CustomerID = (Guid)Idcustomer;
            Data.Status = false;
            await _ContentEmailAppService.CreateAsync(Data);
            ContentEmail = await _ContentEmailAppService.GetListsEmailAsync((Guid)Idcustomer);
            var item = ContentEmail.LastOrDefault();
            string valueid = item.Id.ToString();
            return new JsonResult(valueid);
        }
        public async Task<IActionResult> OnPostSelectEmail(string id)
        {
            Guid guid = new Guid(id);
            SelectEmail = await _ContentEmailAppService.GetEmailAsync(guid);
            return new JsonResult(SelectEmail);
        }
        public async Task<IActionResult> OnPostDeleteEmail(string id)
        {
            Guid guid = new Guid(id);
            var check = await _ContentEmailAppService.DeleteAsync(guid);
            if (check == true)
            {
                string code = id.Split("-")[0];
                var path = Path.Combine(_environment.ContentRootPath, "wwwroot/FilesUpload/" + foderfileUser);
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    if (file.Name.Contains(code))
                    {
                        file.Delete();
                    }
                }
                return new JsonResult("OK");
            }
            else
            {
                return new JsonResult("Error");
            }
        }
        private async Task saveEmail()
        {
            Guid guid = new Guid(Request.Form["id"]);
            var Data = new CreateUpdateContentEmailDto();
            Data.Subject = Request.Form["subject"];
            Data.Body = Request.Form["body"];
            /*Data.SenderEmail = Request.Form["fromemail"];*/
            Data.Status = Convert.ToBoolean(Request.Form["status"]);
            string attachment = null;
            string code = Request.Form["id"].ToString().Split("-")[0];
            var path = Path.Combine(_environment.ContentRootPath, "wwwroot/FilesUpload/" + foderfileUser);
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                if (file.Name.Contains(code))
                {
                    file.Delete();
                }
            }
            if (Request.Form.Files.Count > 0)
            {
                var files = Request.Form.Files;
                try
                {
                    foreach (var FileUpload in files)
                    {
                        var file = Path.Combine(_environment.ContentRootPath, "wwwroot/FilesUpload/" + foderfileUser, code + "-" + FileUpload.FileName);
                        listsfile.Add(file);
                        attachment += FileUpload.FileName + ",";
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await FileUpload.CopyToAsync(fileStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (attachment != null)
            {
                attachment = attachment.Remove(attachment.Length - 1);
            }
            Data.Attachment = attachment;
            //Guid guid = new Guid(md.id);
            //var Data = new CreateUpdateContentEmailDto();
            //Data.Subject = md.subject;
            //Data.Body = md.body;
            //Data.SenderEmail = md.fromemail;
            //Data.Status = Convert.ToBoolean(md.status);
            //Data.Time = md.datetime;
            await _ContentEmailAppService.UpdateDataAsync(guid, Data);
        }
        public async Task<IActionResult> OnPostUpdateEmail()
        {
            await saveEmail();
            return new JsonResult("OK");
        }
    }
}