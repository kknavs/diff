#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DiffApplication/DiffApplication.Rest/DiffApplication.Rest.csproj", "DiffApplication/DiffApplication.Rest/"]
COPY ["DiffApplication/DiffApplication.Infrastructure/DiffApplication.Infrastructure.csproj", "DiffApplication/DiffApplication.Infrastructure/"]
COPY ["DiffApplication/DiffApplication.Domain/DiffApplication.Domain.csproj", "DiffApplication/DiffApplication.Domain/"]

RUN dotnet restore "DiffApplication/DiffApplication.Rest/DiffApplication.Rest.csproj"

COPY . .
WORKDIR "/src/DiffApplication/DiffApplication.Rest/"
RUN dotnet build "./DiffApplication.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DiffApplication.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiffApplication.Rest.dll"]