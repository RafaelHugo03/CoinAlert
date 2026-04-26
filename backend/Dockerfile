FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/CoinAlertApi/CoinAlertApi.csproj src/CoinAlertApi/
RUN dotnet restore src/CoinAlertApi/CoinAlertApi.csproj

COPY src/CoinAlertApi/ src/CoinAlertApi/
RUN dotnet publish src/CoinAlertApi/CoinAlertApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CoinAlertApi.dll"]
