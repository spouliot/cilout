using CilOut;

Spectre.Console.Cli.CommandApp app = new ();
app.Configure (config => {
	config.Settings.ApplicationName = "cilout";
	config.AddCommand<IsTrimmableCommand> ("is-trimmable");
});
return app.Run (args);
