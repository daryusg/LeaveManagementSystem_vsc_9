using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations;

//users
//cip...141
public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasData(
            new ApplicationUser
            {
                Id = "8a862852-9e68-4bcc-b624-220e9b060cf9",
                Email = "admin_bu1@localhost.com",
                NormalizedEmail = "ADMIN_BU1@LOCALHOST.COM",
                UserName = "admin_bu1@localhost.com",
                NormalizedUserName = "ADMIN_BU1@LOCALHOST.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEJ5jT7bufJvPmguGXW9QAbwUO4GGPlOPUpZDSdL3J5uk9pgBbth4gkSONtFF3+A8kg==", //hasher.HashPassword(null, "P@ssw0rd"),
                EmailConfirmed = true,
                SecurityStamp = "8dd5bb17-2a8e-4998-a66b-8611e32a9b9a", //Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = "bff9869ba62a4a4986479a91c0d6890b", //Guid.NewGuid().ToString("N")
                FirstName = "Admin_Bu1",
                LastName = "Default",
                DateOfBirth = new DateOnly(1991, 07, 01)
            },
            new ApplicationUser
            {
                Id = "a23d75b8-c842-4164-9cb1-f9e7c2366c3b",
                Email = "testuser@leavemanagement.com",
                NormalizedEmail = "TESTUSER@LEAVEMANAGEMENT.COM",
                UserName = "testuser@leavemanagement.com",
                NormalizedUserName = "TESTUSER@LEAVEMANAGEMENT.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEFmo3ccUYE1zfv2SyMqUSjAxLxupY6bcQEuHLVIgF+ShU/lOkD2lbsPYrlcXNnKNnQ==", //hasher.HashPassword(null, "P@ssw0rd"),
                EmailConfirmed = true,
                SecurityStamp = "49123653-2d14-4579-a213-e8263f280755", //Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = "939075471b23410b832121d4f56ebeb3", //Guid.NewGuid().ToString("N")
                FirstName = "test",
                LastName = "user",
                DateOfBirth = new DateOnly(1992, 07, 01)
            }
        );
    }
}