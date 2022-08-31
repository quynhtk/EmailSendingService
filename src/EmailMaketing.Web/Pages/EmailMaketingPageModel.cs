using EmailMaketing.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EmailMaketing.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class EmailMaketingPageModel : AbpPageModel
{
    protected EmailMaketingPageModel()
    {
        LocalizationResourceType = typeof(EmailMaketingResource);
    }
}
