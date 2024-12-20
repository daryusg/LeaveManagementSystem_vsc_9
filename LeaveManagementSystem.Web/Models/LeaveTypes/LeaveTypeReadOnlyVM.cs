using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes;

public class LeaveTypeReadOnlyVM
{
    public int Id { get; set; } //cip...75
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty; //cip...75
    public int Days { get; set; }
}
