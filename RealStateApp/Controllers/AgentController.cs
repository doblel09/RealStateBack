using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;
using static NuGet.Packaging.PackagingConstants;

namespace RealStateApp.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;


        public AgentsController(IUserService userService, IPropertyService propertyService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _propertyService = propertyService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Agents(string searchQuery = null)
        {
            // Obtener todos los agentes
            var agents = await _userService.GetAllAgents();

            // Filtrar agentes si hay una consulta de búsqueda
            if (!string.IsNullOrEmpty(searchQuery))
            {
                agents = agents.Where(a => a.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                                        || a.LastName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Ordenar agentes por nombre
            agents = agents.OrderBy(a => a.FirstName).ToList();

            // Pasar la consulta de búsqueda a la vista
            ViewData["SearchQuery"] = searchQuery;

            // Mapear a UserViewModel para la vista
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

            // Obtiene las propiedades asociadas al agente
            var properties = await _propertyService.GetAllPropertiesByAgentIdAsync(agentId);

            return View(properties); // Asegúrate de tener una vista asociada


        }
    }
}

