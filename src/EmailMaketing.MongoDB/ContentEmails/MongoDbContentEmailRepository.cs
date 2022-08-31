using EmailMaketing.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace EmailMaketing.ContentEmails
{
    public class MongoDbContentEmailRepository : MongoDbRepository<EmailMaketingMongoDbContext, ContentEmail, Guid>, IContentEmailRepository
    {
        public MongoDbContentEmailRepository(IMongoDbContextProvider<EmailMaketingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<ContentEmail> FindByIdSenderEmailAsync(Guid id)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable.FirstOrDefaultAsync(c => c.SenderEmailID == id);
        }

        public async Task<List<ContentEmail>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable
                .OrderByDescending(x => x.CreationTime)
                .As<IMongoQueryable<ContentEmail>>()
                .ToListAsync();
        }
    }
}
