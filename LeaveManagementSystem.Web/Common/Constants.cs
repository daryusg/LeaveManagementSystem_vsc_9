namespace LeaveManagementSystem.Web.Common;

public static class Constants
{
    public static class Roles //cip...112 Constants.Roles.cAdministrator    Constants.Roles.cSupervisor    Constants.Roles.cEmployee
    {
        public const string cAdministrator = "Administrator";
        public const string cSupervisor = "Supervisor";
        public const string cEmployee = "Employee";
    }

    public static int cMonthsPerYear = 12; //cip...125

    public enum LeaveRequestStatusEnum //cip...145 (was LeaveRequestStatus but LeaveRequestStatusEnum in git. LeaveRequestStatus is, also, a table so not auto deternmined by vsc)
    {
        Pending = 1,
        Approved,
        Declined,
        Cancelled
    }
}
