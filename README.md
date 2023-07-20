# Ookii.BinarySize

Ookii.BinarySize is a modern library for parsing and displaying quantities of bytes using
human-readable representation.

It provides functionality to parse numeric values that end with a byte size unit, such as "512 B",
"3 MB", "10 M", or "4.5GiB", and format them for display in the same way, automatically choosing the
best size prefix, or using the one you specified depending on the format string.

- Supports units with SI prefixes ("KB", "MB", "GB", "TB", "PB", and "EB"), and IEC prefixes
  ("KiB", "MiB", "GiB", "TiB", "PiB", and "EiB"), with and without the "B".
- Interpret SI prefixes as either [powers or two or powers of ten](https://en.wikipedia.org/wiki/Byte#Multiple-byte_units).
- Supports values up to approximately positive and negative 8 EiB, using [`Int64`][] (`long`) to
  store the value.
- Supports .Net Standard 2.0, .Net Standard 2.1, and .Net 6.0 and up.
- Supports math and binary operators, and .Net 7 generic math.
- Trim-friendly.

Besides just display formatting and parsing user input, BinarySize provides everything needed to
easily use human-readable quantities of bytes in places such as configuration files, serialized
XML and JSON, and [command line arguments](src/Samples/ListDirectory).

Ookii.BinarySize comes in two packages; the core functionality is in Ookii.BinarySize, and
additional extension methods for [`IAsyncEnumerable<T>`][] are available in the Ookii.BinarySize.Async
package. Both are available on NuGet.

Ookii.BinarySize       | [![NuGet](https://img.shields.io/nuget/v/Ookii.BinarySize)](https://www.nuget.org/packages/Ookii.BinarySize/)
-----------------------|--------------------------------------------------------------------------------------------------------------------------
Ookii.BinarySize.Async | [![NuGet](https://img.shields.io/nuget/v/Ookii.BinarySize.Async)](https://www.nuget.org/packages/Ookii.BinarySize.Async/)

Keep reading to get started, or check out the [Samples](src/Samples).

## Formatting

Formatting is typically done using the [`BinarySize.ToString()`][] method, or by using the
[`BinarySize`][] structure in a compound formatting string. You can use a format string to customize
the output. This format string can use any of the supported units by itself, or following a numeric
format string. Spaces around the unit are preserved.

For example "KB", " MiB", "#.0 G", as well as simply "B", are all accepted format strings.

In addition to an explicit unit, you can also use the prefix "A" to automatically pick the largest
prefix where the value is a whole number, or "S" to pick the largest prefix where the value is
larger than 1.

For example, the following displays a value using several different formats:

```csharp
var value = BinarySize.FromGibi(2.5);
Console.WriteLine($"{value: B} is equal to:");
Console.WriteLine($"Default formatting: {value}");
Console.WriteLine($"Automatic formatting: {value: AB}");
Console.WriteLine($"Shortest formatting: {value:#.0 SiB}");
Console.WriteLine($"Explicit formatting: {value:#,###Ki}");
```

This outputs the following:

```text
2684354560 B is equal to:
Default formatting: 2560 MiB
Automatic formatting: 2560 MB
Shortest formatting: 2.5 GiB
Explicit formatting: 2,621,440Ki
```

As you can see, the default formatting is equivalent to " AiB", which formats the value in MiB
because it is not a whole number of GiB. Using "SiB" will use the GiB unit.

The formatting above uses [powers of two](https://en.wikipedia.org/wiki/Byte#Multiple-byte_units)
for both SI and IEC unit, so 1 KB equals 1 KiB equals 1,024 bytes. It is also possible to format
using the IEC recommended standard where 1 KiB equal 1,024 bytes, but 1 kB equals 1,000 bytes.

Do do this, use a lower-case prefix in the unit, and do not include an 'i' (which is for IEC units
only).

```csharp
var value = BinarySize.FromGibi(2.5);
Console.WriteLine($"{value: B} is equal to (decimal):");
Console.WriteLine($"Automatic formatting: {value: aB}");
Console.WriteLine($"Shortest formatting: {value:#.0 sB}");
Console.WriteLine($"Explicit formatting: {value:#,###k}");
```

This outputs the following:

```text
2684354560 B is equal to (decimal):
Automatic formatting: 2684354560 B
Shortest formatting: 2.7 GB
Explicit formatting: 2,684,355k
```

Note that automatic formatting formatted as bytes, since there is no higher decimal prefix that
could be used in this case.

The unit prefixes will always be output as upper case, except for the decimal version of "kilo",
which is a lower-case "k".

See [`BinarySize.ToString()`][] for full documentation on the format string.

## Parsing

Parsing can be done using the [`BinarySize.Parse()`][] and [`BinarySize.TryParse()`][] methods. Supported
input is any number, optionally followed by an SI or IEC size prefix, and optionally ending with
a 'B' character. The case of the unit, and any spacing surrounding it, will be ignored.

The following code parses several strings with different styles of unit, and displays their values
in bytes on the console.

```csharp
var values = new[] { "100", "100B", "10 KB", "2.5 MiB", "5G" };
foreach (var value in values)
{
    var size = BinarySize.Parse(value);
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

To use the IEC standard of interpreting SI units as powers of ten, we can use the [`BinarySizeOptions`][] enumeration.

```csharp
var values = new[] { "100", "100B", "10 KB", "2.5 MiB", "5G" };
foreach (var value in values)
{
    var size = BinarySize.Parse(value, BinarySizeOptions.UseIecStandard);
    Console.WriteLine($"'{value}' == {size.Value} bytes");
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

As you can see, IEC units are not affected by this options.

If you want to always treat SI units as powers of ten, or do so in a context where you cannot
specify a [`BinarySizeOptions`][] value (such as a value that is being serialized), you can instead use
the [`IecBinarySize`][] structure; this is a wrapper around [`BinarySize`][] which defaults to this parsing
behavior.

## Other features

Besides offering formatting and parsing behavior, [`BinarySize`][] aims to make using it as convenient
as possible, and offers several features that make it as easy to use as the primitive numeric types.

- Provide scaling constants [`Kibi`][], [`Mebi`][], [`Gibi`][], [`Tebi`][], [`Pebi`][] and
  [`Exbi`][].
- Create scaled values with the [`FromKibi()`][], [`FromMebi()`][], [`FromGibi()`][],
  [`FromTebi()`][], [`FromPebi()`][] and [`FromExbi()`][] methods.
- Retrieve the values at any scale using the [`AsKibi`][], [`AsMebi`][], [`AsGibi`][], [`AsTebi`][],
  [`AsPebi`][] and [`AsExbi`][] properties.
- Implements math, shift and binary operators.
- Provides conversion operators to and from `long`.
- Implements comparison operators, as well as [`IEquatable<T>`][], [`IComparable<T>`][],
  and [`GetHashCode()`][].
- Provides extension methods for [`IEnumerable<T>`][], and through the
  [Ookii.BinarySize.Async](https://www.nuget.org/packages/Ookii.BinarySize.Async) package, also for
  [`IAsyncEnumerable<T>`][].

The [`BinarySize`][] and [`IecBinarySize`][] structure can be used in context where they are
automatically serialized, such as configuration files, serialized XML or JSON data, XAML, and
others, because it provides a [`TypeConverter`][], a [`JsonConverter`][], and implements
[`IXmlSerializable`][].

Ookii.BinarySize also supports modern .Net functionality. It supports parsing from a
[`ReadOnlySpan<char>`][], and formatting with [`ISpanFormattable`][]. The .Net 7.0 version of the
assembly also implements [`ISpanParsable<TSelf>`][], and supports the interfaces for generic math.

## Requirements

Ookii.BinarySize is a class library for use in your own applications for [Microsoft .Net](https://dotnet.microsoft.com/).
It can be used with applications supporting one of the following:

- .Net Standard 2.0
- .Net Standard 2.1
- .Net 6.0
- .Net 7.0 and later

## Building and testing

To build Ookii.CommandLine, make sure you have the following installed:

- [Microsoft .Net 7.0 SDK](https://dotnet.microsoft.com/download) or later

To build the library, tests and samples, simply use the `dotnet build` command in the `src`
directory. You can run the unit tests using `dotnet test`. The tests should pass on all platforms
(Windows and Linux have been tested).

The tests are built and run for .Net 7.0, .Net 6.0, and .Net Framework 4.8. Running the .Net
Framework tests on a non-Windows platform may require the use of [Mono](https://www.mono-project.com/).

Ookii.BinarySize uses a strongly-typed resources file, which will not update correctly unless the
`Resources.resx` file is edited with [Microsoft Visual Studio](https://visualstudio.microsoft.com/).
I could not find a way to make this work correctly with both Visual Studio and the `dotnet` command.

The class library documentation is generated using [Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB).

## Learn more

- [What's new in Ookii.BinarySize](docs/ChangeLog.md)
- [Class library documentation](https://www.ookii.org/Link/BinarySizeDoc)
- [Samples](src/Samples)

[`AsExbi`]: https://www.ookii.org/docs/binarysize-1.0/html/P_Ookii_BinarySize_AsExbi.htm
[`AsGibi`]: https://www.ookii.org/docs/binarysize-1.0/html/P_Ookii_BinarySize_AsGibi.htm
[`AsKibi`]: https://www.ookii.org/docs/binarysize-1.0/html/P_Ookii_BinarySize_AsKibi.htm
[`AsMebi`]: https://www.ookii.org/docs/binarysize-1.0/html/P_Ookii_BinarySize_AsMebi.htm
[`AsPebi`]: https://www.ookii.org/docs/binarysize-1.0/html/P_Ookii_BinarySize_AsPebi.htm
[`AsTebi`]: https://www.ookii.org/docs/binarysize-1.0/html/P_Ookii_BinarySize_AsTebi.htm
[`BinarySize.Parse()`]: https://www.ookii.org/docs/binarysize-1.0/html/Overload_Ookii_BinarySize_Parse.htm
[`BinarySize.ToString()`]: https://www.ookii.org/docs/binarysize-1.0/html/Overload_Ookii_BinarySize_ToString.htm
[`BinarySize.TryParse()`]: https://www.ookii.org/docs/binarysize-1.0/html/Overload_Ookii_BinarySize_TryParse.htm
[`BinarySize`]: https://www.ookii.org/docs/binarysize-1.0/html/T_Ookii_BinarySize.htm
[`BinarySizeOptions`]: https://www.ookii.org/docs/binarysize-1.0/html/T_Ookii_BinarySizeOptions.htm
[`Exbi`]: https://www.ookii.org/docs/binarysize-1.0/html/F_Ookii_BinarySize_Exbi.htm
[`FromExbi()`]: https://www.ookii.org/docs/binarysize-1.0/html/M_Ookii_BinarySize_FromExbi.htm
[`FromGibi()`]: https://www.ookii.org/docs/binarysize-1.0/html/M_Ookii_BinarySize_FromGibi.htm
[`FromKibi()`]: https://www.ookii.org/docs/binarysize-1.0/html/M_Ookii_BinarySize_FromKibi.htm
[`FromMebi()`]: https://www.ookii.org/docs/binarysize-1.0/html/M_Ookii_BinarySize_FromMebi.htm
[`FromPebi()`]: https://www.ookii.org/docs/binarysize-1.0/html/M_Ookii_BinarySize_FromPebi.htm
[`FromTebi()`]: https://www.ookii.org/docs/binarysize-1.0/html/M_Ookii_BinarySize_FromTebi.htm
[`GetHashCode()`]: https://www.ookii.org/docs/binarysize-1.0/html/M_Ookii_BinarySize_GetHashCode.htm
[`Gibi`]: https://www.ookii.org/docs/binarysize-1.0/html/F_Ookii_BinarySize_Gibi.htm
[`IAsyncEnumerable<T>`]: https://learn.microsoft.com/dotnet/api/system.collections.generic.iasyncenumerable-1
[`IComparable<T>`]: https://learn.microsoft.com/dotnet/api/system.icomparable-1
[`IecBinarySize`]: https://www.ookii.org/docs/binarysize-1.0/html/T_Ookii_IecBinarySize.htm
[`IEnumerable<T>`]: https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1
[`IEquatable<T>`]: https://learn.microsoft.com/dotnet/api/system.iequatable-1
[`Int64`]: https://learn.microsoft.com/dotnet/api/system.int64
[`ISpanFormattable`]: https://learn.microsoft.com/dotnet/api/system.ispanformattable
[`ISpanParsable<TSelf>`]: https://learn.microsoft.com/dotnet/api/system.ispanparsable-1
[`IXmlSerializable`]: https://learn.microsoft.com/dotnet/api/system.xml.serialization.ixmlserializable
[`JsonConverter`]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverter
[`Kibi`]: https://www.ookii.org/docs/binarysize-1.0/html/F_Ookii_BinarySize_Kibi.htm
[`Mebi`]: https://www.ookii.org/docs/binarysize-1.0/html/F_Ookii_BinarySize_Mebi.htm
[`Pebi`]: https://www.ookii.org/docs/binarysize-1.0/html/F_Ookii_BinarySize_Pebi.htm
[`ReadOnlySpan<char>`]: https://learn.microsoft.com/dotnet/api/system.readonlyspan-1
[`Tebi`]: https://www.ookii.org/docs/binarysize-1.0/html/F_Ookii_BinarySize_Tebi.htm
[`TypeConverter`]: https://learn.microsoft.com/dotnet/api/system.componentmodel.typeconverter
