A collection of commands to query .net assemblies and output results that
are easy to read and diff.

The tool and it's commands can be useful by themselves or when used in
conjunction with [`appcompare`](https://github.com/spouliot/appcompare).

## How to install

Release packages are available from nuget. They can be installed from the
command-line.

```bash
$ dotnet tool install --global cilout
```

To update or to re-install the latest version execute:

```bash
$ dotnet tool update --global cilout
```

## USAGE:

```bash
cilout [OPTIONS] <COMMAND>
```

## OPTIONS:

* `-h`, `--help`     Prints help information
* `-v`, `--version`  Prints version information

## COMMANDS:

* [`assembly <assembly>`](https://github.com/spouliot/cilout/wiki/Assembly)
    * [`is-trimmable`](https://github.com/spouliot/cilout/wiki/AssemblyIsTrimmable)
    * [`has-entrypoint`](https://github.com/spouliot/cilout/wiki/AssemblyHasEntryPoint)
    * [`references`](https://github.com/spouliot/cilout/wiki/AssemblyReferences)
    * [`definitions`](https://github.com/spouliot/cilout/wiki/AssemblyDefinitions)
* [`appbundle <bundle>`](https://github.com/spouliot/cilout/wiki/AppBundle)
    * [`is-trimmable`](https://github.com/spouliot/cilout/wiki/AppBundleIsTrimmable)
    * [`has-entrypoint`](https://github.com/spouliot/cilout/wiki/AppBundkeHasEntryPoint)

## RETURN CODES:

| Return Code | Description                                                  |
|-------------|--------------------------------------------------------------|
| `0`         | Execution was successful                                     |
| positive    | Command specific (see the command documentation for details) |
| `-1`        | Error while validating the arguments or running the command  |

## EXAMPLES:

```bash
$ cilout -v

1.2.0
```
