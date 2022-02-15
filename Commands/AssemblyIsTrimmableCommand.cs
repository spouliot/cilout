using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public sealed class AssemblyIsTrimmableCommand : Command<AssemblyIsTrimmableCommand.Settings> {

	public sealed class Settings : AssemblySettings {

		[Description ("No visible output. Use return code.")]
		[CommandOption ("-q|--quiet")]
		[DefaultValue (false)]
		public bool Quiet { get; init; }
	}

	public override int Execute ([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		var assembly = settings.Assembly!;
		var trimmable = IsTrimmable (assembly);

		if (!settings.Quiet)
			ShowResult (assembly, trimmable);

		return (int) trimmable;
	}

	public static void ShowResult (string assemblyName, ReturnCodes trimmable)
	{
		var asm = assemblyName.EscapeMarkup ();
		switch (trimmable) {
		case ReturnCodes.Success:
			AnsiConsole.WriteLine ($"Assembly `{asm}` is trimmable.");
			break;
		case ReturnCodes.Failure:
			AnsiConsole.MarkupLine ($"Assembly `{asm}` is [bold]not[/] trimmable.");
			break;
		case ReturnCodes.CouldNotReadAssembly:
			AnsiConsole.MarkupLine ($"[red]Error: [/] Could not read assembly `{asm}`.");
			break;
		}
	}

	public static ReturnCodes IsTrimmable (string assembly)
	{
		try {
			AssemblyDefinition ad = AssemblyDefinition.ReadAssembly (assembly);
			if (!ad.HasCustomAttributes)
				return ReturnCodes.Failure;
			foreach (var ca in ad.CustomAttributes) {
				if (ca.AttributeType.FullName != "System.Runtime.CompilerServices.AssemblyMetadataAttribute")
					continue;
				if (ca.ConstructorArguments.Count != 2)
					continue;
				if ((ca.ConstructorArguments [0].Value as string) != "Trimmable")
					continue;
				if ((ca.ConstructorArguments [1].Value as string) == "true")
					return ReturnCodes.Success;
			}
			return ReturnCodes.Failure;
		} catch (Exception e) {
			if (Environment.GetEnvironmentVariable ("V") is not null)
				AnsiConsole.WriteException (e);
			return ReturnCodes.CouldNotReadAssembly;
		}
	}
}
