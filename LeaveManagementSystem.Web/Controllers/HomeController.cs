using System.Diagnostics;

namespace LeaveManagementSystem.Web.Controllers;

/*
class naming convention: <address/Views folder>Controller eg HomeController
in Program.cs:
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

i have a Home controller therefore i have a Home Views folder
i have an Index action therefore i have an Index veiw file (Home/Index.cshtml)
i have an Privacy action therefore i have an Privacy veiw file (Home/Privacy.cshtml)
*/
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        //define business logic
        //...
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //--------------------------------------------------------------------------------------------------------
    /*
    note: (https://localhost:7053/Home/About) i dont have a view (About.cshtml) so i get an error which is also displayed the FULL
    details in the debug console because a fail is more serious than a warning (see "Logging:" section in appsettings.Development.json):
        An unhandled exception occurred while processing the request.
        InvalidOperationException: The view 'About' was not found. The following locations were searched:
        /Views/Home/About.cshtml
        /Views/Shared/About.cshtml
        /Pages/Shared/About.cshtml
        Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.EnsureSuccessful(IEnumerable<string> originalLocations)
    */
    public IActionResult About()
    {
        return View();
    }
    //--------------------------------------------------------------------------------------------------------

    /*
    Error.cshtml is in the Shared folder and it expects data model of type ErrorViewModel:
        @model ErrorViewModel
        @{
            ViewData["Title"] = "Error";
        }
    */
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // *********************************************************************************************
        // ***   once i hit this action: get the data, prepare the data, send the data to the view   ***
        // ***   the view must be bound to the sent data type                                        ***
        // *********************************************************************************************
        //queries
        //calculations
        //...
        //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        var model = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
        return View(model);
    }
}
