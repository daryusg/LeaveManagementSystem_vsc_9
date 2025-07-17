namespace LeaveManagementSystem.Application.Models.LeaveAllocations;

public class EmployeeVM
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    public string? RoleName { get; set; }  // ✅ Added 11/07/25. 17/07/25 made nullable due to modalerror ((LeaveManagementSystem.Application\Models\LeaveAllocations\EmployeeVM.cs\EditAllocation(LeaveAllocationEditVM allocation))Edit Allocation)
    public int Level { get; set; }  // ✅ Added 11/07/25
}
