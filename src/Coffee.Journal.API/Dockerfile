# Base stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

## Copy project file and restore dependencies
COPY src/Coffee.Journal.API/Coffee.Journal.API.csproj Coffee.Journal.API/
COPY src/Coffee.Journal.Application/Coffee.Journal.Application.csproj Coffee.Journal.Application/
COPY src/Coffee.Journal.Domain/Coffee.Journal.Domain.csproj Coffee.Journal.Domain/
COPY src/Coffee.Journal.Infrastructure/Coffee.Journal.Infrastructure.csproj Coffee.Journal.Infrastructure/
RUN dotnet restore Coffee.Journal.API/Coffee.Journal.API.csproj

## Copy source code and build
COPY src/Coffee.Journal.API/ Coffee.Journal.API/
COPY src/Coffee.Journal.Application/ Coffee.Journal.Application/
COPY src/Coffee.Journal.Domain/ Coffee.Journal.Domain/
COPY src/Coffee.Journal.Infrastructure/ Coffee.Journal.Infrastructure/
WORKDIR /src/Coffee.Journal.API
RUN dotnet build -c Release --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Final stage
FROM base AS final
# SqlClient does not support Globalization Invariant Mode, so install ICU on Alpine
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Coffee.Journal.API.dll"]