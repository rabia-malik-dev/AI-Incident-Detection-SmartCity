# ML Service (FastAPI)

**Week 2:** YOLOv8 + ByteTrack + `WrongDirectionRule`.

## Run with Docker (recommended)

From repo root:

```powershell
docker compose build ml-service
docker compose up ml-service
```

First start downloads **yolov8n.pt** (may take a few minutes).

| URL | Purpose |
|-----|---------|
| http://localhost:8000/health | Liveness |
| http://localhost:8000/docs | Swagger UI |

## Test analyze endpoint

PowerShell (replace path to a short traffic `.mp4`):

```powershell
curl.exe -X POST "http://localhost:8000/analyze/video" `
  -F "file=@C:\path\to\clip.mp4" `
  -F "camera_profile_json={\"allowed_direction_deg\": 90.0}"
```

**`allowed_direction_deg`:** direction of *legal* traffic in the image (0° = right, 90° = down). Wrong-way vehicles move roughly **opposite** that angle.

Response fields align with DB `Violations`: `violation_type`, `confidence`, `frame_index`, `track_id`, `frame_image_path`, etc.

Snapshots are written under Docker volume `snapshots` → `/data/snapshots` in the container.

## Local dev (optional)

```powershell
cd ml-service
python -m venv .venv
.\.venv\Scripts\activate
pip install -r requirements.txt
$env:SNAPSHOTS_DIR = ".\data\snapshots"
$env:UPLOADS_DIR = ".\data\uploads"
uvicorn app.main:app --reload --port 8000
```

## Next phases

| Phase | Rules |
|-------|--------|
| 2b | `HelmetRule` |
| 3 | `SpeedRule` + camera profile |
| 4 | `AccidentHeuristicRule` |

Eval clips: `ml-service/eval/clips/` (N ≥ 20 per type).
