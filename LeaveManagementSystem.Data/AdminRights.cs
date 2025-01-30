namespace LeaveManagementSystem.Data;

public class AdminRights : BaseEntity
{
    //my code cip...163
    public ApplicationUser? RequestedBy { get; set; }
    public string RequestedById { get; set; }
    public DateTime RequestedByDate { get; set; }
    public ApplicationUser? ApprovedBy { get; set; }
    public string? ApprovedId { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public ApplicationUser? RevokedBy { get; set; }
    public string? RevokedById { get; set; }
    public DateTime? RevokedByDate { get; set; }
    public string Commenmts { get; set; } = string.Empty;
}
