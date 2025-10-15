using RealStateApp.Core.Application.Dtos.Improvement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Property
{
    public class PropertyImprovementDto
    {
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }
        public ImprovementDto Improvement { get; set; }
    }
}
