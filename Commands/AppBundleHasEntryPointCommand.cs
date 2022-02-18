using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public sealed class AppBundleHasEntryPointCommand : Command<AppBundleHasEntryPointCommand.Settings> {

	public sealed class Settings : AppBundleSettings {

		[Description ("No visible output. Use return code.")]
		[CommandOption ("-q|--quiet")]
		[DefaultValue (false)]
		public bool Quiet { get; init; }
	}

	public override int Execute ([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		var result = ReturnCodes.Failure;
		var appbundle = settings.AppBundle!;
		foreach (var file in Directory.EnumerateFiles (appbundle, "*", SearchOption.AllDirectories)) {
			switch (Path.GetExtension (file).ToLowerInvariant ()) {
			case ".dll":
			case  ".exe":
				var assembly_result = AssemblyHasEntryPointCommand.HasEntryPoint (file, out var entry_point);

				if (!settings.Quiet)
					AssemblyHasEntryPointCommand.ShowResult (file, assembly_result, entry_point);

				if (assembly_result == ReturnCodes.Success)
					result = assembly_result;
				break;
			}
		}
		return (int) result;
	}
}
