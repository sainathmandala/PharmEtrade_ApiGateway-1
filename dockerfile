# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /src

# Copy the project files
COPY ["PharmEtrade_ApiGateway/PharmEtrade_ApiGateway.csproj", "PharmEtrade_ApiGateway/"]

# Restore dependencies
RUN dotnet restore "PharmEtrade_ApiGateway/PharmEtrade_ApiGateway.csproj"

# Copy the entire project
COPY . .

# Build and publish the application
RUN dotnet publish "PharmEtrade_ApiGateway/PharmEtrade_ApiGateway.csproj" -c Release -o /app

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory for the runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app .

# Expose port 5000
EXPOSE 5000

# Set the entry point for the application
ENTRYPOINT ["dotnet", "PharmEtrade_ApiGateway.dll", "--urls", "http://0.0.0.0:5000"]
