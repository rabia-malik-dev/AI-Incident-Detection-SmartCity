# Chapter 6: Technology Stack
This chapter outlines the complete set of technologies, tools, frameworks, and libraries used to develop, deploy, and manage 
the AI-based Incident Detection System. The stack was selected to ensure scalability, modularity, performance, and ease of maintenance.

##  Frontend
| Technology    | Purpose                   | Rationale                                                                                              |
| ------------- | ------------------------- | ------------------------------------------------------------------------------------------------------ |
| **Angular**   | Frontend Web Framework    | Provides a structured SPA architecture with TypeScript support, data binding, and reusable components. |
| **Bootstrap** | UI Styling & Layout       | Ensures responsive and mobile-friendly UI design.                                                      |
| **Chart.js**  | Graphs and Visualizations | For displaying analytics in a simple and interactive way.                                              |

# Backend
| Technology                | Purpose                  | Rationale                                            |
| ------------------------- | ------------------------ | ---------------------------------------------------- |
| **ASP.NET Core Web API**  | Backend Logic & REST API | Secure, scalable, and performant .NET-based backend. |
| **Entity Framework Core** | ORM for SQL Server       | Simplifies DB access through LINQ and model binding. |

# Machine Learning Service
| Technology               | Purpose                      | Rationale                                            |
| ------------------------ | ---------------------------- | ---------------------------------------------------- |
| **Python**               | AI/ML logic                  | Widely used for ML with rich ecosystem of libraries. |
| **FastAPI**              | Lightweight ML API           | High-performance, async-ready, easy integration.     |
| **YOLOv8 (Ultralytics)** | Object detection from frames | State-of-the-art real-time detection algorithm.      |
| **OpenCV**               | Image/Video processing       | Efficient frame handling from input video.           |

#  Database Layer
| Technology     | Purpose                              | Rationale                                                               |
| -------------- | ------------------------------------ | ----------------------------------------------------------------------- |
| **SQL Server** | Data storage, logs, incident records | Reliable RDBMS with support for complex queries, views, and procedures. |

# DevOps & Utilities
| Technology                  | Purpose                         | Rationale                                         |
| --------------------------- | ------------------------------- | ------------------------------------------------- |
| **Postman**                 | API Testing                     | Helps test REST APIs during development.          |
| **Swagger**                 | API Documentation               | Auto-generated interactive API docs for backend.  |
| **Git & GitHub**            | Version Control & Collaboration | Industry-standard code hosting and tracking.      |
| **VS Code / Visual Studio** | Development Environments        | For efficient coding, debugging, and integration. |

