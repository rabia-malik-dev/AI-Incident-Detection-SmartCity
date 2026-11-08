# Chapter 3: System Overview

## 3.1 High-Level Description

A modular smart-city incident platform:

- **Mode 1 (MVP):** Upload `.mp4` from Angular; backend processes; ML returns violations; SQL stores; dashboard displays.
- **Mode 2 (future):** RTSP live streams with queue workers.

**Violation types** (see Chapter 9): `WrongDirection`, `HelmetMissing`, `Overspeeding`, `PossibleAccident`.

## 3.2 Operational Modes

| Mode | Description |
| ---- | ----------- |
| Manual upload | Admin/officer uploads file |
| RTSP (future) | Configured camera URL, continuous processing |

## 3.3 Users and Roles

| Role | Responsibilities |
| ---- | ---------------- |
| Admin | Upload, camera profiles, exports, user management |
| Traffic Officer | View/filter violations, alerts |
| System | Frame extraction, ML jobs, persistence |

## 3.4 Components

### Frontend (`frontend/` — Angular)

- Auth, upload, violation grid, filters by type/date, charts, export CSV/PDF
- Camera profile admin (speed/direction calibration)—phase 3+

### Backend (`backend/` — .NET)

- JWT auth, video storage metadata, job orchestration
- FFmpeg frame extraction; HTTP calls to `ml-service`
- EF Core → SQL Server; serves snapshot URLs to Angular

### ML (`ml-service/` — FastAPI)

- **YOLOv8** detection + **ByteTrack** tracking
- **Rule engine:** direction, helmet, speed, accident heuristics
- Returns structured violation JSON (not raw tensors only)

### Database (`database/` — SQL Server)

Users, Videos, ProcessingJobs, Violations, CameraProfiles.

## 3.5 Stack Summary

| Layer | Tech |
| ----- | ---- |
| UI | Angular 17+ |
| API | ASP.NET Core 8 |
| ML | Python, FastAPI, Ultralytics, OpenCV |
| DB | SQL Server 2022 |
| Ops | Docker Compose |

## 3.6 Data Flow

1. Angular uploads video to backend  
2. Backend stores file (volume) and enqueues job  
3. Backend sends video or frames to `ml-service`  
4. ML runs track + rules → violations  
5. Backend saves rows + snapshot paths  
6. Angular loads violations API  

## 3.7 Scalability

- Services scale independently in containers  
- ML can use GPU image; CPU acceptable for portfolio  
- New violation = new rule class + eval clips, same API contract  
