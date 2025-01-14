using System.Reflection;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Rebels.ShortLink.Api.EndPoints;
using Rebels.ShortLink.Api.Services;

namespace Rebels.ShortLink.Api;

public static class CompositionContainer
{
    public static WebApplication Compose(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", 
                new OpenApiInfo
                {
                    Title = "Rebels ShortLink API", 
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Marco Peters",
                        Url = new Uri("https://malentech.net")
                    }
                });
        });
        builder.Services.AddSingleton<IShortLinkService, ShortLinkService>();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapEndpoints();

        return app;
    }
}