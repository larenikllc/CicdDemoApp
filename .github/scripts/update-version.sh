#!/usr/bin/env bash
set -euo pipefail

version="${1:?Usage: update-version.sh <version>}"

if [[ ! "${version}" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
  echo "Invalid semantic version: ${version}" >&2
  exit 1
fi

perl -0pi -e "s#<Version>[^<]+</Version>#<Version>${version}</Version>#; s#<AssemblyVersion>[^<]+</AssemblyVersion>#<AssemblyVersion>${version}.0</AssemblyVersion>#; s#<FileVersion>[^<]+</FileVersion>#<FileVersion>${version}.0</FileVersion>#; s#<InformationalVersion>[^<]+</InformationalVersion>#<InformationalVersion>${version}</InformationalVersion>#" Directory.Build.props

echo "Updated Directory.Build.props to version ${version}."
