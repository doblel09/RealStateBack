using RealStateApp.Core.Application.Dtos.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Favorite
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public int PropertyId { get; set; }
        public PropertyDto Property { get; set; }
    }
}
