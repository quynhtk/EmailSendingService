using EmailMaketing.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EmailMaketing.Permissions;

public class EmailMaketingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //Define your own permissions here. Example:
        //myGroup.AddPermission(EmailMaketingPermissions.MyPermission1, L("Permission:MyPermission1"));

        var emailMaketingGroup = context.AddGroup(EmailMaketingPermissions.GroupName, L("Permission:EmailMaketing"));
        var emailMaketingPermission = emailMaketingGroup.AddPermission(EmailMaketingPermissions.Customers.Default, L("Permission:Customers"));
        emailMaketingPermission.AddChild(EmailMaketingPermissions.Customers.Create, L("Permission:Customers.Create"));
        emailMaketingPermission.AddChild(EmailMaketingPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        emailMaketingPermission.AddChild(EmailMaketingPermissions.Customers.Delete, L("Permission:Customers.Delete"));


        var senderEmailPermission = emailMaketingGroup.AddPermission(EmailMaketingPermissions.SenderEmails.Default, L("Permission:SenderEmails"));
        senderEmailPermission.AddChild(EmailMaketingPermissions.SenderEmails.Create, L("Permission:SenderEmails.Create"));
        senderEmailPermission.AddChild(EmailMaketingPermissions.SenderEmails.Edit, L("Permission:SenderEmails.Edit"));
        senderEmailPermission.AddChild(EmailMaketingPermissions.SenderEmails.Delete, L("Permission:SenderEmails.Delete"));

        var contentEmailPermission = emailMaketingGroup.AddPermission(EmailMaketingPermissions.ContentEmails.Default, L("Permission:ContentEmails"));
        contentEmailPermission.AddChild(EmailMaketingPermissions.ContentEmails.Create, L("Permission:ContentEmails.Create"));
        contentEmailPermission.AddChild(EmailMaketingPermissions.ContentEmails.Edit, L("Permission:ContentEmails.Edit"));
        contentEmailPermission.AddChild(EmailMaketingPermissions.ContentEmails.Delete, L("Permission:ContentEmails.Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EmailMaketingResource>(name);
    }
}
