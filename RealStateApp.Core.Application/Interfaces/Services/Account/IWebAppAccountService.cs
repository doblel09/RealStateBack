using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.ViewModels.Home;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.Interfaces.Services.Account
{
    public interface IWebAppAccountService : IBaseAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> UpdateUserAsync(UpdateUserRequest request);
        Task<UpdateUserViewModel> GetUserByIdAsync(string userId);
        Task<StatisticsViewModel> GetStatisticsAsync();
        Task<List<UserViewModel>> GetAllUsers(string userRole);
        Task SignOutAsync();
    }
}