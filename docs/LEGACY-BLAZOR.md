# Legacy Blazor spike (removed)

An early **.NET 8 Blazor WASM** solution (`AIIncidentDetection.Client` / `Server` / `Shared`) was removed from `main` in favor of the documented microservice layout.

**Current layout:**

- `frontend/` — Angular
- `backend/` — ASP.NET Core Web API
- `ml-service/` — FastAPI + YOLOv8 + ByteTrack + rules
- `database/` — SQL Server schema

Domain concepts (users, videos, violations) will be implemented as entities in `backend/` with `Violation.Type` per [Chapter 9](./Chapter9-Violation-Types-and-Phases.md).
