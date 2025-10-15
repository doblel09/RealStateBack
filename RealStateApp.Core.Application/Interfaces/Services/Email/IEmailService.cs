using RealStateApp.Core.Application.Dtos.Email;
using RealStateApp.Core.Domain.Settings;

namespace RealStateApp.Core.Application.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}