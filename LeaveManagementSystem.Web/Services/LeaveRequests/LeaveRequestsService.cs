namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public class LeaveRequestsService (IMapper _mapper, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, ApplicationDbContext _context): ILeaveRequestsService
{
    public async Task CancelLeaveRequestAsync(int Id) //cip...151/152
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(Id);
        leaveRequest.LeaveRequestStatusId = (int)Constants.LeaveRequestStatusEnum.Cancelled;

        //add the requested days back to the allocation (reversed from CreateLeaveRequestAsync, however, logically, the request contains the employeeid)
        var numberOfDays = (leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber) + 1;
        var allocationToRestore = await _context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId && q.EmployeeId == leaveRequest.EmployeeId);
        allocationToRestore.Days += numberOfDays;

        await _context.SaveChangesAsync();
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
        leaveRequest.LeaveRequestStatusId = (int)Common.Constants.LeaveRequestStatusEnum.Pending;

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

    public async Task<IEnumerable<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync() //cip...150
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        var leaveRequests = await _context.LeaveRequests
            .Include(q => q.LeaveType) //join the LeaveType table
            .Where(q => q.EmployeeId == user.Id)
            .ToListAsync();
        //instead of using the _mapper object, i can to a new data type map on-the-fly
        var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
        {
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            Id = q.Id,
            LeaveType = q.LeaveType.Name,
            LeaveRequestStatus = (Constants.LeaveRequestStatusEnum)q.LeaveRequestStatusId, //cip...150 cast to an enum
            NumberOfDays = (q.EndDate.DayNumber - q.StartDate.DayNumber) + 1
        });
        return model;
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
