using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public sealed class AssemblyHasEntryPointCommand : Command<AssemblyHasEntryPointCommand.Settings> {

	public sealed class Settings : AssemblySettings {

		[Description ("No visible output. Use return code.")]
		[CommandOption ("-q|--quiet")]
		[DefaultValue (false)]
		public bool Quiet { get; init; }
	}

	public override int Execute ([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		var assembly = settings.Assembly!;
		var result = HasEntryPoint (assembly, out var entry_point);

		if (!settings.Quiet)
			ShowResult (assembly, result, entry_point);

		return (int) result;
	}

	public static void ShowResult (string assemblyName, ReturnCodes trimmable, string? entryPoint)
	{
		var asm = assemblyName.EscapeMarkup ();
		switch (trimmable) {
		case ReturnCodes.Success:
			AnsiConsole.WriteLine ($"Assembly `{asm}` has `{entryPoint}` as its entry point.");
			break;
		case ReturnCodes.Failure:
			AnsiConsole.MarkupLine ($"Assembly `{asm}` has [bold]no[/] entry point.");
			break;
		case ReturnCodes.CouldNotReadAssembly:
			AnsiConsole.MarkupLine ($"[red]Error: [/] Could not read assembly `{asm}`.");
			break;
		}
	}

	public static ReturnCodes HasEntryPoint (string assembly, out string? entryPoint)
	{
		entryPoint = null;
		try {
			AssemblyDefinition ad = AssemblyDefinition.ReadAssembly (assembly);
			if (ad.MainModule.EntryPoint == null)
				return ReturnCodes.Failure;
			entryPoint = ad.MainModule.EntryPoint.FullName;
			return ReturnCodes.Success;
		} catch (Exception e) {
			if (Environment.GetEnvironmentVariable ("V") is not null)
				AnsiConsole.WriteException (e);
			return ReturnCodes.CouldNotReadAssembly;
		}
	}
}
