# 🚦 AI Incident Detection for Smart Cities

An AI-powered, full-stack system that detects traffic violations such as wrong-direction driving or over-speeding from live or uploaded camera feeds, using YOLOv8 (Computer Vision), .NET Core, Angular, and SQL Server.

---

## 📌 Table of Contents
- [Tech Stack](#-tech-stack)
- [Features](#-features)
- [System Architecture](#-system-architecture)
- [Project Structure](#-project-structure)
- [Setup & Installation](#-setup--installation)
- [API Endpoints](#-api-endpoints)
- [Demo Screenshots](#-demo-screenshots)
- [Future Improvements](#-future-improvements)
- [License](#-license)

---

## 🧰 Tech Stack

| Layer      | Technology                            |
|------------|----------------------------------------|
| Frontend   | Angular                                |
| Backend    | ASP.NET Core Web API                   |
| AI/ML      | Python, YOLOv8, FastAPI                |
| Database   | SQL Server                             |
| Deployment | Azure, Firebase (or Netlify)           |
| Tools      | Git, VS Code, Postman, Draw.io         |

---

## ✨ Features

- 🎥 Upload traffic videos or use live camera feeds
- 🚨 Detect traffic violations:
  - Wrong-direction driving
  - Over-speeding (future)
  - Accidents (optional)
- 📊 Real-time alert dashboard
- 🗂 Store and view incident reports with timestamp and video evidence
- 📍 Map-based view (future enhancement)
- 📤 Export reports (PDF or Excel)

---

## 🧱 System Architecture

```text
+-------------------+       Upload/Feed        +------------------+
|                   |  ─────────────────────▶  |                  |
|    Angular Front  |                         |   ASP.NET Core    |
|     (Dashboard)   |  ◀─────────────────────  |    Backend API   |
|                   |     Incident Data        |                  |
+-------------------+                         +------------------+
                                                   │
                                                   ▼
                                         +---------------------+
                                         | Python ML (YOLOv8)  |
                                         | FastAPI Microservice|
                                         +---------------------+
                                                   │
                                                   ▼
                                            +---------------+
                                            | SQL Server DB |
                                            +---------------+
