

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealStateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parametros para registrar un susuario
    /// </summary>
    public class RegisterRequest
    {
        /// <example>Luis</example>
        public string FirstName { get; set; }
        /// <example>Florentino</example>
        public string LastName { get; set; }
        /// <example>crav814</example>
        public string Username { get; set; }
        /// <example>estesmicorreo@email.com</example>
        public string Email { get; set; }
        /// <example>123Pa$$word!</example>
        public string Password { get; set; }
        /// <example>123Pa$$word!</example>
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        /// <example>8095967094</example>
        public string Phone { get; set; }
        /// <example>./photos/771742/pexels-photo-771742.jpeg</example>
        public required IFormFile ProfilePicture { get; set; }
        /// <example>40200132157</example>
        public string Cedula { get; set; }
        
    }
}
