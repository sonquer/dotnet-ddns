FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY ["Openddns.Host/Openddns.Host.csproj", "Openddns.Host/"]
COPY ["Openddns.Application/Openddns.Application.csproj", "Openddns.Application/"]
COPY ["Openddns.Providers/Openddns.Providers.csproj", "Openddns.Providers/"]
COPY ["Openddns.Core/Openddns.Core.csproj", "Openddns.Core/"]
COPY ["Openddns.Infrastructure/Openddns.Infrastructure.csproj", "Openddns.Infrastructure/"]
RUN dotnet restore "Openddns.Host/Openddns.Host.csproj"
COPY . .
WORKDIR "/Openddns.Host"
RUN dotnet build "Openddns.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Openddns.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Openddns.Host.dll"]