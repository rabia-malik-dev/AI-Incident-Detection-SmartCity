# Chapter 7: Docker & DevOps

## 7.1 Goals

- **One command** local stack: `docker compose up`
- **Microservices:** `frontend`, `backend`, `ml-service`, `database` as separate containers
- **You own integration:** .NET orchestrates video storage and ML calls; Angular talks only to .NET

## 7.2 Container map

| Service | Image base | Port (host) | Role |
| ------- | ---------- | ----------- | ---- |
| `db` | `mcr.microsoft.com/mssql/server:2022-latest` | 1433 | SQL Server |
| `backend` | `mcr.microsoft.com/dotnet/aspnet:8.0` | 5000 / 5001 | ASP.NET Core Web API |
| `ml-service` | `python:3.11-slim` (+ optional CUDA image) | 8000 | FastAPI + YOLOv8 + rules |
| `frontend` | `node:20` → nginx for prod | 4200 | Angular dev server or static build |

Optional: `redis` (job queue, phase 2), `azurite` (blob storage for videos).

## 7.3 Volumes

| Volume | Use |
| ------ | --- |
| `sqldata` | SQL Server data files |
| `uploads` | Raw uploaded videos (shared backend ↔ ML) |
| `snapshots` | Violation frame images |
| `ml-models` | YOLO weights (avoid re-download in container) |

## 7.4 Networking

All services on compose network `smartcity-net`:

- Frontend → `http://backend:8080` (or host-mapped port in dev)
- Backend → `http://ml-service:8000`
- Backend → `Server=db;Database=IncidentDetection;...`

## 7.5 Environment variables (examples)

**Backend**

- `ConnectionStrings__Default`
- `MlService__BaseUrl=http://ml-service:8000`
- `Jwt__Issuer`, `Jwt__Key` (dev secrets via `.env`, not committed)

**ML**

- `YOLO_WEIGHTS=/models/yolov8s.pt`
- `DEVICE=cpu` or `cuda`

**Database**

- `ACCEPT_EULA=Y`
- `MSSQL_SA_PASSWORD` (from `.env`)

## 7.6 Dockerfiles (per repo folder)

```text
backend/Dockerfile          → multi-stage: sdk build, aspnet run
frontend/Dockerfile         → npm ci, ng build, nginx
ml-service/Dockerfile       → pip install, ultralytics, uvicorn
database/Dockerfile         → optional; usually only schema + use official SQL image
```

Root `docker-compose.yml` wires build contexts to those paths.

## 7.7 CI/CD (future)

- GitHub Actions: build three images, run unit tests, optional ML smoke test on one clip
- Push to Azure Container Registry or Docker Hub
- Deploy to Azure Container Apps / AKS (stretch)

## 7.8 Local dev without full Docker

- SQL Server in Docker only **or** LocalDB on Windows
- Run `ml-service` and `backend` with `dotnet run` / `uvicorn` while iterating
- Use full compose for demos and “works on my machine” proof for practice walkthroughs and portfolio reviewers
