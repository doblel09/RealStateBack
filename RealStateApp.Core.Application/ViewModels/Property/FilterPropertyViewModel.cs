using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Property
{
    public class FilterPropertyViewModel
    {
        public int? PropertyTypeId { get; set; }
        public int? SaleTypeId { get; set; }
        public string? UniqueCode { get; set; } 
        public string? AgentId { get; set; }
    }
}
