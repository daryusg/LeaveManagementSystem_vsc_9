namespace LeaveManagementSystem.Data;

// cip...120
public class Period : BaseEntity
{
    [MaxLength(50)]
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    //-----   03/05/25   ------------------
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; } //null when record is created.
    public DateTime? ModifiedDate { get; set; } //null when record is created.
    //-----   03/05/25   ------------------
}
