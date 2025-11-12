# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# SIMPLIFICATION: Copy ALL source code from the build context at once.
# This is less optimized for local caching but much more reliable for CI builds.
COPY . .

# Restore dependencies for the entire solution.
RUN dotnet restore "UrlShortener.sln"

# Publish the API project.
RUN dotnet publish "UrlShortener.API/UrlShortener.API.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 for the web application
EXPOSE 80

# Set the entrypoint for the container
ENTRYPOINT ["dotnet", "UrlShortener.API.dll"]
