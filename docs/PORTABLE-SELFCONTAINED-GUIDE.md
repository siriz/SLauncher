# Portable Self-Contained Deployment Guide

> **This guide is out of date.** SLauncher no longer stores user data in a
> `UserCache` folder next to the executable, so the "carry the whole folder
> around" workflow described in earlier versions of this guide is no longer
> possible.

## What changed

Settings, saved items and tabs are now stored per-user in
`%LOCALAPPDATA%\SLauncher\UserCache`, not next to `SLauncher.exe`. This makes
the app resilient to reinstalls, updates and rebuilds, but it means the data
no longer travels with a copy of the executable.

## Moving your setup to another PC

Use the **Backup & Restore** section in Settings instead:

1. On the source PC: **Settings → Backup & Restore → Export...** and save the
   `.slauncherbackup` file.
2. Copy that file to the destination PC.
3. On the destination PC: **Settings → Backup & Restore → Import...**, select
   the file, and restart SLauncher when prompted.

The exported file contains your settings, saved items and tabs. The favicon
cache is intentionally excluded since it's just a regenerable cache, not
configuration.

SLauncher itself is still self-contained (no separate .NET or Windows App SDK
install required) - only the "data lives next to the exe" part of this guide
no longer applies.
