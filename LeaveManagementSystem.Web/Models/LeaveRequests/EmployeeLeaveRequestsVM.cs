namespace LeaveManagementSystem.Web.Models.LeaveRequests;

//cip...156
public class EmployeeLeaveRequestsVM
{
    [Display(Name="Total")]
    public int TotalRequests { get; set; }
    [Display(Name="Pending Requests")]
    public int PendingRequests { get; set; }
    [Display(Name="Approved Requests")]
    public int ApprovedRequests { get; set; }
    [Display(Name="Rejected Requests")]
    public int DeclinedRequests { get; set; }
   public List<LeaveRequestReadOnlyVM> LeaveRequests { get; set; } = [];
}