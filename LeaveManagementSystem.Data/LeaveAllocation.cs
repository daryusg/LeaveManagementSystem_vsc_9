using System;

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
    
    public int Days_Original { get; set; } //03/05/25
    public int Days { get; set; }
    //-----   03/05/25   ------------------
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; } //null when record is created.
    public DateTime? ModifiedDate { get; set; } //null when record is created.
    //-----   03/05/25   ------------------
}
