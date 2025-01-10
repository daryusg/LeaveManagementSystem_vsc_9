using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationsController (ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService): Controller
    {
        [Authorize(Roles = Constants.Roles.cAdministrator)]
        // GET: LeaveAllocationsController
        public async Task<IActionResult> Index() //cip...131
        {
            var employees = await _leaveAllocationsService.GetEmployeesAsync();
            return View(employees);
        }

        [Authorize(Roles = Constants.Roles.cAdministrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? employeeId) //cip...132
        {
            await _leaveAllocationsService.AllocateLeaveAsync(employeeId);
            return RedirectToAction(nameof(Details), new {employeeId}); 
        }

        [Authorize(Roles = Constants.Roles.cAdministrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _leaveAllocationsService.GetEmployeeAllocationAsync(id.Value);
            if (allocation == null)
            {
                return NotFound();
            }
            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            if (await _leaveTypesService.DaysExceedMaximum(allocation.LeaveType.Id, allocation.Days))
            {
                ModelState.AddModelError("Days", "The allocation exceeds the maximum leave type value");
            }

            if (ModelState.IsValid)
            {
                await _leaveAllocationsService.EditAllocation(allocation);
                return RedirectToAction(nameof(Details), new { userId = allocation.Employee.Id });
            }

            var days = allocation.Days;
            allocation = await _leaveAllocationsService.GetEmployeeAllocation(allocation.Id);
            allocation.Days = days;
            return View(allocation);
        }

        public async Task<IActionResult> Details(string? employeeId)
        {
            var employeeLeaveAllocationsVM = await _leaveAllocationsService.GetEmployeeAllocationsAsync(employeeId);
            return View(employeeLeaveAllocationsVM);
        }
    }
}
