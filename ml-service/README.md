# ML Service (FastAPI)

**YOLOv8** detection + **ByteTrack** tracking + **RuleEngine** for all violation types.

Start here in Phase 2 (before backend) to learn the pipeline—see [Chapter 8](../docs/Chapter8-ML-Detection-Pipeline.md).

```bash
# (after implementation)
uvicorn app.main:app --host 0.0.0.0 --port 8000
```

Place eval clips under `eval/clips/` for metrics (N ≥ 20 per type).
