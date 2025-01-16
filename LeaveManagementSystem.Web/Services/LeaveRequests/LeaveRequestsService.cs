namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public class LeaveRequestsService (IMapper _mapper, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, ApplicationDbContext _context): ILeaveRequestsService
{
    public async Task CancelLeaveRequestAsync(int leaveRequestId)
    {
        throw new NotImplementedException();
    }

    //cip...145
    public async Task CreateLeaveRequestAsync(LeaveRequestCreateVM model)
    {
        //map data to leave request data model
        var leaveRequest = _mapper.Map<LeaveRequest>(model);

        //get logged in employee id
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        leaveRequest.EmployeeId = user.Id;

        //set LeaveRequestStatusId to Pending
        leaveRequest.LeaveRequestStatusId = (int)Common.Constants.LeaveRequestStatus.Pending;

        //save leave request
        //_context.LeaveRequests.Add(leaveRequest);
        //or
        _context.Add(leaveRequest);

        //deduct requested days from allocation
        var numberOfDays = (model.EndDate.DayNumber - model.StartDate.DayNumber) + 1;
        var allocationToDecuct = await _context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id);
        allocationToDecuct.Days -= numberOfDays;

        await _context.SaveChangesAsync(); //if any of the previous ops failed then this won't save.
    }

    public async Task<LeaveRequestsVm> GetAllLeaveRequestsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<EmployeeLeaveRequestsVM> GetEmployeeLeaveRequestsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetUsersMaxDaysForLeaveTypeAsync(int leaveTypeId) //cip...146
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        
        var allocationToDecuct = await _context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == leaveTypeId && q.EmployeeId == user.Id);
        return allocationToDecuct.Days;
    }

    public async Task<bool> RequestDatesExceedAllocationAsync(LeaveRequestCreateVM model) //cip...146
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        
        var numberOfDays = (model.EndDate.DayNumber - model.StartDate.DayNumber) + 1;
        var allocationToDecuct = await _context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id);
        return numberOfDays > allocationToDecuct.Days;
    }

    public async Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model)
    {
        throw new NotImplementedException();
    }
}
