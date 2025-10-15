using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Middlewares;

namespace RealStateApp.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IImprovementService _improvementService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;

        public PropertyController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, ISaleTypeService saleTypeService, ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor, IImprovementService improvementService)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
            _httpContextAccessor = httpContextAccessor;
            _validateUserSession = validateUserSession;
            _improvementService = improvementService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }


        public async Task<IActionResult> Index(FilterPropertyViewModel filters)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_userViewModel != null && _userViewModel.Roles.Contains("Agent"))
            {
                filters.AgentId = _userViewModel.Id;
            }
            var properties = await _propertyService.GetAllViewModelWithFilters(filters);
            return View(properties);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var property = await _propertyService.GetPropertyById(id);
            if (property == null)
            {
                return NotFound();
            }

            if (property.Images == null)
            {
                property.Images = new List<string>();
            }

            return View(property);
        }



        public async Task<IActionResult> AddProperty()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            SavePropertyViewModel vm = new();
            vm.PropertyTypes = await _propertyTypeService.GetAllListViewModel();
            vm.SaleTypes = await _saleTypeService.GetAllListViewModel();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddProperty(SavePropertyViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                vm.PropertyTypes = await _propertyTypeService.GetAllListViewModel();
                vm.SaleTypes = await _saleTypeService.GetAllListViewModel();
                return View(vm);
            }

            if(vm.PropertyTypes != null && vm.PropertyTypes.Count == 0)
            {
                ModelState.AddModelError("", "No hay tipos de propiedades diponibles");
            }

            if (vm.SaleTypes != null && vm.SaleTypes.Count == 0)
            {
                ModelState.AddModelError("", "No hay tipos de ventas diponibles");
            }

            if (vm.Files != null && vm.Files.Any())
            {
                foreach (var file in vm.Files)
                {
                    string fotoPath = FileManager.UploadFile(file, "file");
                    if (string.IsNullOrEmpty(fotoPath))
                    {
                        ModelState.AddModelError("", "Hubo un problema al subir la foto.");
                        return View(vm);
                    }

                    vm.Images.Add(fotoPath);
                }
            }
            else
            {
                ModelState.AddModelError("Files", "El campo Imágenes es obligatorio");
            }

            await _propertyService.Add(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditProperty(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            var property = await _propertyService.GetByIdSaveViewModel(id);
            property.PropertyTypes = await _propertyTypeService.GetAllListViewModel();
            property.SaleTypes = await _saleTypeService.GetAllListViewModel();
            property.Improvements = await _improvementService.GetAllListViewModel();

            return View("AddProperty", property);
        }

        [HttpPost]
        public async Task<IActionResult> EditProperty(SavePropertyViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                vm.PropertyTypes = await _propertyTypeService.GetAllListViewModel();
                vm.SaleTypes = await _saleTypeService.GetAllListViewModel();
                vm.Improvements = await _improvementService.GetAllListViewModel();
                return View("AddProperty", vm);
            }

            var propertyVm = await _propertyService.GetByIdSaveViewModel(vm.Id);

            if (vm.Files != null && vm.Files.Any())
            {
                var updatedImages = new List<string>();
                foreach (var file in vm.Files)
                {
                    var imagePath = FileManager.UploadFile(file, "file", true, propertyVm.Images.FirstOrDefault());
                    updatedImages.Add(imagePath);
                }
                vm.Images = updatedImages;
            }
            else
            {
                vm.Images = propertyVm.Images;
            }

            await _propertyService.Update(vm, vm.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProperty(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            return View(await _propertyService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(SavePropertyViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            await _propertyService.Delete(vm.Id);
            return RedirectToAction("Index");
        }



        #region PropertyTypes
        public async Task<IActionResult> PropertyTypes()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }

            var propertyTypes = await _propertyTypeService.GetAllListViewModel();
            return View(propertyTypes);
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
            SavePropertyTypeViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyTypeViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _propertyTypeService.Add(vm);
            return RedirectToAction("PropertyTypes");
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
            var propertiType = await _propertyTypeService.GetByIdSaveViewModel(id);
            return View("Create", propertiType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyTypeViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            await _propertyTypeService.Update(vm, vm.Id);
            return RedirectToAction("PropertyTypes");
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
            return View(await _propertyTypeService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SavePropertyTypeViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "User");
            }
            await _propertyTypeService.Delete(vm.Id);
            return RedirectToAction("PropertyTypes");
        }





        #endregion
    }
}
