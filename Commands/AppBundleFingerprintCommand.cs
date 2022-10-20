using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Claunia.PropertyList;

namespace CilOut.Commands;

public sealed class AppBundleFingerprintCommand : Command<AppBundleFingerprintCommand.Settings> {

	public sealed class Settings : AppBundleSettings {

		[Description ("Output using a diff-friendly, tab seperated values, format")]
		[CommandOption ("-d|--diff-friendly")]
		[DefaultValue (false)]
		public bool DiffFriendly { get; init; }
	}

	public override int Execute ([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		var result = ReturnCodes.Success;
		var appbundle = settings.AppBundle!;
		SortedDictionary<string, SortedDictionary<string, string>> fingerprints = new ();
		foreach (var file in Directory.EnumerateFiles (appbundle, "*", SearchOption.AllDirectories)) {
			switch (Path.GetExtension (file).ToLowerInvariant ()) {
			case ".dll":
			case  ".exe":
				var assembly_result = AssemblyFingerprintCommand.Fingerprint (file, out var minutiae);
				if (assembly_result == ReturnCodes.Success)
					fingerprints.Add (file, minutiae);

				if (assembly_result != ReturnCodes.Success)
					result = assembly_result;
				break;
			case ".plist":
				try {
					fingerprints.Add (file, ParseInfoPlist (file));
				}
				catch {
					result = ReturnCodes.Failure;
				}
				break;
			}
		}

		foreach (var (file, minutiae) in fingerprints) {
			var n = appbundle.Length;
			if (file [n] == '/')
				n++;
			var assembly = file [n..];
			if (settings.DiffFriendly) {
				AssemblyFingerprintCommand.TabSeparatedValues (assembly, minutiae);
			} else {
				AssemblyFingerprintCommand.ShowResult (assembly, result, minutiae);
			}
		}

		return (int) result;
	}

	static SortedDictionary<string,string> ParseInfoPlist (string filename)
	{
		SortedDictionary<string,string> minutiae = new ();
		NSDictionary? info = (NSDictionary) PropertyListParser.Parse (filename);
		if (info is not null) {
			foreach (var element in info) {
				switch (element.Key) {
				case "DTCompiler":
				case "DTPlatformBuild":
				case "DTPlatformName":
				case "DTPlatformVersion":
				case "DTSDKBuild":
				case "DTSDKName":
				case "DTXcode":
				case "DTXcodeBuild":
					minutiae.Add (element.Key, element.Value.ToString ()!);
					break;
				case "com.microsoft.ios":
				case "com.microsoft.macos":
				case "com.microsoft.maccatalyst":
				case "com.microsoft.tvos":
				case "com.microsoft.watchos":
					NSDictionary? dict = (NSDictionary) element.Value;
					if (dict is not null) {
						foreach (var subelement in dict)
							minutiae.Add (element.Key + "/" + subelement.Key, subelement.Value.ToString ()!);
					}
					break;
				}
			}
		}
		return minutiae;
	}
}
