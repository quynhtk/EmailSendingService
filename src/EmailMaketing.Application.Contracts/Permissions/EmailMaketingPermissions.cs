namespace EmailMaketing.Permissions;

public static class EmailMaketingPermissions
{
    public const string GroupName = "EmailMaketing";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class SenderEmails
    {
        public const string Default = GroupName + ".SenderEmails";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class ContentEmails
    {
        public const string Default = GroupName + ".ContentEmails";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
