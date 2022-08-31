using Volo.Abp.Settings;

namespace EmailMaketing.Settings;

public class EmailMaketingSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EmailMaketingSettings.MySetting1));
    }
}
