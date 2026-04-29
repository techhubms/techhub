# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY TechHub.slnx .
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY src/TechHub.Api/TechHub.Api.csproj src/TechHub.Api/
COPY src/TechHub.Core/TechHub.Core.csproj src/TechHub.Core/
COPY src/TechHub.Infrastructure/TechHub.Infrastructure.csproj src/TechHub.Infrastructure/
COPY src/TechHub.ServiceDefaults/TechHub.ServiceDefaults.csproj src/TechHub.ServiceDefaults/

# Restore dependencies
RUN dotnet restore src/TechHub.Api/TechHub.Api.csproj

# Copy source code
COPY src/ src/

# Copy docs files referenced as embedded resources
COPY docs/writing-style-guidelines.md docs/

# Build and publish
WORKDIR /src/src/TechHub.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Install curl for healthcheck and yt-dlp for YouTube transcript fetching
# Pin yt-dlp to a specific version for reproducible builds
RUN apt-get update && apt-get install -y curl python3 && rm -rf /var/lib/apt/lists/* \
    && curl -L https://github.com/yt-dlp/yt-dlp/releases/download/2026.03.17/yt-dlp -o /usr/local/bin/yt-dlp \
    && chmod a+rx /usr/local/bin/yt-dlp

# Copy published application
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080

# Expose port
EXPOSE 8080

# Run as non-root user
USER $APP_UID

ENTRYPOINT ["dotnet", "TechHub.Api.dll"]
