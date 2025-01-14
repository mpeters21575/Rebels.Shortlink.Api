using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Services;

namespace Rebels.ShortLink.Api.EndPoints;

public static class EncodeEndpoint
{
    public static IEndpointRouteBuilder MapEncodeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/encode", async (
            IValidator<EncodeRequest> validator,
            [FromBody] EncodeRequest request,
            IShortLinkService shortLinkService,
            HttpRequest httpRequest) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                return Results.BadRequest(errors);
            }

            var shortId = shortLinkService.EncodeUrl(request.Url);
            var host = $"{httpRequest.Scheme}://{httpRequest.Host}";
            var shortLink = $"{host}/redirect/{shortId}";

            return Results.Ok(new EncodeResponse(shortId, shortLink));
        }).Produces<DecodeResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("EncodeUrl")
        .WithOpenApi(operation =>
        {
            operation.OperationId = "EncodeUrl";
            operation.Summary = "Encodes a URL to a shorter version.";
            return operation;
        });

        return routes;
    }
}