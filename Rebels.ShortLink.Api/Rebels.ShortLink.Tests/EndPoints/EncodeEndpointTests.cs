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

public class EncodeEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient CreateClientWithMocks(
        IValidator<EncodeRequest> validator,
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
    public async Task Encode_ReturnsOk_WhenRequestIsValid()
    {
        var validUrl = "https://rebels.io/become-a-rebel";
        var expectedShortId = "6054e900";
        var validatorMock = new Mock<IValidator<EncodeRequest>>();
            
        validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<EncodeRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        var serviceMock = new Mock<IShortLinkService>();
        serviceMock.Setup(s => s.EncodeUrl(validUrl)).Returns(expectedShortId);

        var client = CreateClientWithMocks(validatorMock.Object, serviceMock.Object);
        var requestObject = new EncodeRequest(validUrl);
        var response = await client.PostAsJsonAsync("/encode", requestObject);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var encodeResponse = await response.Content.ReadFromJsonAsync<EncodeResponse>();
        encodeResponse.Should().NotBeNull();
        encodeResponse!.Id.Should().Be(expectedShortId);
        encodeResponse.ShortLink.Should().Contain($"/redirect/{expectedShortId}");
    }

    [Fact]
    public async Task Encode_ReturnsBadRequest_WhenValidationFails()
    {
        var invalidUrl = ""; 
        var validationErrors = new ValidationResult(new[]
        {
            new ValidationFailure("Url", "URL cannot be empty.")
        });
        var validatorMock = new Mock<IValidator<EncodeRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<EncodeRequest>(), CancellationToken.None))
            .ReturnsAsync(validationErrors);

        var serviceMock = new Mock<IShortLinkService>();
        var client = CreateClientWithMocks(validatorMock.Object, serviceMock.Object);
        var requestObject = new EncodeRequest(invalidUrl);
        var response = await client.PostAsJsonAsync("/encode", requestObject);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errors = await response.Content.ReadFromJsonAsync<string[]>();
        errors.Should().Contain("URL cannot be empty.");
    }
}