using Rebels.ShortLink.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Compose();

await app.RunAsync();

/// <summary>
/// Used for the Endpoint Unit Tests
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program { }