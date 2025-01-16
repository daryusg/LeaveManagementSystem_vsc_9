using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations;

//cip...141
public class LeaveRequestStatusConfig : IEntityTypeConfiguration<LeaveRequestStatus>
{
    public void Configure(EntityTypeBuilder<LeaveRequestStatus> builder)
    {
        builder.HasData(
            new LeaveRequestStatus
            {
                Id = 1,
                Name = "Pending"
            },
            new LeaveRequestStatus
            {
                Id = 2,
                Name = "Approved"
            },
            new LeaveRequestStatus
            {
                Id = 3,
                Name = "Declined"
            },
            new LeaveRequestStatus
            {
                Id = 4,
                Name = "Cancelled"
            }
        );
    }
}
