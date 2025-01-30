namespace LeaveManagementSystem.Application.MappingProfiles;

//cip...128
public class LeaveAllocationAutoMapperProfile : Profile
{
    public LeaveAllocationAutoMapperProfile()
    {
        CreateMap<LeaveAllocation, LeaveAllocationVM>();
        CreateMap<LeaveAllocation, LeaveAllocationEditVM>();
        CreateMap<ApplicationUser, EmployeeVM>(); //cip...131. i changed EmployeeListVM to (a more correct) EmployeeVM
        CreateMap<Period, PeriodVM>();
    }
}
