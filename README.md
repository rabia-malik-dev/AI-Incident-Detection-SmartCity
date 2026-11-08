# AI Incident Detection for Smart Cities

An AI-powered, **Dockerized microservice** system that detects traffic violations from uploaded video (and future live CCTV): **wrong-direction driving**, **helmet missing**, **over-speeding**, and **possible accidents**.

**Stack:** Angular · ASP.NET Core Web API · FastAPI · YOLOv8 + ByteTrack · SQL Server · Docker Compose

**Implementation:** all application code lives under `frontend/`, `backend/`, `ml-service/`, and `database/` (see [roadmap](./docs/IMPLEMENTATION-ROADMAP.md)).

---

## Documentation (SRS)

Full specification: **[`docs/README.md`](./docs/README.md)**

| Topic | Link |
| ----- | ---- |
| Requirements | [Chapter 4](./docs/Chapter4-%20System%20Requirements.md) |
| Design & folders | [Chapter 5](./docs/Chapter5-System%20Design.md) |
| Docker | [Chapter 7](./docs/Chapter7-Docker-and-DevOps.md) |
| ML pipeline (detect + track + rules) | [Chapter 8](./docs/Chapter8-ML-Detection-Pipeline.md) |
| All violation types & phases | [Chapter 9](./docs/Chapter9-Violation-Types-and-Phases.md) |
| **Build order** | [**IMPLEMENTATION-ROADMAP**](./docs/IMPLEMENTATION-ROADMAP.md) |

---

## Tech stack

| Layer | Technology |
| ----- | ---------- |
| Frontend | Angular 17+, TypeScript, Bootstrap, Chart.js |
| Backend | ASP.NET Core 8 Web API, EF Core, JWT |
| ML | Python 3.11, FastAPI, Ultralytics YOLOv8, **ByteTrack**, OpenCV, rule engine |
| Database | SQL Server 2022 (containerized) |
| DevOps | Docker, Docker Compose, GitHub Actions (planned) |

---

## Features (by phase)

| Phase | Capability |
| ----- | ----------- |
| 1 | Video upload, wrong-direction detection, dashboard, CSV export |
| 2 | Helmet-missing (two-wheelers) |
| 3 | Over-speeding (camera calibration) |
| 4 | Possible-accident heuristics |
| 5 | RTSP live feeds, queues (Redis) |

---

## Repository layout (target)

```text
AI-Incident-Detection-SmartCity/
├── frontend/              # Angular SPA
├── backend/               # .NET Web API
├── ml-service/            # FastAPI + YOLOv8 + tracking + rules
├── database/              # schema.sql, seeds, README
├── docs/                  # SRS chapters + roadmap
├── docker-compose.yml
├── .env.example
└── README.md
```

---

## Quick start (when implemented)

```bash
cp .env.example .env
# set MSSQL_SA_PASSWORD and JWT_KEY

docker compose up --build
```

| Service | URL (default) |
| ------- | --------------- |
| Angular | http://localhost:4200 |
| API + Swagger | http://localhost:5000/swagger |
| ML health | http://localhost:8000/health |

---

## ML pipeline (summary)

```text
Frame → YOLOv8.track (ByteTrack) → RuleEngine → violations JSON
```

Detection finds vehicles/riders; tracking links frames; **rules** decide violation type. See [Chapter 8](./docs/Chapter8-ML-Detection-Pipeline.md).

---

## Quality metrics

We do **not** claim a fixed “90% accuracy” without data. For each violation type, report **precision / recall / F1** on **N ≥ 20 labeled test clips** under `ml-service/eval/`. Update this table as you run evals:

| Type | N clips | Precision | Recall | F1 |
| ---- | ------- | --------- | ------ | -- |
| WrongDirection | TBD | TBD | TBD | TBD |
| HelmetMissing | TBD | TBD | TBD | TBD |
| Overspeeding | TBD | TBD | TBD | TBD |
| PossibleAccident | TBD | TBD | TBD | TBD |

**Performance target:** process **1 minute of video in &lt; 30 seconds** on reference hardware (document CPU/GPU in README).

---

## License

MIT (add `LICENSE` file when ready).
