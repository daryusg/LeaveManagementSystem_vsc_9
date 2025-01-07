namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public interface ILeaveAllocationsService
{
    Task AllocateLeaveAsync(string employeeId); //cip...124
    Task<List<LeaveAllocation>> GetAllocations(string employeeId); //cip...126
}
