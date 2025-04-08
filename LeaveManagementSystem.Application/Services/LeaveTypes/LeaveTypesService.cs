using Microsoft.Extensions.Logging;

namespace LeaveManagementSystem.Application.Services.LeaveTypes;

//cip...90, cip...178
public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper, ILogger <LeaveTypesService> _logger) : ILeaveTypesService
{
    //for index
    public async Task<List<LeaveTypeReadOnlyVM>> GetAllAsync() //cip...90
    {
        var data = await _context.LeaveTypes.ToListAsync();
        return _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
    }

    //for details
    public async Task<T?> GetAsync<T>(int? id) where T : class //cip...90
    {
        var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(m => m.Id == id);
        if (leaveType == null)
            return null;
        return _mapper.Map<T>(leaveType);
    }

    //for delete
    public async Task RemoveAsync(int id) //cip...90
    {
        var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(m => m.Id == id);
        if (leaveType != null)
        {
            _context.Remove(leaveType);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditAsync(LeaveTypeEditVM model) //cip...91
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Update(leaveType);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(LeaveTypeCreateVM model) //cip...91
    {
        _logger.LogInformation("Creating Leave Type: {leaveTypeName} ({days} days)", model.Name, model.Days); //cip...178
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Add(leaveType);
        await _context.SaveChangesAsync();
    }

    // public async Task<bool> DaysExceedMaximum(int leaveTypeId, int days) //cip...135
    // {
    //     var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId);
    //     return leaveType.NumberOfDays < days;
    // }    

    public async Task<int> GetMaxDaysForLeaveType(int leaveTypeId) //my code cip...135
    {
        var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId);
        return leaveType.NumberOfDays;
    }    
    //-------------------------------------------------------------------------------------------------
    //cip...91
    public bool LeaveTypeExists(int id) //cip...93
    {
        return _context.LeaveTypes.Any(e => e.Id == id);
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsAsync(int? id, string name) //cip...93
    {
        if (id == null)
            //new record, no need to validate the id
            return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower()));
        else
            //existing record, validate the id
            return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower()) && q.Id != id);
    }
}
