# Chapter 2: Problem Statement & Objectives

## 2.1 Real-World Problem

Traffic violations—including **wrong-direction driving**, **helmet non-compliance**, **speeding**, and **accidents**—increase risk and congestion. Cities generate huge volumes of video with limited staff to review it.

## 2.2 Limitations of Current Solutions

| Traditional Method | Limitations |
| ------------------ | ----------- |
| Manual CCTV review | Slow, inconsistent, expensive |
| Patrols / sensors | Limited coverage; high capex |
| Standalone AI demos | No auth, storage, dashboard, or deployment story |

## 2.3 Project Statement

Build an **AI-powered, microservice** platform that ingests video, runs **object detection + multi-object tracking + rule-based violations**, stores evidence, and presents results in an **Angular** dashboard. Integration and **Docker** are first-class so the project mirrors production engineering—not a notebook-only demo.

## 2.4 Key Goals and Objectives

| # | Objective | Description |
| - | --------- | ----------- |
| 1 | Automate detection | ML pipeline for all target violation types (phased) |
| 2 | Scalable architecture | Separate `frontend`, `backend`, `ml-service`, `database` containers |
| 3 | Full-stack reporting | Auth, uploads, dashboard, export |
| 4 | Modular ML | Rule plugins; shared detect+track core |
| 5 | Portfolio / hiring signal | Documented SRS, metrics on labeled clips, demo via Docker Compose |

## 2.5 System Benefits

| Stakeholder | Benefit |
| ----------- | ------- |
| Traffic authorities | Faster review, structured evidence |
| City governments | Smart-city alignment |
| You (candidate) | Shows .NET + Angular + ML integration + DevOps |
| Developers | Clear folders and roadmap |

## 2.6 Success Criteria

| ID | Criterion |
| -- | --------- |
| SC-1 | Each violation type (when its phase ships) meets documented **precision/recall/F1** on **N ≥ 20** labeled clips in `ml-service/eval/` |
| SC-2 | Incidents visible in dashboard with snapshot, type, confidence, timestamp |
| SC-3 | **1 minute of video processed in &lt; 30 seconds** on documented reference hardware (CPU/GPU noted) |
| SC-4 | `docker compose up` runs DB + API + ML + UI for demo |
| SC-5 | Design supports RTSP extension without rewriting core services |

**Deprecated:** single global “&gt;90% accuracy” without dataset definition.
