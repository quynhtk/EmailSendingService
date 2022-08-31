using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EmailMaketing.MongoDB;

[DependsOn(
    typeof(EmailMaketingTestBaseModule),
    typeof(EmailMaketingMongoDbModule)
    )]
public class EmailMaketingMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var stringArray = EmailMaketingMongoDbFixture.ConnectionString.Split('?');
        var connectionString = stringArray[0].EnsureEndsWith('/') +
                                   "Db_" +
                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });
    }
}
