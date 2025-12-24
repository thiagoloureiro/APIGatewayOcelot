FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["APIGatewayOcelot/APIGatewayOcelot.csproj", "APIGatewayOcelot/"]
RUN dotnet restore "APIGatewayOcelot/APIGatewayOcelot.csproj"
COPY . .
WORKDIR "/src/APIGatewayOcelot"
RUN dotnet build "./APIGatewayOcelot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./APIGatewayOcelot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIGatewayOcelot.dll"]
