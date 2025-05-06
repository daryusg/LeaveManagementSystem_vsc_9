namespace LeaveManagementSystem.Data;

public class LeaveType : BaseEntity // cip...58. cip...120
{
    //cip...120 public int Id { get; set; } //cip...58 can also be LeaveTypeId. if i veer from naming convention then i can use the [Key] attribute to inform ef.
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public int NumberOfDays { get; set; }
    public List<LeaveAllocation>? LeaveAllocations { get; set; } //cip...132. option 2
    //-----   03/05/25   ------------------
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; } //null when record is created.
    public DateTime? ModifiedDate { get; set; } //null when record is created.
    //-----   03/05/25   ------------------
}
