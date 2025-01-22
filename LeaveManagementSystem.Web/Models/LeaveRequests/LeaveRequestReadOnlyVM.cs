using System;

namespace LeaveManagementSystem.Web.Models.LeaveRequests;

//cip...149
public class LeaveRequestReadOnlyVM
{
    public int Id { get; set; }

    [DisplayName("Start Date")]
    public DateOnly StartDate { get; set; }

    [DisplayName("End Date")]
    public DateOnly EndDate { get; set; }

    [DisplayName("Total Days")]
    public int NumberOfDays { get; set; }

    [DisplayName("Leave Type")]
    public string LeaveType { get; set; } = string.Empty;

    [DisplayName("Status")]
    public Constants.LeaveRequestStatusEnum LeaveRequestStatus { get; set; }
}
