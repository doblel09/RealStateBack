using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;
using RealStateApp.Infrastructure.Persistence.Seeds;

namespace RealStateApp.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IFavoriteRepository, FavoriteRepository>();
            services.AddTransient<IImprovementRepository, ImprovementRepository>();
            services.AddTransient<IOfferRepository, OfferRepository>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            
            services.AddTransient<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddTransient<ISaleTypeRepository, SaleTypeRepository>();
            #endregion
        }

        public static async Task RunSeeds(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    await DefaultPropertyType.SeedAsync(context);
                    await DefaultImprovement.SeedAsync(context);
                    await DefaultSaleType.RunSeeds(context);
            }
        }

        /*
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration config)
        {

            #region "Database connection"
            if (config.GetValue<bool>("UseDatabaseInMemory"))
            {
                services.AddDbContext<ApplicationContext>(options =>
                options.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString, m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion

        }
        */
    }
}
