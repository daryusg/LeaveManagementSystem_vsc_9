using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LeaveManagementSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //cip...107. default user from default (IdentityUser) to ApplicationUser.
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // must be here

        // Apply your entity configurations
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Register a DateOnly -> DateTime converter for all properties of type DateOnly 23/04/25 from chatgpt
        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d)
        );

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateOnly))
                {
                    property.SetValueConverter(dateOnlyConverter);
                }
            }
        }
    }

    public DbSet<LeaveType> LeaveTypes { get; set; } //cip...58
    public DbSet<Period> Periods { get; set; } //cip...121
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; } //cip...121
    public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; } //cip...140
    public DbSet<LeaveRequest> LeaveRequests { get; set; } //cip...140
}
