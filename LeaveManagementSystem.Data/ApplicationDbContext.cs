using System.Reflection;

namespace LeaveManagementSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //cip...107. default user from default (IdentityUser) to ApplicationUser.
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) //cip...103
    {
        base.OnModelCreating(builder); //must be here

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //cip...141
    }

    public DbSet<LeaveType> LeaveTypes { get; set; } //cip...58
    public DbSet<Period> Periods { get; set; } //cip...121
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; } //cip...121
    public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; } //cip...140
    public DbSet<LeaveRequest> LeaveRequests { get; set; } //cip...140
}
