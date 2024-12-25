using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) //cip...103
    {
        base.OnModelCreating(builder); //must be here

        //users
        //note: ConcurrencyStamp, PasswordHash & SecurityStamp need static values in ef 9 (https://www.udemy.com/course/complete-aspnet-core-31-and-entity-framework-development/learn/lecture/43693496#questions/22709311).
        var hasher = new PasswordHasher<IdentityUser>();
        builder.Entity<IdentityUser>().HasData(
            new IdentityUser
            {
                Id = "cb6397fe-acf8-49dd-b791-01bf0b069aee",
                Email = "admin@localhost.com",
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                UserName = "admin@localhost.com",
                NormalizedUserName = "ADMIN@LOCALHOST.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEN5WCPZ+e5Tcc6puplTNrflD+R6WpF82fsT2aCWMlDDmwAlhys5FMsfFVgax4+GI7Q==", //hasher.HashPassword(null, "P@ssw0rd"),
                EmailConfirmed = true,
                SecurityStamp = "26d82787-3f4c-4fe2-b6c6-1660a3c8d58a", //Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = "adb2ad4efab64d049c8a713991f0bd37" //Guid.NewGuid().ToString("N")
            },
            new IdentityUser
            {
                Id = "8a862852-9e68-4bcc-b624-220e9b060cf9",
                Email = "admin_bu1@localhost.com",
                NormalizedEmail = "ADMIN_BU1@LOCALHOST.COM",
                UserName = "admin_bu1@localhost.com",
                NormalizedUserName = "ADMIN_BU1@LOCALHOST.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEJ5jT7bufJvPmguGXW9QAbwUO4GGPlOPUpZDSdL3J5uk9pgBbth4gkSONtFF3+A8kg==", //hasher.HashPassword(null, "P@ssw0rd"),
                EmailConfirmed = true,
                SecurityStamp = "8dd5bb17-2a8e-4998-a66b-8611e32a9b9a", //Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = "bff9869ba62a4a4986479a91c0d6890b" //Guid.NewGuid().ToString("N")
            },
            new IdentityUser
            {
                Id = "a23d75b8-c842-4164-9cb1-f9e7c2366c3b",
                Email = "testuser@leavemanagement.com",
                NormalizedEmail = "TESTUSER@LEAVEMANAGEMENT.COM",
                UserName = "testuser@leavemanagement.com",
                NormalizedUserName = "TESTUSER@LEAVEMANAGEMENT.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEFmo3ccUYE1zfv2SyMqUSjAxLxupY6bcQEuHLVIgF+ShU/lOkD2lbsPYrlcXNnKNnQ==", //hasher.HashPassword(null, "P@ssw0rd"),
                EmailConfirmed = true,
                SecurityStamp = "49123653-2d14-4579-a213-e8263f280755", //Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = "939075471b23410b832121d4f56ebeb3" //Guid.NewGuid().ToString("N")
            }
        );

        //roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "39c75408-f38a-4bec-b52a-9aa5fcee5bc8",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            },
            new IdentityRole
            {
                Id = "2f26c8ac-3971-40df-bcce-ee90609328c6",
                Name = "Supervisor",
                NormalizedName = "SUPERVISOR"
            },
            new IdentityRole
            {
                Id = "f9080104-d003-43fe-b7b8-91c02c6bacd2",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
        );

        //user-role
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = "cb6397fe-acf8-49dd-b791-01bf0b069aee",
                RoleId = "f9080104-d003-43fe-b7b8-91c02c6bacd2"
            },
            new IdentityUserRole<string>
            {
                UserId = "8a862852-9e68-4bcc-b624-220e9b060cf9",
                RoleId = "f9080104-d003-43fe-b7b8-91c02c6bacd2"
            },
            new IdentityUserRole<string>
            {
                UserId = "a23d75b8-c842-4164-9cb1-f9e7c2366c3b",
                RoleId = "39c75408-f38a-4bec-b52a-9aa5fcee5bc8"
            }
        );
    }

    public DbSet<LeaveType> LeaveTypes { get; set; }
}
