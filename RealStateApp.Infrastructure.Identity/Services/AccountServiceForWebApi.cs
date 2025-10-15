using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using RealStateApp.Core.Application.Interfaces.Services.Email;
using RealStateApp.Core.Application.Services;

using RealStateApp.Infrastructure.Identity.Entities;
using StockApp.Core.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace RealStateApp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApi(
          IPropertyRepository propertyRepository,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,          
          IEmailService emailService,
          IMapper mapper,
          IOptions<JWTSettings> jwtSettings
            ) : BaseAccountService(propertyRepository, userManager, emailService, jwtSettings, mapper), IWebApiAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IPropertyRepository _propertyRepository = propertyRepository;




        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No Accounts registered with {request.Email}", (int)HttpStatusCode.NotFound);

            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                throw new ApiException($"Invalid credentials for {request.Email}", (int)HttpStatusCode.BadRequest);
                
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account no confirmed for {request.Email}", (int)HttpStatusCode.NoContent);
                
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Cedula = user.Cedula;
            response.PhoneNumber = user.PhoneNumber;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.Picture = user.ProfilePicture;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }

        public async Task<string> GetCustomerNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ApiException($"User with ID {userId} not found", (int)HttpStatusCode.NotFound);
            }
            return user.FirstName + ' ' + user.LastName;
        }

        public override async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin, string userRole)
        {
            var response = await base.RegisterUserAsync(request, origin, userRole);

            if (response.HasError)
            {
                throw new ApiException(response.Error,response.ErrorCode ?? (int)HttpStatusCode.InternalServerError);
            }

            return response;
        }

        public async Task<AuthenticationResponse> GetUserById(string email, string token)
        {
            var user = await _userManager.FindByIdAsync(email);
            if (user == null)
            {
                throw new ApiException($"No Accounts registered with {email}", (int)HttpStatusCode.NotFound);
            }
            AuthenticationResponse response = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Cedula = user.Cedula,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName,
                Picture = user.ProfilePicture,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                IsVerified = user.EmailConfirmed,
                JWToken = token,
                RefreshToken = GenerateRefreshToken().Token
            };
            return response;
        }

        public async Task UpdateUser(UpdateUserApiRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApiException($"No Accounts registered", (int)HttpStatusCode.NotFound);
            }

            if (user.FirstName != request.FirstName)
                user.FirstName = request.FirstName;

            if (user.LastName != request.LastName)
                user.LastName = request.LastName;

            if (user.PhoneNumber != request.PhoneNumber)
                user.PhoneNumber = request.PhoneNumber;

            if (user.Email != request.Email) user.Email = request.Email;

            if (request.ProfilePicture != null)
            {
                var newProfilePicture = FileManager.UploadFile(request.ProfilePicture, "profile");
                if (user.ProfilePicture is not null)
                    FileManager.DeleteFile(user.ProfilePicture);

                user.ProfilePicture = newProfilePicture;
            }

            if (request.Password != null) {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.Password);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new ApiException($"Error updating user", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}