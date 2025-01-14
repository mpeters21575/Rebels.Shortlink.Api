using System.Reflection;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Rebels.ShortLink.Api.EndPoints;
using Rebels.ShortLink.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Rebels ShortLink API", Version = "v1" });
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
app.MapEncodeEndpoint();
app.MapDecodeEndpoint();
app.MapRedirectEndpoint();

await app.RunAsync();