using FluentValidation.TestHelper;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Models.Decode;
using Rebels.ShortLink.Api.Validators;

namespace Rebels.ShortLink.Tests.Services.ShortLinkServiceTests;

public sealed class DecodeTests
{
    private readonly DecodeRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var request = new DecodeRequest("");
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Short ID cannot be empty.");
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Not_Alphanumeric()
    {
        var request = new DecodeRequest("@@@???");
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Short ID must be alphanumeric.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var request = new DecodeRequest("abc123");
        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}