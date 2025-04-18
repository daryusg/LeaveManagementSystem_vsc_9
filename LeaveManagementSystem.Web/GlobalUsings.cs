//cip...107 -------------------------------------------------
global using LeaveManagementSystem.Data;

global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Linq;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Threading;
global using System.Threading.Tasks;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.Extensions.Logging;

global using Microsoft.EntityFrameworkCore;
//cip...107 -------------------------------------------------
global using LeaveManagementSystem.Common;
global using LeaveManagementSystem.Application.Services.LeaveAllocations;
global using LeaveManagementSystem.Application.Services.LeaveRequests;
global using LeaveManagementSystem.Application.Services.LeaveTypes; //cip...142

global using LeaveManagementSystem.Application.Models.LeaveAllocations;
global using LeaveManagementSystem.Application.Models.LeaveRequests;
global using LeaveManagementSystem.Application.Models;

global using LeaveManagementSystem.Application.Models.Periods; //16/04/25