using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Seeds
{
    public static class DefaultImprovement
    {
        public static async Task SeedAsync(ApplicationContext context)
        {
            if (!context.Improvements.Any())
            {
                var improvements = new List<Improvement>
                {
                    new Improvement { Name = "Piscina", Description = "Propiedad tiene acesso a piscina" },
                    new Improvement { Name = "Aire Acondicionado", Description = "Aire acondicionado" },
                    new Improvement { Name = "Calentador de Agua", Description = "Propiedad tiene acesso a calentador" },
                    new Improvement { Name = "Cocina Equipada", Description = "Propiedad tiene acesso a cocina equipada" },
                    new Improvement { Name = "Parqueo", Description = "Propiedad tiene acesso a parqueo" },
                    new Improvement { Name = "Internet", Description = "Propiedad tiene acesso a internet" },

                };
                context.Improvements.AddRange(improvements);
                await context.SaveChangesAsync();
            }
        }
    }
}
