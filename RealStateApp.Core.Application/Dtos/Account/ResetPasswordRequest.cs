using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parametros para reiniciar password
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <example>estesmicorreo@email.com </example>
        public string Email { get; set; }
        /// <example>Thisismypassword1.</example>
        public string Password { get; set; }
        /// <example>Thisismypassword1.</example>
        public string ConfirmPassword { get; set; }
        /// <example>12192031djsfasan1e921imsdkas.</example>
        public string Token { get; set; }
    }
}
