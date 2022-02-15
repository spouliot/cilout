using Mono.Cecil;
using Spectre.Console;

namespace CilOut.Commands;

public sealed class AssemblyReferencesCommand : AssemblyTreeCommandBase<AssemblyReferencesCommand.Settings> {

	public class Settings : AssemblyTreeBaseSettings {
	}

	protected override ReturnCodes BuildTree (AssemblyDefinition assembly, Settings settings, Tree tree)
	{
		var level = settings.Level;
		foreach (var md in assembly.Modules) {
			if (!md.HasAssemblyReferences)
				continue;

			foreach (var ar in md.AssemblyReferences.OrderBy ((arg) => arg.ToString ())) {
				var arnode = tree.AddNode ($"AR: {ar}");
				if (level > AssemblyTreeBaseSettings.MetadataLevel.Assembly) {
					var type_refs = md.GetTypeReferences ();
					foreach (var tr in type_refs.Where ((arg) => arg.Scope.ToString () == ar.FullName).OrderBy ((arg) => arg.FullName)) {
						var trnode = arnode.AddNode ($"TR: {tr}");
						if (level > AssemblyTreeBaseSettings.MetadataLevel.Type) {
							var member_refs = md.GetMemberReferences ();
							foreach (var mr in member_refs.Where ((arg) => arg.DeclaringType.FullName == tr.FullName).OrderBy ((arg) => arg.FullName))
								trnode.AddNode ($"MR: {mr.Beautify ()}");
						}
					}
				}
			}
		}
		return ReturnCodes.Success;
	}
}
