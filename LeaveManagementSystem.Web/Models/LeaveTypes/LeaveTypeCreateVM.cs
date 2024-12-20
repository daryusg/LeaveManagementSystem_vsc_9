using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes;

public class LeaveTypeCreateVM
{
    [Required] //cip...81 it's not nullable so why???
    [Length(4,150,ErrorMessage ="Invalid field length (4-150)" )] //cip...81. ErrorMessage is nullable so i have to declare it explicitly. 
    public string Name { get; set; } = string.Empty; //cip...75
    public int Days { get; set; }
}
