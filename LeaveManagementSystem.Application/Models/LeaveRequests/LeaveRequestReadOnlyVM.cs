namespace LeaveManagementSystem.Application.Models.LeaveRequests;

//cip...149
public class LeaveRequestReadOnlyVM
{
    public int Id { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")] //16/04/25 date standardisation
    [DisplayName("Start Date")]
    public DateOnly StartDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")] //16/04/25 date standardisation
    [DisplayName("End Date")]
    public DateOnly EndDate { get; set; }

    [DisplayName("Total Days")]
    public int NumberOfDays { get; set; }

    [DisplayName("Leave Type")]
    public string LeaveType { get; set; } = string.Empty;

    [DisplayName("Status")]
    public Data.Constants.LeaveRequestStatusEnum LeaveRequestStatus { get; set; }
}
