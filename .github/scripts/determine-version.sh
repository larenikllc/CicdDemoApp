#!/usr/bin/env bash
set -euo pipefail

latest_tag="$(git tag --list 'v*' --sort=-v:refname | grep -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | head -n 1 || true)"

if [[ -n "${latest_tag}" ]]; then
  base_version="${latest_tag#v}"
  commit_range="${latest_tag}..HEAD"
else
  base_version="0.0.0"
  commit_range="HEAD"
fi

commit_messages="$(git log --format='%B' "${commit_range}" 2>/dev/null || true)"
bump="none"

if grep -Eq '^BREAKING CHANGE:|^[a-z]+(\([^)]*\))?!:' <<< "${commit_messages}"; then
  bump="major"
elif grep -Eq '^feat(\([^)]*\))?:' <<< "${commit_messages}"; then
  bump="minor"
elif grep -Eq '^(fix|refactor|perf)(\([^)]*\))?:' <<< "${commit_messages}"; then
  bump="patch"
fi

IFS='.' read -r major minor patch <<< "${base_version}"

case "${bump}" in
  major)
    major=$((major + 1)); minor=0; patch=0 ;;
  minor)
    minor=$((minor + 1)); patch=0 ;;
  patch)
    patch=$((patch + 1)) ;;
esac

version="${major}.${minor}.${patch}"
should_release="false"
if [[ "${bump}" != "none" ]]; then
  should_release="true"
fi

echo "Base version: ${base_version}"
echo "Version bump: ${bump}"
echo "Calculated version: ${version}"

if [[ -n "${GITHUB_OUTPUT:-}" ]]; then
  {
    echo "version=${version}"
    echo "bump=${bump}"
    echo "should_release=${should_release}"
    echo "latest_tag=${latest_tag}"
  } >> "${GITHUB_OUTPUT}"
fi
