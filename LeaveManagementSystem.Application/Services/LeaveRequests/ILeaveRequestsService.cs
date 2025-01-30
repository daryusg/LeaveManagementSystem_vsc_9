namespace LeaveManagementSystem.Application.Services.LeaveRequests;

//cip...142
public interface ILeaveRequestsService
{
    Task CreateLeaveRequestAsync(LeaveRequestCreateVM model);
    Task<IEnumerable<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync();
    Task<EmployeeLeaveRequestsVM> GetAllLeaveRequestsAsync();
    Task CancelLeaveRequestAsync(int leaveRequestId);
    Task ReviewLeaveRequestAsync(int leaveRequestId, bool approved); //cip...159
    Task<bool> RequestDatesExceedAllocationAsync(LeaveRequestCreateVM model); //cip...146
    Task<int> GetUsersMaxDaysForLeaveTypeAsync(int leaveTypeId); //my code cip...146
    Task <ReviewLeaveRequestVM> GetLeaveRequestForReviewAsync(int leaveRequestId); //cip...158
}
