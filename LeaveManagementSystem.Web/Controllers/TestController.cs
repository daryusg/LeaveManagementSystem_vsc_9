using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    /*
    cip...26
    Best Practices

    Follow the naming conventions:
        Controller names must follow the format <RouteName>Controller.
        View folder must exist with a name of <RouteName>.
        Each action defined in RouteNameController should have a matching view:
            eg An Index action must have a matching Index.cshtml view file (in the folder with the same name as the controller).
        Create folders for files of similar natures.
    */
    public class TestController : Controller
    {
        /*
        FROM DEBUG CONSOLE:
        fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
            An unhandled exception has occurred while executing the request.
            System.InvalidOperationException: The view 'Index' was not found. The following locations were searched:
            /Views/Test/Index.cshtml
            /Views/Shared/Index.cshtml
            /Pages/Shared/Index.cshtml
        */
        // GET: TestController
        public ActionResult Index()
        {
            var model = new TestViewModel{
                Name = "kev",
                DateOfBirth = DateOnly.Parse("1 Jul 1991") /*null*/
                //DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-54))
            };
            return View(model);
        }
    }
}
