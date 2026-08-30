#!/usr/bin/env bash
set -euo pipefail

version="${1:?Usage: generate-changelog.sh <version> [output-file]}"
output_file="${2:-CHANGELOG.md}"
latest_tag="$(git tag --list 'v*' --sort=-v:refname | grep -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | head -n 1 || true)"

if [[ -n "${latest_tag}" ]]; then
  commit_range="${latest_tag}..HEAD"
else
  commit_range="HEAD"
fi

subjects="$(git log --format='%s' "${commit_range}" 2>/dev/null || true)"

{
  echo "# Changelog"
  echo
  echo "## v${version}"
  echo

  wrote_section="false"

  if matches="$(grep -E '^feat(\([^)]*\))?!?:' <<< "${subjects}" || true)" && [[ -n "${matches}" ]]; then
    echo "### Features"
    echo
    sed 's/^/- /' <<< "${matches}"
    echo
    wrote_section="true"
  fi

  if matches="$(grep -E '^fix(\([^)]*\))?!?:' <<< "${subjects}" || true)" && [[ -n "${matches}" ]]; then
    echo "### Fixes"
    echo
    sed 's/^/- /' <<< "${matches}"
    echo
    wrote_section="true"
  fi

  if matches="$(grep -E '^(perf|refactor)(\([^)]*\))?!?:' <<< "${subjects}" || true)" && [[ -n "${matches}" ]]; then
    echo "### Performance and refactoring"
    echo
    sed 's/^/- /' <<< "${matches}"
    echo
    wrote_section="true"
  fi

  if [[ "${wrote_section}" == "false" ]]; then
    echo "- No versioned changes were found."
    echo
  fi
} > "${output_file}"

echo "Generated ${output_file} for v${version}."
