using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Rebels.ShortLink.Api.Models;
using Rebels.ShortLink.Api.Services;

namespace Rebels.ShortLink.Tests.EndPoints;

public class DecodeEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient CreateClientWithMocks(
        IValidator<DecodeRequest> validator,
        IShortLinkService shortLinkService)
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(validator);
                services.AddSingleton(shortLinkService);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Decode_ReturnsOk_WhenIdIsValid()
    {
        var validId = "validuri123";
        var originalUrl = "https://rebels.io/become-a-rebel";
            
        var validatorMock = new Mock<IValidator<DecodeRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<DecodeRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        var serviceMock = new Mock<IShortLinkService>();
        serviceMock.Setup(s => s.DecodeUrl(validId)).Returns(originalUrl);

        var client = CreateClientWithMocks(validatorMock.Object, serviceMock.Object);
        var response = await client.GetAsync($"/decode/{validId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var decodeResponse = await response.Content.ReadFromJsonAsync<DecodeResponse>();
        decodeResponse.Should().NotBeNull();
        decodeResponse!.OriginalUrl.Should().Be(originalUrl);
    }

    [Fact]
    public async Task Decode_ReturnsBadRequest_WhenValidationFails()
    {
        var invalidId = "@H@";
        var validatorMock = new Mock<IValidator<DecodeRequest>>();
        var validationErrors = new ValidationResult(new[]
        {
            new ValidationFailure("Id", "Short ID must be alphanumeric.")
        });
        validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<DecodeRequest>(), default))
            .ReturnsAsync(validationErrors);

        var serviceMock = new Mock<IShortLinkService>();
        var client = CreateClientWithMocks(validatorMock.Object, serviceMock.Object);
        var response = await client.GetAsync($"/decode/{invalidId}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errors = await response.Content.ReadFromJsonAsync<string[]>();
        errors.Should().Contain("Short ID must be alphanumeric.");
    }

    [Fact]
    public async Task Decode_ReturnsNotFound_WhenUrlNotFound()
    {
        var nonExistentId = "nonexistentid";
        var validatorMock = new Mock<IValidator<DecodeRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<DecodeRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        var serviceMock = new Mock<IShortLinkService>();
        serviceMock.Setup(s => s.DecodeUrl(nonExistentId)).Returns((string?)null);

        var client = CreateClientWithMocks(validatorMock.Object, serviceMock.Object);
        var response = await client.GetAsync($"/decode/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var rawMessage = await response.Content.ReadAsStringAsync();
        rawMessage.Trim('"').Should().Be("No URL found for the given ID.");
    }
}