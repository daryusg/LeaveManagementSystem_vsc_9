namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public interface ILeaveAllocationsService
{
    Task AllocateLeaveAsync(string employeeId); //cip...124
    Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync(string? employeeId); //cip...128/129/131
    Task<List<EmployeeVM>> GetEmployeesAsync(); //cip...131
    Task<LeaveAllocationEditVM> GetEmployeeAllocationAsync(int allocationId); //cip...133
    Task EditAllocationAsync(LeaveAllocationEditVM allocationEditVM); //cip...134
}
