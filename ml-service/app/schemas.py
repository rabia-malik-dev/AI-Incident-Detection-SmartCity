from typing import Any, Literal

from pydantic import BaseModel, Field


ViolationType = Literal[
    "WrongDirection",
    "HelmetMissing",
    "Overspeeding",
    "PossibleAccident",
]


class CameraProfileDto(BaseModel):
    name: str | None = None
    allowed_direction_deg: float = Field(90.0, description="Expected travel direction in image coordinates (degrees).")
    speed_limit_kmh: float | None = None
    meters_per_pixel: float | None = None
    roi_polygon_json: str | None = None


class BboxDto(BaseModel):
    x1: float
    y1: float
    x2: float
    y2: float


class ViolationDto(BaseModel):
    violation_type: ViolationType
    confidence: float
    frame_index: int
    frame_time_seconds: float
    track_id: int | None = None
    object_class: str | None = None
    bbox: BboxDto | None = None
    frame_image_path: str | None = None
    detail: dict[str, Any] | None = None


class AnalyzeVideoResponse(BaseModel):
    frames_processed: int
    processing_time_ms: int
    violations: list[ViolationDto]
