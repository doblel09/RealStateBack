

using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.SaleType
{
    public class SaveSaleTypeViewModel
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe colocar el tipo de venta")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debe colocar una descripcion")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
