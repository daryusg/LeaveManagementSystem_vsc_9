using AutoMapper;
using Constants = LeaveManagementSystem.Data.Constants;

namespace LeaveManagementSystem.Web.Controllers;

//cip...122
[Authorize(Roles = Constants.Roles.cAdministrator)]
public class PeriodsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PeriodsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this._mapper = mapper;
    }

    // GET: Periods
    public async Task<IActionResult> Index()
    {
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
    public async Task<IActionResult> Edit(int id, [Bind("Name,StartDate,EndDate,Id")] PeriodVM periodVM)
    {
        var period = _mapper.Map<Period>(periodVM); 
        if (id != period.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
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
        if (period != null)
        {
            _context.Periods.Remove(period);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PeriodExists(int id)
    {
        return _context.Periods.Any(e => e.Id == id);
    }
}
