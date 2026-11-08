/*
  Dev-only seed data. Run AFTER schema.sql.
  Password hashing will be done by backend (ASP.NET Identity / BCrypt).
  For manual UI testing before auth exists, insert users from the API seed endpoint later.
*/

USE IncidentDetection;
GO

-- Sample camera profile for wrong-direction + speed rules (Phase 1–3)
IF NOT EXISTS (SELECT 1 FROM dbo.CameraProfiles WHERE Name = N'Default one-way lane (dev)')
BEGIN
    INSERT INTO dbo.CameraProfiles (Name, AllowedDirectionDeg, SpeedLimitKmh, MetersPerPixel, RoiPolygonJson)
    VALUES (
        N'Default one-way lane (dev)',
        90.0,
        60.0,
        0.05,
        N'[[0,0],[1920,0],[1920,1080],[0,1080]]'
    );
END
GO

PRINT N'Dev seed applied (camera profile only). Create users via backend auth when ready.';
GO
