using Rebels.ShortLink.Api;

await WebApplication
    .CreateBuilder(args)
    .Compose()
    .RunAsync();

namespace Rebels.ShortLink.Api
{
    /// <summary>
    /// Used for the Endpoint Unit Tests
    /// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program { }
}