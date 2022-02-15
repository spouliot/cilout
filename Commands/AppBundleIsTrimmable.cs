using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public sealed class AppBundleIsTrimmableCommand : Command<AppBundleIsTrimmableCommand.Settings> {

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
				var trimmable = AssemblyIsTrimmableCommand.IsTrimmable (file);

				if (!settings.Quiet)
					AssemblyIsTrimmableCommand.ShowResult (file, trimmable);

				if (trimmable == ReturnCodes.Success)
					result = trimmable;
				break;
			}
		}
		return (int) result;
	}
}
