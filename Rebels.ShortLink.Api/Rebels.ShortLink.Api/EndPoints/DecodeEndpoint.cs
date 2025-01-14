using FluentValidation;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Models.Decode;
using Rebels.ShortLink.Api.Services;

namespace Rebels.ShortLink.Api.EndPoints;

public static class DecodeEndpoint
{
    public static IEndpointRouteBuilder MapDecodeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/decode/{id}", async (
                string id,
                IValidator<DecodeRequest> validator,
                IShortLinkService shortLinkService) =>
            {
                var request = new DecodeRequest(id);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                    return Results.BadRequest(errors);
                }

                var originalUrl = $"{shortLinkService.DecodeUrl(id)}";
                if (string.IsNullOrWhiteSpace(originalUrl))
                {
                    return Results.NotFound("No URL found for the given ID.");
                }

                return Results.Ok(new DecodeResponse(originalUrl));
            })
            .Produces<DecodeResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DecodeUrl")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "DecodeUrl";
                operation.Summary = "Decodes a short ID back to its original URL.";
                return operation;
            });

        return routes;
    }
}