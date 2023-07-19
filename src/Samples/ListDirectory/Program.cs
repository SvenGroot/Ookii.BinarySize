using ListDirectory;
using Ookii;

// Parse the command line arguments using Ookii.CommandLine.
var arguments = Arguments.Parse();
if (arguments == null)
{
    return 1;
}

// Enumerate the files, and convert the info to a temporary format that uses BinarySize for
// convenience.
var files = arguments.Path.EnumerateFiles()
    .Select(f => new { f.Name, Size = (BinarySize)f.Length });

// Filter by minimum size, if the user used that option.
if (arguments.MinSize is BinarySize minSize)
{
    files = files.Where(f => f.Size >= minSize);
}

// List every file, and display the size using the shortest possible representation (the largest
// possible unit, using fractional values where necessary).
foreach (var file in files)
{
    Console.WriteLine($"{file.Name}: {file.Size:0.# SiB}");
}

// Use the Sum extension method provided for BinarySize to add up all the values.
var total = files.Sum(f => f.Size);
Console.WriteLine();
Console.WriteLine($"Total: {total:0.# SiB}");

return 0;
