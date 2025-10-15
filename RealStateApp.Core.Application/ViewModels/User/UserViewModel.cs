

namespace RealStateApp.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string Cedula { get; set; }
        public bool IsActive { get; set; }
        public int PropertiesQuantity { get; set; }
    }
}
