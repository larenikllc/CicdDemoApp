# CicdDemoApp

CicdDemoApp is a minimal .NET 10 console todo list built to demonstrate a branch-driven GitHub Actions CI/CD pipeline.

## Application features

- Add a todo item
- List todo items
- Mark an item as completed
- Delete an item
- Keep data in memory for the current console session

## Run locally

```bash
dotnet restore CicdDemoApp.sln
dotnet run --project src/CicdDemoApp/CicdDemoApp.csproj
```

Run the tests with:

```bash
dotnet test CicdDemoApp.sln
```

## CI/CD behavior

| Event | Build and tests | Deployment | Release behavior |
| --- | --- | --- | --- |
| Pull request to `develop` | Yes | None | GitHub Copilot Lite review is configured in repository settings |
| Push to `develop` | Yes | None | Integration validation only |
| Push to `release/*` | Yes | `Deploy to test` | Preview changelog and versioned artifact |
| Push to `hotfix/*` | Yes | `Deploy to test` | Preview changelog and versioned artifact |
| Pull request to `master` | Yes | None | GitHub Copilot Balanced review and human approval are configured in repository settings |
| Push to `master` | Yes | `Deploy to prod` | Semantic version, changelog, stored artifact, tag, and GitHub Release |

The test and production deployments are intentionally dummy jobs. Their logs print exactly `Deploy to test` and `Deploy to prod`.

## Conventional Commits

Release versions are calculated from commit messages:

- `feat:` creates a minor version bump.
- `fix:`, `refactor:`, and `perf:` create a patch version bump.
- `feat!:` or a `BREAKING CHANGE:` footer creates a major version bump.
- `docs:`, `style:`, `test:`, `chore:`, and `ci:` do not create a release.

Install the optional local commit hook with:

```bash
npm install
```

The hook uses commitlint and Husky. It is intentionally local-only and can be bypassed with `--no-verify` when a maintainer makes an informed decision.

## Branching model

Day-to-day work starts from `develop` in `feature/*` branches. Planned releases branch from `develop` as `release/*`; urgent production fixes branch from `master` as `hotfix/*`. Only reviewed pull requests should reach `master`.

The detailed flow, required repository settings, hotfix/release overlap rule, versioning, and artifact rollback are documented in [docs/BRANCHING-STRATEGY.md](docs/BRANCHING-STRATEGY.md).

## Manual security scan

Run the `Manual security scan` workflow from the Actions tab and select the Git ref to scan. It reports vulnerable direct and transitive NuGet dependencies.

## Artifact rollback

Run the `Roll back production artifact` workflow with the workflow run ID and exact artifact name from a previous successful master build. The workflow downloads the stored artifact without rebuilding and performs the dummy production deployment.
