# 5.1 Design Overview

The AI-based traffic violation detection system is designed using a layered, modular, and scalable architecture that integrates computer
vision with modern web application development. The architecture follows a microservices-friendly structure, separating the frontend, 
backend, AI model, and database for maintainability and scalability.

# 5.2 High-Level Architecture

        +------------------------+
        |  User (Admin/Officer) |
        +-----------+------------+
                    |
                    v
        +-----------+------------+
        |     Angular Frontend   |
        +-----------+------------+
                    |
                    v
        +-----------+------------+
        |   .NET Core Backend    |
        +-----------+------------+
              |              |
              v              v
        +-----------+--------------+
        | SQL Server DB +  FastAPI | 
        |          YOLOv8 ML Model |
        +-----------+--------------+


# 5.3 Component Breakdown

#### 5.3.1. Frontend (Angular)

a) Upload .mp4 videos

b) Display detection results (images, timestamps)

c) Filter by date/type/location

d) Export violations report as PDF/CSV

e) Calls APIs hosted by .NET backend

#### 5.3.2. Backend (.NET Core API)

a) Handles user authentication and session management

b) Accepts uploaded video and stores metadata

c) Extracts video frames (using FFmpeg/OpenCV in C# or Python bridge)

d) Sends frames to ML microservice via REST API

e) Saves violations to SQL Server

f) Sends results to frontend for rendering

#### 5.3.3. ML Microservice (FastAPI + YOLOv8)

a) Receives frame images from backend

b) Runs YOLOv8 object detection

c) Detects wrong-direction vehicles based on bounding boxes and movement direction

d) Returns detection results as JSON (object, coordinates, confidence)

e) Can be deployed independently for scalability

#### 5.3.4. Database (SQL Server)

Stores:

a) User credentials

b) Uploaded video metadata

c) Detection logs (video ID, frame timestamp, violation type)

d) Image paths for detected violations

e) Report exports log 

# 5.4 Data Flow Diagram (Simplified)

This section explains the flow of data through various components of the system — from video input to violation output on the dashboard. The system handles both user-uploaded videos and (optionally) live CCTV streams.

##### Step-by-Step Flow Description

a) Video Upload

A user (e.g., traffic officer or admin) uploads a video containing potential traffic violations via the Angular web interface.

b) Request to Backend

The video is sent to the .NET Core backend using a secure HTTP POST request.

c) Frame Extraction

The backend extracts frames from the video using a helper module (FFmpeg/OpenCV).

d) Send to ML Service

The extracted frames are sent to the FastAPI microservice running a trained YOLOv8 model.

e) Violation Detection

The ML service processes each frame to detect wrong-direction driving or other violations and returns a list of results with bounding box data, object labels, and confidence scores.

f) Save to Database

Backend stores the violation results (timestamp, frame reference, type of violation) into a SQL Server database.

g) Display on Dashboard

Frontend fetches the results and displays them in a dashboard with violation counts, thumbnails, and downloadable reports.



    +-----------------------------+
    |     User uploads video      |
    +-------------+---------------+
                  |
                  v
    +-------------+---------------+
    |           Frontend          |
    +-------------+---------------+
                  |
                  v
    +-------------+---------------+
    |         Backend API         |
    |   (Receives & extracts)     |
    +-------------+---------------+
                  |
                  v
    +-------------+---------------+
    |   Frame Extraction Module   |
    +-------------+---------------+
                  |
                  v
    +-------------+---------------+
    |   ML API (YOLOv8 Model)     |
    |     Detects Violations      |
    +-------------+---------------+
                  |
                  v
    +-------------+---------------+
    |     Store to SQL Server     |
    +-------------+---------------+
                  |
                  v
    +-----------------------------+
    | Show on Angular Dashboard   |
    +-----------------------------+

# 5.5 Technology Stack Summary
| Layer      | Technologies Used                         |
| ---------- | ----------------------------------------- |
| Frontend   | Angular, TypeScript, SCSS                 |
| Backend    | .NET Core 6+, Web API, C#                 |
| AI Service | Python, FastAPI, YOLOv8, OpenCV           |
| Database   | SQL Server                                |
| DevOps     | GitHub, Docker (optional), Azure (future) |

