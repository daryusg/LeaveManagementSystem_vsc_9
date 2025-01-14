using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace LeaveManagementSystem.Web.Controllers
{
    //cip...141
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        //Employee View Requests
        // GET: LeaveRequestsController
        public async Task<ActionResult> Index()
        {
            return View();
        }

        //Employee Create Requests
        public async Task<ActionResult> Create()
        {
            return View();
        }

        //Employee Create Requests (POST)
        [HttpPost]
        public async Task<ActionResult> Create(int Create /*will be a vm*/) // [HttpPost] param will ALWAYS be a view model.
        {
            return View();
        }

    }
}
