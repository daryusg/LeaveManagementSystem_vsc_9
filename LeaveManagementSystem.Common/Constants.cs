/*
    IMPORTANT: Due to a circular ref issue, this file is in 2 places:
        1. LeaveManagementSystem.Data. Used by LeaveManagementSystem.Data.IdentityRoleConfig.cs
        2. LeaveManagementSystem.Common. Used everywhere else

        BOTH NEED UPDATING IN SYNC.
*/
namespace LeaveManagementSystem.Common;

public static class Constants
{
    public const string cSolutionName = "Leave Management System";
    
    public static class Roles //cip...112 Constants.Roles.cAdministrator    Constants.Roles.cSupervisor    Constants.Roles.cEmployee
    {
        public const string cAdministrator = "Administrator";
        public const string cSupervisor = "Supervisor";
        public const string cEmployee = "Employee";
    }

    public static class Policies //cip...165 Constants.Policies.cAdminSupervisorOnly
    {
        public const string cAdminSupervisorOnly = "AdminSupervisorOnly";
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
