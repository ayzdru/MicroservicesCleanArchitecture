FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Basket/CleanArchitecture.Services.Basket.API/CleanArchitecture.Services.Basket.API.csproj", "Services/Basket/CleanArchitecture.Services.Basket.API/"]
RUN dotnet restore "Services/Basket/CleanArchitecture.Services.Basket.API/CleanArchitecture.Services.Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket/CleanArchitecture.Services.Basket.API"
RUN dotnet build "CleanArchitecture.Services.Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchitecture.Services.Basket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Services.Basket.API.dll"]