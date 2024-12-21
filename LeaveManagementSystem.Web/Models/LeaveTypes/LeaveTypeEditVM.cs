using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes;

public class LeaveTypeEditVM : BaseLeaveTypeVm
{
    [Length(4, 150, ErrorMessage ="Invalid field length (4-150)" )]
    public string Name { get; set; } = string.Empty;
    [Range(1, 90)]
    [Display(Name="Maximum allocation of days")] //cip...83
    public int Days { get; set; }
}

