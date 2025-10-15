using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Middlewares;
using RealStateApp.Core.Application.Dtos.Agent;


namespace RealStateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;

        public HomeController(IUserService userService, IPropertyService propertyService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _propertyService = propertyService;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            
            if (_userViewModel != null && _userViewModel.Roles.Contains("Developer"))
            {
                
                return RedirectToAction("DevAccess", "Home");
            }

            if (_userViewModel != null && _userViewModel.Roles.Contains("Admin"))
            {
                var statistics = await _userService.GetStatisticsAsync();
                return View(statistics);
            }

            return RedirectToAction("Properties");
        }

        public async Task<IActionResult> Properties(FilterPropertyViewModel filters, string? AgentId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_userViewModel != null && _userViewModel.Roles.Contains("Agent"))
            {
                filters.AgentId = _userViewModel.Id;
            }

            if (AgentId != null)
            {
                filters.AgentId = AgentId;
            }

            var properties = await _propertyService.GetAllViewModelWithFilters(filters);

            return View(properties);
        } 


        public async Task<IActionResult> Agents(string searchQuery = null)
        {
            
            var agents = await _userService.GetAllAgents();

           
            if (!string.IsNullOrEmpty(searchQuery))
            {
                agents = agents.Where(a => a.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                                        || a.LastName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            
            agents = agents.OrderBy(a => a.FirstName).ToList();

            
            ViewData["SearchQuery"] = searchQuery;

            
            var agentViewModels = agents.Select(a => new UserViewModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                Phone = a.Phone,
                ProfilePicture = a.ProfilePicture 

            }).ToList();

            return View(agentViewModels);
        }
        public async Task<IActionResult> AgentProperties(string agentId)
        {
            if (string.IsNullOrEmpty(agentId))
            {
                return Unauthorized("No se encontró el ID del agente.");
            }

            
            var properties = await _propertyService.GetAllPropertiesByAgentIdAsync(agentId);

            return View(properties); 


        }
        public IActionResult DevAccess()
        {
            return View();
        }
    }

}
