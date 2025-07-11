using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Application.Services.LeaveAllocations;

//cip...124
//public class LeaveAllocationsService(ApplicationDbContext _context, IMapper _mapper, IFunctions _functions) : ILeaveAllocationsService
public class LeaveAllocationsService(ApplicationDbContext _context, IMapper _mapper, IFunctions _functions
    , UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager) : ILeaveAllocationsService //11/07/25
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
        catch (Exception e)
        {
            throw new Exception("Invalid period data", e.InnerException);
        }
        //calculate leave based on the number of months left in the period
        var monthsRemaining = period.EndDate.Month - currentDate.Month;
        //for each leave type, create an allocation entry
        foreach (var leaveType in leaveTypes)
        {
            //cip...132 don't add leave types that already exist. option 2. works but not efficient
            //if(await AllocationExists(employeeId, period.Id, leaveType.Id))
            //    continue;
            var accrualRate = decimal.Divide(leaveType.NumberOfDays, Data.Constants.cMonthsPerYear); //cip...125
            var days = (int)Math.Ceiling(accrualRate * monthsRemaining); //cip...125
            var leaveAllocation = new LeaveAllocation
            {
                EmployeeId = employeeId,
                // LeaveType = leaveType, //navigation property
                LeaveTypeId = leaveType.Id, //fk property NOTE: tw's recommendation: use fk property. DON'T DO BOTH. do 1 or t'other.
                //check out tw's ef core course for full explanation (https://www.udemy.com/course/entity-framework-core-a-full-tour/?couponCode=NEWYEARCAREER).
                // Period = period, //navigation property
                PeriodId = period.Id, //fk property NOTE: tw's recommendation: use fk property. DON'T DO BOTH. do 1 or t'other.
                Days_Original = days, //03/05/25
                Days = days,
            };
            _context.Add(leaveAllocation);
        }
        //save to db once. all fail or none fail. this needs to be in line with the reqs.
        //18/06/25 saving userid for audit fields
        Guid userId = Guid.Parse(employeeId);
        _context.SetCurrentUser(userId);
        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync(string? employeeId) //cip...128
    {
        if (string.IsNullOrEmpty(employeeId)) //cip..131
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
        // var users = await _functions.GetUsersAsync(Data.Constants.Roles.cEmployee);
        // var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeVM>>(users.ToList()); //cip...131 NOTE: users is IListF
        var allUsers = _userManager.Users.ToList(); // or use ToListAsync() if IQueryable
        var allRoles = _roleManager.Roles.ToList(); // ApplicationRole with Level

        var employees = new List<EmployeeVM>();

        foreach (var user in allUsers)
        {
            var roleNames = await _userManager.GetRolesAsync(user);

            // Get matching ApplicationRole instances from the role names
            var userRoles = allRoles.Where(r => roleNames.Contains(r.Name)).ToList();

            // Get the highest level (Administrator = 3, Supervisor = 2, Employee = 1)
            // int highestLevel = userRoles.Any() ? userRoles.Max(r => (r as ApplicationRole)?.Level ?? 0) : 0;
            var topRole = userRoles
                .OfType<ApplicationRole>()                       // safely cast
                .OrderByDescending(r => r.Level)                 // highest level first
                .FirstOrDefault();                               // take the top one

            string roleName = topRole?.Name ?? "Unknown";
            int level = topRole?.Level ?? 0;

            employees.Add(new EmployeeVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleName = roleName,
                Level = level
            });
        }
        var orderedEmployees = employees
            .OrderBy(e => e.Level)
            .ThenBy(e => e.Email)
            .ToList();

        return orderedEmployees;
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
        //03/05/25 added modifiedby and modifieddate

        //18/06/25 saving userid for audit fields
        Guid userId = Guid.Parse(await _functions.GetEmployeeIdAsync());
        _context.SetCurrentUser(userId);
        await _context.LeaveAllocations
            .Where(q => q.Id == allocationEditVM.Id)
            .ExecuteUpdateAsync(s1 => s1.SetProperty(s2 => s2.Days, allocationEditVM.Days)
                .SetProperty(s2 => s2.ModifiedBy, new Guid(_functions.GetEmployeeId()))
                .SetProperty(s2 => s2.ModifiedDate, DateTime.UtcNow));
    }
}
