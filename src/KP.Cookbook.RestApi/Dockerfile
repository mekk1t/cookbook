#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/KP.Cookbook.RestApi/KP.Cookbook.RestApi.csproj", "src/KP.Cookbook.RestApi/"]
COPY ["src/KP.Cookbook.Features/KP.Cookbook.Features.csproj", "src/KP.Cookbook.Features/"]
COPY ["src/KP.Cookbook.Cqrs/KP.Cookbook.Cqrs.csproj", "src/KP.Cookbook.Cqrs/"]
COPY ["src/KP.Cookbook.Database/KP.Cookbook.Database.csproj", "src/KP.Cookbook.Database/"]
COPY ["src/KP.Cookbook.Domain/KP.Cookbook.Domain.csproj", "src/KP.Cookbook.Domain/"]
RUN dotnet restore "src/KP.Cookbook.RestApi/KP.Cookbook.RestApi.csproj"
COPY . .
WORKDIR "/src/src/KP.Cookbook.RestApi"
RUN dotnet build "KP.Cookbook.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KP.Cookbook.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KP.Cookbook.RestApi.dll"]
