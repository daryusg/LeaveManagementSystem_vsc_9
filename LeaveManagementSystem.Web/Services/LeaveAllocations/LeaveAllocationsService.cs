namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

//cip...124
public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
{
    public async Task AllocateLeaveAsync(string employeeId) //cip...130 i need the employeeId param as this routine is used at registration and therefore the user is NOT logged in
    {
        //NOTE: the following line can't be used here as we use this routine at registration and therefore the user is NOT logged in
        //var employeeId = await GetEmployeeIdAsync(); //my own cip...127
        //var employee = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        //cip...132 don't add leave types that already exist. option 2. i added 
        //get all the leave types
        var leaveTypes = await _context.LeaveTypes
            .Where(q1 => !q1.LeaveAllocations.Any(q2 => q2.EmployeeId == employeeId)) //14/01/25 (??) ToDo: rewatch and dismantle this to gain comprehension
            //IMHO: use the LeaveAllocation-LeaveType join to get all leavetypes with no leave allocations for this emloyee.
            .ToListAsync();

        //get the current period based on the year
        var currentDate = DateTime.Now;
        Period period;
        try
        {
            period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
        }
        catch(Exception e)
        {
            throw new Exception("Invalid period data", e.InnerException);
        }
        //calculate leave based on the number of months left in the period
        var monthsRemaining = period.EndDate.Month - currentDate.Month;
        //for each leave type, create an allocation entry
        foreach(var leaveType in leaveTypes)
        {
            //cip...132 don't add leave types that already exist. option 2. works but not efficient
            //if(await AllocationExists(employeeId, period.Id, leaveType.Id))
            //    continue;
            var accrualRate = decimal.Divide(leaveType.NumberOfDays, Constants.cMonthsPerYear); //cip...125
            var leaveAllocation = new LeaveAllocation
            {
                EmployeeId = employeeId,
                // LeaveType = leaveType, //navigation property
                LeaveTypeId = leaveType.Id, //fk property NOTE: tw's recommendation: use fk property. DON'T DO BOTH. do 1 or t'other.
                //check out tw's ef core course for full explanation (https://www.udemy.com/course/entity-framework-core-a-full-tour/?couponCode=NEWYEARCAREER).
                // Period = period, //navigation property
                PeriodId = period.Id, //fk property NOTE: tw's recommendation: use fk property. DON'T DO BOTH. do 1 or t'other.
                Days = (int)Math.Ceiling(accrualRate * monthsRemaining)  //cip...125
            };
            _context.Add(leaveAllocation);
        }
        //save to db once. all fail or none fail. this needs to be in line with the reqs.
        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync(string? employeeId) //cip...128
    {
        if(string.IsNullOrEmpty(employeeId)) //cip..131
            employeeId = await GetEmployeeIdAsync(); //get the id of the logged in user
        var user = await GetEmployeeAsync(employeeId); //get _httpContextAccessor.HttpContext?.User details

        var allocations = await GetAllocationsAsync(employeeId);
        var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
        
        var leaveTypesCount = await _context.LeaveTypes.CountAsync(); //cip...132
        var employeeVm = new EmployeeAllocationVM
        {
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id,
            LeaveAllocations = allocationVmList,
            IsCompletedAllocation = leaveTypesCount == allocations.Count
        };

        return employeeVm;
    }

    public async Task<List<EmployeeVM>> GetEmployeesAsync() //cip...131
    {
        var users = await _userManager.GetUsersInRoleAsync(Constants.Roles.cEmployee);
        var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeVM>>(users.ToList()); //cip...131 NOTE: users is IList
        return(employees);
    }
    //-------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------
    protected async Task<string> GetEmployeeIdAsync() //my own cip...127
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        return user.Id;
    }

    private async Task<ApplicationUser> GetEmployeeAsync(string? employeeId) //my own cip...128
    {
        var user = string.IsNullOrEmpty(employeeId) //cip...131
            ? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User)
            : await _userManager.FindByIdAsync(employeeId);

        return user;
    }

    //cip...126...moved when made private cip..131
    private async Task<List<LeaveAllocation>> GetAllocationsAsync(string employeeId) //cip...131 made private
    {
        var currentDate = DateTime.Now; //cip...130
        // option 1. cip...130. 2 queries
        // var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year); //cip...130
        // var leaveAllocations = await _context.LeaveAllocations
        //     .Include(q => q.LeaveType) //join the LeaveType table. fills in the LeaveType field via LeaveypeId
        //     //.Include(q => q.Employee) //cip...128 refactored. the EmployeeAllocationVM contains LeaveAllocations
        //     .Include(q => q.Period) //join the Period table. fills in the Period field via PeriodId
        //     .Where(q => q.EmployeeId == employeeId && q.PeriodId == period.Id)
        //     .ToListAsync(); //this is where it executes the query. cip...126
        // option 2. cip...130. 1 query
        var leaveAllocations = await _context.LeaveAllocations
            .Include(q => q.LeaveType) //join the LeaveType table. fills in the LeaveType field via LeaveypeId
            //.Include(q => q.Employee) //cip...128 refactored. the EmployeeAllocationVM contains LeaveAllocations
            .Include(q => q.Period) //join the Period table. fills in the Period field via PeriodId
            .Where(q => q.EmployeeId == employeeId && q.Period.EndDate.Year == currentDate.Year)
            .ToListAsync(); //this is where it executes the query. cip...126
        return leaveAllocations;
    }

    private async Task<bool> AllocationExists(string employeeId, int periodId, int leaveTypeId) //cip...132
    {
        var exists = await _context.LeaveAllocations.AnyAsync(q => q.EmployeeId == employeeId && q.PeriodId == periodId && q.LeaveTypeId ==leaveTypeId);
        return(exists);
    }
}
