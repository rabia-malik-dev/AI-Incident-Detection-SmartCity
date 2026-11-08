# Database

SQL Server schema and seeds for the incident detection platform.

**Source of truth:** `schema.sql` (EF Core migrations in `backend/` should match this).

## Prerequisites

- Docker: `docker compose up -d db` from repo root
- SSMS or Azure Data Studio → `localhost,1433`, SQL login `sa`, password from `.env`

## Apply schema (Week 1)

1. Open **SSMS** → connect to `localhost,1433`
2. **File → Open → File** → select `database/schema.sql`
3. **Execute** (F5)
4. Optional: run `database/seed.dev.sql`
5. Refresh **Databases → IncidentDetection → Tables** — you should see:
   - `Users`
   - `CameraProfiles`
   - `Videos`
   - `ProcessingJobs`
   - `Violations`

## Files

| File | Purpose |
| ---- | ------- |
| `schema.sql` | Database + tables, FKs, indexes |
| `seed.dev.sql` | Dev camera profile (no users until backend auth) |

## Violation types (`Violations.ViolationType`)

`WrongDirection` · `HelmetMissing` · `Overspeeding` · `PossibleAccident`

## Docker

Service `db` in root `docker-compose.yml`; data persisted in volume `sqldata`.
