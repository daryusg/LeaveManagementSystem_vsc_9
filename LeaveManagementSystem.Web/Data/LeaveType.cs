using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data;

public class LeaveType : BaseEntity // cip...120
{
    //cip...120 public int Id { get; set; } //cip...58 can also be LeaveTypeId. if i veer from naming convention then i can use the [Key] attribute to inform ef.
    public string Name { get; set; } = string.Empty;
    public int NumberOfDays { get; set; }
    public List<LeaveAllocation>? LeaveAllocations { get; set; } //cip...132. option 2
}
