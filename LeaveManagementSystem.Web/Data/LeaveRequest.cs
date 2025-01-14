namespace LeaveManagementSystem.Web.Data;

//cip...140
public class LeaveRequest : BaseEntity
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public LeaveType? LeaveType { get; set; }
    public int LeaveTypeId { get; set; }
    public LeaveRequestStatus? LeaveRequestStatus { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public int LeaveRequestStatusId { get; set; }
    public ApplicationUser? Employee { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public string EmployeeId { get; set; } = default!; //cip...140  default! default value !, null forgiving operator (https://learn.microsoft.com/en-us/answers/questions/1012207/c-what-is-the-meaning-of-default). from cip...124. fk property
    public ApplicationUser? Reviewer { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public string? ReviewerId { get; set; } //cip...140. from cip...124. fk property. this needs to be nullable because it will be reviewed AFTER creation.
    public string? RequestComments { get; set; }
}  