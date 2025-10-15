using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealStateApp.Core.Application.Dtos.Account
{
    public class UpdateUserApiRequest
    {
        public string FirstName { get; set; }
        /// <example>Florentino</example>
        public string LastName { get; set; }
        public string Email { get; set; }
        /// <example>Thisismypassword1.</example>
        public string? Password { get; set; }
        /// <example>Thisismypassword1.</example>
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        /// <example>8095967094</example>
        public string PhoneNumber { get; set; }
        /// <example>./photos/771742/pexels-photo-771742.jpeg</example>
        public IFormFile? ProfilePicture { get; set; }
        /// <example>40200132157</example>
        public string Cedula { get; set; }
    }
}
