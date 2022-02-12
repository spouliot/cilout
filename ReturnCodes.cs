namespace CilOut;

public enum ReturnCodes {
	Success = 0,
	Failure = 1,
	// other positive values are command-specific

	ValidationError = -1, // should not be needed very often (override ValidationResult)
	CouldNotReadAssembly = -2,
}
