using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.Customers
{
    public class GetCustomerInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
