using RealStateApp.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Domain.Entities
{
    public class Property : AuditableBaseEntity
    {
        public string UniqueCode { get; set; } // 6 digits
        public decimal Price { get; set; }
        public double SizeInSquareMeters { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; } // 1-4 images
        public bool IsAvailable { get; set; } // true = available, false = sold
        
        // Navigation Properties
        public int PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; }

        public int SaleTypeId { get; set; }
        public SaleType? SaleType { get; set; }

        public string AgentId { get; set; }

        public ICollection<Improvement>? Improvements { get; set; }
        public ICollection<Offer>? Offers { get; set; }
    }

    


}
