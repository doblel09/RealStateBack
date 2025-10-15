using RealStateApp.Infrastructure.Identity;
using RealStateApp.Infrastructure.Persistence;
using RealStateApp.Infrastructre.Shared;
using RealStateApi.Extensions;
using RealStateApp.Core.Application;
using Microsoft.AspNetCore.Mvc;
using RealStateApi.Middleware;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using RealStateApp.Infrastructure.Identity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressMapClientErrors = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddTransient<IBaseAccountService, BaseAccountService>();
builder.Services.AddIdentityInfrastructureForWebApi(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayerForWebApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();
builder.Services.AddAuthorization();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



//builder.Services.AddIdentityInfrastructureForWebApi(builder.Configuration);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtensions(app);
}

await app.Services.RunIdentitySeeds();
await app.Services.RunSeeds();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.UseExceptionHandler();
app.UseSession();
app.UseCors("CorsPolicy");
await app.Services.RunIdentitySeeds();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.RunAsync();
