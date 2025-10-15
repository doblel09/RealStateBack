using Swashbuckle.AspNetCore.SwaggerUI;

namespace RealStateApi.Extensions
{
    public static class AppExtension
    {
        public static void UseSwaggerExtensions(this IApplicationBuilder app, IEndpointRouteBuilder routeBuilder)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DefaultModelRendering(ModelRendering.Model);
                var versionDescriptions = routeBuilder.DescribeApiVersions();

                if (versionDescriptions != null && versionDescriptions.Any())
                {
                    foreach (var apiVersion in versionDescriptions)
                    {
                        var url = $"/swagger/{apiVersion.GroupName}/swagger.json";
                        var name = $"RealState Api - {apiVersion.GroupName.ToUpperInvariant()}";
                        options.SwaggerEndpoint(url, name);
                    }
                }
            });
        }
    }
}
