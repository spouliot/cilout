using Mono.Cecil;
using Spectre.Console;

namespace CilOut;

static public class CecilFormaters {

	static public string Beautify (this MemberReference mr)
	{
		var name = mr.FullName;
		var ctor_pos = name.IndexOf ("::.ctor(");
		if (ctor_pos != -1) {
			name = name [(ctor_pos + 2)..];
		} else {
			var cctor_pos = name.IndexOf ("::.cctor(");
			if (cctor_pos != -1)
				name = name [(cctor_pos + 2)..];
			else
				name = name.Replace (mr.DeclaringType.FullName + "::", "");
		}
		return name.EscapeMarkup ();
	}
}
