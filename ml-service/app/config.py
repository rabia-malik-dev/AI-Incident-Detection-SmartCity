from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    model_config = SettingsConfigDict(env_file=".env", extra="ignore")

    yolo_weights: str = "yolov8n.pt"
    device: str = "cpu"
    uploads_dir: str = "./data/uploads"
    snapshots_dir: str = "./data/snapshots"
    frame_stride_seconds: float = 1.0
    wrong_direction_min_speed_px: float = 3.0
    wrong_direction_angle_tolerance_deg: float = 55.0

    vehicle_class_ids: tuple[int, ...] = (2, 3, 5, 7)  # COCO: car, motorcycle, bus, truck


settings = Settings()
