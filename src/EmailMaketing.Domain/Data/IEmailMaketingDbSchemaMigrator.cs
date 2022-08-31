using System.Threading.Tasks;

namespace EmailMaketing.Data;

public interface IEmailMaketingDbSchemaMigrator
{
    Task MigrateAsync();
}
