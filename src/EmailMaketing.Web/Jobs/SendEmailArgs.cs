using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace EmailMaketing.Jobs
{
    [BackgroundJobName("SendEmailJob")]
    public class SendEmailArgs
    {
        public string To { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<string> File { get; set; }
    }
}
