namespace LeaveManagementSystem.Common;

public interface IFunctions
{
    Task<IList<ApplicationUser>> GetUsersAsync(string role); //my code cip...162
    Task UpdateAllocationDaysAsync(LeaveRequest leaveRequest, bool deductDays); //cip...162
    Task<LeaveAllocation> GetCurrentAllocationAsync(int leaveTypeId, string employeeId); //cip...162
    Task<Period> GetCurrentPeriodAsync(); //my code cip...161
    Task<ApplicationUser> GetEmployeeAsync(string? employeeId = null);
    string GetEmployeeId(); //03/05/25
    Task<string> GetEmployeeIdAsync();
    Task<bool> isAuthorisedAdminAsync(Guid guidCreatedBy); //03/05/25
    Task<List<LeaveAllocation>> GetAllocationsAsync(string employeeId);
    Task<bool> AllocationExistsAsync(string employeeId, int periodId, int leaveTypeId);
}
