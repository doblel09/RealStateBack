

using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers;


namespace RealStateApp.Middlewares
{
    public class ValidateUserSession(IHttpContextAccessor httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public bool HasUser()
        {
            AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (userViewModel == null)
            {
                return false;
            }
            return true;
        }
    }
}
