using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using diplom_2.Models;

namespace diplom_2.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        
        public ActionResult UsersList()
        {
           return View(db.Users.ToList());
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
        

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (UserManager.FindByEmail(model.Email)!=null)
                if (UserManager.FindByEmail(model.Email).Deleted == true)
                    return View(model);

            // Login failures do not result in account suspension
            // Change to shouldLockout: true to make password errors trigger account lockout
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require user to pre-login with username and password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The code below protects against brute-force attacks against two-factor codes.
            // If the user enters the wrong codes within the specified time, his account
            // will be blocked for the specified period.
            // Account lockout settings can be configured in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неправильный код.");
                    return View(model);
            }
        }

        public ActionResult Recovery(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.Deleted = false;
            db.SaveChanges();
            return RedirectToAction("UsersList");
        }

        public ActionResult Delete(string id)
        {
          
            ApplicationUser user = db.Users.Find(id);
            if (user.SubManagers != null)
                if (user.SubManagers.Count!=0)
                {
                    user.SubManagers.Clear();
               
                   
                }
            if (user.ParentManager!=null)
            {
                user.ParentManager.SubManagers.Remove(user);
            }
            user.Deleted = true;
            db.SaveChanges();
            //UserManager.Delete(user);
            return RedirectToAction("UsersList");
        }


        //
        // GET: /Account/Register      
        public ActionResult Edit(string id)
        {
            //Find the user being edited
            ApplicationUser user = UserManager.FindById(id);
            //Create a model object of the RegisterViewModel class, because
            // it is more convenient for us to make the Edit view as similar as possible to the Register view,
            //which expects data of type RegisterViewModel from the server.
            EditViewModel model = new EditViewModel();
            //Fill the model object with data
            model.Id = id; //we take the user ID from the method parameter
                           //other information from the user object.
            model.Name = user.Name;
            model.Surname = user.Surname;
            model.Email = user.Email;            
            model.SelectedRole = user.Roles.ToList()[0].RoleId;
            model.Phone = user.Phone;
            model.City = user.City;
            if (user.ParentManager != null)
                model.ParentManagerId = user.ParentManager.Id;
            return View(model);
        }

        [HttpPost]      
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {       

            //ApplicationUser user = UserManager.FindById(model.Id);
            ApplicationUser user = db.Users.Find(model.Id);
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.City = model.City;
            if (model.SelectedRole == "remove manager")
                user.ParentManager = db.Users.Find(model.ParentManagerId);
            else
            {
                if (user.ParentManager != null)
                    if (user.ParentManager.SubManagers!=null)
                    user.ParentManager.SubManagers.Remove(user);
            }

            db.SaveChanges();


            //user.Roles.ToList()[0].RoleId - get current user role id
            //db.Roles.Find(user.Roles.ToList()[0].RoleId) - find it by id in the list of all available roles
            //db.Roles.Find(user.Roles.ToList()[0].RoleId).Name - get role name
            if (user.Roles.ToList().Count == 0)
            {
                UserManager.AddToRole(model.Id, db.Roles.Find(model.SelectedRole).Name);
                return RedirectToAction("UsersList", "Account");
            }
            string roleName = db.Roles.Find(user.Roles.ToList()[0].RoleId).Name;

            //Check if the edited user's role has changed
            if (model.SelectedRole != roleName)
            {//Если изменилось

                //Remove user with model.Id from roleName
                UserManager.RemoveFromRole(model.Id, roleName);

                //Add it to a new role
                UserManager.AddToRole(model.Id, model.SelectedRole);
                if (User.Identity.Name == user.Email)
                    return Redirect("LogOff");
                return RedirectToAction("UsersList", "Account");
            }
            return RedirectToAction("UsersList", "Account");
        }

        //
        // GET: /Account/Register    
        public ActionResult Register()
        {          
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, Surname = model.Surname, Phone = model.Phone, City = model.City };
                var result = await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {                   
                    UserManager.AddToRole(user.Id, model.SelectedRole);

                    // For more information on how to enable account verification and password reset, see: http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Account Verification", "Verify your account by clicking <a href=\"" + callbackUrl + "\">here</a>");                    
                    return RedirectToAction("UsersList");
                }
                AddErrors(result);
            }

            // If this message appears, there is an error; redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
    
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]    
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Do not show if the user does not exist or is not verified
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account verification and password reset, see: http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset password", "Reset your password by clicking <a href=\"" + callbackUrl + "\">здесь</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If this message appears, there is an error; redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
    
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
     
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
   
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't show that the user doesn't exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
     
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
 
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to an external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
     
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
      
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Create and send a token
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
     
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Logging in the user through this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then he is prompted to create one
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
     
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get user details from external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff            
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure     
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Companion Applications
        // Used to protect against XSRF attacks when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}