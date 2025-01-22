using System;
using NuGet.Protocol;

namespace LeaveManagementSystem.Web.Common;

public class Functions(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor) : IFunctions
{
    public Task<IList<ApplicationUser>> GetUsers(string role) //my code cip...162
    {
        var users = _userManager.GetUsersInRoleAsync(role);
        return users;
    }

    public async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays) //cip...162
    {
        var numberOfDays = (leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber) + 1;
        var allocation = await GetCurrentAllocationAsync(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);

        if(deductDays)
            allocation.Days -= numberOfDays;
        else
            allocation.Days += numberOfDays;
        _context.Entry(allocation).State = EntityState.Modified;
    }

    public async Task<LeaveAllocation> GetCurrentAllocationAsync(int leaveTypeId, string employeeId) //cip...162
    {
        var period = await GetCurrentPeriodAsync();
        var allocation = await _context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == leaveTypeId
            && q.EmployeeId == employeeId
            && q.PeriodId == period.Id);
        return allocation;
    }

    public async Task<Period> GetCurrentPeriodAsync() //my code cip...161
    {
        //get the current period based on, in this case, the year. copied from LeaveAllocations.AllocateLeaveAsync.
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
        return period;
    }

    public async Task<ApplicationUser> GetEmployeeAsync(string? employeeId = null) //my code LeaveAllocationService.cs. cip...128
    {
        var user = string.IsNullOrEmpty(employeeId) //cip...131
            ? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            : await _userManager.FindByIdAsync(employeeId);

        return user;
    }
    //-------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------
    public async Task<string> GetEmployeeIdAsync() //my code cip...127
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        return user.Id;
    }

    //cip...126...moved when made private cip..131
    public async Task<List<LeaveAllocation>> GetAllocationsAsync(string employeeId) //cip...131 made private
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

    public async Task<bool> AllocationExists(string employeeId, int periodId, int leaveTypeId) //cip...132
    {
        var exists = await _context.LeaveAllocations.AnyAsync(q => q.EmployeeId == employeeId && q.PeriodId == periodId && q.LeaveTypeId == leaveTypeId);
        return (exists);
    }
}
