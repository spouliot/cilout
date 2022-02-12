using CilOut.Commands;

Spectre.Console.Cli.CommandApp app = new ();
app.Configure (config => {
	config.Settings.ApplicationName = "cilout";
	config.AddCommand<IsTrimmableCommand> ("is-trimmable")
		.WithDescription ("Check for the presence of [[assembly: AssemblyMetadata (\"Trimmable\", \"true\")]] inside the specified assembly.");
});
return app.Run (args);
