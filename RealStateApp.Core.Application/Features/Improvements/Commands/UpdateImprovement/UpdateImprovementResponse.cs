using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    public class UpdateImprovementResponse
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
