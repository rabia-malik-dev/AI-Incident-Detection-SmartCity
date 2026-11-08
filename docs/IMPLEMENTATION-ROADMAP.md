# Implementation Roadmap (Docker-first)

Build order for a developer integrating all services. Each step should be **runnable in Docker** (or hybrid) before moving on.

---

## Phase 0 — Documentation & skeleton (you are here)

- [x] SRS on `main` under `docs/`
- [ ] Create empty `frontend/`, `backend/`, `ml-service/`, `database/`
- [ ] Add root `docker-compose.yml` (start with **database only**)
- [ ] Add `.env.example` (SQL password, JWT secret, ML URL)

**Verify:** `docker compose up db` → connect with SSMS / Azure Data Studio.

---

## Phase 1 — Database + schema

**Build first:** `database/`

1. `database/schema.sql` — Users, Videos, Violations, CameraProfiles, ProcessingJobs
2. Optional `database/seed.sql` — admin user (dev only)
3. Backend EF migrations can replace or mirror `schema.sql` (pick one source of truth; document in `database/README.md`)

**Docker:** `db` service + volume `sqldata`.

**Verify:** Tables exist; backend not required yet.

---

## Phase 2 — ML microservice (no .NET yet)

**Build second:** `ml-service/`

Why before backend? You can learn AI and prove detection with **Postman/curl** only.

1. `Dockerfile`, `requirements.txt` (ultralytics, fastapi, uvicorn, opencv-python-headless)
2. `app/main.py` — health + `POST /analyze/frames` or `/analyze/video`
3. `app/pipeline.py` — YOLOv8 + `model.track()` + `RuleEngine` with **WrongDirectionRule** only
4. Mount `uploads/` volume; write snapshot images to `snapshots/`

**Docker:** add `ml-service` to compose (port 8000).

**Verify:** `curl -F "file=@test.mp4" http://localhost:8000/analyze/video` returns JSON violations.

---

## Phase 3 — Backend API (orchestration)

**Build third:** `backend/`

1. ASP.NET Core 8 Web API project in `backend/`
2. EF Core → SQL Server connection string from compose (`Server=db,...`)
3. Endpoints: auth (JWT), upload video, trigger processing, list violations
4. Processing worker (hosted service or background queue): extract frames (FFmpeg), call `ml-service`, save Violations + snapshot paths
5. Swagger enabled

**Docker:** add `backend` service; `depends_on: [db, ml-service]`; mount `uploads`, `snapshots`.

**Verify:** Upload via Swagger → rows in `Violations` table; no frontend yet.

---

## Phase 4 — Angular frontend

**Build fourth:** `frontend/`

1. `ng new` in `frontend/` (routing, SCSS, Bootstrap)
2. Login, upload page, violations table, filters by `Violation.Type`, thumbnail from backend URLs
3. `environment.ts` → API base URL (docker: `http://localhost:5000`)

**Docker:** `frontend` service (dev: `ng serve --host 0.0.0.0`; prod: nginx stage).

**Verify:** Full E2E wrong-direction on one test clip.

---

## Phase 5 — Full `docker compose up`

Root compose starts: **db → ml-service → backend → frontend** (order via healthchecks).

```text
docker compose up --build
```

Document ports in root `README.md`.

---

## Phase 6 — Violation phases 2–4 (product depth)

| Order | Work |
| ----- | ---- |
| 6a | `HelmetRule` + eval clips + dashboard filter |
| 6b | Camera profile CRUD + `SpeedRule` |
| 6c | `AccidentHeuristicRule` + severity in UI |
| 6d | CSV/PDF export, Chart.js summary |

Each phase: update `ml-service/eval/` metrics table in README.

---

## Phase 7 — Production hardening

- HTTPS, secrets via Docker secrets / Azure Key Vault
- GPU image for `ml-service` (optional)
- GitHub Actions build pipelines
- RTSP ingest (separate worker container)

---

## What to build first? (short answer)

```text
1. database (Docker SQL)
2. ml-service (Docker)     ← learn AI here
3. backend (Docker)        ← integration you own
4. frontend (Docker)
5. compose everything
6. helmet → speed → accident rules
```

**Never block frontend on ML:** stub ML with mock JSON in backend until `ml-service` is ready—then swap URL.

---

## Suggested weekly milestones (part-time)

| Week | Goal |
| ---- | ---- |
| 1 | DB in Docker + schema |
| 2 | ML track + wrong-direction rule + FastAPI |
| 3 | Backend upload + call ML + save violations |
| 4 | Angular dashboard MVP |
| 5 | Docker compose demo + README demo GIF |
| 6–8 | Helmet, speed, accident phases + eval metrics |
