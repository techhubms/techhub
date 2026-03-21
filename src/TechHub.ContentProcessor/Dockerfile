# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY TechHub.slnx .
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY src/TechHub.ContentProcessor/TechHub.ContentProcessor.csproj src/TechHub.ContentProcessor/
COPY src/TechHub.Core/TechHub.Core.csproj src/TechHub.Core/
COPY src/TechHub.Infrastructure/TechHub.Infrastructure.csproj src/TechHub.Infrastructure/

# Restore dependencies
RUN dotnet restore src/TechHub.ContentProcessor/TechHub.ContentProcessor.csproj

# Copy source code
COPY src/ src/

# Build and publish
WORKDIR /src/src/TechHub.ContentProcessor
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# Set environment variables
ENV DOTNET_ENVIRONMENT=Production

# Run as non-root user
USER $APP_UID

ENTRYPOINT ["dotnet", "TechHub.ContentProcessor.dll"]
