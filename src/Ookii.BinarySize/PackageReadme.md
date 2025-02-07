# Ookii.BinarySize

Ookii.BinarySize is a modern library for parsing and displaying quantities of bytes, using
human-readable representation.

It provides functionality to parse numeric values that end with a multiple-byte unit, such as B, KB,
MiB, kilobyte, mebibytes, and so on, and to format them for display in the same way. It can
automatically choose the best unit, or you can choose the one you want, based on the format string.

- Supports units with SI prefixes ("KB", "MB", "GB", "TB", "PB", and "EB"), and IEC prefixes
  ("KiB", "MiB", "GiB", "TiB", "PiB", and "EiB"), with and without the "B".
- Also supports unabbreviated units for both parsing and formatting: "byte", "kilobyte", "megabyte",
  "gigabyte", "terabyte", "petabyte", and "exabyte", and their IEC equivalents "kibibyte",
  "mebibyte", "gibibyte", "tebibyte", "pebibyte", and "exbibyte".
- Interpret SI prefixes as either [powers of two or powers of ten](https://en.wikipedia.org/wiki/Byte#Multiple-byte_units).
- Parse and store values up to approximately positive and negative 8 EiB, using `Int64` (`long`)
  as the underlying storage.
- Provides an unsigned version that can parse and store values up to approximately 16 EiB, using
  `UInt64` (`ulong`) as the underlying storage.
- Support for localizing units and prefixes.
- Implements arithmetic and binary operators, and supports .Net 7 generic math.
- Trim-friendly.

Besides display formatting and parsing user input, BinarySize provides everything needed to easily
use human-readable byte sizes in places such as configuration files, serialized XML and
JSON, and command line arguments.

You can format values using custom format strings to pick a unit, or choose one automatically, using
either powers of ten or powers of two for SI units.

```csharp
var value = BinarySize.FromGibi(2.5);
Console.WriteLine($"{value: B} is equal to:");
Console.WriteLine($"Default formatting: {value}");
Console.WriteLine($"Automatic formatting: {value: AB}");
Console.WriteLine($"Shortest formatting: {value:#.0 SiB}");
Console.WriteLine($"Explicit formatting: {value:#,###Ki}");
Console.WriteLine($"Unabbreviated formatting: {value:#,### Sibyte}");
Console.WriteLine($"Automatic formatting (decimal): {value: aB}");
Console.WriteLine($"Shortest formatting (decimal): {value:#.0 sB}");
Console.WriteLine($"Explicit formatting (decimal): {value:#,###k}");
Console.WriteLine($"Unabbreviated formatting (decimal): {value:#,### sbyte}");
```

Which outputs:

```text
2684354560 B is equal to:
Default formatting: 2560 MiB
Automatic formatting: 2560 MB
Shortest formatting: 2.5 GiB
Explicit formatting: 2,621,440Ki
Unabbreviated formatting: 2.5 gibibytes
Automatic formatting (decimal): 2684354560 B
Shortest formatting (decimal): 2.7 GB
Explicit formatting (decimal): 2,684,355k
Unabbreviated formatting (decimal): 2.7 gigabytes
```

See the documentation for [`BinarySize.ToString()`][] for information on the format string.

You can also parse values that contain any supported unit.

```csharp
var values = new[] { "100", "100B", "10 KB", "2.5 MiB", "5G" };
foreach (var value in values)
{
    var size = BinarySize.Parse(value, CultureInfo.InvariantCulture);
    Console.WriteLine($"'{value}' == {size.Value} bytes");
}
```

Which outputs:

```text
'100' == 100 bytes
'100B' == 100 bytes
'10 KB' == 10240 bytes
'2.5 MiB' == 2621440 bytes
'5G' == 5368709120 bytes
```

See the [GitHub project](https://www.github.com/SvenGroot/Ookii.BinarySize) for more information.

[`BinarySize.ToString()`]: https://www.ookii.org/docs/binarysize-1.2/html/M_Ookii_BinarySize_ToString_1.htm
