# Software Requirements Specification (SRS)

**AI Incident Detection for Smart Cities** — traffic violation detection from uploaded video (and future live CCTV).

| Chapter | Document |
| ------- | -------- |
| 1 | [Introduction](./Chapter1%20-%20Introduction.md) |
| 2 | [Problem Statement & Objectives](./Chapter2-%20Problem%20Statment%20%26%20Objectives.md) |
| 3 | [System Overview](./Chapter3-System%20Overview.md) |
| 4 | [System Requirements](./Chapter4-%20System%20Requirements.md) |
| 5 | [System Design](./Chapter5-System%20Design.md) |
| 6 | [Technology Stack](./Chapter6-%20Technology%20Stack.md) |
| 7 | [Docker & DevOps](./Chapter7-Docker-and-DevOps.md) |
| 7b | [CI/CD](./Chapter7-CICD.md) |
| 8 | [ML Detection Pipeline](./Chapter8-ML-Detection-Pipeline.md) |
| 9 | [Violation Types & Phases](./Chapter9-Violation-Types-and-Phases.md) |
| — | [**Implementation Roadmap**](./IMPLEMENTATION-ROADMAP.md) |

**Repository layout (target):** `frontend/` · `backend/` · `ml-service/` · `database/` · `docs/` · `docker-compose.yml`

> **Note:** A former Blazor WASM spike was removed; implementation uses `frontend/`, `backend/`, `ml-service/`, `database/` only. See [LEGACY-BLAZOR.md](./LEGACY-BLAZOR.md).
