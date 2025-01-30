namespace LeaveManagementSystem.Application.Services.LeaveAllocations;

//cip...124
public class LeaveAllocationsService(ApplicationDbContext _context, IMapper _mapper, IFunctions _functions) : ILeaveAllocationsService
{
    public async Task AllocateLeaveAsync(string employeeId) //cip...130 i need the employeeId param as this routine is used at registration and therefore the user is NOT logged in
    {
        //NOTE: the following line can't be used here as we use this routine at registration and therefore the user is NOT logged in
        //var employeeId = await GetEmployeeIdAsync(); //my code cip...127
        //var employee = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        //cip...132 don't add leave types that already exist. option 2. i added 
        //get all the leave types
        var leaveTypes = await _context.LeaveTypes
            .Where(q1 => !q1.LeaveAllocations.Any(q2 => q2.EmployeeId == employeeId)) //14/01/25 (??) ToDo: rewatch and dismantle this to gain comprehension
            //IMHO: use the LeaveAllocation-LeaveType join to get all leavetypes with no leave allocations for this emloyee.
            .ToListAsync();

        //get the current period based on, in this case, the year
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
            var accrualRate = decimal.Divide(leaveType.NumberOfDays, Data.Constants.cMonthsPerYear); //cip...125
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
            employeeId = await _functions.GetEmployeeIdAsync(); //get the id of the logged in user
        var user = await _functions.GetEmployeeAsync(employeeId); //get _httpContextAccessor.HttpContext?.User details

        var allocations = await _functions.GetAllocationsAsync(employeeId);
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
        var users = await _functions.GetUsers(Data.Constants.Roles.cEmployee);
        var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeVM>>(users.ToList()); //cip...131 NOTE: users is IList
        return employees;
    }

    public async Task<LeaveAllocationEditVM> GetEmployeeAllocationAsync(int allocationId) //cip...134
    {
        var allocation = await _context.LeaveAllocations
            .Include(q => q.LeaveType)
            .Include(q => q.Employee)
            .FirstOrDefaultAsync(q => q.Id == allocationId);

        var model = _mapper.Map<LeaveAllocationEditVM>(allocation);
        return model;
    }

    public async Task EditAllocationAsync(LeaveAllocationEditVM allocationEditVM) //cip...134
    {
        //option 1
        //var leaveAllocation = await GetEmployeeAllocationAsync(allocationEditVM.Id) ?? throw new Exception("Leave allocation record does not exist."); //option 1
        //option 2
        // if(leaveAllocation == null)
        // {
        //     throw new Exception("Leave allocation record does not exist.");
        // }
        //leaveAllocation.Days = allocationEditVM.Days;
        //option 1a _context.Update(leaveAllocation); //update all the fields
        //option 1b _context.Entry(leaveAllocation).State = EntityState.Modified; //update the modified fields
        //await _context.SaveChangesAsync();
        //option 2
        await _context.LeaveAllocations
            .Where(q => q.Id == allocationEditVM.Id)
            .ExecuteUpdateAsync(s1 => s1.SetProperty(s2 => s2.Days, allocationEditVM.Days));
    }
}
