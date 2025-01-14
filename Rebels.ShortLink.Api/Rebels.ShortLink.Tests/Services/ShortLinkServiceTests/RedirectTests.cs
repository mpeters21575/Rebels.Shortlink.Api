using FluentValidation.TestHelper;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Validators;

namespace Rebels.ShortLink.Tests.Services.ShortLinkServiceTests;

public sealed class RedirectTests
{
    private readonly RedirectRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var request = new RedirectRequest("");
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Short ID cannot be empty.");
    }

    [Fact]
    public void Should_Have_Error_When_Id_ContainsNonAlphanumeric()
    {
        var request = new RedirectRequest("abc@123");
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Short ID must be alphanumeric.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var request = new RedirectRequest("abc123");
        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}