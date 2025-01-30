namespace LeaveManagementSystem.Data;

//cip...140
public class LeaveRequestStatus : BaseEntity //BaseEntity has Id
{
    [StringLength(50)]
    public string Name { get; set; }
}