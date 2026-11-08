# Chapter 4: System Requirements

## 4.1 Functional Requirements

### 4.1.1 User Authentication (FR-1)

| Field | Detail |
| ----- | ------ |
| **Description** | Email/password login; JWT for API calls |
| **Actors** | Admin, Traffic Officer |
| **Priority** | High |

### 4.1.2 Video Upload (FR-2)

| Field | Detail |
| ----- | ------ |
| **Description** | Authenticated upload of `.mp4` (max size configurable); optional camera profile id |
| **Actors** | Admin |
| **Priority** | High |

### 4.1.3 Processing Jobs (FR-3)

| Field | Detail |
| ----- | ------ |
| **Description** | Backend creates `ProcessingJob`; extracts frames (FFmpeg); invokes ML; handles ML timeout/failure without crashing host |
| **Actors** | System |
| **Priority** | High |

### 4.1.4 ML Analysis — Detect, Track, Rules (FR-4)

| Field | Detail |
| ----- | ------ |
| **Description** | `ml-service` runs **YOLOv8** + **ByteTrack** (`model.track`) and **RuleEngine** for enabled violation types |
| **Outputs** | JSON: `violationType`, `confidence`, `trackId`, `frameIndex`, `bbox`, `snapshotRef` |
| **Priority** | High |

See [Chapter 8](./Chapter8-ML-Detection-Pipeline.md).

### 4.1.5 Violation Types (FR-5)

The system shall support and persist these types (phased delivery):

| Type | Phase |
| ---- | ----- |
| `WrongDirection` | 1 |
| `HelmetMissing` | 2 |
| `Overspeeding` | 3 |
| `PossibleAccident` | 4 |

### 4.1.6 Persist Violations (FR-6)

Store violations linked to `VideoId`, with timestamp, type, confidence, snapshot path, optional `trackId`.

### 4.1.7 Dashboard & Filters (FR-7)

Angular UI: list, filter by type/date/video, thumbnail evidence, severity for accidents.

### 4.1.8 Export (FR-8)

CSV export (PDF optional); filtered by date range and violation types.

### 4.1.9 Camera Profiles (FR-9)

Admin CRUD for direction, ROI, speed limit, calibration (required for speed/direction rules)—phase 3 integration minimum for speed.

### 4.1.10 Docker Deployment (FR-10)

`docker compose up` shall start `db`, `backend`, `ml-service`, `frontend` with documented ports and `.env.example`.

---

## 4.2 Non-Functional Requirements

### 4.2.1 Performance (NFR-1)

- 1-minute video processed in **&lt; 30 seconds** on reference hardware (documented in README).
- Dashboard loads **≤ 500** violations in **&lt; 3 seconds**.

### 4.2.2 Scalability (NFR-2)

- RTSP extension via separate worker (design only until phase 5).
- ML service **containerized**; optional GPU image.

### 4.2.3 Availability (NFR-3)

- ML failure returns job `Failed` with message; API remains available.
- Target 99.5% uptime in production (stretch).

### 4.2.4 Security (NFR-4)

- Password hashing (ASP.NET Identity or equivalent).
- JWT on all mutating/PII routes; ML service internal network only in compose.

### 4.2.5 Usability (NFR-5)

- Responsive Angular UI; minimal training for upload + review flow.

### 4.2.6 Maintainability (NFR-6)

- One folder per microservice; SRS in `docs/`; README per service.

### 4.2.7 Portability (NFR-7)

- Docker Compose local; cloud deploy (Azure Container Apps) documented as future.

### 4.2.8 ML Quality (NFR-8)

- Per violation type: report **precision, recall, F1** on **N ≥ 20** labeled clips in `ml-service/eval/` before claiming production readiness for that type.
