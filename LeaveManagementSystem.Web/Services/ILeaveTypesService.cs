namespace LeaveManagementSystem.Web.Services
{
    public interface ILeaveTypesService
  {
    Task CreateAsync(LeaveTypeCreateVM model);
    Task EditAsync(LeaveTypeEditVM model);
    Task<List<LeaveTypeReadOnlyVM>> GetAllAsync();
    Task<T?> GetAsync<T>(int? id) where T : class;
    Task RemoveAsync(int id);
    bool LeaveTypeExists(int id);
    Task<bool> CheckIfLeaveTypeNameExistsAsync(int? id, string name);
  }
}