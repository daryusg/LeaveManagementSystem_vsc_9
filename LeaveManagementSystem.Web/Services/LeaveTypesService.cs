using System;
using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services;

//cip...90
public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper) : ILeaveTypesService
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
    var leaveType = _mapper.Map<LeaveType>(model);
    _context.Add(leaveType);
    await _context.SaveChangesAsync();
  }
  //-------------------------------------------------------------------------------------------------
  //-------------------------------------------------------------------------------------------------
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
      return (await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower())));
    else
      //existing record, validate the id
      return (await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower()) && (q.Id != id)));
  }
}
