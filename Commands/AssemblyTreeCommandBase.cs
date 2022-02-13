using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public class AssemblyTreeBaseSettings : AssemblySettings {

	public enum MetadataLevel {
		Assembly,
		Type,
		Member,
	}

	[Description ("Level: `assembly`, `types` or `members` (default)")]
	[CommandOption ("-l|--level")]
	public MetadataLevel Level { get; init; } = MetadataLevel.Member;

	[Description ("Save output as HTML inside the specified file.")]
	[CommandOption ("--html")]
	public string? HtmlFile { get; init; }

	[Description ("Save output as text inside the specified file.")]
	[CommandOption ("--text")]
	public string? TextFile { get; init; }
}


public abstract class AssemblyTreeCommandBase<TSettings> : Command<TSettings> where TSettings : AssemblyTreeBaseSettings {

	public override int Execute ([NotNull] CommandContext context, [NotNull] TSettings settings)
	{
		if ((settings.HtmlFile is not null) || (settings.TextFile is not null))
			AnsiConsole.Record ();

		var assembly = settings.Assembly!;
		var result = ShowTree (assembly, settings);
		switch (result) {
		case ReturnCodes.Success:
			if (settings.HtmlFile is not null)
				File.WriteAllText (settings.HtmlFile, AnsiConsole.ExportHtml ());
			if (settings.TextFile is not null)
				File.WriteAllText (settings.TextFile, AnsiConsole.ExportText ());
			break;
		case ReturnCodes.CouldNotReadAssembly:
			var asm = assembly!.EscapeMarkup ();
			AnsiConsole.MarkupLine ($"[red]Error: [/] Could not read assembly `{asm}`.");
			break;
		}
		return (int) result;
	}

	protected ReturnCodes ShowTree (string assembly, TSettings settings)
	{
		try {
			AssemblyDefinition ad = AssemblyDefinition.ReadAssembly (assembly);
			Tree atree = new ($"A: {ad.FullName}") {
				Guide = new DiffFriendlyTreeGuide (),
			};
			var result = BuildTree (ad, settings, atree);
			if (result == ReturnCodes.Success) {
				AnsiConsole.Write (atree);
				AnsiConsole.WriteLine ();
			}
			return result;
		} catch (Exception e) {
			if (Environment.GetEnvironmentVariable ("V") is not null)
				AnsiConsole.WriteException (e);
			return ReturnCodes.CouldNotReadAssembly;
		}
	}

	protected abstract ReturnCodes BuildTree (AssemblyDefinition assembly, TSettings settings, Tree tree);
}
