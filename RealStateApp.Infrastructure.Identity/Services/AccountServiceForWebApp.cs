using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using RealStateApp.Core.Application.Interfaces.Services.Email;
using RealStateApp.Core.Application.ViewModels.Home;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Infrastructure.Identity.Entities;
using StockApp.Core.Domain.Settings;

namespace RealStateApp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApp : BaseAccountService, IWebAppAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public AccountServiceForWebApp(
                IPropertyRepository propertyRepository,
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager,
              RoleManager<IdentityRole> roleManager,
              IEmailService emailService,
              IOptions<JWTSettings> jwtSettings,
              IMapper mapper
            ) : base(propertyRepository, userManager, emailService, jwtSettings, mapper)
        {
            _propertyRepository = propertyRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }
            if (user.EmailConfirmed == false)
            {
                response.HasError = true;
                response.Error = $"Accounts registered with {request.Email} deactivate";
                return response;
            }

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            if (rolesList.Contains("Developer"))
            {
                response.HasError = true;
                response.Error = $"Accounts registered with {request.Email} does not have permission to access this site";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.Email}";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account no confirmed for {request.Email}";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }




        public async Task<List<UserViewModel>> GetAllUsers(string userRole)
        {
            var role = await _roleManager.FindByNameAsync(userRole);

            var usersInRole = await _userManager.GetUsersInRoleAsync(userRole);
            var userViewModels = usersInRole.Select(u => new UserViewModel
            {
                Id = u.Id,
                Username = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Cedula = u.Cedula,
                Phone = u.PhoneNumber,
                ProfilePicture = u.ProfilePicture,
                IsActive = u.EmailConfirmed,
            }).ToList();

            return userViewModels;
        }

        public async Task<StatisticsViewModel> GetStatisticsAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var statistics = new StatisticsViewModel
            {
                ActiveAgents = allUsers.Count(u => u.EmailConfirmed && _userManager.IsInRoleAsync(u, "Agent").Result),
                InactiveAgents = allUsers.Count(u => !u.EmailConfirmed && _userManager.IsInRoleAsync(u, "Agent").Result),
                ActiveClients = allUsers.Count(u => u.EmailConfirmed && _userManager.IsInRoleAsync(u, "Customer").Result),
                InactiveClients = allUsers.Count(u => !u.EmailConfirmed && _userManager.IsInRoleAsync(u, "Customer").Result),
                ActiveDevelopers = allUsers.Count(u => u.EmailConfirmed && _userManager.IsInRoleAsync(u, "Developer").Result),
                InactiveDevelopers = allUsers.Count(u => !u.EmailConfirmed && _userManager.IsInRoleAsync(u, "Developer").Result)
            };

            return statistics;
        }


        public async Task<UpdateUserViewModel> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UpdateUserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Cedula = user.Cedula,
                Phone = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                IsActive = user.EmailConfirmed,
            };
        }

        public async Task<RegisterResponse> UpdateUserAsync(UpdateUserRequest request)
        {

            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return new RegisterResponse
                {
                    HasError = true,
                    Error = "Usuario no encontrado"
                };
            }

            RegisterResponse response = new()
            {
                HasError = false
            };

            if (user.UserName != request.Username)
            {
                var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);
                if (userWithSameUserName != null)
                {
                    response.HasError = true;
                    response.Error = $"El nombre de usuario '{request.Username}' ya está en uso.";
                    return response;
                }
            }

            if (user.Email != request.Email)
            {
                var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
                if (userWithSameEmail != null)
                {
                    response.HasError = true;
                    response.Error = $"El correo electrónico '{request.Email}' ya está registrado.";
                    return response;
                }
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Cedula = request.Cedula;
            user.UserName = request.Username;
            user.PhoneNumber = request.Phone;
            user.EmailConfirmed = request.IsActive;

            if (string.IsNullOrEmpty(request.ProfilePicture))
            {
                user.ProfilePicture = user.ProfilePicture;
            }
            else
            {
                user.ProfilePicture = request.ProfilePicture;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    HasError = true,
                    Error = "Ocurrió un error al actualizar el usuario"
                };
            }

            return new RegisterResponse
            {
                HasError = false
            };
        }


        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}