using System;
using System.Collections.Generic;
using System.Text;
using EmailMaketing.Localization;
using Volo.Abp.Application.Services;

namespace EmailMaketing;

/* Inherit your application services from this class.
 */
public abstract class EmailMaketingAppService : ApplicationService
{
    protected EmailMaketingAppService()
    {
        LocalizationResource = typeof(EmailMaketingResource);
    }
}
