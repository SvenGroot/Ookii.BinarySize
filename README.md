# Ookii.BinarySize

Ookii.BinarySize is a modern library for parsing and displaying quantities of bytes, using
human-readable representation.

It provides functionality to [parse numeric values](#parsing) that end with a multiple-byte unit,
such as B, KB, MiB, kilobyte, mebibytes, and so on, and to [format them for display](#formatting) in
the same way. It can automatically choose the best unit, or you can choose the one you want, based
on the format string.

- Supports units with SI prefixes ("KB", "MB", "GB", "TB", "PB", and "EB"), and IEC prefixes
  ("KiB", "MiB", "GiB", "TiB", "PiB", and "EiB"), with and without the "B".
- Also supports unabbreviated units for both parsing and formatting: "byte", "kilobyte", "megabyte",
  "gigabyte", "terabyte", "petabyte", and "exabyte", and their IEC equivalents "kibibyte",
  "mebibyte", "gibibyte", "tebibyte", "pebibyte", and "exbibyte".
- Interpret SI prefixes as either [powers of two or powers of ten](https://en.wikipedia.org/wiki/Byte#Multiple-byte_units).
- Parse and store values up to approximately positive and negative 8 EiB, using [`Int64`][] (`long`)
  as the underlying storage.
- Support for [localizing units and prefixes](#localization).
- Provided as a library for [.Net Standard 2.0, .Net Standard 2.1, and .Net 6.0 and up](#requirements).
- Implements arithmetic and binary operators, and supports .Net 7 generic math.
- Trim-friendly.

Besides display formatting and parsing user input, BinarySize provides everything needed to easily
use human-readable byte sizes in places such as configuration files, serialized XML and
JSON, and [command line arguments](src/Samples/ListDirectory).

Ookii.BinarySize comes in two packages; the core functionality is in Ookii.BinarySize, and
additional extension methods for [`IAsyncEnumerable<T>`][] are available in the Ookii.BinarySize.Async
package. Both are available on NuGet.

Package                | Version
-----------------------|--------------------------------------------------------------------------------------------------------------------------
Ookii.BinarySize       | [![NuGet](https://img.shields.io/nuget/v/Ookii.BinarySize)](https://www.nuget.org/packages/Ookii.BinarySize/)
Ookii.BinarySize.Async | [![NuGet](https://img.shields.io/nuget/v/Ookii.BinarySize.Async)](https://www.nuget.org/packages/Ookii.BinarySize.Async/)

Keep reading to get started, or check out the [samples](src/Samples). You can also try it out online
on [.Net Fiddle](https://dotnetfiddle.net/zYiUV7).

To use the library, store your size values as a [`BinarySize`][] structure, which supports
formatting and parsing, along with overloaded operators and other operations provided for
convenience.

## Formatting

To create a string from a [`BinarySize`][] value, use the [`BinarySize.ToString()`][] method, or use
the value directly in a compound formatting string.

The default format for [`BinarySize`][] will automatically use the largest unit where the value is a
whole number, using IEC units and a "B" suffix, and a space between the number and the unit. For
example, "42 MiB".

You can use a format string to customize the output. This format string can consist of just a
multi-byte unit, to use default number formatting followed by that unit, or it can use a numeric
format string followed by a unit. Spaces around the unit are preserved. For example "KB", " MiB",
"#.0 G", as well as simply "B", are all accepted format strings.

To use unabbreviated units, end the format string with "byte". This will automatically expand the
unit and pick between the singular and plural version of "byte" or "bytes". For example, "Mibyte".

To automatically pick the largest unit where the value is a whole number, use "A" instead of an
explicit size prefix, or use "S" to pick the largest prefix where the value is larger than 1, but
may have a fractional component. This also works with long units; e.g. "Abyte" or "Sibyte". The
default format is equivalent to " AiB".

For example, the following displays a value using several different formats:

```csharp
var value = BinarySize.FromGibi(2.5);
Console.WriteLine($"{value: B} is equal to:");
Console.WriteLine($"Default formatting: {value}");
Console.WriteLine($"Automatic formatting: {value: AB}");
Console.WriteLine($"Shortest formatting: {value:#.0 SiB}");
Console.WriteLine($"Explicit formatting: {value:#,###Ki}");
Console.WriteLine($"Unabbreviated formatting: {value:#.0 Sibyte}");
```

This outputs the following:

```text
2684354560 B is equal to:
Default formatting: 2560 MiB
Automatic formatting: 2560 MB
Shortest formatting: 2.5 GiB
Explicit formatting: 2,621,440Ki
Unabbreviated formatting: 2.5 gibibytes
```

In the example above, the default format displays the value in MiB because it is not a whole number
of GiB. The "SiB" format string does use the GiB unit, because it allows factional values.

The formatting above uses [powers of two](https://en.wikipedia.org/wiki/Byte#Multiple-byte_units)
for both SI and IEC units. This means that 1 KB is the same as 1 KiB, both equal to 1,024 bytes, 1
MB is equal to 1 MiB, both equaling 1,048,576 bytes, and so on.

If you wish to use the IEC standard where only IEC units are powers of two, and SI units are always
powers of ten, this can be done by using a lower-case unit prefix in the format string, without
including an 'i'. This applies to explicit units ("k", "m", "g", "t", "p" and "e"), as well as the
automatic units "a" and "s", and works with both abbreviated and unabbreviated units.

With this option, 1 kB equals 1,000 bytes, 1 MB equals 1,000,000 bytes, and so on.

The abbreviated unit prefixes will always be output as uppercase, even if they are lowercase in the
format string. The only exception is the decimal version of "kilo", which is a lowercase "k" to
conform to the SI standard.

```csharp
var value = BinarySize.FromGibi(2.5);
Console.WriteLine($"{value: B} is equal to (decimal):");
Console.WriteLine($"Automatic formatting: {value: aB}");
Console.WriteLine($"Shortest formatting: {value:#.0 sB}");
Console.WriteLine($"Explicit formatting: {value:#,###k}");
Console.WriteLine($"Unabbreviated formatting: {value:#.0 sbyte}");
Console.WriteLine();

value = (BinarySize)2500000000;
Console.WriteLine($"And {value: B} is equal to (decimal):");
Console.WriteLine($"Automatic formatting: {value: aB}");
Console.WriteLine($"Shortest formatting: {value:#.0 sB}");
Console.WriteLine($"Explicit formatting: {value:#,###k}");
Console.WriteLine($"Unabbreviated formatting: {value:#.0 sbyte}");
```

This outputs the following:

```text
2684354560 B is equal to (decimal):
Automatic formatting: 2684354560 B
Shortest formatting: 2.7 GB
Explicit formatting: 2,684,355k
Unabbreviated formatting: 2.7 gigabytes

And 2500000000 B is equal to (decimal):
Automatic formatting: 2500 MB
Shortest formatting: 2.5 GB
Explicit formatting: 2,500,000k
Unabbreviated formatting: 2.5 gigabytes
```

Note that using "aB" formatted 2.5 GiB as plain bytes, since there is no higher decimal prefix that
could be used while keeping a whole number.

See [`BinarySize.ToString()`][] for full documentation on the format string.

## Parsing

To parse a string value into a [`BinarySize`][], use the [`BinarySize.Parse()`][] and
[`BinarySize.TryParse()`][] methods. The input can be a number, optionally followed by an SI or IEC
multi-byte unit, and optionally ending with a 'B' character. The case of the unit, and any spacing
surrounding it, will be ignored.

The following code parses several strings with different styles of unit, and displays their values
in bytes on the console.

```csharp
var values = new[] { "100", "100B", "10 KB", "2.5 MiB", "5G" };
foreach (var value in values)
{
    var size = BinarySize.Parse(value, CultureInfo.InvariantCulture);
    Console.WriteLine($"'{value}' == {size.Value} bytes");
}
```

This would print the following output.

```text
'100' == 100 bytes
'100B' == 100 bytes
'10 KB' == 10240 bytes
'2.5 MiB' == 2621440 bytes
'5G' == 5368709120 bytes
```

Just as with formatting, the default behavior is to treat both SI and IEC units as powers of two, as
can be seen by the values for "10 KB" and "5G".

To use the IEC standard of interpreting SI units as powers of ten, we can use the
[`BinarySizeOptions.UseIecStandard`][] flag.

```csharp
var values = new[] { "100", "100B", "10 KB", "2.5 MiB", "5G" };
foreach (var value in values)
{
    var size = BinarySize.Parse(value, BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture);
    Console.WriteLine($"'{value}' == {size: byte}");
}
```

Which gives this output instead.

```text
'100' == 100 bytes
'100B' == 100 bytes
'10 KB' == 10000 bytes
'2.5 MiB' == 2621440 bytes
'5G' == 5000000000 bytes
```

As you can see, IEC units ("MiB" in this example) are not affected by this option.

If you want to always treat SI units as powers of ten when parsing, or do so in a context where you
cannot specify a [`BinarySizeOptions`][] value (such as a value that is being serialized), you can
instead use the [`IecBinarySize`][] structure; this is a wrapper around [`BinarySize`][] which
defaults to this parsing behavior.

By default, parsing only accepts abbreviated units. To also allow parsing of full units (e.g.
"kilobytes"), use the [`BinarySizeOptions.AllowLongUnits`][] flag. Use the
[`BinarySizeOptions.AllowLongUnitsOnly`][] flag to disallow the use of abbreviated units, and accept
only unabbreviated ones. Both singular and plural units will be accepted regardless of the actual
value, and case is ignored.

```csharp
var values = new[] { "100 bytes", "10 Kilobytes", "2.5 mebibyte", "5giga" };
foreach (var value in values)
{
    var size = BinarySize.Parse(value, BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture);
    Console.WriteLine($"'{value}' == {size: byte}");
}
```

Which gives this output.

```text
'100 bytes' == 100 bytes
'10 Kilobytes' == 10240 bytes
'2.5 mebibyte' == 2621440 bytes
'5giga' == 5368709120 bytes
```

This can also be combined with the [`BinarySizeOptions.UseIecStandard`][] flag.

## Localization

By default, Ookii.BinarySize uses English-language units, regardless of the culture used for
formatting or parsing. However, it's possible to create custom localized units using the
[`BinaryUnitInfo`][] class.

The [`BinaryUnitInfo`][] class defines the strings used for both the abbreviated (short) and
unabbreviated (long) versions of base unit ("B" or "byte"), and the various SI and IEC prefixes.
When using a custom [`BinaryUnitInfo`][], these strings will be used instead of the defaults for
both parsing and formatting. The [`BinaryUnitInfo`][] class also allows you to add a separator in
between the prefix and base unit, and to specify how string comparisons are performed when parsing.

[`BinaryUnitInfo`][] implements the [`IFormatProvider`][] interface, so it can be used directly with
the [`Parse()`][] and [`ToString()`][ToString()_0] methods. If this is done, the current culture is
used for number formatting.

To use an explicit culture in combination with a custom [`BinaryUnitInfo`][] object, you can use the
[`WithBinaryUnitInfo()`][] extension method. This returns a new [`CultureInfo`][] object that wraps
the original one, but also supports the provided [`BinaryUnitInfo`][] object.

For example, the below creates a custom [`BinaryUnitInfo`][] using French units, and combines it
with a French [`CultureInfo`][] to use the appropriate number formatting.

```csharp
var unitInfo = new BinaryUnitInfo()
{
    LongByte = "octet",
    LongBytes = "octets",
    LongConnector = "-",
    LongMega = "méga",
    LongMebi = "mébi",
    LongTera = "téra",
    LongTebi = "tébi",
    LongPeta = "péta",
    LongPebi = "pébi",
    // For short (abbreviated) units, only "B" is replaced with "o" in French. The prefixes are the
    // same.
    ShortByte = "o",
    ShortBytes = "o",
};

var culture = CultureInfo.GetCultureInfo("fr-FR").WithBinaryUnitInfo(unitInfo);
var stringValue = binarySizeValue.ToString(" Sibyte", culture);
```

For a complete example, check out the [localization sample](src/Samples/Localization/).

## Other features

Besides offering formatting and parsing, Ookii.BinarySize aims to make using it as convenient as
possible, and offers several features for that purpose.

- Provides size constants [`Kibi`][], [`Mebi`][], [`Gibi`][], [`Tebi`][], [`Pebi`][] and
  [`Exbi`][].
- Create values with a specific size using the [`FromKibi()`][], [`FromMebi()`][], [`FromGibi()`][],
  [`FromTebi()`][], [`FromPebi()`][] and [`FromExbi()`][] methods.
- Retrieve the values scaled to any size using the [`AsKibi`][], [`AsMebi`][], [`AsGibi`][],
  [`AsTebi`][], [`AsPebi`][] and [`AsExbi`][] properties.
- Implements arithmetic, shift and binary operators.
- Implements comparison operators, as well as [`IEquatable<T>`][], [`IComparable<T>`][],
  and [`GetHashCode()`][].
- Convert to and from `long` by casting.
- Provides extension methods for [`IEnumerable<T>`][], and with the
  [Ookii.BinarySize.Async](https://www.nuget.org/packages/Ookii.BinarySize.Async) package, also for
  [`IAsyncEnumerable<T>`][].

The [`BinarySize`][] and [`IecBinarySize`][] structures can be used in contexts where they are
automatically serialized, such as configuration files, serialized XML or JSON data, XAML, and
others, because they provide a [`TypeConverter`][], a [`JsonConverter`][], and implement
[`IXmlSerializable`][].

Ookii.BinarySize also supports modern .Net functionality. It supports parsing from a
[`ReadOnlySpan<char>`][], and formatting with [`ISpanFormattable`][]. The .Net 7.0 version of the
library also implements [`ISpanParsable<TSelf>`][], and supports the interfaces for
[generic math](https://learn.microsoft.com/dotnet/standard/generics/math).

## Requirements

Ookii.BinarySize is a class library for use in your own applications for [Microsoft .Net](https://dotnet.microsoft.com/).
Assemblies are provided targeting the following:

- .Net Standard 2.0
- .Net Standard 2.1
- .Net 6.0
- .Net 7.0 and later

## Building and testing

To build Ookii.BinarySize, make sure you have the following installed:

- [Microsoft .Net 8.0 SDK](https://dotnet.microsoft.com/download) or later

To build the library, tests and samples, simply use the `dotnet build` command in the `src`
directory. You can run the unit tests using `dotnet test`. The tests should pass on all platforms
(Windows and Linux have been tested).

The tests are built and run for .Net 8.0, .Net 7.0, .Net 6.0, and .Net Framework 4.8. Running the
.Net Framework tests on a non-Windows platform may require the use of
[Mono](https://www.mono-project.com/).

Ookii.BinarySize uses a strongly-typed resources file, which will not update correctly unless the
`Resources.resx` file is edited with [Microsoft Visual Studio](https://visualstudio.microsoft.com/).
I could not find a way to make this work correctly with both Visual Studio and the `dotnet` command.

The class library documentation is generated using [Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB).

## Learn more

- [What's new in Ookii.BinarySize](docs/ChangeLog.md)
- [Class library documentation](https://www.ookii.org/Link/BinarySizeDoc)
- [Samples](src/Samples)

[`AsExbi`]: https://www.ookii.org/docs/binarysize-1.1/html/P_Ookii_BinarySize_AsExbi.htm
[`AsGibi`]: https://www.ookii.org/docs/binarysize-1.1/html/P_Ookii_BinarySize_AsGibi.htm
[`AsKibi`]: https://www.ookii.org/docs/binarysize-1.1/html/P_Ookii_BinarySize_AsKibi.htm
[`AsMebi`]: https://www.ookii.org/docs/binarysize-1.1/html/P_Ookii_BinarySize_AsMebi.htm
[`AsPebi`]: https://www.ookii.org/docs/binarysize-1.1/html/P_Ookii_BinarySize_AsPebi.htm
[`AsTebi`]: https://www.ookii.org/docs/binarysize-1.1/html/P_Ookii_BinarySize_AsTebi.htm
[`BinarySize.Parse()`]: https://www.ookii.org/docs/binarysize-1.1/html/Overload_Ookii_BinarySize_Parse.htm
[`BinarySize.ToString()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_ToString_1.htm
[`BinarySize.TryParse()`]: https://www.ookii.org/docs/binarysize-1.1/html/Overload_Ookii_BinarySize_TryParse.htm
[`BinarySize`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_BinarySize.htm
[`BinarySizeOptions.AllowLongUnits`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_BinarySizeOptions.htm
[`BinarySizeOptions.AllowLongUnitsOnly`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_BinarySizeOptions.htm
[`BinarySizeOptions.UseIecStandard`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_BinarySizeOptions.htm
[`BinarySizeOptions`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_BinarySizeOptions.htm
[`BinaryUnitInfo`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_BinaryUnitInfo.htm
[`CultureInfo`]: https://learn.microsoft.com/dotnet/api/system.globalization.cultureinfo
[`Exbi`]: https://www.ookii.org/docs/binarysize-1.1/html/F_Ookii_BinarySize_Exbi.htm
[`FromExbi()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_FromExbi.htm
[`FromGibi()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_FromGibi.htm
[`FromKibi()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_FromKibi.htm
[`FromMebi()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_FromMebi.htm
[`FromPebi()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_FromPebi.htm
[`FromTebi()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_FromTebi.htm
[`GetHashCode()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_BinarySize_GetHashCode.htm
[`Gibi`]: https://www.ookii.org/docs/binarysize-1.1/html/F_Ookii_BinarySize_Gibi.htm
[`IAsyncEnumerable<T>`]: https://learn.microsoft.com/dotnet/api/system.collections.generic.iasyncenumerable-1
[`IComparable<T>`]: https://learn.microsoft.com/dotnet/api/system.icomparable-1
[`IecBinarySize`]: https://www.ookii.org/docs/binarysize-1.1/html/T_Ookii_IecBinarySize.htm
[`IEnumerable<T>`]: https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1
[`IEquatable<T>`]: https://learn.microsoft.com/dotnet/api/system.iequatable-1
[`IFormatProvider`]: https://learn.microsoft.com/dotnet/api/system.iformatprovider
[`Int64`]: https://learn.microsoft.com/dotnet/api/system.int64
[`ISpanFormattable`]: https://learn.microsoft.com/dotnet/api/system.ispanformattable
[`ISpanParsable<TSelf>`]: https://learn.microsoft.com/dotnet/api/system.ispanparsable-1
[`IXmlSerializable`]: https://learn.microsoft.com/dotnet/api/system.xml.serialization.ixmlserializable
[`JsonConverter`]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverter
[`Kibi`]: https://www.ookii.org/docs/binarysize-1.1/html/F_Ookii_BinarySize_Kibi.htm
[`Mebi`]: https://www.ookii.org/docs/binarysize-1.1/html/F_Ookii_BinarySize_Mebi.htm
[`Parse()`]: https://www.ookii.org/docs/binarysize-1.1/html/Overload_Ookii_IecBinarySize_Parse.htm
[`Pebi`]: https://www.ookii.org/docs/binarysize-1.1/html/F_Ookii_BinarySize_Pebi.htm
[`ReadOnlySpan<char>`]: https://learn.microsoft.com/dotnet/api/system.readonlyspan-1
[`Tebi`]: https://www.ookii.org/docs/binarysize-1.1/html/F_Ookii_BinarySize_Tebi.htm
[`TypeConverter`]: https://learn.microsoft.com/dotnet/api/system.componentmodel.typeconverter
[`WithBinaryUnitInfo()`]: https://www.ookii.org/docs/binarysize-1.1/html/M_Ookii_CultureInfoExtensions_WithBinaryUnitInfo.htm
[ToString()_0]: https://www.ookii.org/docs/binarysize-1.1/html/Overload_Ookii_BinarySize_ToString.htm
