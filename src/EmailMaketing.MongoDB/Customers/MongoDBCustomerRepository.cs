using EmailMaketing.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace EmailMaketing.Customers
{
    public class MongoDBCustomerRepository : MongoDbRepository<EmailMaketingMongoDbContext, Customer, Guid>, ICustomerRepository
    {
        public MongoDBCustomerRepository(IMongoDbContextProvider<EmailMaketingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Customer> FindByCustomerWithIDAsync(Guid id)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> FindByCustomerWithUserIDAsync(Guid id)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable.FirstOrDefaultAsync(c => c.UserID == id);
        }

        public async Task<List<Customer>> GetListAsync(int SkipCount, int maxResultCount, string sorting, string filter)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable
                .WhereIf<Customer, IMongoQueryable<Customer>>(
                    !filter.IsNullOrWhiteSpace(),
                    customer => customer.FullName.Contains(filter)
                ).OrderBy(sorting)
                .As<IMongoQueryable<Customer>>()
                .Skip(SkipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
