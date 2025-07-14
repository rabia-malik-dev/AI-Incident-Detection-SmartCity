# 1.1 Background and Motivation
> With the growing number of vehicles in urban environments, traffic violations have become a major cause of road congestion,
accidents, and fatalities. Traditional systems rely on manual CCTV monitoring, which is labor-intensive, time-consuming, and error-prone. 
There is a strong need for an intelligent, automated solution that can assist authorities in detecting violations accurately and efficiently.
The use of Artificial Intelligence (AI) in smart city applications has proven effective in enabling real-time insights and automation. 
This project utilizes AI-based video analysis to detect traffic violations—specifically wrong-direction driving—through a scalable, 
full-stack web application integrated with machine learning.

# 1.2 Purpose of the Document
> This Software Requirements Specification (SRS) document outlines the functional and non-functional requirements,
system architecture, operational flow, and interface details for the AI-Based Traffic Violation Detection System.
It serves as a reference for developers, testers, stakeholders, and recruiters to understand the technical depth and vision of the system.

# 1.3 Intended Audience
> This document is intended for the following audiences:

a) Software developers and engineers

b) Technical architects and AI/ML researchers

c) Government smart city planners

d) FANG recruiters and technical hiring teams

e) Academic reviewers or mentors

# 1.4 Project Scope
This project aims to develop a real-time, scalable solution that automates the detection of traffic violations from uploaded videos or live CCTV streams. 

It includes:
a) An Angular-based frontend dashboard

b) A .NET Core backend API for video handling and data management

c) A Python-based ML microservice using YOLOv8 for incident detection

d) A SQL Server database for storing incidents and reports

Initially, the system supports manual video uploads for proof-of-concept. It is designed to scale toward live CCTV integration, 
enabling real-time monitoring and incident detection in smart cities.

# 1.5 Document Organization
This SRS is organized into the following chapters:

a) Introduction – Overview, scope, and purpose

b) Problem Statement & Objectives – What the system solves

c) System Overview – Architecture, user roles, and features

d) System Requirements – Functional and non-functional specifications

e) System Design – Architecture diagrams and data flows

f) Technology Stack – Tools, libraries, and rationale



