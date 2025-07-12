# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution
COPY ["LibraryManagementSystem.sln", "./"]

# Copy project files
COPY ["LibraryManagementSystem.Api/LibraryManagementSystem.Api.csproj", "LibraryManagementSystem.Api/"]
COPY ["LibraryManagementSystem.Application/LibraryManagementSystem.Application.csproj", "LibraryManagementSystem.Application/"]
COPY ["LibraryManagementSystem.Domain/LibraryManagementSystem.Domain.csproj", "LibraryManagementSystem.Domain/"]
COPY ["LibraryManagementSystem.Infrastructure/LibraryManagementSystem.Infrastructure.csproj", "LibraryManagementSystem.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "LibraryManagementSystem.sln"

# Copy all source files
COPY . .

# Build project
WORKDIR "/src/LibraryManagementSystem.Api"
RUN dotnet build "LibraryManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish to folder
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LibraryManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "LibraryManagementSystem.Api.dll"]
