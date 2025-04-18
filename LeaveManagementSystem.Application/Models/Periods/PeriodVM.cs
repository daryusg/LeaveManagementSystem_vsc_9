namespace LeaveManagementSystem.Application.Models.Periods;

public class PeriodVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")] //16/04/25 date standardisation
    [DisplayName("Start Date")]
    public DateOnly StartDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")] //16/04/25 date standardisation
    [DisplayName("End Date")]
    public DateOnly EndDate { get; set; }
}
