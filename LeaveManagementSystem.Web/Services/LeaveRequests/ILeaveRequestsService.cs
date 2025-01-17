namespace LeaveManagementSystem.Web.Services.LeaveRequests;

//cip...142
public interface ILeaveRequestsService
{
    Task CreateLeaveRequestAsync(LeaveRequestCreateVM model);
    Task<IEnumerable<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync();
    Task<LeaveRequestsVm> GetAllLeaveRequestsAsync();
    Task CancelLeaveRequestAsync(int leaveRequestId);
    Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model);
    Task<bool> RequestDatesExceedAllocationAsync(LeaveRequestCreateVM model); //cip...146
    Task<int> GetUsersMaxDaysForLeaveTypeAsync(int leaveTypeId); //my code cip...146
}
