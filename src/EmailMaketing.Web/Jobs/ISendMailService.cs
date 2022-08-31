using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMaketing.Jobs
{
    public interface ISendMailService
    {
        Task SendMail(SendEmailArgs args);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
