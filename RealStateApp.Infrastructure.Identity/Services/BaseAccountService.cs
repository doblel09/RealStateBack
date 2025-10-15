using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockApp.Core.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using RealStateApp.Core.Application.Interfaces.Services.Email;
using RealStateApp.Infrastructure.Identity.Entities;
using RealStateApp.Core.Application.Dtos.Account;
using System.Net;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Exceptions;
using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.Properties.Extension;
using RealStateApp.Core.Application.Helpers;


namespace RealStateApp.Infrastructure.Identity.Services
{
    public class BaseAccountService(
            IPropertyRepository propertyRepository,
          UserManager<ApplicationUser> userManager,
          IEmailService emailService,
          IOptions<JWTSettings> jwtSettings,
          IMapper mapper    
            ) : IBaseAccountService
    {
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailService _emailService = emailService;
        private readonly JWTSettings _jwtSettings = jwtSettings.Value;
        private readonly IMapper mapper = mapper;
        public virtual async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin, string userRole)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.Username}' is already taken.";
                response.ErrorCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                response.ErrorCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            if (request.Password != request.ConfirmPassword)
            {
                response.HasError = true;
                response.Error = $"Passwords must match.";
                response.ErrorCode = (int)HttpStatusCode.BadRequest;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                Cedula = request.Cedula,
                PhoneNumber = request.Phone,
                PhoneNumberConfirmed = true,
                
            };

            var profilePic = FileManager.UploadFile(request.ProfilePicture, user.Id.ToString(), imagePath: "profile");

            user.ProfilePicture = profilePic;

            if (userRole != "Customer" && userRole != "Agent")
            {
                user.EmailConfirmed = true;
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                if (userRole == "Customer")
                {
                    await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());
                    var verificationUri = await SendVerificationEmailUrl(user, origin);
                    await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
                    {
                        To = user.Email,
                        Body = $"Please confirm your account visiting this URL {verificationUri}",
                        Subject = "Confirm registration"
                    });
                }
                else if (userRole == "Agent")
                {
                    await _userManager.AddToRoleAsync(user, Roles.Agent.ToString());
                }
                else if (userRole == "Developer")
                {
                    await _userManager.AddToRoleAsync(user, Roles.Developer.ToString());
                }
                else if (userRole == "Admin")
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }             
            }
            else
            {
                FileManager.DeleteFile(profilePic);
                response.HasError = true;
                var errorDescriptions = result.Errors.Select(e => e.Description).ToList();
                if (errorDescriptions.Any(e => e.Contains("Passwords", StringComparison.OrdinalIgnoreCase)))
                {
                    response.Error = string.Join(" | ", errorDescriptions);
                    response.ErrorCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    response.Error = "An error occurred trying to register the user.";
                    response.ErrorCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred wgile confirming {user.Email}.";
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }
            var verificationUri = await SendForgotPasswordUri(user, origin);

            _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest
            {
                To = user.Email,
                Subject = "Reset Password",
                Body = $"<h1>Bienvenido a SocialMedia</h1><p>Reinicia tu password usando <a href='{verificationUri}'>este enlace.</a>"
            });
            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }
            if (request.Password != request.ConfirmPassword)
            {
                response.HasError = true;
                response.Error = $"Passwords must match.";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }

            return response;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NullReferenceException("Usuario no encontrado");
            }
            await _userManager.DeleteAsync(user);
        }
        //WebApi exclusive method
        public async Task<AgentDto> GetAgentByIdAsync(string id, bool propertyCreation = false)
        {
            var agent = await _userManager.FindByIdAsync(id) ?? throw new ApiException("Agent not found", (int)HttpStatusCode.NotFound);

            if (!await _userManager.IsInRoleAsync(agent, Roles.Agent.ToString()))
            throw new ApiException("Agent not found", (int)HttpStatusCode.NotFound);

            var properties = await _propertyRepository.GetAll()
                .Where(p => p.AgentId == id)
                .Include(p=> p.PropertyType)
                .Include(p => p.SaleType)
                .ToListAsync();


            return new AgentDto
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                Email = agent.Email,
                Phone = agent.PhoneNumber,
                Properties = properties.ToDto(mapper) ?? [],
                PropertyCount = properties.Count,
                ProfilePicture = agent.ProfilePicture
            };
        }
        public async Task<List<AgentDto>> AgentListAsync()
        {
            var agents = await _userManager.GetUsersInRoleAsync(Roles.Agent.ToString()) ?? throw new ApiException("Agents not found", (int)HttpStatusCode.NoContent);

            var agentDtos = new List<AgentDto>();

            foreach (var user in agents)
            {
                var propertyCount = await _propertyRepository.GetAll()
                    .CountAsync(p => p.AgentId == user.Id && p.IsAvailable);

                agentDtos.Add(new AgentDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PropertyCount = propertyCount,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture
                });
            }

            return agentDtos;
        }

        public async Task<bool> ConfirmCustomerRole(string id)
        {
            var customer = await _userManager.FindByIdAsync(id) ?? throw new ApiException("Customer not found", (int)HttpStatusCode.NoContent);
            if (!await _userManager.IsInRoleAsync(customer, Roles.Customer.ToString())) return false;
            return true;

        }

        public async Task ToggleAgentStatus(string id)
        {
            var agent = await _userManager.FindByIdAsync(id) ?? throw new ApiException("Agents not found", (int)HttpStatusCode.NoContent);
            if (!await _userManager.IsInRoleAsync(agent, Roles.Agent.ToString())) throw new ApiException("Agents not found", (int)HttpStatusCode.NoContent);
            if (agent.EmailConfirmed == false) agent.EmailConfirmed = true;
            else agent.EmailConfirmed = false;
            await _userManager.UpdateAsync(agent);
        }

        #region ProtectedMethods

        protected async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);

            return jwtSecurityToken;
        }

        protected RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        protected string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }

        protected async Task<string> SendVerificationEmailUrl(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }

        protected async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }
        #endregion
    }
}