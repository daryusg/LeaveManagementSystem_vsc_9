using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationsController (ILeaveAllocationsService _leaveAllocationsService): Controller
    {
        // GET: LeaveAllocationsController
        public ActionResult Index()
        {
            var employeeId = "";
            _leaveAllocationsService.GetAllocations(employeeId);
            return View();
        }

    }
}
