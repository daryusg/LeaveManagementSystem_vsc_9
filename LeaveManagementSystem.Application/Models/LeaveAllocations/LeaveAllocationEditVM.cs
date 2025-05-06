namespace LeaveManagementSystem.Application.Models.LeaveAllocations;

public class LeaveAllocationEditVM : LeaveAllocationVM
{
    public EmployeeVM? Employee { get; set; }
    //-----   03/05/25   ------------------
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    //-----   03/05/25   ------------------
}
