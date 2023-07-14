using ListDirectory;
using Ookii;

var arguments = Arguments.Parse();
if (arguments == null)
{
    return 1;
}

var files = arguments.Path.EnumerateFiles().Select(f => new { f.Name, Size = (BinarySize)f.Length });
if (arguments.MinSize is BinarySize minSize)
{
    files = files.Where(f => f.Size >= minSize);
}

foreach (var file in files)
{
    Console.WriteLine($"{file.Name}: {file.Size:0.# SiB}");
}

var total = files.Select(f => f.Size).Sum();
Console.WriteLine();
Console.WriteLine($"Total: {total:0.# SiB}");

return 0;
