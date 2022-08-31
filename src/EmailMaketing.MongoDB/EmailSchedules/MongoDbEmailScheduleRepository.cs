using EmailMaketing.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace EmailMaketing.EmailSchedules
{
    public class MongoDbEmailScheduleRepository : MongoDbRepository<EmailMaketingMongoDbContext, EmailSchedule, Guid>, IEmailScheduleRepository
    {
        public MongoDbEmailScheduleRepository(IMongoDbContextProvider<EmailMaketingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
