global using Microsoft.EntityFrameworkCore;
global using LeaveManagementSystem.Application.Models.LeaveTypes;
//cip...107 -------------------------------------------------
global using LeaveManagementSystem.Application.Models.LeaveAllocations; //cip...128
global using LeaveManagementSystem.Application.Models.Periods; //cip...128



global using LeaveManagementSystem.Application.Models.LeaveRequests; //cip...143

global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;

global using LeaveManagementSystem.Data;

global using System.Net.Mail;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Microsoft.Extensions.Configuration;

global using AutoMapper;
global using LeaveManagementSystem.Common;
//cip...173 -------------------------------------------------
global using System.Reflection;
global using LeaveManagementSystem.Application.Services.Email;
global using LeaveManagementSystem.Application.Services.LeaveAllocations;
global using LeaveManagementSystem.Application.Services.LeaveRequests;
global using LeaveManagementSystem.Application.Services.LeaveTypes;