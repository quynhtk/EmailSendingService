
using EmailMaketing.SenderEmails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;

namespace EmailMaketing.Jobs
{
    public class RegistrationMailService : ApplicationService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly SenderEmailAppService _senderEmailAppService;

        public RegistrationMailService(IBackgroundJobManager backgroundJobManager, SenderEmailAppService senderEmailAppService)
        {
            _backgroundJobManager = backgroundJobManager;
            _senderEmailAppService = senderEmailAppService;
        }

        public async Task sendeMail1N()
        {
            string htmlbody = "";
            htmlbody = "<p>" + randomtext() + "<p>";

            var listEmail = new List<SendEmailArgs>();
            var listEmailReceiver = new List<string>() { "phongnguyen.httdn@gmail.com", "letg3313@gmail.com", "mrlong.itqn@gmail.com", "ndlong@sdc.udn.vn" };



            listEmail.Add(new SendEmailArgs
            {
                To = "ndlong@sdc.udn.vn",
                Subject = "Test gửi email 1-n",
                Body = "Dang gui email 1-n " + htmlbody,
                EmailAddress = "HenryDao0810@gmail.com",
                Name = "Nguyen le",
                Password = "leuzxdmiwryorxxi",
                File = new List<string>()
            });

            listEmail.Add(new SendEmailArgs
            {
                To = "mrlong.itqn@gmail.com",
                Subject = "Test gửi email 1-n",
                Body = "Dang gui email 1-n " + htmlbody,
                EmailAddress = "HenryDao0810@gmail.com",
                Name = "Nguyen le",
                Password = "leuzxdmiwryorxxi",
                File = new List<string>()
            });

            listEmail.Add(new SendEmailArgs
            {
                To = "letg3313@gmail.com",
                Subject = "Test gửi email 1-n",
                Body = "Dang gui email 1-n " + htmlbody,
                EmailAddress = "HenryDao0810@gmail.com",
                Name = "Nguyen le",
                Password = "leuzxdmiwryorxxi",
                File = new List<string>()
            });
            listEmail.Add(new SendEmailArgs
            {
                To = "phongnguyen.httdn@gmail.com",
                Subject = "Test gửi email 1-n",
                Body = "Dang gui email 1-n " + htmlbody,
                EmailAddress = "HenryDao0810@gmail.com",
                Name = "Nguyen le",
                Password = "leuzxdmiwryorxxi",
                File = new List<string>()
            });

            foreach (var item in listEmail)
            {
                await _backgroundJobManager.EnqueueAsync(item, BackgroundJobPriority.High, TimeSpan.FromMinutes(1));
            }

        }
        public async Task sendeMail()
        {
            string htmlbody = "";
            htmlbody = "<p>" + randomtext() + "<p>";

            var listEmail = new List<SendEmailArgs>();
            var listEmailReceiver = new List<string>() { "phongnguyen.httdn@gmail.com", "letg3313@gmail.com", "mrlong.itqn@gmail.com", "ndlong@sdc.udn.vn" };
            var listEmailSender = await _senderEmailAppService.GetListSenderAsync();

            foreach (var emailReceiver in listEmailReceiver)
            {
                foreach (var emailSender in listEmailSender)
                {
                    listEmail.Add(new SendEmailArgs
                    {
                        To = emailReceiver,
                        Subject = "Gửi email nhiều nhiều",
                        Body = "Thử gửi email nhiều nhiều" + htmlbody,
                        EmailAddress = emailSender.Email,
                        Name = "Nguyen Le",
                        Password = emailSender.Password,
                        File = new List<string>()
                    });
                }
            }

            foreach (var item in listEmail)
            {
                await _backgroundJobManager.EnqueueAsync(item, BackgroundJobPriority.High, TimeSpan.FromMinutes(1));

            }

        }

        public async Task sendeMailN1()
        {
            string htmlbody = "";
            htmlbody = "<p>" + randomtext() + "<p>";

            var listEmail = new List<SendEmailArgs>();
            var listEmailSender = await _senderEmailAppService.GetListSenderAsync();

            foreach (var emailSender in listEmailSender)
            {
                listEmail.Add(new SendEmailArgs
                {
                    To = "ndlong@sdc.udn.vn",
                    Subject = "Gửi email nhiều 1",
                    Body = "Thử gửi email nhiều 1" + htmlbody,
                    EmailAddress = emailSender.Email,
                    Name = "Nguyen Le",
                    Password = emailSender.Password,
                    File = new List<string>()
                });
            }

            foreach (var item in listEmail)
            {
                /*await _backgroundJobManager.EnqueueAsync(item, BackgroundJobPriority.High, TimeSpan.FromMinutes(1));*/

            }

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
    }
}
