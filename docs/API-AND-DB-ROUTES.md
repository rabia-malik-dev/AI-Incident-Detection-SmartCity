# API and database routes (Week 3)

## Network (Docker)

| Client | Target | URL |
|--------|--------|-----|
| Browser | Backend | http://localhost:5000/swagger |
| Browser | ML (debug) | http://localhost:8000/docs |
| SSMS | SQL | `localhost,1433` → database `IncidentDetection` |
| Backend container | SQL | `db,1433` |
| Backend container | ML | `http://ml-service:8000` |

## Backend API routes

| Method | Route | Auth | DB tables |
|--------|--------|------|-----------|
| POST | `/api/auth/register` | No | Users |
| POST | `/api/auth/login` | No | Users (read) |
| POST | `/api/videos/upload` | JWT | Videos, ProcessingJobs |
| GET | `/api/videos` | JWT | Videos, ProcessingJobs |
| GET | `/api/processing-jobs/{id}` | JWT | ProcessingJobs, Violations |
| GET | `/api/violations` | JWT | Violations, Videos |
| GET | `/api/camera-profiles` | JWT | CameraProfiles |
| GET | `/api/snapshots/{fileName}` | JWT | file only |

Internal: worker calls ML `POST /analyze/video` and inserts **Violations**.

## Practice flow

Login → upload → poll job → violations → optional snapshot → confirm in SSMS.
