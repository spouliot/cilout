using System.ComponentModel;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public class AssemblySettings : CommandSettings {

    [Description ("Assembly to analyze")]
    [CommandArgument (0, "<assembly>")]
    public string? Assembly { get; init; }
}
