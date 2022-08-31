using System;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.ContentEmails
{
    public class GetContentEmailInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
