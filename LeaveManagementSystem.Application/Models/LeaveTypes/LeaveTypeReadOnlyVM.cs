namespace LeaveManagementSystem.Application.Models.LeaveTypes;

public class LeaveTypeReadOnlyVM : BaseLeaveTypeVm
{
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty; //cip...75
    [Display(Name="Maximum allocation of days")] //cip...83
    public int Days { get; set; }
}
