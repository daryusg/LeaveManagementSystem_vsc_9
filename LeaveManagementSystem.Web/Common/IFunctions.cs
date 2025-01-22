using System;

namespace LeaveManagementSystem.Web.Common;

public interface IFunctions
{
    Task<IList<ApplicationUser>> GetUsers(string role); //my code cip...162
    Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays); //cip...162
    Task<LeaveAllocation> GetCurrentAllocationAsync(int leaveTypeId, string employeeId); //cip...162
    Task<Period> GetCurrentPeriodAsync(); //my code cip...161
    Task<ApplicationUser> GetEmployeeAsync(string? employeeId = null);
    Task<string> GetEmployeeIdAsync();
    Task<List<LeaveAllocation>> GetAllocationsAsync(string employeeId);
    Task<bool> AllocationExists(string employeeId, int periodId, int leaveTypeId);
}
