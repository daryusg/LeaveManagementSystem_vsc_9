using System;
using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeaveType, LeaveTypeReadOnlyVM>()
        .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays)); //cip...77 map from:LeaveType to:IndexVM
        //note: Id and Name will be mapped automatically because they've the same name in both classes.
        //NumberOfDays won't be found in the destination class.
    }
}
