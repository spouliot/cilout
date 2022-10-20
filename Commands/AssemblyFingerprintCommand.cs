using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public sealed class AssemblyFingerprintCommand : Command<AssemblyFingerprintCommand.Settings> {

	public sealed class Settings : AssemblySettings {

		[Description ("Output using a diff-friendly, tab seperated values, format")]
		[CommandOption ("-d|--diff-friendly")]
		[DefaultValue (false)]
		public bool DiffFriendly { get; init; }
	}

	public override int Execute ([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		var assembly = settings.Assembly!;
		var result = Fingerprint (assembly, out var minutiae);

		if (settings.DiffFriendly) {
			TabSeparatedValues (Path.GetFileName (assembly), minutiae);
		} else {
			ShowResult (assembly, result, minutiae);
		}

		return (int) result;
	}

	public static void TabSeparatedValues (string assemblyName, SortedDictionary<string,string> minutiae)
	{
		foreach (var (key, value) in minutiae) {
			Console.Write (assemblyName);
			Console.Write ('\t');
			Console.Write (key);
			Console.Write ('\t');
			Console.WriteLine (value);
		}
	}

	public static void ShowResult (string assemblyName, ReturnCodes result, SortedDictionary<string,string> minutiae)
	{
		var asm = assemblyName.EscapeMarkup ();
		switch (result) {
		case ReturnCodes.Success:
			AnsiConsole.WriteLine ($"Assembly `{asm}` fingerprint is composed of");
			foreach (var (key, value) in minutiae) {
				AnsiConsole.Write (key);
				AnsiConsole.Write (" : '");
				AnsiConsole.Write (value);
				AnsiConsole.WriteLine ('\'');
			}
			break;
		case ReturnCodes.Failure:
			AnsiConsole.MarkupLine ($"Assembly `{asm}` has [bold]no[/] minutiae.");
			break;
		case ReturnCodes.CouldNotReadAssembly:
			AnsiConsole.MarkupLine ($"[red]Error: [/] Could not read assembly `{asm}`.");
			break;
		}
	}

	public static ReturnCodes Fingerprint (string assembly, out SortedDictionary<string,string> minutiae)
	{
		minutiae = new ();
		try {
			AssemblyDefinition ad = AssemblyDefinition.ReadAssembly (assembly);
			// FullName includes [Assembly]Version, Culture and PublicKeyToken
			minutiae.Add ("FullName", ad.FullName);
			if (ad.HasCustomAttributes) {
				foreach (var ca in ad.CustomAttributes) {
					switch (ca.AttributeType.FullName) {
					case "System.Reflection.AssemblyCompanyAttribute":
					case "System.Reflection.AssemblyConfigurationAttribute":
					case "System.Reflection.AssemblyFileVersionAttribute": // can differ from AssemblyVersion
					case "System.Reflection.AssemblyInformationalVersionAttribute":
					case "System.Runtime.Versioning.TargetFrameworkAttribute":
					case "System.Runtime.Versioning.TargetPlatformAttribute":
					case "System.Runtime.Versioning.SupportedOSPlatform":
						minutiae.Add (ca.AttributeType.Name, GetConstructorArgument (ca, 0));
						break;
					case "System.Reflection.AssemblyMetadataAttribute":
						minutiae.Add (ca.AttributeType.Name + "." + GetConstructorArgument (ca, 0), GetConstructorArgument (ca, 1));
						break;
					}
				}
			}
			return minutiae.Count > 0 ? ReturnCodes.Success : ReturnCodes.Failure;
		} catch (Exception e) {
			if (Environment.GetEnvironmentVariable ("V") is not null)
				AnsiConsole.WriteException (e);
			return ReturnCodes.CouldNotReadAssembly;
		}
	}

	static string GetConstructorArgument (CustomAttribute ca, int n)
	{
		if (!ca.HasConstructorArguments)
			return "";
		if (n >= ca.ConstructorArguments.Count)
			return "";
		return ca.ConstructorArguments [0].Value.ToString () ?? "";
	}
}
