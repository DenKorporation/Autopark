FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/hosts/Autopark.PublicApi.Host/Autopark.PublicApi.Host.csproj", "src/hosts/Autopark.PublicApi.Host/"]
COPY ["src/publicapi/Autopark.PublicApi.Bl/Autopark.PublicApi.Bl.csproj", "src/publicapi/Autopark.PublicApi.Bl/"]
COPY ["src/common/Autopark.Common.Bl/Autopark.Common.Bl.csproj", "src/common/Autopark.Common.Bl/"]
COPY ["src/common/Autopark.Common/Autopark.Common.csproj", "src/common/Autopark.Common/"]
COPY ["src/publicapi/Autopark.PublicApi.Dal/Autopark.PublicApi.Dal.csproj", "src/publicapi/Autopark.PublicApi.Dal/"]
COPY ["src/common/Autopark.Dal.Core/Autopark.Dal.Core.csproj", "src/common/Autopark.Dal.Core/"]
COPY ["src/publicapi/Autopark.PublicApi.Models/Autopark.PublicApi.Models.csproj", "src/publicapi/Autopark.PublicApi.Models/"]
COPY ["src/publicapi/Autopark.PublicApi.Shared/Autopark.PublicApi.Shared.csproj", "src/publicapi/Autopark.PublicApi.Shared/"]
COPY ["src/authorization/Autopark.Authorization.Shared/Autopark.Authorization.Shared.csproj", "src/authorization/Autopark.Authorization.Shared/"]
COPY ["src/common/Autopark.Common.Web/Autopark.Common.Web.csproj", "src/common/Autopark.Common.Web/"]
RUN dotnet restore "src/hosts/Autopark.PublicApi.Host/Autopark.PublicApi.Host.csproj"
COPY . .
WORKDIR "/src/src/hosts/Autopark.PublicApi.Host"
RUN dotnet build "Autopark.PublicApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Autopark.PublicApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Autopark.PublicApi.Host.dll"]
