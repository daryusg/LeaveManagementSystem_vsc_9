namespace LeaveManagementSystem.Data;

//cip...140
public class LeaveRequestStatus //18/06/25 no need to inherit from BaseEntity as i don't need the audit fields
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
}