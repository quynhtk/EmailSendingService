using System;
using Volo.Abp.Application.Dtos;

namespace EmailMaketing.Customers
{
    public class GetCustomerTypeLookup : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
