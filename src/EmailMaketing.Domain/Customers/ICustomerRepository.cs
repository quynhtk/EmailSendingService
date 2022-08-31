using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EmailMaketing.Customers
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<List<Customer>> GetListAsync(
                int SkipCount,
                int maxResultCount,
                string sorting,
                string filter
            );
        Task<Customer> FindByCustomerWithUserIDAsync(Guid id);
        Task<Customer> FindByCustomerWithIDAsync(Guid id);
    }
}
