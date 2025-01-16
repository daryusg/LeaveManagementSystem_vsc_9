using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Models.LeaveRequests;

//cip...143
public class LeaveRequestCreateVM
{
    [DisplayName("Start Date")] //cip...144
    public DateOnly StartDate { get; set; }
    [DisplayName("End Date")] //cip...144
    public DateOnly EndDate { get; set; }
    //public LeaveType? LeaveType { get; set; }
    [DisplayName("Leave Type")] //cip...144
    public int LeaveTypeId { get; set; }
    /*
    public LeaveRequestStatus? LeaveRequestStatus { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public int LeaveRequestStatusId { get; set; }
    public ApplicationUser? Employee { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public string EmployeeId { get; set; } = default!; //cip...140  default! default value !, null forgiving operator (https://learn.microsoft.com/en-us/answers/questions/1012207/c-what-is-the-meaning-of-default). from cip...124. fk property
    public ApplicationUser? Reviewer { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public string? ReviewerId { get; set; } //cip...140. from cip...124. fk property. this needs to be nullable because it will be reviewed AFTER creation.
    */
    [DisplayName("Additional Information")] //cip...144
    public string? RequestComments { get; set; }
    public SelectList? LeaveTypes { get; set; } //cip...144 not in the datamodel so there's no mapping but it's not needed because this is for the SelectList/dropdown.
}