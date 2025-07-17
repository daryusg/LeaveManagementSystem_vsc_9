using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configurations;

//Periods
public class PeriodsConfig : IEntityTypeConfiguration<Period>
{
    public void Configure(EntityTypeBuilder<Period> builder)
    {
        Guid.TryParse("cb6397fe-acf8-49dd-b791-01bf0b069aee", out Guid guid); //admin@daryus.com 17/07/25 chatgpt
        DateTime createdDate = new DateTime(2025, 7, 17);

        builder.HasData(
            new Period
            {
                Id = 1,
                Name = "2024-2025",
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 12, 31),
                CreatedBy = guid,
                CreatedDate = createdDate
            },
            new Period
            {
                Id = 2,
                Name = "2025-2026",
                StartDate = new DateOnly(2025, 1, 1),
                EndDate = new DateOnly(2025, 12, 31),
                CreatedBy = guid,
                CreatedDate = createdDate
            }
        );
    }
}
