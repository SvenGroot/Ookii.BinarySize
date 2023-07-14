using Ookii;
using Ookii.CommandLine;
using System.ComponentModel;

namespace ListDirectory;

[GeneratedParser]
[ParseOptions(IsPosix = true)]
[Description("Lists the files in a directory, including their sizes in a human-readable format.")]
partial class Arguments
{
    [CommandLineArgument(IsPositional = true, IsShort = true, IncludeDefaultInUsageHelp = false)]
    [Description("The path of the directory to list. The default value is the current working directory.")]
    public DirectoryInfo Path { get; set; } = new DirectoryInfo(".");

    [CommandLineArgument(IsShort = true)]
    [Description("The minimum size of the files to include in the listing. This value can use suffixes such as 'KB' or 'MiB'.")]
    public BinarySize? MinSize { get; set; }
}
