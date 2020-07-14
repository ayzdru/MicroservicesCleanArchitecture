FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY . .
ENTRYPOINT ["dotnet", "CleanArchitecture.Services.Basket.API.dll"]