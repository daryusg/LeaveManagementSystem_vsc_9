namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Administrator")] //cip...112
    public class LeaveTypesController(ILeaveTypesService _leaveTypesService) : Controller //cip...93
    {
        private const string duplicateName = "Duplicate name"; //cip...84
        private const string invalidName = "Invalid name";

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var viewData = await _leaveTypesService.GetAllAsync(); //cip...93
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

            var viewData = await _leaveTypesService.GetAsync<LeaveTypeReadOnlyVM>(id);

            if (viewData == null)
            {
                return NotFound();
            }

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
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), invalidName);
            }
            //additonal validation 2. cip...84
            if (await _leaveTypesService.CheckIfLeaveTypeNameExistsAsync(null, leaveTypeCreate.Name))
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), duplicateName);

            if (ModelState.IsValid)
            {
                await _leaveTypesService.CreateAsync(leaveTypeCreate);
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
            
            var viewData = await _leaveTypesService.GetAsync<LeaveTypeEditVM>(id); //cip...93
            if (viewData == null)
            {
                return NotFound();
            }
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

            if (await _leaveTypesService.CheckIfLeaveTypeNameExistsAsync(leaveType.Id, leaveType.Name)) //cip...93
                ModelState.AddModelError(nameof(leaveType.Name), duplicateName);

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypesService.EditAsync(leaveType); //cip...93
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_leaveTypesService.LeaveTypeExists(leaveType.Id)) //cip...93
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

            var viewData = await _leaveTypesService.GetAsync<LeaveTypeReadOnlyVM>(id); //cip...93
            if (viewData == null)
            {
                return NotFound();
            }
            return View(viewData);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypesService.RemoveAsync(id); //cip...93
            return RedirectToAction(nameof(Index));
        }
    }
}
