import time
import uuid
from pathlib import Path

import cv2
import numpy as np
from ultralytics import YOLO

from app.config import settings
from app.rules.base import RuleContext, TrackObservation
from app.rules.engine import RuleEngine
from app.schemas import AnalyzeVideoResponse, CameraProfileDto, ViolationDto


class VideoAnalyzer:
    def __init__(self, model: YOLO) -> None:
        self.model = model
        Path(settings.snapshots_dir).mkdir(parents=True, exist_ok=True)

    def analyze(self, video_path: Path, profile: CameraProfileDto) -> AnalyzeVideoResponse:
        start = time.perf_counter()
        cap = cv2.VideoCapture(str(video_path))
        if not cap.isOpened():
            raise ValueError(f"Cannot open video: {video_path}")

        fps = cap.get(cv2.CAP_PROP_FPS) or 25.0
        stride_frames = max(1, int(fps * settings.frame_stride_seconds))
        engine = RuleEngine(profile)
        ctx = RuleContext()
        all_violations: list[ViolationDto] = []
        frame_index = 0
        processed = 0

        while True:
            ret, frame = cap.read()
            if not ret:
                break
            if frame_index % stride_frames != 0:
                frame_index += 1
                continue

            results = self.model.track(
                frame,
                persist=True,
                tracker="bytetrack.yaml",
                device=settings.device,
                verbose=False,
                classes=list(settings.vehicle_class_ids),
            )
            observations = self._parse_tracks(results, frame_index, frame_index / fps)
            hits = engine.evaluate_frame(observations, ctx)
            for v in hits:
                v.frame_image_path = self._save_snapshot(frame, v)
            all_violations.extend(hits)
            processed += 1
            frame_index += 1

        cap.release()
        elapsed_ms = int((time.perf_counter() - start) * 1000)
        return AnalyzeVideoResponse(
            frames_processed=processed,
            processing_time_ms=elapsed_ms,
            violations=all_violations,
        )

    def _parse_tracks(self, results, frame_index: int, t_seconds: float) -> list[TrackObservation]:
        observations: list[TrackObservation] = []
        r0 = results[0]
        if r0.boxes is None or r0.boxes.id is None:
            return observations

        boxes = r0.boxes.xyxy.cpu().numpy()
        ids = r0.boxes.id.cpu().numpy().astype(int)
        classes = r0.boxes.cls.cpu().numpy().astype(int)
        confs = r0.boxes.conf.cpu().numpy()
        names = r0.names

        for box, tid, cls_id, conf in zip(boxes, ids, classes, confs, strict=True):
            x1, y1, x2, y2 = box
            cx, cy = (x1 + x2) / 2.0, (y1 + y2) / 2.0
            observations.append(
                TrackObservation(
                    track_id=int(tid),
                    class_id=int(cls_id),
                    class_name=str(names[int(cls_id)]),
                    confidence=float(conf),
                    bbox_xyxy=box,
                    frame_index=frame_index,
                    frame_time_seconds=t_seconds,
                    center=(float(cx), float(cy)),
                )
            )
        return observations

    def _save_snapshot(self, frame: np.ndarray, violation: ViolationDto) -> str:
        if violation.bbox is None:
            name = f"{uuid.uuid4().hex}.jpg"
        else:
            b = violation.bbox
            annotated = frame.copy()
            cv2.rectangle(
                annotated,
                (int(b.x1), int(b.y1)),
                (int(b.x2), int(b.y2)),
                (0, 0, 255),
                2,
            )
            cv2.putText(
                annotated,
                violation.violation_type,
                (int(b.x1), max(20, int(b.y1) - 8)),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.6,
                (0, 0, 255),
                2,
            )
            frame = annotated
            name = f"{violation.violation_type}_{violation.track_id}_{violation.frame_index}_{uuid.uuid4().hex[:8]}.jpg"

        out = Path(settings.snapshots_dir) / name
        cv2.imwrite(str(out), frame)
        return str(out)
