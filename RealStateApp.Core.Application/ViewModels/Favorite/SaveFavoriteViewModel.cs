

using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Favorite
{
    public class SaveFavoriteViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public int PropertyId { get; set; }
    }
}
