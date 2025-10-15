using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Behaviours;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using System.Reflection;


namespace RealStateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayerForWebApp(this IServiceCollection services)
        {
            ApplicationLayerGenericService(services);
            ApplicationLayerGenericConfigurations(services);

            #region Services
            services.AddTransient<IUserService, UserService>();
            #endregion
        }

        public static void AddApplicationLayerForWebApi(this IServiceCollection services)
        {
            ApplicationLayerGenericService(services);
            ApplicationLayerGenericConfigurations(services);
        }

        private static void ApplicationLayerGenericService(this IServiceCollection services)
        {
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            #endregion
        }

        private static void ApplicationLayerGenericConfigurations(this IServiceCollection services)
        {
            #region Configurations
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            #endregion
        }
    }
}