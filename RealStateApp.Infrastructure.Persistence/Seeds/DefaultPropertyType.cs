using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Seeds
{
    public class DefaultPropertyType
    {
        public static async Task SeedAsync(ApplicationContext context)
        {
            if (!context.PropertyTypes.Any())
            {
                var propertyTypes = new List<PropertyType>
                {
                    new PropertyType { Name = "Apartamento", Description = "Apartamento" },
                    new PropertyType { Name = "Casa", Description = "Casa" },
                    new PropertyType { Name = "Condominio" , Description = "Condominio"},
                    new PropertyType { Name = "Terreno", Description = "Terreno" }
                };
                context.PropertyTypes.AddRange(propertyTypes);
                await context.SaveChangesAsync();
            }
        }
    }
}
