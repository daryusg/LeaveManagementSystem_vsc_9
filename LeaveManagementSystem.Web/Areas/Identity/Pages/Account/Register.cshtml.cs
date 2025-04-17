// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Constants = LeaveManagementSystem.Data.Constants;

namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILeaveAllocationsService _leaveAllocationsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RegisterModel(
            ILeaveAllocationsService leaveAllocationsService, //cip...125
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, //cip...108
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment hostEnvironment /*cip...191*/)
        {
            this._leaveAllocationsService = leaveAllocationsService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            this._roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            this._hostEnvironment = hostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        //public InputModel Input { get; set; } //amended in 109 due to failure here: Input.RoleNames = roles; // cip...108. fill the combo prior.
        public InputModel Input { get; set; } = new InputModel(); //cip...109

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        //---   cip...110   ---
        public string[] RoleNames { get; set; } //placed here alongside ReturnUrl & ExternalLogins.

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            //---------------------
            //---   cip...108   ---
            //---------------------
            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date Of Birth")]
            public DateOnly DateOfBirth { get; set; }
            //---   cip...109   ---
            [Required] //cip...110
            [Display(Name = "Role Name")]
            public string RoleName { get; set; }
            //---------------------
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            RoleNames = await GetRoles(); // cip...108. fill the combo prior.
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //note: i've put this here because i need RoleNames whether pass or fail (await _userManager.AddToRolesAsync(user, Input.RoleNames);).
            RoleNames = await GetRoles(); // cip...110. OnGetAsync sets ReturnUrl, ExternalLogins & Input.RoleNames. OnGetAsync needs to do the same. RoleNames isn't bound to the form so, on error, isn't pre-populated prior to return.

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None); //cip...108 this sets the username. currently, email.
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.FirstName = Input.FirstName; //cip...108
                user.LastName = Input.LastName; //cip...108
                user.DateOfBirth = Input.DateOfBirth; //cip...108

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (Input.RoleName == Constants.Roles.cSupervisor)
                    {
                        await _userManager.AddToRolesAsync(user, RoleNames);
                        //await _userManager.AddToRolesAsync(user, ["Employee", "Supervisor"]); //cip...108
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, Input.RoleName);
                        //await _userManager.AddToRolesAsync(user, ["Employee"]); //cip...108
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    await _leaveAllocationsService.AllocateLeaveAsync(userId); //cip...125. this should (probably) be added AFTER user confirmation.
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    
                    //get the template //cip...191 start
                    var emailTemplatePath = Path.Combine(_hostEnvironment.WebRootPath, "templates", "email_layout.html");
                    var template = await System.IO.File.ReadAllTextAsync(emailTemplatePath);
                    //inject dynamic content
                    var messageBody = template
                        .Replace("{UserName}", $"{user.FirstName} {user.LastName}") //both Input & user (see above)
                        .Replace("{MessageContent}", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", messageBody); //cip...191 end

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        //cip...109 my enhancement
        private async Task<string[]> GetRoles()
        {
            string[] roles = await _roleManager.Roles
                .Select(q => q.Name)
                .Where(q => q != Constants.Roles.cAdministrator)
                .ToArrayAsync(); // cip...108. fill the combo prior.
            return roles; //Password1!
        }
    }
}
