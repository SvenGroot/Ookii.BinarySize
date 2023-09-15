# List directory sample

This sample shows the files in a directory, displaying their sizes in human-readable format, along
with the total size of all the files in the directory. This demonstrates formatting values, and the
use of the [`Sum()`][] extension method.

Files can be filtered by a minimum size using a command line argument, which also uses the
[`BinarySize`][] structure to let the user use sizes with multiple-byte units on the command line.
Command line parsing in this sample is done using
[Ookii.CommandLine](https://github.com/SvenGroot/Ookii.CommandLine), which supports any type that
can be converted from a string using a [`Parse()`][] method, so [`BinarySize`][] can be used without
any further effort on our part.

The current directory is listed by default, but a command line argument can also be used to specify
a different one. You can also output the sizes using SI units, based on powers of ten, by passing
the `--si-units` argument. Run `./ListDirectory --help` for more information.

Running it without arguments lists the current directory.

```text
ListDirectory.deps.json: 1.4 KiB
ListDirectory.dll: 14.5 KiB
ListDirectory.exe: 151 KiB
ListDirectory.pdb: 13.7 KiB
ListDirectory.runtimeconfig.json: 147 B
Ookii.BinarySize.dll: 37 KiB
Ookii.BinarySize.pdb: 22.2 KiB
Ookii.BinarySize.xml: 75.7 KiB
Ookii.CommandLine.dll: 197 KiB

Total: 512.6 KiB
```

Or, we can list only those files that are larger than one-hundred kilobytes:

```pwsh
./ListDirectory --min-size 100KB
```

Which outputs:

```text
ListDirectory.exe: 151 KiB
Ookii.CommandLine.dll: 197 KiB

Total: 348 KiB
```

[`BinarySize`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_BinarySize.htm
[`Parse()`]: https://www.ookii.org/docs/binarysize-1.1/html/Overload_Ookii_IecBinarySize_Parse.htm
[`Sum()`]: https://www.ookii.org/docs/binarysize-1.1/html/Overload_Ookii_EnumerableExtensions_Sum.htm
