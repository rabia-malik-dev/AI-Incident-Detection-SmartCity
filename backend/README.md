# Backend (ASP.NET Core 8) — Week 3

Orchestration API: JWT auth, video upload, `ProcessingJobs`, ML integration, SQL persistence.

## Run with Docker (recommended)

From repo root (`.env` with `MSSQL_SA_PASSWORD` and `Jwt__Key`):

```powershell
docker compose up --build db ml-service backend
```

| URL | Purpose |
|-----|---------|
| http://localhost:5000/swagger | API docs |
| http://localhost:8000/docs | ML (internal + debug) |

## Week 3 test flow (Swagger)

1. **POST** `/api/auth/login`  
   ```json
   { "email": "admin@smartcity.local", "password": "Admin@12345" }
   ```
2. Copy `token` → **Authorize** → `Bearer {token}`
3. **GET** `/api/camera-profiles` → note `id` (e.g. `1`)
4. **POST** `/api/videos/upload` — multipart `file`, optional `cameraProfileId`, `title`
5. **GET** `/api/processing-jobs/{id}` — wait until `Completed` or `Failed`
6. **GET** `/api/violations` — rows from SQL + `snapshotUrl`

Default admin is seeded on startup (`Seed` in appsettings / env).

## Local run (without Docker backend container)

```powershell
cd backend/SmartCity.Api
# Set ConnectionStrings, Jwt, MlService BaseUrl http://localhost:8000
dotnet run
```

Requires SQL Server + ml-service reachable.

## Architecture

- `VideoIngestService` → disk + `Videos` + `ProcessingJobs` (Pending)
- `ProcessingJobWorker` → `ProcessingJobProcessor` → HTTP ML → `Violations`
- Shared Docker volumes: `uploads`, `snapshots` (with ml-service)
