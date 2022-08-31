using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.ContentEmails
{
    public class SenderLookup : EntityDto<Guid>
    {
        public String email { get; set; }
    }
}
