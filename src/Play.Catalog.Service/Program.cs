using System.Collections.Immutable;
using MassTransit;
using Microsoft.OpenApi.Models;
using Play.Catalog.Service.Entities;
using Play.Common.Identity;
using Play.Common.MassTransit;
using Play.Common.MongoDB;
using Play.Common.Settings;


var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

const string AllowedOriginSetting = "AllowedOrigin";

// Add services to the container.

ServiceSettings serviceSettings;

serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddMongo()
                .AddMongoRepository<Item>("items")
                .AddMassTransitWithRabbitMq()
                .AddJwtBearerAuthentication();
                

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Catalog.Service", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Catalog.Service v1");
        c.RoutePrefix = string.Empty;
    });

    app.UseCors(options =>
    {
        options.WithOrigins(Configuration[AllowedOriginSetting])
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program {}