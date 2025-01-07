using System;
using NuGet.Packaging.Signing;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public class LeaveAllocationsService(ApplicationDbContext _context) : ILeaveAllocationsService
{
    //cip...124
    public async Task AllocateLeaveAsync(string employeeId)
    {
        //get all the leave types
        var leaveTypes = await _context.LeaveTypes.ToListAsync();

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

    //cip...126
    public async Task<List<LeaveAllocation>> GetAllocations(string employeeId)
    {
        var allocations = await _context.LeaveAllocations
            .Include(q => q.LeaveType) //join the LeaveType table. fills in the LeaveType field
            .Include(q => q.Employee) //join the Employee table. fills in the Employee field
            .Include(q => q.Period) //join the Period table. fills in the Period field
            .Where(q => q.EmployeeId == employeeId)
            .ToListAsync(); //this is where it executes the query. cip...126
        return allocations;
    }
}
