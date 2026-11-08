from abc import ABC, abstractmethod
from dataclasses import dataclass

import numpy as np

from app.schemas import CameraProfileDto, ViolationDto


@dataclass
class TrackObservation:
    track_id: int
    class_id: int
    class_name: str
    confidence: float
    bbox_xyxy: np.ndarray
    frame_index: int
    frame_time_seconds: float
    center: tuple[float, float]


class RuleContext:
    """Mutable state shared across frames for a single video analyze run."""

    def __init__(self) -> None:
        self.track_last_center: dict[int, tuple[float, float]] = {}
        self.emitted: set[tuple[int, str]] = set()  # (track_id, violation_type) dedupe


class BaseRule(ABC):
    @abstractmethod
    def evaluate(
        self,
        observation: TrackObservation,
        profile: CameraProfileDto,
        ctx: RuleContext,
    ) -> ViolationDto | None:
        pass
