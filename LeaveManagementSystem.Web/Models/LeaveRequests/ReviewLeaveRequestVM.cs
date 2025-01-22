namespace LeaveManagementSystem.Web.Models.LeaveRequests;

public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM //cip...158
{
    public EmployeeVM Employee { get; set; } = new EmployeeVM();
    [DisplayName("Additional Information")] //cip...160
    public string RequestComments { get; set; } //cip...159
}