using Microsoft.AspNetCore.Mvc.Filters;
using RealStateApp.Controllers;

namespace RealStateApp.Middlewares
{
    public class LoginAuthorize(ValidateUserSession userSession) : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession = userSession;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_userSession.HasUser())
            {
                var controller = (UserController)context.Controller;
                context.Result = controller.RedirectToAction("index", "home");
            }
            else
            {
                await next();
            }
        }
    }
}
