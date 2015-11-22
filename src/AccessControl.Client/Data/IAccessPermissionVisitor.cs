namespace AccessControl.Client.Data
{
    internal interface IAccessPermissionVisitor
    {
        void Visit(PermanentUserAccess permission);
        void Visit(PermanentGroupAccess permission);
        void Visit(ScheduledUserAccess permission);
    }
}