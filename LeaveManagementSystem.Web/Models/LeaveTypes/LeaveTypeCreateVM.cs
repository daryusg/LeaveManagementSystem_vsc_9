using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes;

public class LeaveTypeCreateVM
{
    [Required] //cip...81 it's not nullable so why???
    [Length(4, 150, ErrorMessage ="Invalid field length (4-150)" )] //cip...81. ErrorMessage is nullable so i have to declare it explicitly. 
    public string Name { get; set; } = string.Empty; //cip...75

    [Required] //cip...81 it's not nullable so why???
    [Range(1, 90)] //cip...81
    [Display(Name="Maximum allocation of days")] //cip...83
    public int Days { get; set; }
}
