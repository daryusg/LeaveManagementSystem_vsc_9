using System.Reflection.Metadata;
using AutoMapper;
using Constants = LeaveManagementSystem.Data.Constants;

namespace LeaveManagementSystem.Web.Controllers;

//cip...122
[Authorize(Roles = Constants.Roles.cAdministrator)]
public class PeriodsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFunctions _functions; //03/05/25

    public PeriodsController(ApplicationDbContext context, IMapper mapper, IFunctions functions)
    {
        _context = context;
        this._mapper = mapper;
        this._functions = functions;
    }

    // GET: Periods
    public async Task<IActionResult> Index()
    {
        if (TempData.ContainsKey("ErrorMessage")) //03/05/25
        {
            ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
        }

        var periodsVM = _mapper.Map<List<PeriodVM>>(await _context.Periods.ToListAsync());

        return View(periodsVM);
    }

    // GET: Periods/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var period = await _context.Periods
            .FirstOrDefaultAsync(m => m.Id == id);
        if (period == null)
        {
            return NotFound();
        }

        var periodVM = _mapper.Map<PeriodVM>(period);
        return View(periodVM);
    }

    // GET: Periods/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Periods/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate,Id")] PeriodVM periodVM)
    {
        var period = _mapper.Map<Period>(periodVM);
        if (ModelState.IsValid)
        {
            //---------------------------------------------------------
            //03/05/25 set createdby and createddate
            period.CreatedBy = new Guid(await _functions.GetEmployeeIdAsync());
            period.CreatedDate = DateTime.Now;
            //---------------------------------------------------------
            _context.Add(period);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(periodVM);
    }

    // GET: Periods/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var period = await _context.Periods.FindAsync(id);
        if (period == null)
        {
            return NotFound();
        }

        var periodVM = _mapper.Map<PeriodVM>(period);
        return View(periodVM);
    }

    // POST: Periods/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CreatedDate,CreatedBy,Name,StartDate,EndDate,Id")] PeriodVM periodVM)
    {
        var period = _mapper.Map<Period>(periodVM);
        if (id != period.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (await _functions.isAuthorisedAdminAsync(period.CreatedBy)) //03/05/25
            {
                try
                {
                    //---------------------------------------------------------
                    //03/05/25 set modifiedby and modifieddate
                    period.ModifiedBy = new Guid(await _functions.GetEmployeeIdAsync());
                    period.ModifiedDate = DateTime.Now;
                    //---------------------------------------------------------
                    _context.Update(period);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodExists(period.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
                TempData["ErrorMessage"] = Constants.cUnauthorisedAccess;

            return RedirectToAction(nameof(Index));
        }
        return View(periodVM);
    }

    // GET: Periods/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var period = await _context.Periods
            .FirstOrDefaultAsync(m => m.Id == id);
        if (period == null)
        {
            return NotFound();
        }

        var periodVM = _mapper.Map<PeriodVM>(period);
        return View(periodVM);
    }

    // POST: Periods/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var period = await _context.Periods.FindAsync(id);
        if (await _functions.isAuthorisedAdminAsync(period.CreatedBy)) //03/05/25
        {
            if (period != null)
            {
                _context.Periods.Remove(period);
            }

            await _context.SaveChangesAsync();
        }
        else
            TempData["ErrorMessage"] = Constants.cUnauthorisedAccess;

        return RedirectToAction(nameof(Index));
    }

    private bool PeriodExists(int id)
    {
        return _context.Periods.Any(e => e.Id == id);
    }
}
