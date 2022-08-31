using Volo.Abp.Modularity;

namespace EmailMaketing;

[DependsOn(
    typeof(EmailMaketingApplicationModule),
    typeof(EmailMaketingDomainTestModule)
    )]
public class EmailMaketingApplicationTestModule : AbpModule
{

}
