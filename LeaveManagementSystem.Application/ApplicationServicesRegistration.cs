using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagementSystem.Application;

public static class ApplicationServicesRegistration //cip...173
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<ILeaveTypesService, LeaveTypesService>(); //cip...91. register the service for dependency injection.
        services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>(); //cip...123. register the service for dependency injection.
        services.AddScoped<ILeaveRequestsService, LeaveRequestsService>(); //cip...142. register the service for dependency injection.
        services.AddScoped<IFunctions, Functions>(); //cip...162. register the service for dependency injection. NOTE: uses AddHttpContextAccessor.

        services.AddTransient<IEmailSender, EmailSender>(); //cip...111
        return services;
    }
}