# 5.6 Folder Structure 

```text
AI-Incident-Detection-SmartCity/
├── frontend/             → Angular application (UI)
├── backend/              → .NET Core Web API (business logic, API)
├── ml-service/           → FastAPI + YOLOv8 model (AI detection)
├── docs/                 → Documentation folder
│   └── src/              → SRS chapters in Markdown
│       └── chapter5.md
├── database/             → Database scripts
│   └── schema.sql        → Tables, relationships, procedures
└── README.md             → Project overview and setup guide
```

# 5.7 Key Design Principles
| Principle       | Application in Project                                   |
| --------------- | -------------------------------------------------------- |
| Modularity      | Frontend, backend, and ML are decoupled                  |
| Reusability     | YOLOv8 detection logic is stateless and reusable         |
| Scalability     | Supports adding more ML endpoints or moving to live RTSP |
| Maintainability | Each component can be updated independently              |
| Security        | Token-based authentication, no public endpoints          |

# 5.8 Data Flow Diagram (DFD) – Level 1

This explains how data moves through the system.

```
           +------------------------+
           |  User (Admin/Officer) |
           +-----------+------------+
                       |
                       v
           +-----------+------------+
           |     Angular Frontend   |
           +-----------+------------+
                       |
                       v
           +-----------+------------+
           | .NET Core Backend API  |
           +-----------+------------+
                       |
          +------------+-------------+
          |  Frame Extraction Module |
          +------------+-------------+
                       |
                       v
           +-------------------------+
           |  ML API (YOLOv8 - AI)   |
           +-------------------------+
                       |
                       v
             +---------------------+
             |   SQL Server DB     |
             +---------------------+
                       |
                       v
           +-------------------------+
           | Angular Dashboard View  |
           +-------------------------+
```

# 5.9 Entity Relationship Diagram (ERD)

This represents the structure of database:

```
+------------------+          +------------------+          +----------------------+
|     Users        |          |     Uploads      |          |     Violations       |
+------------------+          +------------------+          +----------------------+
| user_id (PK)     |◄──────┐  | upload_id (PK)   |◄──────┐  | violation_id (PK)    |
| name             |       └─▶| user_id (FK)     |       └─▶| upload_id (FK)       |
| email            |          | title            |          | frame_timestamp      |
| password_hash    |          | video_id (FK)    |          | object_detected      |
| role             |          | upload_time      |          | direction            |
+------------------+          +------------------+          | confidence           |
                                                           +----------+-----------+
                                                                      |
                                                                      v
                                                  +---------------------------+
                                                  |     ML_Detection_Results  |
                                                  +---------------------------+
                                                  | result_id (PK)            |
                                                  | violation_id (FK)         |
                                                  | frame_number              |
                                                  | bbox_coordinates          |
                                                  | detection_score           |
                                                  +---------------------------+

                         +------------------+
                         |     Videos       |
                         +------------------+
                         | video_id (PK)    |
                         | filename         |
                         | storage_path     |
                         | duration         |
                         +------------------+
```

# 5.10  Component Diagram

Breakdown of system components and services:

```
               +----------------------------+
               |    Angular Frontend        |
               |----------------------------|
               | UI Components              |
               | - Upload Video             |
               | - Dashboard                |
               | - Reports                  |
               +----------------------------+
                         |
                         v
               +----------------------------+
               | .NET Core Backend API      |
               |----------------------------|
               | Controllers                |
               | - Video Upload             |
               | - Fetch Violations         |
               | Services                   |
               | - Frame Extraction         |
               | - DB Interaction           |
               +----------------------------+
                     |             |
         +-----------+             +-------------+
         v                                         v
+--------------------------+        +------------------------------+
|   SQL Server Database    |        |   FastAPI ML Microservice    |
|--------------------------|        |------------------------------|
| - Users Table            |        | YOLOv8 Detection Pipeline    |
| - Uploads Table          |        | - Load Model                 |
| - Violations Table       |        | - Detect Violations          |
| - Results Table          |        | - Return Results (JSON)      |
+--------------------------+        +------------------------------+
```







