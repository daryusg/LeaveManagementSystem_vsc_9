using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    //cip...143
    [Authorize]
    public class LeaveRequestsController (ILeaveTypesService _leaveTypesService, ILeaveRequestsService _leaveRequestsService, IFunctions _functions) : Controller
    {
        //Employee View Requests
        // GET: LeaveRequestsController
        public async Task<IActionResult> Index() //cip...150
        {
            var model = await _leaveRequestsService.GetEmployeeLeaveRequestsAsync();
            return View(model);
        }

        //Employee Create Requests
        //cip...144
        //public async Task<IActionResult> Create() * 23/01/25 adding admin functionality *
        public async Task<IActionResult> Create(string? employeeId, string? leaveTypeId)
        {
            var leaveTypes = await _leaveTypesService.GetAllAsync();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId ?? string.Empty); //SelectList.SelectList(System.Collections.IEnumerable items, string dataValueField, string dataTextField)
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList,
                EmployeeId = employeeId // * 23/01/25 adding admin functionality *
            };
            return View(model);
        }

        //Employee Create Requests (POST)
        //cip...145
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model) // [HttpPost] param will ALWAYS be a view model.
        {
            //NOTE: the following is implemented in cip...147 by LeaveRequestCreateVM inheriting from IValidatableObject.
            //make sure the end date is after the start date.
            if (ModelState.IsValid)
                if (model.StartDate.DayNumber > model.EndDate.DayNumber)
                    ModelState.AddModelError(nameof(model.EndDate), $"The End Date cannot precede the Start Date.");

            //cip...146 validate that the days don't exceed the allocation.
            if (ModelState.IsValid)
                if (await _leaveRequestsService.RequestDatesExceedAllocationAsync(model))
                {
                    var maxDays = await _leaveRequestsService.GetUsersMaxDaysForLeaveTypeAsync(model.LeaveTypeId);
                    var msgDaysLeft = maxDays + (maxDays == 1 ? " day" : " days") + " left";
                    ModelState.AddModelError(string.Empty, $"You have exceeded your allocation ({msgDaysLeft})"); //page
                    ModelState.AddModelError(nameof(model.EndDate), $"You have exceeded your allocation ({msgDaysLeft})");//page item
                }
        
            if (ModelState.IsValid)
            {
                await _leaveRequestsService.CreateLeaveRequestAsync(model);
                //ToDo: if the model.employeeId is different to _functions.GetEmployeeIdAsync() then i need to go back to LeaveAllocations.Details.cshtml
                if(model.EmployeeId != await _functions.GetEmployeeIdAsync())
                    return RedirectToAction(nameof(LeaveAllocationsController.Details), nameof(LeaveAllocationsController), new { employeeId = model.EmployeeId}); //admin work
                else
                    return RedirectToAction(nameof(Index)); //cip...148 tw informed that this should've been added before (cip...146?)
            }
            //NOTE: LeaveTypes is null at this point because it's unbound
            //refill...
            var leaveTypes = await _leaveTypesService.GetAllAsync();
            model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");
            return View(model);
        }

        //Employee Cancel Requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int gobbledygook)
        {
            await _leaveRequestsService.CancelLeaveRequestAsync(gobbledygook);
            return RedirectToAction(nameof(Index));
        }

        //Admin/Supervisor review Requests
        public async Task<IActionResult> ListRequests()
        {
            var model = await _leaveRequestsService.GetAllLeaveRequestsAsync();
            return View(model);
        }

        //Admin/Supervisor review Requests
        public async Task<IActionResult> Review(int leaveRequestId) //cip...158
        {
            var model = await _leaveRequestsService.GetLeaveRequestForReviewAsync(leaveRequestId);
            return View(model);
        }

        //Admin/Supervisor review Requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int leaveRequestId, bool approved) //cip...159
        {
            await _leaveRequestsService.ReviewLeaveRequestAsync(leaveRequestId, approved);
            return RedirectToAction(nameof(ListRequests));
        }
    }
}
