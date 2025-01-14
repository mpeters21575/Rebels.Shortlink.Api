using System.Collections.Concurrent;
using FluentValidation;

namespace Rebels.ShortLink.Api.Services;

public interface IShortLinkService
{
    string EncodeUrl(string originalUrl);
    string? DecodeUrl(string shortId);
}

public sealed class ShortLinkService : IShortLinkService
{
    private static readonly ConcurrentDictionary<string, string> UrlMap = new();

    public string EncodeUrl(string originalUrl)
    {
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x)
            .NotEmpty().WithMessage("URL cannot be empty.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Invalid URL format.");

        var validationResult = validator.Validate(originalUrl);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors[0].ErrorMessage, nameof(originalUrl));
        }

        var shortId = Guid.NewGuid().ToString("N")[..8];

        UrlMap[shortId] = originalUrl;

        return shortId;
    }

    public string? DecodeUrl(string shortId) 
        => string.IsNullOrWhiteSpace(shortId) 
            ? null 
            : UrlMap.GetValueOrDefault(shortId);
}