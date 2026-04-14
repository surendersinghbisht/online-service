# =========================
# 1. Build Stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy specific csproj and restore dependencies
COPY ["onilne-service.csproj", "./"]
RUN dotnet restore "./onilne-service.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "onilne-service.csproj" -c Release -o /app/publish


# =========================
# 2. Runtime Stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose port (Render uses 10000 by default)
EXPOSE 10000

# Set environment
ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the app
ENTRYPOINT ["dotnet", "onilne-service.dll"]
