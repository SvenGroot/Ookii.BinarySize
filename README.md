# Ookii.BinarySize

Ookii.BinarySize is a modern library for parsing and displaying quantities of bytes using
human-readable representation.

It provides functionality to parse numeric values that end with a byte size unit, such as "512 B",
"3 MB", "10 M", or "4.5GiB", and format them for display in the same way, automatically choosing the
best size prefix, or using the one you specified depending on the format string.

- Supports units with SI prefixes ("KB", "MB", "GB", "TB", "PB", and "EB"), and the IEC prefixes.
  ("KiB", "MiB", "GiB", "TiB", "PiB", and "EiB"), and with and without the "B".
- Interpret SI prefixes as either powers or two or powers of ten.
- Supports values up to approximately positive and negative 8 EiB, using Int64 (long) to store the
  value.
- Supports .Net Standard 2.0, .Net Standard 2.1, and .Net 6.0 and up.
- Supports math and binary operators, and .Net 7 generic math.
- Trim-friendly.

Besides just display formatting and parsing user input, BinarySize provides everything needed to
easily use human-readable quantities of bytes in places such as configuration files, serialized
XML and JSON, and [command line arguments](src/Samples/ListDirectory).

## Formatting

Formatting is typically done using the `BinarySize.ToString()` method, or by using the `BinarySize`
structure in a compound formatting string. You can use a format string to customize the output.
This format string can use any of the supported units by itself, or following a numeric format
string. Spaces around the unit are preserved.

For example "KB", " MiB", "#.0 G", as well as simply "B", are all accepted format strings.

In addition to an explicit unit, you can also use the prefix "A" to automatically pick the largest
prefix where the value is a whole number, or "S" to pick the largest prefix where the value is
larger than 1.

For example, the following displays a value using several different formats:

```csharp
var value = BinarySize.FromGibi(2.5);
Console.WriteLine($"{value.Value} bytes is equal to:");
Console.WriteLine($"Default formatting: {value}");
Console.WriteLine($"Automatic formatting: {value: AB}");
Console.WriteLine($"Shortest formatting: {value:#.0 SiB}");
Console.WriteLine($"Explicit formatting: {value:#,###Ki}");
```

This outputs the following:

```text
2684354560 bytes is equal to:
Default formatting: 2560 MiB
Automatic formatting: 2560 MB
Shortest formatting: 2.5 GiB
Explicit formatting: 2,621,440Ki
```

As you can see, the default formatting is equivalent to " AiB", which formats the value in MiB
because it is not a whole number of GiB. Using "SiB" will use the GiB unit.

TODO: Decimal.

## Parsing

Parsing can be done using the `BinarySize.Parse()` and `BinarySize.TryParse()` methods. Supported
input is any number, optionally followed by an SI or IEC size prefix, and optionally ending with
a 'B' character. The case of the unit, and any spacing surrounding it, will be ignored.

The following code parses several strings with different styles of unit, and displays their values
in bytes on the console.

```csharp
var values = new[] { "100", "100B", "10 KB", "2.5 MiB", "5G" };
foreach (var value in values)
{
    Console.WriteLine($"'{value}' == {BinarySize.Parse(value).Value} bytes");
}
```

This would print the following output.

```text
'100' == 100 bytes
'100B' == 100 bytes
'10 KB' == 10240 bytes
'2.5 MiB' == 2621440 bytes
'
```
