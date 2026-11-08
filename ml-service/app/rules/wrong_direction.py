import math

from app.config import settings
from app.rules.base import BaseRule, RuleContext, TrackObservation
from app.schemas import BboxDto, CameraProfileDto, ViolationDto


def _deg_to_unit_vector(deg: float) -> tuple[float, float]:
    rad = math.radians(deg)
    return math.cos(rad), math.sin(rad)


class WrongDirectionRule(BaseRule):
    def evaluate(
        self,
        observation: TrackObservation,
        profile: CameraProfileDto,
        ctx: RuleContext,
    ) -> ViolationDto | None:
        if observation.class_id not in settings.vehicle_class_ids:
            return None

        last = ctx.track_last_center.get(observation.track_id)
        ctx.track_last_center[observation.track_id] = observation.center
        if last is None:
            return None

        dx = observation.center[0] - last[0]
        dy = observation.center[1] - last[1]
        speed = math.hypot(dx, dy)
        if speed < settings.wrong_direction_min_speed_px:
            return None

        move_angle = math.degrees(math.atan2(dy, dx))
        allowed = profile.allowed_direction_deg
        opposite = (allowed + 180.0) % 360.0
        diff = abs((move_angle - opposite + 180.0) % 360.0 - 180.0)
        if diff > settings.wrong_direction_angle_tolerance_deg:
            return None

        key = (observation.track_id, "WrongDirection")
        if key in ctx.emitted:
            return None
        ctx.emitted.add(key)

        x1, y1, x2, y2 = observation.bbox_xyxy
        return ViolationDto(
            violation_type="WrongDirection",
            confidence=float(observation.confidence),
            frame_index=observation.frame_index,
            frame_time_seconds=observation.frame_time_seconds,
            track_id=observation.track_id,
            object_class=observation.class_name,
            bbox=BboxDto(x1=float(x1), y1=float(y1), x2=float(x2), y2=float(y2)),
            detail={
                "move_angle_deg": round(move_angle, 2),
                "expected_opposite_deg": round(opposite, 2),
                "angle_diff_deg": round(diff, 2),
            },
        )
