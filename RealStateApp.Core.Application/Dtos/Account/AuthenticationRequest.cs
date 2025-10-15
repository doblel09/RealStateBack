using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parametros para iniciar sesion
    /// </summary>
    public class AuthenticationRequest
    {
        /// <example>estesmicorreo@email.com</example>
        [SwaggerParameter(Description = "Email de usuario")]
        public string Email { get; set; }
        /// <example>123Pa$$word!</example>
        [SwaggerParameter(Description = "Password de usuario")]
        public string Password { get; set; }
    }
}
