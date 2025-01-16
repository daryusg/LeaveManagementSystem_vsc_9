using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace LeaveManagementSystem.Web.Models.LeaveRequests;

//cip...143
public class LeaveRequestCreateVM : IValidatableObject
{
    [DisplayName("Start Date")] //cip...144
    [Required] //cip...146
    public DateOnly StartDate { get; set; }

    [DisplayName("End Date")] //cip...144
    [Required] //cip...146
    public DateOnly EndDate { get; set; }

    [DisplayName("Leave Type")] //cip...144
    [Required] //cip...146
    public int LeaveTypeId { get; set; }
    /*
    public LeaveRequestStatus? LeaveRequestStatus { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public int LeaveRequestStatusId { get; set; }
    public ApplicationUser? Employee { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public string EmployeeId { get; set; } = default!; //cip...140  default! default value !, null forgiving operator (https://learn.microsoft.com/en-us/answers/questions/1012207/c-what-is-the-meaning-of-default). from cip...124. fk property
    public ApplicationUser? Reviewer { get; set; } //cip...140. from cip...124. navigation property. it's best to make this nullable.
    public string? ReviewerId { get; set; } //cip...140. from cip...124. fk property. this needs to be nullable because it will be reviewed AFTER creation.
    */
    [DisplayName("Additional Information")] //cip...144
    [StringLength(250)] //cip...147
    public string? RequestComments { get; set; }
    public SelectList? LeaveTypes { get; set; } //cip...144 not in the datamodel so there's no mapping but it's not needed because this is for the SelectList/dropdown.

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate > EndDate)
        {
            //yield return new ValidationResult("The End Date cannot precede the Start Date.", new[] { nameof(StartDate), nameof(EndDate) });
            //or
            yield return new ValidationResult("The End Date cannot precede the Start Date.", [nameof(StartDate), nameof(EndDate)]);
        }
    }
}