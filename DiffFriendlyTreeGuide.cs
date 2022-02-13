using Spectre.Console;
using Spectre.Console.Rendering;

namespace CilOut;

public sealed class DiffFriendlyTreeGuide : TreeGuide {

	public override string GetPart (TreeGuidePart part)
	{
		return part switch {
			TreeGuidePart.Space => "  ",
			TreeGuidePart.Continue => "  ",
			TreeGuidePart.Fork => "* ",
			TreeGuidePart.End => "* ",
			_ => throw new ArgumentOutOfRangeException (nameof (part), part, "Unknown tree part."),
		};
	}
}
