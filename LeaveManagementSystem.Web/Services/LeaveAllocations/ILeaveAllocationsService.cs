namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public interface ILeaveAllocationsService
{
    Task AllocateLeaveAsync(string employeeId);

}
