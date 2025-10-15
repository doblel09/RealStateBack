using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.ViewModels.Home;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin, string userRole);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task<RegisterResponse> UpdateUserAsync(UpdateUserViewModel request);
        Task<UpdateUserViewModel> GetUserByIdAsync(string userId);
        Task<StatisticsViewModel> GetStatisticsAsync();
        Task<List<AgentDto>> GetAllAgents();
        Task<UserViewModel> GetAgentByIdAsync(string userId);
        Task<List<UserViewModel>> GetAllUsers(string userRole);
        Task DeleteAsync(string id);
        Task SignOutAsync();
    }
}