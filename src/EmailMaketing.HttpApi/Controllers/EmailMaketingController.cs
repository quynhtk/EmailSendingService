using EmailMaketing.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EmailMaketing.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EmailMaketingController : AbpControllerBase
{
    protected EmailMaketingController()
    {
        LocalizationResource = typeof(EmailMaketingResource);
    }
}
