# Coffee Journal API

REST API for a public, multi-user coffee journal.

## The idea

I love specialty coffee. This year I had the privilege to discover this world, and
I wanted to explore and experience new coffee shops that sell specialty coffee.
I'm also improving my V60 method and equipment, so I wanted to know what other
people use to brew their coffee.

That is what this app is for. Users sign in, share the coffee equipment they
brew with, and review roasters, coffees and coffee shops. The catalog can be read
without signing in.

## Tech stack

- .NET 10, ASP.NET Core minimal API, Clean Architecture
  (`Domain`, `Application`, `Infrastructure`, `API`)
- EF Core with SQL Server
- Azure Blob Storage for images
- Microsoft Entra External ID for authentication (JWT bearer)
- OpenTelemetry to Azure Application Insights
- Terraform and GitHub Actions on Azure App Service (Linux container)

## Project structure

| Path | Content |
|------|---------|
| `src/Coffee.Journal.API` | Endpoints, authentication, exception handlers, telemetry |
| `src/Coffee.Journal.Application` | Use cases, request/response models, validation |
| `src/Coffee.Journal.Domain` | Entities and repository contracts |
| `src/Coffee.Journal.Infrastructure` | EF Core, migrations, repositories, blob storage, health checks |
| `tests/` | Unit tests (xUnit) |
| `infra/` | Terraform for Azure |

## Run locally

Requirements: .NET 10 SDK, SQL Server LocalDB, and
[Azurite](https://learn.microsoft.com/azure/storage/common/storage-use-azurite)
(blob storage emulator).

1. Start Azurite.
2. Run the API:
   ```bash
   dotnet run --project src/Coffee.Journal.API --launch-profile https
   ```
3. Open https://localhost:7050/scalar for the API reference.

Migrations are applied automatically at startup. The development settings
(`appsettings.Development.json`) use LocalDB, Azurite and allow
`http://localhost:4200` as the web client.

## Configuration

| Setting | Purpose |
|---------|---------|
| `ConnectionStrings:CoffeeJournal` | SQL Server connection string |
| `BlobStorage:ConnectionString`, `BlobStorage:ContainerName` | Image storage |
| `AllowedWebClient:0`, `:1`, ... | Allowed CORS origins |
| `Authentication:*` | Entra authority, issuer, audience, required scope |
| `APPLICATIONINSIGHTS_CONNECTION_STRING` | Turns on telemetry (off when empty) |

## Authentication and roles

- Reads on the catalog are public. Writes need a valid token with the
  `access_as_user` scope.
- Writes to roasters, coffees and coffee shops also need the `Contributor`
  or `Admin` role. Roles are stored in the database and checked on each request.
- The first `Admin` is created by inserting a row in `ProfileRoles`.

## Health checks

- `GET /health/live`: the process is up.
- `GET /health/ready`: the app can reach the database.

## Tests

```bash
dotnet test
```

## Deployment

- `CI-CD-Pipeline.yml`: build, test, push the image to Docker Hub, deploy to
  Azure App Service, then call `/health/ready` as a smoke test.
- `provisioning.yml`: Terraform plan and apply for `infra/`.
  Needs the repository variables `ALLOWED_WEB_CLIENT_0` and `ALLOWED_WEB_CLIENT_1`.
