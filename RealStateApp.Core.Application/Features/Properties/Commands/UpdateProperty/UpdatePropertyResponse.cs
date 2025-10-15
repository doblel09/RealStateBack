using RealStateApp.Core.Application.Dtos.Improvement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyResponse
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
        public List<ImprovementDto> Improvements { get; set; }
        public string AgentId { get; set; }
    }
}
