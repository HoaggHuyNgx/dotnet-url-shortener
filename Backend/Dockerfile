# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy all the project files and solution file
COPY *.sln .
COPY UrlShortener.API/*.csproj ./UrlShortener.API/
COPY UrlShortener.Core/*.csproj ./UrlShortener.Core/
COPY UrlShortener.Infrastructure/*.csproj ./UrlShortener.Infrastructure/

# Copy the rest of the source code BEFORE restoring
# This ensures that restore runs on the most up-to-date code, fixing potential CI issues
COPY . .

# Restore dependencies for the entire solution
RUN dotnet restore *.sln

# Set the working directory to the API project and publish
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
