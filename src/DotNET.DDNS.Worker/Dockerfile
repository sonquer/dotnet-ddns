FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DotNET.DDNS.Worker/DotNET.DDNS.Worker.csproj", "DotNET.DDNS.Worker/"]
COPY ["DotNET.DDNS.Application/DotNET.DDNS.Application.csproj", "DotNET.DDNS.Application/"]
COPY ["DotNET.DDNS.Providers.Ovh/DotNET.DDNS.Providers.Ovh.csproj", "DotNET.DDNS.Providers.Ovh/"]
COPY ["DotNET.DDNS.Providers/DotNET.DDNS.Providers.csproj", "DotNET.DDNS.Providers/"]
RUN dotnet restore "DotNET.DDNS.Worker/DotNET.DDNS.Worker.csproj"
COPY . .
WORKDIR "/src/DotNET.DDNS.Worker"
RUN dotnet build "DotNET.DDNS.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNET.DDNS.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNET.DDNS.Worker.dll"]
