using FluentValidation.TestHelper;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Validators;

namespace Rebels.ShortLink.Tests.Services.ShortLinkServiceTests;

public sealed class EncodeTests
{
    private readonly EncodeRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Url_Is_Empty()
    {
        var request = new EncodeRequest("");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Url)
            .WithErrorMessage("URL cannot be empty.");
    }

    [Fact]
    public void Should_Have_Error_When_Url_Is_Invalid()
    {
        var request = new EncodeRequest("invalid_url");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Url)
            .WithErrorMessage("Invalid URL format.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Url_Is_Valid()
    {
        var request = new EncodeRequest("https://rebels.io/");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}