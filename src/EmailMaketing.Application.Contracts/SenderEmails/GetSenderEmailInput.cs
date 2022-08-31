using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.SenderEmails
{
    public class GetSenderEmailInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
