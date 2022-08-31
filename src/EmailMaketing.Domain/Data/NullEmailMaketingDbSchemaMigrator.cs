using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EmailMaketing.Data;

/* This is used if database provider does't define
 * IEmailMaketingDbSchemaMigrator implementation.
 */
public class NullEmailMaketingDbSchemaMigrator : IEmailMaketingDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
