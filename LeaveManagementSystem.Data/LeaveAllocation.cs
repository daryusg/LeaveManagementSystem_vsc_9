namespace LeaveManagementSystem.Data;

// cip...120
public class LeaveAllocation : BaseEntity
{
    public LeaveType? LeaveType { get; set; } //cip...124. navigation property. it's best to make this nullable.
    public int LeaveTypeId { get; set; } //cip...124. fk property
    
    public ApplicationUser? Employee { get; set; } //cip...124. navigation property. it's best to make this nullable.
    public string EmployeeId { get; set; } //cip...124. fk property

    public Period? Period { get; set; } //cip...124. navigation property. it's best to make this nullable.
    public int PeriodId { get; set; } //cip...124. fk property
    
    public int Days { get; set; }
}
