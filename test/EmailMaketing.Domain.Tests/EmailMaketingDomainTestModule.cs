using EmailMaketing.MongoDB;
using Volo.Abp.Modularity;

namespace EmailMaketing;

[DependsOn(
    typeof(EmailMaketingMongoDbTestModule)
    )]
public class EmailMaketingDomainTestModule : AbpModule
{

}
