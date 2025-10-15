using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Middlewares;

namespace RealStateApp.Controllers
{
    public class ImprovementController(IImprovementService improvementService, ValidateUserSession validateUserSession) : Controller
    {
        private readonly IImprovementService _improvementService = improvementService;
        private readonly ValidateUserSession _validateUserSession = validateUserSession;


        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            var improvement = await _improvementService.GetAllListViewModel();
            return View(improvement);
        }


        public IActionResult Create()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            SaveImprovementViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveImprovementViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _improvementService.Add(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var improvement = await _improvementService.GetByIdSaveViewModel(id);
            return View("Create", improvement);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveImprovementViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            await _improvementService.Update(vm, vm.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            return View(await _improvementService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SaveImprovementViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            await _improvementService.Delete(vm.Id);
            return RedirectToAction("Index");
        }

    }
}
