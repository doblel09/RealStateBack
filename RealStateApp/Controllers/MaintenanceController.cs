using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Middlewares;

namespace RealStateApp.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IUserService _userService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;

        public MaintenanceController(IUserService userService, ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index(string userRole)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }


            ViewBag.UserRole = userRole;

            if (!ModelState.IsValid)
            {
                return View();
            }
            var users = await _userService.GetAllUsers(userRole);
            return View(users);
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (!_validateUserSession.HasUser() && _userViewModel != null && !_userViewModel.Roles.Contains("Agent"))
            {
                return RedirectToAction("Index", "User");
            }
            ViewBag.UserRole = "Agent";

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        public async Task<IActionResult> Edit(string id, string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            var userVm = await _userService.GetUserByIdAsync(id);

            return View(userVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel vm, string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.File != null)
            {
                var userVm = await _userService.GetUserByIdAsync(vm.Id);
                vm.ProfilePicture = FileManager.UploadFile(vm.File, vm.Id, true, userVm.ProfilePicture);
                string fotoPath = FileManager.UploadFile(vm.File, vm.Id);
                if (string.IsNullOrEmpty(fotoPath))
                {
                    ModelState.AddModelError("", "Hubo un problema al subir la foto.");
                    return View(vm);
                }

                vm.ProfilePicture = fotoPath;
            }


            await _userService.UpdateUserAsync(vm);
            if (_userViewModel != null && _userViewModel.Roles.Contains("Agent"))
            {
                return RedirectToAction("Profile", new { vm.Id });
            }
                return RedirectToAction("Index", new { userRole });
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string id, string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            var user = await _userService.GetUserByIdAsync(id);
            user.IsActive = true;
            await _userService.UpdateUserAsync(user);

            return RedirectToAction("Index", new { userRole });
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(string id, string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            var user = await _userService.GetUserByIdAsync(id);
            user.IsActive = false;
            await _userService.UpdateUserAsync(user);

            return RedirectToAction("Index", new { userRole });
        }

        public async Task<IActionResult> Delete(string id, string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) { return NotFound(); }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateUserViewModel vm, string userRole)
        {
            ViewBag.UserRole = userRole;

            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            await _userService.DeleteAsync(vm.Id);
            return RedirectToAction("Index", new { userRole });
        }
    }
}
