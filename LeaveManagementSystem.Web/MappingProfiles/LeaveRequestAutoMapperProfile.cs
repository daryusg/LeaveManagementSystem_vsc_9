namespace LeaveManagementSystem.Web.MappingProfiles;

public class LeaveRequestAutoMapperProfile : Profile
{
    public LeaveRequestAutoMapperProfile()
    {
        CreateMap<LeaveRequestCreateVM, LeaveRequest>(); //cip...143
    }
}


