using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Middlewares;
using RealStateApp.Core.Application.ViewModels.Home;

namespace RealStateApp.Controllers
{
    public class UserController(IUserService userService, ValidateUserSession validateUserSession) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly ValidateUserSession _validateUserSession = validateUserSession;

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }


        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        public IActionResult UserType()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            return View();
        }

        public IActionResult Register(string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!ModelState.IsValid)
            {
                return View();
            }
            SaveUserViewModel vm = new();

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm, string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];


            if (vm.File != null)
            {
                string fotoPath = FileManager.UploadFile(vm.File, vm.Id);
                if (string.IsNullOrEmpty(fotoPath))
                {
                    ModelState.AddModelError("", "Hubo un problema al subir la foto.");
                    return View(vm);
                }

                vm.ProfilePicture = fotoPath;
            }


            RegisterResponse response = await _userService.RegisterAsync(vm, origin, vm.UserRole);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            if(_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Maintenance", new { userRole });
            }

            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ForgotPasswordResponse response = await _userService.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordResponse response = await _userService.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}

