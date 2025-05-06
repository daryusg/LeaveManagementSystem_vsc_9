namespace LeaveManagementSystem.Application.Models.LeaveRequests;

public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM //cip...158
{
    public EmployeeVM Employee { get; set; } = new EmployeeVM();
    [DisplayName("Additional Information")] //cip...160
    public string RequestComments { get; set; } //cip...159
    //-----   03/05/25   ------------------
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    //-----   03/05/25   ------------------
}