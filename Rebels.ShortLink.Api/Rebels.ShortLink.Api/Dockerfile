FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rebels.ShortLink.Api/Rebels.ShortLink.Api.csproj", "Rebels.ShortLink.Api/"]
RUN dotnet restore "Rebels.ShortLink.Api/Rebels.ShortLink.Api.csproj"
COPY . .
WORKDIR "/src/Rebels.ShortLink.Api"
RUN dotnet build "Rebels.ShortLink.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Rebels.ShortLink.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rebels.ShortLink.Api.dll"]
