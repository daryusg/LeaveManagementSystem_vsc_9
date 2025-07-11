namespace LeaveManagementSystem.Data;

using Microsoft.AspNetCore.Identity;

public class ApplicationRole : IdentityRole //17/07/25
{
    public int Level { get; set; }
}
