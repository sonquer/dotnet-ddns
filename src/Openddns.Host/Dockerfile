FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Openddns.Host/Openddns.Host.csproj", "Openddns.Host/"]
COPY ["Openddns.Application/Openddns.Application.csproj", "Openddns.Application/"]
COPY ["Openddns.Providers/Openddns.Providers.csproj", "Openddns.Providers/"]
COPY ["Openddns.Core/Openddns.Core.csproj", "Openddns.Core/"]
COPY ["Openddns.Infrastructure.SQLite/Openddns.Infrastructure.SQLite.csproj", "Openddns.Infrastructure.SQLite/"]
RUN dotnet restore "Openddns.Host/Openddns.Host.csproj"
COPY . .
WORKDIR "/src/Openddns.Host"
RUN dotnet build "Openddns.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Openddns.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Openddns.Host.dll"]