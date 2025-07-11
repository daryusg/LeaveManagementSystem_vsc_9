namespace LeaveManagementSystem.Data;

// cip...120
public abstract class BaseEntity
{
    public int Id { get; set; }
    //-----   03/05/25 then moved here 18/06/25  ------------------------
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; } //null when record is created.
    public DateTime? ModifiedDate { get; set; } //null when record is created.
    //-----   03/05/25   ------------------------------------------------
}
