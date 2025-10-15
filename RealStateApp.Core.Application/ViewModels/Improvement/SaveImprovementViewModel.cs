

using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Improvement
{
    public class SaveImprovementViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe colocar el nombre de la mejora")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debe colocar una descripcion")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
