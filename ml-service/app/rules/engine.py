from app.rules.base import BaseRule, RuleContext, TrackObservation
from app.rules.wrong_direction import WrongDirectionRule
from app.schemas import CameraProfileDto, ViolationDto


class RuleEngine:
    def __init__(self, profile: CameraProfileDto, enabled_rules: list[str] | None = None) -> None:
        enabled = set(enabled_rules or ["WrongDirection"])
        self.rules: list[BaseRule] = []
        if "WrongDirection" in enabled:
            self.rules.append(WrongDirectionRule())
        self.profile = profile

    def evaluate_frame(
        self,
        observations: list[TrackObservation],
        ctx: RuleContext,
    ) -> list[ViolationDto]:
        violations: list[ViolationDto] = []
        for obs in observations:
            for rule in self.rules:
                hit = rule.evaluate(obs, self.profile, ctx)
                if hit:
                    violations.append(hit)
        return violations
