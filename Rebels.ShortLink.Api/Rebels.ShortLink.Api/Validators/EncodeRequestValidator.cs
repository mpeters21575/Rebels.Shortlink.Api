using FluentValidation;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Models.Encode;

namespace Rebels.ShortLink.Api.Validators;

public class EncodeRequestValidator : AbstractValidator<EncodeRequest>
{
    public EncodeRequestValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("URL cannot be empty.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Invalid URL format.");
    }
}