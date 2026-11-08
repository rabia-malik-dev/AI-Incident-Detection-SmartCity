/*
  Incident Detection — SQL Server schema (source of truth for Week 1+)
  Run in SSMS connected to localhost,1433 as sa.
  Docker DB must be running: docker compose up -d db
*/

SET NOCOUNT ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'IncidentDetection')
BEGIN
    CREATE DATABASE IncidentDetection;
END
GO

USE IncidentDetection;
GO

/* -------------------------------------------------------------------------- */
/*  Users                                                                      */
/* -------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id            INT            IDENTITY(1, 1) NOT NULL,
        Email         NVARCHAR(256)  NOT NULL,
        PasswordHash  NVARCHAR(512)  NOT NULL,
        DisplayName   NVARCHAR(200)  NOT NULL,
        Role          NVARCHAR(50)   NOT NULL CONSTRAINT CK_Users_Role CHECK (Role IN (N'Admin', N'TrafficOfficer')),
        CreatedAtUtc  DATETIME2(3)   NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Users_Email UNIQUE (Email)
    );
END
GO

/* -------------------------------------------------------------------------- */
/*  Camera profiles (direction, speed, ROI — used by ML rules)                */
/* -------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.CameraProfiles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CameraProfiles
    (
        Id                    INT             IDENTITY(1, 1) NOT NULL,
        Name                  NVARCHAR(200)   NOT NULL,
        AllowedDirectionDeg   FLOAT           NULL,
        SpeedLimitKmh         FLOAT           NULL,
        MetersPerPixel        FLOAT           NULL,
        RoiPolygonJson        NVARCHAR(MAX)   NULL,
        CreatedAtUtc          DATETIME2(3)    NOT NULL CONSTRAINT DF_CameraProfiles_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_CameraProfiles PRIMARY KEY CLUSTERED (Id)
    );
END
GO

/* -------------------------------------------------------------------------- */
/*  Videos                                                                     */
/* -------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.Videos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Videos
    (
        Id               INT             IDENTITY(1, 1) NOT NULL,
        UserId           INT             NOT NULL,
        CameraProfileId  INT             NULL,
        Title            NVARCHAR(300)   NULL,
        FileName         NVARCHAR(500)   NOT NULL,
        StoragePath      NVARCHAR(1000)  NOT NULL,
        DurationSeconds  FLOAT           NULL,
        UploadedAtUtc    DATETIME2(3)    NOT NULL CONSTRAINT DF_Videos_UploadedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_Videos PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Videos_Users FOREIGN KEY (UserId) REFERENCES dbo.Users (Id),
        CONSTRAINT FK_Videos_CameraProfiles FOREIGN KEY (CameraProfileId) REFERENCES dbo.CameraProfiles (Id)
    );

    CREATE INDEX IX_Videos_UserId_UploadedAt ON dbo.Videos (UserId, UploadedAtUtc DESC);
END
GO

/* -------------------------------------------------------------------------- */
/*  Processing jobs                                                            */
/* -------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.ProcessingJobs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProcessingJobs
    (
        Id              INT             IDENTITY(1, 1) NOT NULL,
        VideoId         INT             NOT NULL,
        Status          NVARCHAR(50)    NOT NULL CONSTRAINT CK_ProcessingJobs_Status CHECK (Status IN (
            N'Pending', N'Running', N'Completed', N'Failed', N'Cancelled'
        )),
        StartedAtUtc    DATETIME2(3)    NULL,
        CompletedAtUtc  DATETIME2(3)    NULL,
        ErrorMessage    NVARCHAR(2000)  NULL,
        CreatedAtUtc    DATETIME2(3)    NOT NULL CONSTRAINT DF_ProcessingJobs_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_ProcessingJobs PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_ProcessingJobs_Videos FOREIGN KEY (VideoId) REFERENCES dbo.Videos (Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_ProcessingJobs_VideoId ON dbo.ProcessingJobs (VideoId);
    CREATE INDEX IX_ProcessingJobs_Status ON dbo.ProcessingJobs (Status);
END
GO

/* -------------------------------------------------------------------------- */
/*  Violations                                                                 */
/* -------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.Violations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Violations
    (
        Id               BIGINT          IDENTITY(1, 1) NOT NULL,
        VideoId          INT             NOT NULL,
        ProcessingJobId  INT             NULL,
        ViolationType    NVARCHAR(50)    NOT NULL CONSTRAINT CK_Violations_Type CHECK (ViolationType IN (
            N'WrongDirection', N'HelmetMissing', N'Overspeeding', N'PossibleAccident'
        )),
        Confidence       FLOAT           NOT NULL,
        DetectedAtUtc    DATETIME2(3)    NOT NULL CONSTRAINT DF_Violations_DetectedAt DEFAULT (SYSUTCDATETIME()),
        FrameIndex       INT             NULL,
        FrameTimeSeconds FLOAT           NULL,
        TrackId          INT             NULL,
        ObjectClass      NVARCHAR(100)   NULL,
        BboxJson         NVARCHAR(500)   NULL,
        FrameImagePath   NVARCHAR(1000)  NULL,
        Severity         NVARCHAR(20)    NULL CONSTRAINT CK_Violations_Severity CHECK (Severity IS NULL OR Severity IN (N'Low', N'Medium', N'High')),
        CONSTRAINT PK_Violations PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Violations_Videos FOREIGN KEY (VideoId) REFERENCES dbo.Videos (Id) ON DELETE CASCADE,
        CONSTRAINT FK_Violations_ProcessingJobs FOREIGN KEY (ProcessingJobId) REFERENCES dbo.ProcessingJobs (Id)
    );

    CREATE INDEX IX_Violations_VideoId ON dbo.Violations (VideoId);
    CREATE INDEX IX_Violations_Type_DetectedAt ON dbo.Violations (ViolationType, DetectedAtUtc DESC);
END
GO

PRINT N'Schema applied successfully to IncidentDetection.';
GO
