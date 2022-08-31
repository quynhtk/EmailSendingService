using EmailMaketing.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace EmailMaketing.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EmailMaketingMongoDbModule),
    typeof(EmailMaketingApplicationContractsModule)
    )]
public class EmailMaketingDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
