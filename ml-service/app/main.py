import json
import shutil
import tempfile
from contextlib import asynccontextmanager
from pathlib import Path

from fastapi import FastAPI, File, Form, HTTPException, UploadFile
from fastapi.responses import RedirectResponse
from ultralytics import YOLO

from app.config import settings
from app.pipeline import VideoAnalyzer
from app.schemas import AnalyzeVideoResponse, CameraProfileDto


@asynccontextmanager
async def lifespan(app: FastAPI):
    Path(settings.uploads_dir).mkdir(parents=True, exist_ok=True)
    Path(settings.snapshots_dir).mkdir(parents=True, exist_ok=True)
    app.state.model = YOLO(settings.yolo_weights)
    app.state.analyzer = VideoAnalyzer(app.state.model)
    yield


app = FastAPI(
    title="Smart City ML Service",
    version="0.1.0",
    description="YOLOv8 + ByteTrack + rule engine for traffic violations.",
    lifespan=lifespan,
)


@app.get("/", include_in_schema=False)
def root() -> RedirectResponse:
    """Browser-friendly entry; interactive API is at /docs."""
    return RedirectResponse(url="/docs")


@app.get("/health")
def health() -> dict:
    return {
        "status": "ok",
        "model": settings.yolo_weights,
        "device": settings.device,
    }


@app.post("/analyze/video", response_model=AnalyzeVideoResponse)
async def analyze_video(
    file: UploadFile = File(...),
    camera_profile_json: str = Form(
        default='{"allowed_direction_deg": 90.0}',
        description="JSON matching CameraProfileDto fields",
    ),
) -> AnalyzeVideoResponse:
    if not file.filename:
        raise HTTPException(status_code=400, detail="Missing filename")

    try:
        profile = CameraProfileDto.model_validate(json.loads(camera_profile_json))
    except (json.JSONDecodeError, ValueError) as exc:
        raise HTTPException(status_code=400, detail=f"Invalid camera_profile_json: {exc}") from exc

    suffix = Path(file.filename).suffix or ".mp4"
    with tempfile.NamedTemporaryFile(delete=False, suffix=suffix) as tmp:
        tmp_path = Path(tmp.name)
        shutil.copyfileobj(file.file, tmp)

    try:
        return app.state.analyzer.analyze(tmp_path, profile)
    except ValueError as exc:
        raise HTTPException(status_code=400, detail=str(exc)) from exc
    finally:
        tmp_path.unlink(missing_ok=True)
