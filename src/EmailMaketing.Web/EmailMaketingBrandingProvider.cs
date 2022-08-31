using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace EmailMaketing.Web;

[Dependency(ReplaceServices = true)]
public class EmailMaketingBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "EmailMaketing";
}
