# 4.1 Functional Requirements

These are the core capabilities your system must perform. The system is divided into four main functional modules:

### 4.1.1 User Authentication

| Requirement ID  | FR-1                                                                                                   |
| --------------- | ------------------------------------------------------------------------------------------------------ |
| **Description** | The system shall allow users (admins or traffic officers) to securely log in using email and password. |
| **Actors**      | Admin, Traffic Officer                                                                                 |
| **Inputs**      | Email, Password                                                                                        |
| **Outputs**     | Auth Token / Access Dashboard                                                                          |
| **Priority**    | High                                                                                                   |

### 4.1.2 Video Upload
| Requirement ID  | FR-2                                                                             |
| --------------- | -------------------------------------------------------------------------------- |
| **Description** | The system shall allow authenticated users to upload `.mp4` videos for analysis. |
| **Actors**      | Admin                                                                            |
| **Inputs**      | Video file                                                                       |
| **Outputs**     | Upload confirmation                                                              |
| **Priority**    | High                                                                             |

### 4.1.3 Frame Extraction and Analysis
| Requirement ID  | FR-3                                                                                                 |
| --------------- | ---------------------------------------------------------------------------------------------------- |
| **Description** | The backend shall extract frames at 1-second intervals and send them to the ML service for analysis. |
| **Actors**      | System                                                                                               |
| **Inputs**      | Uploaded video                                                                                       |
| **Outputs**     | List of violation results                                                                            |
| **Priority**    | High                                                                                                 |

### 4.1.4 Violation Detection via YOLOv8
| Requirement ID  | FR-4                                                                                                       |
| --------------- | ---------------------------------------------------------------------------------------------------------- |
| **Description** | The FastAPI ML microservice shall return objects violating traffic rules (e.g., wrong-direction vehicles). |
| **Actors**      | ML Service                                                                                                 |
| **Inputs**      | Image frames                                                                                               |
| **Outputs**     | JSON with detected classes, bounding boxes, and confidence                                                 |
| **Priority**    | High                                                                                                       |

### 4.1.5 Store Violations in Database
| Requirement ID  | FR-5                                                                                                  |
| --------------- | ----------------------------------------------------------------------------------------------------- |
| **Description** | The backend shall store violation results including time, video ID, and image snapshot in SQL Server. |
| **Actors**      | Backend                                                                                               |
| **Inputs**      | ML detection results                                                                                  |
| **Outputs**     | Database entry                                                                                        |
| **Priority**    | High                                                                                                  |

### 4.1.6 View Dashboard & Reports
| Requirement ID  | FR-6                                                                                                       |
| --------------- | ---------------------------------------------------------------------------------------------------------- |
| **Description** | Users shall view all detected violations via a frontend dashboard, filterable by date, type, and location. |
| **Actors**      | Admin, Traffic Officer                                                                                     |
| **Inputs**      | None                                                                                                       |
| **Outputs**     | Visual table/chart                                                                                         |
| **Priority**    | Medium–High                                                                                                |

### 4.1.7 Export Violations Report
| Requirement ID  | FR-7                                                              |
| --------------- | ----------------------------------------------------------------- |
| **Description** | The system shall allow exporting violation reports as PDF or CSV. |
| **Actors**      | Admin                                                             |
| **Inputs**      | Date range                                                        |
| **Outputs**     | Downloaded report                                                 |
| **Priority**    | Medium                                                            |

# 4.2 Non-Functional Requirements
These describe how the system performs rather than what it does.

### 4.2.1 Performance Requirements
| NFR-ID                                                                     | NFR-1 |
| -------------------------------------------------------------------------- | ----- |
| The system must process a 1-minute video in under 30 seconds.              |       |
| The dashboard must load violations within 3 seconds for up to 500 entries. |       |

### 4.2.2 Scalability
| NFR-ID                                                                       | NFR-2 |
| ---------------------------------------------------------------------------- | ----- |
| The system must support scaling to real-time CCTV stream ingestion via RTSP. |       |
| The ML service must be deployable independently via Docker or FastAPI.       |       |

### 4.2.3 Availability & Reliability
| NFR-ID                                                          | NFR-3 |
| --------------------------------------------------------------- | ----- |
| System should maintain 99.5% uptime in production environments. |       |
| Failures in the ML service should not crash the entire backend. |       |

### 4.2.4 Security
| NFR-ID                                                          | NFR-4 |
| --------------------------------------------------------------- | ----- |
| Passwords must be stored using salted hashing.                  |       |
| All API routes must be protected by token-based authentication. |       |

### 4.2.5 Usability
| NFR-ID                                                                 | NFR-5 |
| ---------------------------------------------------------------------- | ----- |
| System UI must be responsive and accessible via web & mobile browsers. |       |
| Users must require < 10 minutes of training to use the system.         |       |

### 4.2.6 Maintainability
| NFR-ID                                                          | NFR-6 |
| --------------------------------------------------------------- | ----- |
| Code should follow standard modular architecture (MVC, layered) |       |
| Components must be documented with comments and README files    |       |

### 4.2.7 Portability
| NFR-ID                                                               | NFR-7 |
| -------------------------------------------------------------------- | ----- |
| The system should be deployable on local or cloud (Azure/AWS/GCP).   |       |
| Backend and ML microservices should support Docker containerization. |       |
