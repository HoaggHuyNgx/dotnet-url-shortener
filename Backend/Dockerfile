# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy all project files first
COPY *.sln .
COPY UrlShortener.API/*.csproj ./UrlShortener.API/
COPY UrlShortener.Core/*.csproj ./UrlShortener.Core/
COPY UrlShortener.Infrastructure/*.csproj ./UrlShortener.Infrastructure/

# Restore dependencies for each project explicitly to ensure reliability in CI
RUN dotnet restore "UrlShortener.Core/UrlShortener.Core.csproj"
RUN dotnet restore "UrlShortener.Infrastructure/UrlShortener.Infrastructure.csproj"
RUN dotnet restore "UrlShortener.API/UrlShortener.API.csproj"

# Copy the rest of the source code
COPY . .

# Publish the API project (no need to restore again)
WORKDIR /source/UrlShortener.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 for the web application
EXPOSE 80

# Set the entrypoint for the container
ENTRYPOINT ["dotnet", "UrlShortener.API.dll"]
