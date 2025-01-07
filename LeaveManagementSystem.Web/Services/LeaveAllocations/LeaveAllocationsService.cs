using System;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public class LeaveAllocationsService(ApplicationDbContext _context) : ILeaveAllocationsService
{
    //cip...124
    public async Task AllocateLeave(string employeeId)
    {
        //get all the leave types
        var leaveTypes = await _context.LeaveTypes.ToListAsync();

        //get the current period based on the year
        var currentDate = DateTime.Now;
        var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
        //calculate leave based on the number of months left in the period
        var monthsRemaining = period.EndDate.Month - currentDate.Month;
        //for each leave type, create an allocation entry
        foreach(var leaveType in leaveTypes)
        {
            var leaveAllocation = new LeaveAllocation
            {
                EmployeeId = employeeId,
                // LeaveType = leaveType, //navigation property
                LeaveTypeId = leaveType.Id, //fk property NOTE: tw's recommendation: use fk property. DON'T DO BOTH. do 1 or t'other.
                //check out tw's ef core course for full explanation (https://www.udemy.com/course/entity-framework-core-a-full-tour/?couponCode=NEWYEARCAREER).
                // Period = period, //navigation property
                PeriodId = period.Id, //fk property NOTE: tw's recommendation: use fk property. DON'T DO BOTH. do 1 or t'other.
                Days = leaveType.NumberOfDays / monthsRemaining
            };
            _context.Add(leaveAllocation);
        }
        //save to db once. all fail or none fail. this needs to be in line with the reqs.
        await _context.SaveChangesAsync();
    }
}
