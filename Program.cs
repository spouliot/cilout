using CilOut.Commands;

Spectre.Console.Cli.CommandApp app = new ();
app.Configure (config => {
	config.Settings.ApplicationName = "cilout";
	config.AddBranch<AssemblySettings> ("assembly", add => {
		add.SetDescription ("Assembly to analyze");
		add.AddCommand<AssemblyFingerprintCommand> ("fingerprint")
			.WithDescription ("Generate a fingerprint of the assembly to detect future changes.");
		add.AddCommand<AssemblyIsTrimmableCommand> ("is-trimmable")
			.WithDescription ("Check for the presence of [[assembly: AssemblyMetadata (\"Trimmable\", \"true\")]] inside the specified assembly.");
		add.AddCommand<AssemblyHasEntryPointCommand> ("has-entrypoint")
			.WithDescription ("Check if the specified assembly has an entry point (e.g. `Main`) defined.");
		add.AddCommand<AssemblyReferencesCommand>("references")
			.WithAlias ("refs")
			.WithDescription ("Show the metadata references, as a tree, inside the specified assembly.");
		add.AddCommand<AssemblyDefinitionsCommand>("definitions")
			.WithAlias ("defs")
			.WithDescription ("Show the metadata definitions, as a tree, inside the specified assembly.");
	});
	config.AddBranch<AppBundleSettings> ("appbundle", add => {
		add.SetDescription ("Application Bundle to analyze");
		add.AddCommand<AppBundleFingerprintCommand> ("fingerprint")
			.WithDescription ("Generate a fingerprint of the appbundle to detect future changes.");
		add.AddCommand<AppBundleIsTrimmableCommand> ("is-trimmable")
			.WithDescription ("Check for the presence of [[assembly: AssemblyMetadata (\"Trimmable\", \"true\")]] inside any assemblies of the appbundle.");
		add.AddCommand<AppBundleHasEntryPointCommand> ("has-entrypoint")
			.WithDescription ("Check if the specified application bundle has an assembly with an entry point (e.g. `Main`) defined.");
	});
});
return app.Run (args);
