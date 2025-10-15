using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Middlewares;

namespace RealStateApp.Controllers
{
    public class SaleTypeController(ISaleTypeService saleTypeService, ValidateUserSession validateUserSession) : Controller
    {
        private readonly ISaleTypeService _saleTypeService = saleTypeService;
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

            var saleType = await _saleTypeService.GetAllListViewModel();
            return View(saleType);
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
            SaveSaleTypeViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSaleTypeViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _saleTypeService.Add(vm);
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
            var saleType = await _saleTypeService.GetByIdSaveViewModel(id);
            return View("Create", saleType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSaleTypeViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            await _saleTypeService.Update(vm, vm.Id);
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
            return View(await _saleTypeService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SaveSaleTypeViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            await _saleTypeService.Delete(vm.Id);
            return RedirectToAction("Index");
        }

    }
}
