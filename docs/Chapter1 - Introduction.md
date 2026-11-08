# Chapter 1: Introduction

## 1.1 Background and Motivation

Urban traffic violations—wrong-way driving, riding without a helmet, speeding, and crashes—drive congestion, injuries, and enforcement cost. Manual CCTV review does not scale. This project delivers an **integrated, Dockerized** smart-city-style platform: **Angular** dashboard, **.NET** orchestration, **Python ML** (detect + track + rules), and **SQL Server** persistence.

## 1.2 Purpose of the Document

This SRS defines functional and non-functional requirements, architecture, ML pipeline, Docker deployment, and phased delivery for recruiters, developers, and stakeholders.

## 1.3 Intended Audience

a) Software developers and engineers  
b) Technical architects and ML engineers  
c) Smart-city / civic tech stakeholders  
d) International (e.g. UK/EU) technical hiring teams  
e) Mentors and academic reviewers  

## 1.4 Project Scope

| Component | Path | Role |
| --------- | ---- | ---- |
| Frontend | `frontend/` | Angular dashboard |
| Backend | `backend/` | ASP.NET Core API, video jobs, auth |
| ML | `ml-service/` | YOLOv8, ByteTrack, rule engine |
| Data | `database/` | SQL Server schema & seeds |
| Ops | `docker-compose.yml` | Local & demo deployment |

**Violation types (full product):** wrong-direction, helmet missing, over-speeding, possible accident (phased implementation—see Chapter 9).

**Input modes:** manual upload (phase 1); RTSP live CCTV (later).

**UI:** Angular only (`frontend/`). Legacy Blazor solution removed—see [LEGACY-BLAZOR.md](./LEGACY-BLAZOR.md).

## 1.5 Document Organization

| Chapter | Topic |
| ------- | ----- |
| 1 | Introduction |
| 2 | Problem statement & objectives |
| 3 | System overview |
| 4 | Requirements |
| 5 | Design |
| 6 | Technology stack |
| 7 | Docker & DevOps |
| 8 | ML detection pipeline |
| 9 | Violation types & phases |
| — | [Implementation roadmap](./IMPLEMENTATION-ROADMAP.md) |
