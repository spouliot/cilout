using CilOut.Commands;

Spectre.Console.Cli.CommandApp app = new ();
app.Configure (config => {
	config.Settings.ApplicationName = "cilout";
	config.AddBranch<AssemblySettings> ("assembly", add => {
		add.AddCommand<AssemblyIsTrimmableCommand> ("is-trimmable")
			.WithDescription ("Check for the presence of [[assembly: AssemblyMetadata (\"Trimmable\", \"true\")]] inside the specified assembly.");
		add.AddCommand<AssemblyReferencesCommand>("references")
			.WithAlias ("refs")
			.WithDescription ("Show the metadata references, as a tree, inside the specified assembly.");
		add.AddCommand<AssemblyDefinitionsCommand>("definitions")
			.WithAlias ("defs")
			.WithDescription ("Show the metadata definitions, as a tree, inside the specified assembly.");
	});
});
return app.Run (args);
