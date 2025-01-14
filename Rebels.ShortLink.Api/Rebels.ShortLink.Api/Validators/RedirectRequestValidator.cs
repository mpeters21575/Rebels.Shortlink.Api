using FluentValidation;
using Rebels.ShortLink.Api.Models;

namespace Rebels.ShortLink.Api.Validators;

public class RedirectRequestValidator : AbstractValidator<RedirectRequest>
{
    public RedirectRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Short ID cannot be empty.")
            .Matches("^[a-zA-Z0-9]+$")
            .WithMessage("Short ID must be alphanumeric.");
    }
}