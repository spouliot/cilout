A collection of commands to query .net assemblies and output results that
are easy to read and diff.

The tool and it's commands can be useful by themselves or when used in
conjunction with [`appcompare`](https://github.com/spouliot/appcompare).


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

## RETURN CODES:

| Return Code | Description                                                  |
|-------------|--------------------------------------------------------------|
| `0`         | Execution was successful                                     |
| positive    | Command specific (see the command documentation for details) |
| `-1`        | Error while validating the arguments or running the command  |

## EXAMPLES:

```bash
$ cilout -v

0.1.0
```
