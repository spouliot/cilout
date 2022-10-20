# ChangeLog

## 1.2.0 (unreleased)

### Features
- Add `appbundle` branch and [`is-trimmable`](https://github.com/spouliot/cilout/wiki/AppBundleIsTrimmable) command to detect any trimmable assembly inside the app bundle
- Add `has-entrypoint` command, to both `assembly` and `appbundle` branches, to detect if the assembly has an entry point

### Updates
- Updated Spectre.Console to 0.45.0
- Added new Spectre.Console.Cli 0.45.0 for removed CLI features from main package

## 1.1.0 (13 Feb 2022)

### Features
- Add `asssembly` branch and adjust [`is-trimmable`](https://github.com/spouliot/cilout/wiki/AssemblyIsTrimmable) command arguments
- Add [`assembly <assembly> references`](https://github.com/spouliot/cilout/wiki/AssemblyReferences) command
- Add [`assembly <assembly> definitions`](https://github.com/spouliot/cilout/wiki/AssemblyDefinitions) command

## 1.0.0 (11 Feb 2022)

### Features
- Add [`is-trimmable <assembly>`](https://github.com/spouliot/cilout/wiki/IsTrimmable) command
