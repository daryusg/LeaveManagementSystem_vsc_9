using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configurations;

//roles
//cip...141
//public class IdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
public class IdentityRoleConfig : IEntityTypeConfiguration<ApplicationRole> //11/07/25
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = "39c75408-f38a-4bec-b52a-9aa5fcee5bc8",
                Name = Constants.Roles.cEmployee,
                NormalizedName = Constants.Roles.cEmployee.ToUpper(),
                Level = 1
            },
            new ApplicationRole
            {
                Id = "2f26c8ac-3971-40df-bcce-ee90609328c6",
                Name = Constants.Roles.cSupervisor,
                NormalizedName = Constants.Roles.cSupervisor.ToUpper(),
                Level = 3
            },
            new ApplicationRole
            {
                Id = "f9080104-d003-43fe-b7b8-91c02c6bacd2",
                Name = Constants.Roles.cAdministrator,
                NormalizedName = Constants.Roles.cAdministrator.ToUpper(),
                Level = 5
            }
        );
    }
}
