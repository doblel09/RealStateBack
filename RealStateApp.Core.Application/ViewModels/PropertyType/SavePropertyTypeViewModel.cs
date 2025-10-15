

using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.PropertyType
{
    public class SavePropertyTypeViewModel
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe colocar el tipo de propiedad")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debe colocar una descripcion")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
