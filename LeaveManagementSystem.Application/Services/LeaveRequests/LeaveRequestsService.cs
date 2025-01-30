namespace LeaveManagementSystem.Application.Services.LeaveRequests;

public class LeaveRequestsService(IMapper _mapper, ApplicationDbContext _context, IFunctions _functions) : ILeaveRequestsService
{
    public async Task CancelLeaveRequestAsync(int Id) //cip...151/152
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(Id);
        leaveRequest.LeaveRequestStatusId = (int)Data.Constants.LeaveRequestStatusEnum.Cancelled;
        //restore the allocation days
        await _functions.UpdateAllocationDays(leaveRequest, false); //cip..162
        await _context.SaveChangesAsync();
    }

    //cip...145
    public async Task CreateLeaveRequestAsync(LeaveRequestCreateVM model)
    {
        //map data to leave request data model
        var leaveRequest = _mapper.Map<LeaveRequest>(model);
        //if it's not the admin tinkering then get the logged in employee id
        leaveRequest.EmployeeId = model.EmployeeId ?? await _functions.GetEmployeeIdAsync(); ;
        //set LeaveRequestStatusId to Pending
        leaveRequest.LeaveRequestStatusId = (int)Data.Constants.LeaveRequestStatusEnum.Pending;
        //save leave request
        //_context.LeaveRequests.Add(leaveRequest);
        //or
        _context.Add(leaveRequest);
        //deduct the allocation days
        await _functions.UpdateAllocationDays(leaveRequest, true); //cip..162
        await _context.SaveChangesAsync(); //if any of the previous ops failed then this won't save.
    }

    public async Task<EmployeeLeaveRequestsVM> GetAllLeaveRequestsAsync() //cip...156
    {
        var leaveRequests = await _context.LeaveRequests
            .Include(q => q.LeaveType) //join the LeaveType table
            .ToListAsync();

        //copied from GetEmployeeLeaveRequestsAsync. instead of using the _mapper object, i can to a new data type map on-the-fly
        var leaveRequestsReadOnlyVM = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
        {
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            Id = q.Id,
            LeaveType = q.LeaveType.Name,
            LeaveRequestStatus = (Data.Constants.LeaveRequestStatusEnum)q.LeaveRequestStatusId, //cip...150 cast to an enum
            NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber + 1
        }).ToList();

        var model = new EmployeeLeaveRequestsVM
        {
            PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)Data.Constants.LeaveRequestStatusEnum.Pending),
            ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)Data.Constants.LeaveRequestStatusEnum.Approved),
            DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)Data.Constants.LeaveRequestStatusEnum.Declined),
            TotalRequests = leaveRequests.Count,
            LeaveRequests = leaveRequestsReadOnlyVM
        };

        return model;
    }

    public async Task<IEnumerable<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync() //cip...150
    {
        var user = await _functions.GetEmployeeAsync();
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
            LeaveRequestStatus = (Data.Constants.LeaveRequestStatusEnum)q.LeaveRequestStatusId, //cip...150 cast to an enum
            NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber + 1
        });
        return model;
    }

    public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReviewAsync(int leaveRequestId) //cip...158
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(q => q.LeaveType) //join the LeaveType table
            .FirstAsync(q => q.Id == leaveRequestId); //cip...158 can't .include with a .find.

        var user = await _functions.GetEmployeeAsync(leaveRequest.EmployeeId);

        var model = new ReviewLeaveRequestVM
        {
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber + 1,
            LeaveRequestStatus = (Data.Constants.LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
            Id = leaveRequestId,
            LeaveType = leaveRequest.LeaveType.Name,
            RequestComments = leaveRequest.RequestComments, //cip...159
            Employee = new EmployeeVM
            {
                Id = leaveRequest.EmployeeId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        };
        return model;
    }

    public async Task<int> GetUsersMaxDaysForLeaveTypeAsync(int leaveTypeId) //cip...146
    {
        var user = await _functions.GetEmployeeAsync();

        var allocationToDecuct = await _context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == leaveTypeId && q.EmployeeId == user.Id);
        return allocationToDecuct.Days;
    }

    public async Task<bool> RequestDatesExceedAllocationAsync(LeaveRequestCreateVM model) //cip...146. must be public
    {
        //var user = await _functions.GetEmployeeAsync();
        var employeeId = model.EmployeeId ?? (await _functions.GetEmployeeAsync()).Id; //cip...164
        var periodId = (await _functions.GetCurrentPeriodAsync()).Id; //cip...161
        var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber + 1;
        var allocation = await _context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == employeeId && q.PeriodId == periodId);
        var ret = numberOfDays > allocation.Days;
        return ret;
    }

    public async Task ReviewLeaveRequestAsync(int leaveRequestId, bool approved) //cip...159
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId); //FindAsync = Use primary key
        leaveRequest.LeaveRequestStatusId = approved ? (int)Data.Constants.LeaveRequestStatusEnum.Approved : (int)Data.Constants.LeaveRequestStatusEnum.Declined;

        leaveRequest.ReviewerId = (await _functions.GetEmployeeAsync()).Id; //set the reviewer
        if (!approved) //if declined then give the days back to the employee
        {
            //restore the allocation days
            await _functions.UpdateAllocationDays(leaveRequest, false); //cip..162
        }
        await _context.SaveChangesAsync();
    }
}
