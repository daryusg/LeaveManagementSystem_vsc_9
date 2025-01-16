namespace LeaveManagementSystem.Web.Services.LeaveRequests;

//cip...142
public interface ILeaveRequestsService
{
    Task CreateLeaveRequestAsync(LeaveRequestCreateVM model);
    Task<EmployeeLeaveRequestsVM> GetEmployeeLeaveRequestsAsync();
    Task<LeaveRequestsVm> GetAllLeaveRequestsAsync();
    Task CancelLeaveRequestAsync(int leaveRequestId);
    Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model);
}
