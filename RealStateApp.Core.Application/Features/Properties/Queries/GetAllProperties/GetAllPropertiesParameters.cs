using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    internal class GetAllPropertiesParameters
    {
        public int? PropertyTypeId { get; set; }
        public int? SaleTypeId { get; set; }
        public decimal? MinPrice  { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? AgentId { get; set; }
    }
}
