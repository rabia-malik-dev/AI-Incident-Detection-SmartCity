# 3.1 High-Level System Description
The AI-Based Traffic Violation Detection System is a modular, full-stack application designed to detect wrong-direction driving 
violations using AI. The system provides two modes:

a) Mode 1: Manual Video Upload (MVP – currently implemented)
Users upload traffic surveillance videos via the web dashboard. The backend extracts frames and passes them to an AI model for 
violation detection.

b) Mode 2: Live CCTV Stream Processing (Scalable extension)
The system can connect to live CCTV cameras (via RTSP), process frames in real time, detect violations, and report them live.

The system follows a microservices-style architecture with clear separation of concerns across the frontend (Angular), 
backend (.NET Core), AI microservice (FastAPI + YOLOv8), and data storage (SQL Server).

# 3.2 Operational Modes

| Mode                    | Description                                                               | User Interaction                                           |
| ----------------------- | ------------------------------------------------------------------------- | ---------------------------------------------------------- |
| **1. Manual Upload**    | Users upload video files through a web interface                          | Admin or Traffic Officer selects and submits a `.mp4` file |
| **2. CCTV RTSP Stream** | System connects to a live camera stream for real-time violation detection | User configures RTSP link (future enhancement)             |

Mode 1 is currently active; Mode 2 is planned for future expansion.

# 3.3 System Users and Roles
| User Role               | Responsibilities                                                          |
| ----------------------- | ------------------------------------------------------------------------- |
| **Admin**               | - Upload videos<br>- View detected violations<br>- Export reports         |
| **Traffic Officer**     | - View dashboard<br>- Monitor live alerts (future)                        |
| **System (Backend/ML)** | - Handle frame extraction<br>- Detect violations<br>- Store results in DB |

# 3.4 System Components
The system is composed of the following major components:

### 1. Frontend (Angular)

1.1 Uploads video files to the backend

1.2 Displays detected violations with images, timestamps, and location (if available)

1.3 Allows report export (CSV/PDF)

1.4 Responsive UI for desktop and mobile

### 2. Backend (.NET Core Web API)

2.1 Receives video uploads from the frontend

2.2 Extracts video frames at intervals

2.3 Sends frames to the AI model via REST API

2.4 Stores and retrieves violation data from SQL Server

### 3. ML Microservice (FastAPI + YOLOv8)

3.1 Receives image frames via API calls

3.2 Runs YOLOv8 object detection

3.3 Returns prediction results (bounding boxes, class labels, confidence scores)

3.4 Identifies wrong-direction vehicles based on angle or motion

### 4. Database (SQL Server)

4.1 Stores user credentials, incident records, video metadata

4.2 Enables reporting, filtering, and data export

# 3.5 Technology Stack Overview
| Layer       | Tech Used                                  |
| ----------- | ------------------------------------------ |
| Frontend    | Angular (v15+), HTML, SCSS, TypeScript     |
| Backend     | .NET Core 6+, C# Web API                   |
| AI/ML Layer | Python, FastAPI, OpenCV, YOLOv8            |
| Database    | SQL Server 2019+                           |
| Deployment  | GitHub, Azure (planned)                    |

# 3.6 Data Flow Summary

1) User uploads video via dashboard

2) Backend extracts video frames (e.g., every 1 second)

3) Frames sent to YOLOv8 model through REST API

4) Detected violations returned to backend

5) Backend stores results in SQL Server

6) Frontend fetches and displays results in the dashboard

# 3.7 Example Use Case Flow
Use Case: Detect Violation from Uploaded Video

Actors: Admin, System

Flow:

a) Admin logs in to dashboard

b) Uploads .mp4 traffic video

c) Backend extracts frames

d) ML service analyzes each frame

e) Detected violations are stored

f) Admin views list of incidents, filters by type/date, and exports report

# 3.8 Why This Architecture Is Scalable

a) Modular: Frontend, backend, ML, and DB are decoupled

b) Cloud-ready: Can be containerized and deployed to Azure, AWS, GCP

c) Expandable: New features like speed detection or license plate recognition can be added without breaking existing modules

d) Real-time support: Easily integrates RTSP streams for live CCTV feeds
