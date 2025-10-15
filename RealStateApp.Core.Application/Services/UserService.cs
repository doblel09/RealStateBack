using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using RealStateApp.Core.Application.ViewModels.Home;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IWebAppAccountService _accountService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        

        public UserService(IWebAppAccountService accountService, IPropertyRepository propertyRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _accountService = accountService;
            _propertyRepository = propertyRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin, string userRole)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterUserAsync(registerRequest, origin, userRole);
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPasswordAsync(forgotRequest, origin);
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetRequest);
        }

        public async Task<RegisterResponse> UpdateUserAsync(UpdateUserViewModel vm)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin") && !_userViewModel.Roles.Contains("Agent"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para actualizar la infomacion de los usuarios");
            }

            UpdateUserRequest registerRequest = _mapper.Map<UpdateUserRequest>(vm);
            return await _accountService.UpdateUserAsync(registerRequest);
        }

        public async Task<UpdateUserViewModel> GetUserByIdAsync(string userId)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin") && !_userViewModel.Roles.Contains("Agent"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para actualizar la infomacion de este usuario");
            }

            var user = await _accountService.GetUserByIdAsync(userId);
            return user;
        }

        public async Task<List<UserViewModel>> GetAllUsers(string userRole)
        {
            var usersInRole = await _accountService.GetAllUsers(userRole);
            
            var filteredUsers = usersInRole;
                
        
            if (_userViewModel != null && _userViewModel.Id != null) {
                var userId = _userViewModel.Id;
                filteredUsers = usersInRole.Where(p => p.Id != userId).ToList();
            } else
            {
                filteredUsers = usersInRole.ToList();
            }

            if (userRole.Equals("Agent", StringComparison.OrdinalIgnoreCase))
            {
                var listProperties = await _propertyRepository.GetAllListAsync();

                foreach (var user in filteredUsers)
                {
                    var propertiesCount = listProperties.Count(p => p.AgentId == user.Id);
                    user.PropertiesQuantity = propertiesCount;
                }
            }

            var userViewModels = filteredUsers.Select(p => _mapper.Map<UserViewModel>(p)).ToList();

            return userViewModels;
        }
        public async Task<List<AgentDto>> GetAllAgents()
        {
            // Obtener todos los usuarios con el rol de "Agent"
            var agents = await _accountService.GetAllUsers("Agent");

            // Obtener la lista de propiedades para calcular la cantidad por agente
            var allProperties = await _propertyRepository.GetAllListAsync();

            // Crear la lista de agentes DTO
            var agentDtos = agents.Select(agent => new AgentDto
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                Email = agent.Email,
                Phone = agent.Phone,
                ProfilePicture = agent.ProfilePicture,
                PropertyCount = allProperties.Count(property => property.AgentId == agent.Id) // Cantidad de propiedades asignadas al agente
            }).ToList();

            return agentDtos;
        }

        public async Task DeleteAsync(string id)
        {
            if (_userViewModel == null || !_userViewModel.Roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar este usuario");
            }
            await _accountService.DeleteAsync(id);
        }

        public async Task<UserViewModel> GetAgentByIdAsync(string userId)
        {

            // Obtener el usuario (agente) por su ID utilizando el servicio de cuentas
            var user = await _accountService.GetUserByIdAsync(userId);

            // Verificar si el usuario existe
            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró un agente con el ID proporcionado.");
            }

            // Mapear el usuario a un UserViewModel para la vista
            var userViewModel = _mapper.Map<UserViewModel>(user);

            return userViewModel;
        }

        public async Task<StatisticsViewModel> GetStatisticsAsync()
        {
            var allProperties = await _propertyRepository.GetAllListAsync();
            var userStatistics = await _accountService.GetStatisticsAsync();

            var statistics = new StatisticsViewModel
            {
                ActiveAgents = userStatistics.ActiveAgents,
                InactiveAgents = userStatistics.InactiveAgents,
                ActiveClients = userStatistics.ActiveClients,
                InactiveClients = userStatistics.InactiveClients,
                ActiveDevelopers = userStatistics.ActiveDevelopers,
                InactiveDevelopers = userStatistics.InactiveDevelopers,
                AvailableProperty = allProperties.Count(p => p.IsAvailable),
                NotAvailableProperty = allProperties.Count(p => !p.IsAvailable)
            };
            return statistics;
        }

    }
}
