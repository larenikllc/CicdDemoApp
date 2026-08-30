# Branching and release strategy

This repository demonstrates the modified Git Flow described in the source material.

## Branches

| Branch | Starts from | Purpose | Deployment |
| --- | --- | --- | --- |
| `master` | Release or hotfix PR | Production-ready code | Dummy production deployment |
| `develop` | `master` initially | Feature integration | None |
| `feature/*` | `develop` | Feature development | None |
| `release/*` | `develop` | Release stabilization | Dummy test deployment |
| `hotfix/*` | `master` | Critical production fixes | Dummy test deployment |

Feature branches merge into `develop` through a lightweight pull request. Release and hotfix branches merge into `master` through the production gate. After a release or hotfix is merged, `master` must be merged back into `develop`.

## Required repository settings

GitHub Copilot review effort and human approvals are repository settings, not GitHub Actions steps.

- For pull requests into `develop`, enable GitHub Copilot review at Lite effort. Treat it as informational and require the `Build and test` status check, but do not require a human approval.
- For pull requests into `master`, enable GitHub Copilot review at Balanced effort, require one human approval, require the `Build and test` status check, and require the branch to be up to date.
- Protect `master` from direct pushes.
- Keep task closure separate from deployment. A release can ship after the master review gate even when a Definition of Done task remains open.

## Versioning

The CI/CD workflow analyzes Conventional Commits since the latest `vMAJOR.MINOR.PATCH` tag.

| Commit | Version impact |
| --- | --- |
| `feat!:` or `BREAKING CHANGE:` | Major |
| `feat:` | Minor |
| `fix:`, `refactor:`, `perf:` | Patch |
| `docs:`, `style:`, `test:`, `chore:`, `ci:` | No change |

For a release build, the workflow updates `Directory.Build.props` in the runner workspace, generates `CHANGELOG.md`, publishes a versioned zip, stores it as a workflow artifact, and creates a GitHub Release. The protected source branch is not rewritten by the workflow.

## Release and hotfix overlap

A hotfix always branches from `master`. If a release is already in progress when a hotfix lands, merge `master` into the open release branch so the later release cannot reintroduce the fixed behavior.

```bash
git switch release/your-release
git merge master
git push
```

Use `cherry-pick` only when the release must not include other changes that have also reached `master`.

## Rollback

Each master build stores a versioned artifact. The manual rollback workflow downloads an artifact from a selected successful run and redeploys it without rebuilding or changing Git history. In this demo, deployment is represented by the `Deploy to prod` message.
