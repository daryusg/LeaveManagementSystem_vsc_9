using LeaveManagementSystem.Web.Services.LeaveAllocations;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationsController (ILeaveAllocationsService _leaveAllocationsService): Controller
    {
        [Authorize(Roles = Constants.Roles.cAdministrator)]
        // GET: LeaveAllocationsController
        public async Task<ActionResult> Index() //cip...131
        {
            var employees = await _leaveAllocationsService.GetEmployeesAsync();
            return View(employees);
        }

        [Authorize(Roles = Constants.Roles.cAdministrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AllocateLeave(string? employeeId) //cip...132
        {
            await _leaveAllocationsService.AllocateLeaveAsync(employeeId);
            return RedirectToAction(nameof(Details), new {employeeId}); 
        }

        public async Task<ActionResult> Details(string? employeeId)
        {
            var employeeLeaveAllocationsVM = await _leaveAllocationsService.GetEmployeeAllocationsAsync(employeeId);
            return View(employeeLeaveAllocationsVM);
        }
    }
}
