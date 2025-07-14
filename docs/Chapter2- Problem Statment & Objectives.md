# Chapter 2: Problem Statement & Objectives

## 2.1 Real-World Problem

In rapidly urbanizing cities, traffic violations—especially wrong-direction driving—have become a frequent and dangerous issue. These 
violations lead to:

a) Increased accident rates

b) Traffic congestion and roadblocks

c) Decreased effectiveness of traffic law enforcement

d) Over-reliance on manual monitoring of thousands of hours of CCTV footage

Despite the availability of high-resolution cameras and growing surveillance coverage, most cities still depend on manual observation, 
which is inefficient, error-prone, and resource-heavy.

## 2.2 Limitations of Current Solutions

| Traditional Method                  | Limitations                                           |
|------------------------------------|-------------------------------------------------------|
| Manual monitoring of CCTV footage   | Time-consuming, requires continuous human attention   |
| Physical checkpoints and patrolling | Limited coverage, low detection rate                  |
| Speed or direction sensors          | Costly to install and maintain at large scale         |
| Existing AI tools (limited)         | Not integrated into full-stack systems, hard to scale |



There is a clear need for an automated, scalable, and AI-powered solution that can work seamlessly with existing video infrastructure 
and produce actionable insights in real-time.

## 2.3 Project Statement

The aim of this project is to build an AI-powered system that detects wrong-direction driving violations from uploaded videos or live CCTV 
streams, logs incidents into a database, and visualizes them through an interactive web dashboard. The system uses YOLOv8 
(You Only Look Once – version 8) for real-time object detection, combined with a .NET Core backend, Angular frontend, 
and FastAPI ML microservice.

## 2.4 Key Goals and Objectives

| #   | Objective               | Description                                                                                           |
| --- | ----------------------- | ----------------------------------------------------------------------------------------------------- |
| 1️⃣ | Automate detection      | Remove need for manual footage review using ML                                                        |
| 2️⃣ | Scalable architecture   | Support both offline (upload) and real-time (CCTV) modes                                              |
| 3️⃣ | Full-stack reporting    | Store, retrieve, and display incidents in a dashboard                                                 |
| 4️⃣ | Modular and upgradable  | Each layer (FE/BE/ML) works independently for easy upgrades                                           |
| 5️⃣ | Recruiter-grade project | Design and document the project to match real-world enterprise systems (e.g., smart city deployments) |


## 2.5 System Benefits

| Stakeholder            | Benefit                                                           |
| ---------------------- | ----------------------------------------------------------------- |
| 👮 Traffic Authorities | Reduce manpower needed for surveillance, act faster on violations |
| 🏙️ City Governments   | Smart city alignment, safer roads                                 |
| 💼 Recruiters          | Demonstrates AI + architecture + software engineering capability  |
| 👩‍💻 Developers       | Modular codebase with real-world tech stack                       |

## 2.6 Success Criteria

The system is considered successful if it meets the following measurable outcomes:

a) Detects wrong-direction driving from uploaded videos with >90% accuracy

b) Saves and displays incident details in a user-friendly dashboard

c) Can process at least 1-minute video in < 30 seconds

d) Scalable to CCTV integration via RTSP stream

e) Deployable to the cloud with minimal configuration
