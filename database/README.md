# Database

SQL Server schema and seeds for the incident detection platform.

## Files

| File | Purpose |
| ---- | ------- |
| `schema.sql` | Tables, FKs, indexes (to be added in Phase 1) |
| `seed.dev.sql` | Dev admin user only—never use in production |

## Core entities

- **Users** — auth, roles (Admin, Officer)
- **Videos** — upload metadata, storage path, duration
- **ProcessingJobs** — status, timestamps, error messages
- **Violations** — `Type` (`WrongDirection`, `HelmetMissing`, `Overspeeding`, `PossibleAccident`), confidence, frame time, snapshot path, `TrackId`
- **CameraProfiles** — ROI, allowed direction, speed limit, calibration

## Docker

Started via root `docker-compose.yml` service `db`.  
EF migrations in `backend/` should match this schema (document which is source of truth).
