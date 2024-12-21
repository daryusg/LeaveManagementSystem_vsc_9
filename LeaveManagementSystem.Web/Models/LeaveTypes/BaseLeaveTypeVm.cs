using System;

namespace LeaveManagementSystem.Web.Models.LeaveTypes;

public abstract class BaseLeaveTypeVm //cip...83. abstract because im never using it directly.
{
    public int Id { get; set; }
}
