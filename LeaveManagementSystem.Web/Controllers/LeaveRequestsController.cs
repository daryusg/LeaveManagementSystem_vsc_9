using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    //cip...142
    [Authorize]
    public class LeaveRequestsController (ILeaveTypesService _leaveTypesService, ILeaveRequestsService _leaveRequestsService): Controller
    {
        //Employee View Requests
        // GET: LeaveRequestsController
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //Employee Create Requests
        //cip...144
        public async Task<IActionResult> Create()
        {
            var leaveTypes = await _leaveTypesService.GetAllAsync();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name"); //SelectList.SelectList(System.Collections.IEnumerable items, string dataValueField, string dataTextField)
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList
            };
            return View(model);
        }

        //Employee Create Requests (POST)
        //cip...145
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model) // [HttpPost] param will ALWAYS be a view model.
        {
            if (ModelState.IsValid)
            {
                await _leaveRequestsService.CreateLeaveRequestAsync(model);
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
        public async Task<IActionResult> Cancel(int leaveRequestId)
        {
            return View();
        }

        //Admin/Supervisor review Requests
        public async Task<IActionResult> ListRequests()
        {
            return View();
        }

        //Admin/Supervisor review Requests
        public async Task<IActionResult> Review(int leaveRequestId)
        {
            return View();
        }

        //Admin/Supervisor review Requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(/*vm*/)
        {
            return View();
        }
    }
}
