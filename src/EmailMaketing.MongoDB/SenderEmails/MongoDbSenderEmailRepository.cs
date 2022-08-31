using EmailMaketing.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace EmailMaketing.SenderEmails
{
    public class MongoDbSenderEmailRepository :
        MongoDbRepository<EmailMaketingMongoDbContext, SenderEmail, Guid>,
        ISenderEmailRepository
    {
        public MongoDbSenderEmailRepository(IMongoDbContextProvider<EmailMaketingMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<SenderEmail> FindByEmailAsync(string email)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<SenderEmail> FindByIdAsync(Guid id)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SenderEmail>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync();
            return await query
                .WhereIf<SenderEmail, IMongoQueryable<SenderEmail>>(
                !filter.IsNullOrWhiteSpace(),
                senderemail => senderemail.Email.Contains(filter))
                .OrderByDescending(x => x.CreationTime)
                .As<IMongoQueryable<SenderEmail>>()
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
        public async Task<List<SenderWithNavigation>> GetListWithNavigationAsync(
          int skipCount,
          int maxResultCount,
          string sorting,
          string filter = null,
          CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync();
            var data = await query
                .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                senderemail => senderemail.Email.Contains(filter))
                .OrderByDescending(x => x.CreationTime)
                .As<IMongoQueryable<SenderEmail>>()
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken: cancellationToken);
            var db = await GetDbContextAsync(cancellationToken);

            var customer = db.Customers.AsQueryable();
            var dataWithNavigator = data.Select(x => new SenderWithNavigation()
            {
                SenderEmail = x,
                Customer = customer.FirstOrDefault(y => y.Id == x.CustomerID)
            });
            return dataWithNavigator.ToList();
        }
    }
}
