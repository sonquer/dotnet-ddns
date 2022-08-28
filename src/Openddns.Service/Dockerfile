#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Openddns.Service/Openddns.Service.csproj", "Openddns.Service/"]
RUN dotnet restore "Openddns.Service/Openddns.Service.csproj"
COPY . .
WORKDIR "/src/Openddns.Service"
RUN dotnet build "Openddns.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Openddns.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Openddns.Service.dll"]