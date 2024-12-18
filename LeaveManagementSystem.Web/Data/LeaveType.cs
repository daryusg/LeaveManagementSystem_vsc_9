using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data;

public class LeaveType
{
    public int Id { get; set; } //cip..58 can also be LeaveTypeId. if i veer from naming convention then i can use the [Key] attribute to inform ef.
    
    [Column(TypeName = "nvarchar(150)")]
    public string Name { get; set; }
    public int NumberOfDays { get; set; }
}
