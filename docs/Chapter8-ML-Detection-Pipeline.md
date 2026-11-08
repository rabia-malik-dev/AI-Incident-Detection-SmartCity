# Chapter 8: ML Detection Pipeline (Detection + Tracking + Rules)

## 8.1 Why three layers?

| Layer | Responsibility | Technology |
| ----- | -------------- | ----------- |
| **Detection** | Find objects (car, truck, bus, motorcycle, person, helmet, …) | **YOLOv8** (Ultralytics), COCO / custom weights |
| **Tracking** | Same object across frames (`track_id`, motion vector) | **ByteTrack** (default) via Ultralytics `model.track()` |
| **Rules** | Turn tracks into **violations** (wrong-way, no helmet, speed, accident heuristics) | **Python Rule Engine** in `ml-service` |

YOLO alone does **not** know “wrong direction” or “speeding.” Tracking + configurable rules do.

## 8.2 Unified stack (one service)

All detection, tracking, and rule evaluation run inside the **`ml-service`** FastAPI microservice:

```text
Video / frame batch
    → OpenCV (decode, optional resize)
    → YOLOv8.track(persist=True, tracker="bytetrack.yaml")
    → RuleEngine.evaluate(tracks, frame_meta, camera_profile)
    → JSON: violations[] with type, confidence, bbox, track_id, frame_index, snapshot_ref
```

**Default tracker:** ByteTrack (bundled with Ultralytics).  
**Optional upgrade:** BoT-SORT for crowded scenes (same API, different tracker YAML).

## 8.3 Rule engine (extensible)

| Rule module | Violation type | Logic (summary) |
| ----------- | -------------- | ---------------- |
| `WrongDirectionRule` | `WrongDirection` | Expected travel vector per **camera profile** (ROI + allowed axis); flag tracks moving opposite |
| `HelmetRule` | `HelmetMissing` | Motorcycle/bike + person overlap; no `helmet` class on rider (detector or fine-tuned head/helmet model) |
| `SpeedRule` | `Overspeeding` | Track displacement / Δt + **calibration** (meters per pixel) vs limit |
| `AccidentHeuristicRule` | `PossibleAccident` | Abrupt stop, high IoU between vehicles, density spike (heuristic until dedicated model) |

Rules are **plugins**: enabling a rule does not require changing the .NET backend contract—only `violationType` enum values.

## 8.4 Camera profile (per upload / per camera)

Stored in backend DB, passed to ML with each job:

```json
{
  "cameraId": "cam-01",
  "allowedDirectionDeg": 90,
  "roiPolygon": [[0,0],[1920,0],[1920,1080],[0,1080]],
  "speedLimitKmh": 60,
  "metersPerPixel": 0.05
}
```

## 8.5 API contract (ML service)

| Endpoint | Purpose |
| -------- | ------- |
| `POST /health` | Liveness |
| `POST /analyze/video` | Multipart video or job id; returns job accepted |
| `POST /analyze/frames` | Batch frames (backend orchestration) |
| `GET /analyze/{jobId}` | Status + violations summary |

Backend persists ML output; frontend never calls ML directly.

## 8.6 Models & weights

| Phase | Weights |
| ----- | ------- |
| Start | `yolov8n.pt` or `yolov8s.pt` (speed vs accuracy) |
| Helmet | Custom fine-tune or public helmet/Rider dataset (documented in repo when added) |
| Production | Versioned under `ml-service/models/` + env var `YOLO_WEIGHTS` |

## 8.7 Evaluation (no vague “90%”)

Accuracy is reported **per violation type** on a **fixed test set**:

- Maintain `ml-service/eval/clips/` with **N ≥ 20 labeled clips per type** (wrong-way, helmet, speed, accident scenarios).
- Report **precision, recall, F1** and **processing time per minute of video** on that set.
- Update README “Metrics” table when N or scores change.

## 8.8 Learning path (for developers new to AI)

1. Run Ultralytics CLI on a sample clip (`yolo predict`).  
2. Run `yolo track` on the same clip; watch `track_id`.  
3. Implement one rule (`WrongDirectionRule`) with a hard-coded camera profile.  
4. Expose via FastAPI; call from Postman.  
5. Wire .NET upload → ML → SQL → Angular dashboard.
