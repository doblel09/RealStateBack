using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;

namespace RealStateApp.Core.Application.Interfaces.Services.Account
{
    public interface IBaseAccountService
    {
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin, string userRole);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task DeleteAsync(string id);

        Task<AgentDto> GetAgentByIdAsync(string id, bool propertyCreation = false);

        Task<List<AgentDto>> AgentListAsync();

        Task ToggleAgentStatus(string id);
        Task<bool> ConfirmCustomerRole(string id);
    }
}