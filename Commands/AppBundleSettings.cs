using System.ComponentModel;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public class AppBundleSettings : CommandSettings {

	[Description ("Application Bundle to analyze")]
	[CommandArgument (0, "<appbundle>")]
	public string? AppBundle { get; init; }
}
