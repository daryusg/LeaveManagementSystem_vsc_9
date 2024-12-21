using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private const string duplicateName = "Duplicate name";

        public LeaveTypesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            //cip...60 _context is a connection to the db (ApplicaionDbContext). LeaveTypes is from "...DbSet<LeaveType> LeaveTypes..." in ApplicationDbContext.cs.
            // var data = SELECT * FROM LeaveTypes
            var data = await _context.LeaveTypes.ToListAsync();
            //convert the data model into a view model. cip...75 (Refactor Index with View Model)
            // var viewData = data.Select(q => new IndexVM
            // {
            //     Id = q.Id,
            //     Name = q.Name,
            //     Days = q.NumberOfDays
            // });
            var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data); //cip...77
            //return the view model to the view.
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Parameterisation securely passes the id and is key for preventing SQL injection attacks.
            //SELECT TOP 1 FROM LeaveTypes WHERE (Id = @id)
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType); //cip...78

            return View(viewData);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate) //cip...80
        {
            //additonal validation 1. cip...81
            const string shouldNotBeginWith = "zzz";
            const string shouldNotContain = "yyy";
            if ((leaveTypeCreate.Name.Substring(0, 3).ToLower() == shouldNotBeginWith) || (leaveTypeCreate.Name.ToLower().Contains(shouldNotContain)))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), "Invalid name");
            }
            //additonal validation 2. cip...84
            if (await CheckIfLeaveTypeNameExists(null, leaveTypeCreate.Name))
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), duplicateName);

            if (ModelState.IsValid)
            {
                var leaveType = _mapper.Map<LeaveType>(leaveTypeCreate); //cip...80
                _context.Add(leaveType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate);
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            //SELECT TOP 1 FROM LeaveTypes WHERE (Id = @id)
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }
            var viewData = _mapper.Map<LeaveTypeEditVM>(leaveType); //cip...82
            return View(viewData);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveType)
        {
            if (id != leaveType.Id)
            {
                return NotFound();
            }

            if (await CheckIfLeaveTypeNameExists(leaveType.Id, leaveType.Name)) //cip...84
                ModelState.AddModelError(nameof(leaveType.Name), duplicateName);

            if (ModelState.IsValid)
            {
                try
                {
                    var model = _mapper.Map<LeaveType>(leaveType); //cip...82
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveType.Id))
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
            return View(leaveType);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }
            var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType); //cip...83
            return View(viewData);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }

        private async Task<bool> CheckIfLeaveTypeNameExists(int? id, string name) //cip...84
        {
            //return (_context.LeaveTypes.Any(q => q.Name.ToLower() == name.ToLower());
            //return (_context.LeaveTypes.Any(q => q.Name.ToLower().Equals(name.ToLower(), StringComparison.InvariantCultureIgnoreCase)));// tw explained that he's had failures with the db conversion.
            if (id == null)
                //new record, no need to validate the id
                return (await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower())));
            else
                //existing record, validate the id
                return (await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower()) && (q.Id != id)));
        }
    }
}
