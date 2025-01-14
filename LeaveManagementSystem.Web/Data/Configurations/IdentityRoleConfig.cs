using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations;

//roles
//cip...141
public class IdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "39c75408-f38a-4bec-b52a-9aa5fcee5bc8",
                Name = Constants.Roles.cEmployee,
                NormalizedName = Constants.Roles.cEmployee.ToUpper()
            },
            new IdentityRole
            {
                Id = "2f26c8ac-3971-40df-bcce-ee90609328c6",
                Name = Constants.Roles.cSupervisor,
                NormalizedName = Constants.Roles.cSupervisor.ToUpper()
            },
            new IdentityRole
            {
                Id = "f9080104-d003-43fe-b7b8-91c02c6bacd2",
                Name = Constants.Roles.cAdministrator,
                NormalizedName = Constants.Roles.cAdministrator.ToUpper()
            }
        );
    }
}
