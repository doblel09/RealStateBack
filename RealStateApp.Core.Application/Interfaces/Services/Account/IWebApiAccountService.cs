using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;

namespace RealStateApp.Core.Application.Interfaces.Services.Account
{
    public interface IWebApiAccountService : IBaseAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<string> GetCustomerNameAsync(string userId);

        Task<AuthenticationResponse> GetUserById(string email, string token);

        Task UpdateUser(UpdateUserApiRequest request, string userId);
    }
}