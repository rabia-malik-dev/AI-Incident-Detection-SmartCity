# Chapter 9: Violation Types & Implementation Phases

This project includes **all** violation types below—not only wrong-direction. Work is delivered in **phases** so each phase is demo-ready.

## 9.1 Violation catalog

| Code | Display name | Phase | Primary technique |
| ---- | -------------- | ----- | ----------------- |
| `WrongDirection` | Wrong-direction driving | **1** | YOLO + ByteTrack + direction rule |
| `HelmetMissing` | Helmet missing (two-wheeler) | **2** | YOLO classes / fine-tuned helmet + rider association |
| `Overspeeding` | Over-speeding | **3** | Tracking + calibration + speed limit in camera profile |
| `PossibleAccident` | Possible accident | **4** | Heuristics (sudden stop, collision proximity); optional model later |
| `RedLight` (optional) | Red-light run | **5** | ROI + signal state (manual phase) or future signal detection |

Dashboard filters and exports support **all types** once the phase is complete.

## 9.2 Phase overview

| Phase | Deliverable | Docker |
| ----- | ----------- | ------ |
| **0** | Docs on `main`, folder skeleton, `docker compose` with DB + healthchecks | `db` only |
| **1** | Wrong-direction E2E: upload → ML → DB → Angular list | `db` + `ml-service` + `backend` + `frontend` |
| **2** | Helmet violations + UI badges | same stack |
| **3** | Speed violations + camera calibration UI (admin) | same stack |
| **4** | Accident heuristics + alert severity | same stack |
| **5** | RTSP live ingest (optional), Redis queue | extended compose |

## 9.3 Shared data model (`Violation.Type`)

Use string/enum aligned across SQL, .NET, and Angular:

`WrongDirection` | `HelmetMissing` | `Overspeeding` | `PossibleAccident`

Do **not** use unrelated examples (e.g. only “Helmet Missing” in seed data) unless Phase 2 is active.

## 9.4 Non-goals (per phase)

- Phase 1 does not require GPU or custom training.
- Phase 3 speed limits require at least one calibrated camera profile (documented in admin UI).
- Phase 4 accident detection is **assistive** (heuristic), not legal evidence—state this in UI disclaimer.
