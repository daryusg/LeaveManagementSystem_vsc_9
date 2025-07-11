namespace LeaveManagementSystem.Data;

// cip...120
public class Period : BaseEntity
{
    [MaxLength(50)]
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
