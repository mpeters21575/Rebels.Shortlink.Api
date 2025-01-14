using Rebels.ShortLink.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Compose();

await app.RunAsync();

public partial class Program { }