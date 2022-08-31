
using EmailMaketing.ContentEmails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace EmailMaketing.Jobs
{
    public class SendEmailJob : AsyncBackgroundJob<SendEmailArgs>, ITransientDependency
    {
        private readonly ContentEmailAppService _contentEmailAppService;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public SendEmailJob(ContentEmailAppService contentEmailAppService, IBackgroundJobManager backgroundJobManager)
        {
            _contentEmailAppService = contentEmailAppService;
            _backgroundJobManager = backgroundJobManager;
        }
        public override async Task ExecuteAsync(SendEmailArgs args)
        {
            await _contentEmailAppService.SendMailAsync(
                    args.To,
                    args.Subject,
                    args.Body,
                    args.EmailAddress,
                    args.Name,
                    args.Password,
                    args.File
                );
        }
    }
}
