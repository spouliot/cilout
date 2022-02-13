using System.ComponentModel;
using Mono.Cecil;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CilOut.Commands;

public sealed class AssemblyDefinitionsCommand : AssemblyTreeCommandBase<AssemblyDefinitionsCommand.Settings> {

	public class Settings : AssemblyTreeBaseSettings {

		[Description ("List only types/members that are externally visible from the assembly")]
		[CommandOption ("--visible")]
		public bool VisibleOnly { get; init; }
	}

	protected override ReturnCodes BuildTree (AssemblyDefinition assembly, Settings settings, Tree tree)
	{
		var level = settings.Level;
		foreach (var md in assembly.Modules) {
			if (!md.HasAssemblyReferences)
				continue;

			if (level > AssemblyTreeBaseSettings.MetadataLevel.Assembly) {
				foreach (var td in md.Types.OrderBy ((arg) => arg.FullName)) {
					if (!settings.VisibleOnly || (settings.VisibleOnly && td.IsPublic))
						BuildType (td, settings, tree.AddNode ($"TD: {td}"));
				}
			}
		}
		return ReturnCodes.Success;
	}

	void BuildType (TypeDefinition type, Settings settings, TreeNode node)
	{
		foreach (var nested in type.NestedTypes.OrderBy ((arg) => arg.FullName))
			BuildType (nested, settings, node.AddNode ($"TD: {nested}"));

		if (settings.Level > AssemblyTreeBaseSettings.MetadataLevel.Type) {
			foreach (var m in type.Methods.OrderBy ((arg) => arg.FullName)) {
				if (!settings.VisibleOnly || (settings.VisibleOnly && (m.IsPublic || m.IsFamily)))
					node.AddNode ($"MD: {m.Beautify ()}");
			}
		}
	}
}
