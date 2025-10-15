using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parametros para solicitar un reinicio de password
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <example>micorreo@email.com</example>
        [SwaggerParameter(Description = "Email de usuario")]
        public string Email { get; set; }
    }
}
