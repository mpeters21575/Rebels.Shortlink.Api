using FluentValidation;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Models.Decode;

namespace Rebels.ShortLink.Api.Validators;

public class DecodeRequestValidator : AbstractValidator<DecodeRequest>
{
    public DecodeRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Short ID cannot be empty.")
            .Matches("^[a-zA-Z0-9]+$")
            .WithMessage("Short ID must be alphanumeric.");
    }
}