using Constants = LeaveManagementSystem.Data.Constants;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationsController (ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService, IFunctions _functions): Controller
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

            var allocationEditVM = await _leaveAllocationsService.GetEmployeeAllocationAsync(id.Value);
            if (allocationEditVM == null)
            {
                return NotFound();
            }
            return View(allocationEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            //if (await _leaveTypesService.DaysExceedMaximum(allocation.LeaveType.Id, allocation.Days)) //cip...135
            var maxDaysForLeaveType = await _leaveTypesService.GetMaxDaysForLeaveType(allocation.LeaveType.Id); //cip...135
            if (maxDaysForLeaveType < allocation.Days)
            {
                ModelState.AddModelError("Days", $"The allocation exceeds the maximum leave type value ({maxDaysForLeaveType}).");
            }

            if (ModelState.IsValid)
            {
                await _leaveAllocationsService.EditAllocationAsync(allocation);
                return RedirectToAction(nameof(Details), new { employeeId = allocation.Employee.Id });
            }

            //cip...135. restoring the form to its original state (see above: EditAllocation(int? id)) + the updated days.
            var days = allocation.Days; //save the allocation.
            allocation = await _leaveAllocationsService.GetEmployeeAllocationAsync(allocation.Id); //reload the (original) allocation
            allocation.Days = days; //set the allocation with the updated days.
            return View(allocation);
        }

        public async Task<IActionResult> Details(string? employeeId)
        {
            var employeeLeaveAllocationsVM = await _leaveAllocationsService.GetEmployeeAllocationsAsync(employeeId);
            return View(employeeLeaveAllocationsVM);
        }

        //Cancel Editing Vacation Allocation
        public async Task<IActionResult> Cancel(string employeeId)
        {
            //ToDo: if the model.employeeId is different to _functions.GetEmployeeIdAsync() then i need to go back to LeaveAllocations.Details.cshtml
            if(employeeId != await _functions.GetEmployeeIdAsync())
                return RedirectToAction(nameof(Details), new { employeeId = employeeId}); //admin work
            else
                return RedirectToAction(nameof(Index)); //cip...148 tw informed that this should've been added before (cip...146?)
        }
    }
}
