using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Services;

namespace Rebels.ShortLink.Api.EndPoints;

public static class RedirectEndpoint
{
    public static IEndpointRouteBuilder MapRedirectEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/redirect/{id}", async (
                [FromRoute] string id,
                [FromServices] IValidator<RedirectRequest> validator,
                [FromServices] IShortLinkService shortLinkService) =>
            {
                var request = new RedirectRequest(id);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                    return Results.BadRequest(errors);
                }

                var originalUrl = shortLinkService.DecodeUrl(id);
                if (string.IsNullOrWhiteSpace(originalUrl))
                {
                    return Results.NotFound("No URL found for the given ID.");
                }

                return Results.Redirect(originalUrl);
            })
            .Produces(StatusCodes.Status302Found)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("RedirectToOriginal")
            .WithOpenApi(op =>
            {
                op.OperationId = "RedirectToOriginal";
                op.Summary = "Redirects to the original URL based on the short link ID.";
                return op;
            });

        return routes;
    }
}