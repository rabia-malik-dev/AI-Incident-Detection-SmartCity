# Chapter 6: Technology Stack

## Frontend (`frontend/`)

| Technology | Purpose |
| ---------- | ------- |
| **Angular 17+** | SPA, routing, services, guards |
| **TypeScript** | Type-safe UI |
| **Bootstrap** | Layout / responsive |
| **Chart.js** | Violation stats by type |

**Not used:** Blazor (removed from repo).

## Backend (`backend/`)

| Technology | Purpose |
| ---------- | ------- |
| **ASP.NET Core 8** | Web API |
| **EF Core 8** | SQL Server ORM |
| **JWT Bearer** | Auth |
| **FFmpeg** (CLI) | Frame extraction in container |

## Machine Learning (`ml-service/`)

| Technology | Purpose |
| ---------- | ------- |
| **Python 3.11** | ML runtime |
| **FastAPI** | HTTP API |
| **Ultralytics YOLOv8** | Object detection |
| **ByteTrack** | Multi-object tracking via `model.track()` |
| **OpenCV** | Video I/O, drawing, snapshots |
| **Custom RuleEngine** | Violation logic (direction, helmet, speed, accident) |

**Alternative tracker:** BoT-SORT (same Ultralytics API).

## Database (`database/`)

| Technology | Purpose |
| ---------- | ------- |
| **SQL Server 2022** | Relational store (Docker image) |
| **schema.sql** | Tables: Users, Videos, Violations, CameraProfiles, ProcessingJobs |

## DevOps

| Technology | Purpose |
| ---------- | ------- |
| **Docker & Compose** | Local microservices |
| **Swagger** | Backend API docs |
| **Postman / curl** | ML API testing during development |
| **GitHub** | Source control |
| **GitHub Actions** | Build images (planned) |

## Integration boundaries

| Caller | Callee |
| ------ | ------ |
| Browser | `backend` only |
| `backend` | `db`, `ml-service`, file volumes |
| `ml-service` | weights volume, `uploads` read |
