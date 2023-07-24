using Ookii;
using Ookii.CommandLine;
using Ookii.CommandLine.Validation;
using System.ComponentModel;

namespace ListDirectory;

// Command line arguments for the sample, which are parsed using Ookii.CommandLine.
[GeneratedParser]
[ParseOptions(IsPosix = true)]
[Description("Lists the files in a directory, including their sizes in a human-readable format.")]
partial class Arguments
{
    [CommandLineArgument(IsPositional = true, IsShort = true, IncludeDefaultInUsageHelp = false)]
    [Description("The path of the directory to list. The default value is the current working directory.")]
    public DirectoryInfo Path { get; set; } = new DirectoryInfo(".");

    // Ookii.CommandLine supports any type with a Parse() method, so we don't need to do anything
    // special to support BinarySize. Using BinarySize here allows the user to use human-readable
    // values on the command line, rather than having to do math to specify a minimum size in bytes.
    [CommandLineArgument(IsShort = true)]
    [ValidateRange(0L, null)]
    [Description("The minimum size of the files to include in the listing. This value can use suffixes such as 'KB' or 'MiB'.")]
    public BinarySize? MinSize { get; set; }

    [CommandLineArgument(IsShort = true)]
    [Description("Use SI units, based on powers of ten, for the output. This doesn't affect how MinSize is parsed, which will always use powers of two.")]
    public bool SiUnits { get; set; }
}
