Rebels

# Rebels.ShortLink API

**Rebels.ShortLink API** is a lightweight URL shortening service built with ASP.NET Core Minimal APIs. It adheres to SOLID principles, uses FluentValidation for input validation, supports localization, and provides a maintainable and extensible architecture.

## Features

- **URL Shortening**: Convert long URLs into short, shareable links.
- **URL Decoding**: Retrieve the original URL from a short link.
- **Redirection**: Automatically redirect from a short URL to the original URL.
- **Input Validation**: Ensures valid input using FluentValidation.
- **Localization**: Supports multiple languages for error messages via resource files.
- **SOLID Principles**: The project design follows SOLID principles for clean, maintainable code.

## Prerequisites

- [.NET 9 SDK (Preview)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) (or an appropriate .NET version)
- Internet connection for restoring NuGet packages

## How to Run

1. **Clone the repository:**

   ```bash
   git clone https://github.com/mpeters1975/Rebels.ShortLink.Api.git
   cd Rebels.ShortLink.Api

   dotnet restore
   dotnet run --project Rebels.ShortLink.Api

## Using the API

Encode a URL

Request:
```
POST /encode HTTP/1.1
Host: localhost:5001
Content-Type: application/json
Accept: application/json

{
  "url": "https://rebels.io/become-a-rebel"
}
```
Response:
```
{
  "id": "6054e900",
  "shortLink": "http://localhost:5001/6054e900"
}
```
## Decode a URL

Request:
```
GET /decode/6054e900 HTTP/1.1
Host: localhost:5001
Accept: application/json
```
Response:
```
{
  "originalUrl": "https://rebels.io/become-a-rebel"
}
```
## Redirect Using Short Link

Visiting:

http://localhost:5001/6054e900

automatically redirects to the original URL.

## SOLID Principles Applied
- **SRP**: Each class (services, validators, endpoints) has a single responsibility.
- **OCP**: Interfaces and dependency injection allow for easy extension without modifying existing code.
- **LSP**: The design relies on abstractions, ensuring implementations can be substituted without breaking functionality.
- **ISP**: Interfaces like IShortLinkService are kept small and focused.
- **DIP**: High-level modules depend on abstractions (IShortLinkService, IValidator<T>), which are injected via DI.

## FluentValidation
- **Validators located in the Validators/ folder enforce rules on incoming requests.**
- **Validation logic ensures that only valid data is processed by the service, providing clear error messages when input is invalid.**

License

This project is licensed under the MIT License. See the LICENSE file for details.
﻿
