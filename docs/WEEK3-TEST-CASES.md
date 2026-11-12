# Week 3 verification (practice test cases)

Use these to confirm **DB**, **ML**, and **BE** work alone and together. See also [API-AND-DB-ROUTES.md](./API-AND-DB-ROUTES.md).

## Preconditions

```powershell
docker compose up -d db ml-service backend
```

| Service | Check |
|---------|--------|
| ML | http://localhost:8000/health |
| BE | http://localhost:5000/swagger |
| DB | SSMS → `localhost,1433` → `IncidentDetection` |

---

## Database

| ID | Test | Expected |
|----|------|----------|
| DB-1 | Five tables exist | `Users`, `CameraProfiles`, `Videos`, `ProcessingJobs`, `Violations` |
| DB-2 | Seed profile | Row in `CameraProfiles` |
| DB-3 | Admin user after API start | Row in `Users` for `admin@smartcity.local` |

---

## ML (`/docs`)

| ID | Test | Expected |
|----|------|----------|
| ML-1 | GET `/health` | `status: ok`, model, device |
| ML-2 | POST `/analyze/video` with short `.mp4` | 200, `frames_processed`, `violations` array |

---

## Backend (`/swagger`)

| ID | Test | Expected |
|----|------|----------|
| BE-1 | POST `/api/auth/login` | JWT token |
| BE-2 | GET `/api/violations` without token | 401 |
| BE-3 | GET `/api/camera-profiles` with token | List |

---

## End-to-end (integration)

| ID | Test | Expected |
|----|------|----------|
| E2E-1 | Upload via `/api/videos/upload` | `videoId`, `processingJobId`, Pending |
| E2E-2 | Poll `/api/processing-jobs/{id}` | Completed (or Failed + message) |
| E2E-3 | GET `/api/violations?videoId=` | Matches `Violations` table in SSMS |
| E2E-4 | GET `/api/snapshots/{fileName}` | Image when path exists |

**Practice outcome:** You can demo upload → SQL rows → API list without calling ML Swagger for the main story.
