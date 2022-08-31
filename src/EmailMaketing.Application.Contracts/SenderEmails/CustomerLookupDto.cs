using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.SenderEmails
{
    public class CustomerLookupDto : EntityDto<Guid>
    {
        public string FullName  { get; set; }
    }
}
