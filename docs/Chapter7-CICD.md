# CI/CD guide

## When to add what

| Stage | When | What you have |
| ----- | ---- | ------------- |
| **CI** (build on push) | **Now** — after Week 3 backend + ML in Docker | GitHub Actions `ci.yml` |
| **CI + tests** | After unit/integration tests exist | `dotnet test`, ML smoke clip |
| **CD to staging** | When Week 4–5 demo is stable | Push images to ACR/Docker Hub, deploy compose |
| **CD to production** | Portfolio / real deployment only | Secrets, migrations, monitoring |

You do **not** need CD before Week 4 Angular; **CI** catches broken builds early.

## Current CI pipeline (GitHub Actions)

**Trigger:** push or PR to `main`

1. **backend** — `dotnet restore`, `build`, `publish` (`SmartCity.Api`)
2. **docker-images** — build `ml-service` and `backend` Dockerfiles (no push)

View runs: **GitHub repo → Actions** tab.

## Local parity

```powershell
cd backend/SmartCity.Api
dotnet build -c Release
docker compose build ml-service backend
```

## CD (future) — typical flow

```text
push main → CI green → build & push images → deploy to Azure Container Apps / VM docker compose pull
```

Secrets (GitHub **Settings → Secrets**):

| Secret | Use |
| ------ | --- |
| `AZURE_CREDENTIALS` or `ACR_*` | Push images |
| `MSSQL_SA_PASSWORD`, `JWT_KEY` | Runtime env (never in repo) |

Do **not** add CD until you choose a host; CI is enough for resume (“automated builds on GitHub Actions”).

## Resume / interview line

*“Every merge to main runs CI: .NET Release build and Docker image builds for API and ML services, aligned with our docker-compose production shape.”*
