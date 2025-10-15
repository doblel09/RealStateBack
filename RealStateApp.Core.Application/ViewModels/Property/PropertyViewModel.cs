

using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.Property
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string UniqueCode { get; set; } // 6 digits
        public decimal Price { get; set; }
        public double SizeInSquareMeters { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; } // 1-4 images
        public bool IsAvailable { get; set; } // true = available, false = sold
        public PropertyTypeViewModel PropertyType { get; set; }
        public SaleTypeViewModel SaleType { get; set; }

        public List<OfferViewModel>? Offers { get; set; }
        public List<ImprovementViewModel>? Improvements { get; set; }
        public string AgentId { get; set; }
        public UserViewModel Agent { get; set; }

    }
}
