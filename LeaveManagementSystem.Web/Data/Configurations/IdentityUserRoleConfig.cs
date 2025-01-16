using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations;

//user-role
//cip...141
/*
ToDo 14/01/25 look into TKey (IdentityUserRoleConfig.cs):
class Microsoft.AspNetCore.Identity.IdentityUserRole<TKey> where TKey : System.IEquatable<TKey>
Represents the link between a user and a role.

TKey is string
*/
public class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
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
}
