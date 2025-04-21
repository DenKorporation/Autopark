FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/hosts/Autopark.ApiGateway/Autopark.ApiGateway.csproj", "src/hosts/Autopark.ApiGateway/"]
COPY ["src/common/Autopark.Common/Autopark.Common.csproj", "src/common/Autopark.Common/"]
RUN dotnet restore "src/hosts/Autopark.ApiGateway/Autopark.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/hosts/Autopark.ApiGateway"
RUN dotnet build "Autopark.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Autopark.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Autopark.ApiGateway.dll"]