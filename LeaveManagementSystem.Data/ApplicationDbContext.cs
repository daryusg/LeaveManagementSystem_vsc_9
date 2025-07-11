using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace LeaveManagementSystem.Data;

//public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //cip...107. default user from default (IdentityUser) to ApplicationUser.
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> //11/07/25
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    private Guid _currentUserId; //18/06/25 from chatgpt. used to set CreatedBy and ModifiedBy.

    public void SetCurrentUser(Guid userId)
    {
        _currentUserId = userId;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // must be here

        // Apply your entity configurations
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    //18/06/25 taken from C:\Users\kev\projects_vscode\EntityFrameworkCore_vsc_9\EntityFrameworkCore.Data/FootballLeagueDbContext.cs & chatgpt
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow; //cip...57. use UTC to avoid timezone issues.
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = now;
                entry.Entity.CreatedBy =  _currentUserId; //set CreatedBy to the current user.
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDate = now;
                entry.Entity.ModifiedBy = _currentUserId; //set ModifiedBy to the current user.
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<LeaveType> LeaveTypes { get; set; } //cip...58
    public DbSet<Period> Periods { get; set; } //cip...121
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; } //cip...121
    public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; } //cip...140
    public DbSet<LeaveRequest> LeaveRequests { get; set; } //cip...140
}
