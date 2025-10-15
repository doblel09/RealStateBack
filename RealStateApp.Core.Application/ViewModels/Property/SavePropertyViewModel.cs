

using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Property
{
    public class SavePropertyViewModel
    {
        public int Id { get; set; }
        public string? UniqueCode { get; set; }

        [Required(ErrorMessage = "El campo Precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El Precio debe ser mayor que cero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo Tamaño en Metros es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El Tamaño en Metros debe ser mayor que cero")]
        public double SizeInSquareMeters { get; set; }

        [Required(ErrorMessage = "El campo Numero de Habitaciones es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Número de Habitaciones debe ser mayor que cero")]
        public int RoomCount { get; set; }

        [Required(ErrorMessage = "El campo Numero de Baños es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Número de Baños debe ser mayor que cero")]
        public int BathroomCount { get; set; }

        [Required(ErrorMessage = "El campo Descripción es obligatorio")]
        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "El campo Tipo de Propiedad es obligatorio")]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "El campo Tipo de Venta es obligatorio")]
        public int SaleTypeId { get; set; }

        [DataType(DataType.Upload)]
        public List<IFormFile>? Files { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        public string? AgentId { get; set; }

        public List<PropertyTypeViewModel>? PropertyTypes { get; set; }
        public List<SaleTypeViewModel>? SaleTypes { get; set; }
        public List<ImprovementViewModel>? Improvements { get; set; }
    }
 }
