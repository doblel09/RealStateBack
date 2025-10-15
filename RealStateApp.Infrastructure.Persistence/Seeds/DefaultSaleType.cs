using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Seeds
{
    public static class DefaultSaleType
    {
        public static async Task RunSeeds(ApplicationContext context)
        {
            if (!context.SaleTypes.Any())
            {
                var saleTypes = new List<SaleType>
                {
                    new SaleType { Name = "Venta", Description = "Propiedad en venta" },
                    new SaleType { Name = "Alquiler", Description = "Propiedad en alquiler" },
                };
                context.SaleTypes.AddRange(saleTypes);
                await context.SaveChangesAsync();
            }
        }
    }
}
