using Ookii.Common;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ookii;

/// <summary>
/// Represents an unsigned quantity of bytes, supporting formatting and parsing using units with
/// binary prefixes such as "KB" or "KiB".
/// </summary>
/// <remarks>
/// <para>
///   The underlying value is stored as a <see cref="ulong"/>, as a whole number of bytes. Scaling
///   is only used when formatting, such as when using the <see cref="ToString(string?, IFormatProvider?)"/>
///   method.
/// </para>
/// <para>
///   Instances of this structure can be created by parsing a string containing a unit with a
///   binary prefix, such as "1.5 GiB".
/// </para>
/// <para>
///   By default, this structure uses the definition that "1 KB" == 1024 bytes, identical to "1
///   KiB", and "1 MB" == "1 MiB" == 1048576 bytes, and so on. This behavior can be changed using
///   the see <see cref="BinarySizeOptions"/> enumeration, and formatting strings used with the
///   <see cref="ToString(string?, IFormatProvider?)"/> method. The <see cref="UIecBinarySize"/>
///   structure provides a wrapper that defaults to the behavior of the
///   <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
[TypeConverter(typeof(UBinarySizeConverter))]
[JsonConverter(typeof(UBinarySizeJsonConverter))]
[Serializable]
[CLSCompliant(false)]
public readonly partial struct UBinarySize : IEquatable<UBinarySize>, IComparable<UBinarySize>, IComparable, IFormattable, IXmlSerializable
#if NET6_0_OR_GREATER
    , ISpanFormattable
#endif
#if NET7_0_OR_GREATER
    , ISpanParsable<UBinarySize>
#endif
{
    /// <summary>
    /// The size of a kibibyte (binary kilobyte); 1,024 bytes.
    /// </summary>
    public const ulong Kibi = BinarySize.Kibi;

    /// <summary>
    /// The size of a mebibyte (binary megabyte); 1,048,576 bytes.
    /// </summary>
    public const ulong Mebi = BinarySize.Mebi;

    /// <summary>
    /// The size of a gibibyte (binary gigabyte); 1,073,741,824 bytes.
    /// </summary>
    public const ulong Gibi = BinarySize.Gibi;

    /// <summary>
    /// The size of a tebibyte (binary terabyte); 1,099,511,627,776 bytes.
    /// </summary>
    public const ulong Tebi = BinarySize.Tebi;

    /// <summary>
    /// The size of a pebibyte (binary petabyte); 1,125,899,906,842,624 bytes.
    /// </summary>
    public const ulong Pebi = BinarySize.Pebi;

    /// <summary>
    /// The size of an exbibyte (binary exabyte); 1,152,921,504,606,846,976 bytes.
    /// </summary>
    public const ulong Exbi = BinarySize.Exbi;

    /// <summary>
    /// Initializes a new instance of the <see cref="UBinarySize"/> structure with the specified
    /// value.
    /// </summary>
    /// <param name="value">The size in bytes.</param>
    public UBinarySize(ulong value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets a <see cref="UBinarySize"/> instance with a value of zero bytes.
    /// </summary>
    public static readonly UBinarySize Zero = default;

    /// <summary>
    /// Represents the minimum <see cref="UBinarySize"/> value.
    /// </summary>
    public static readonly UBinarySize MinValue = (UBinarySize)ulong.MinValue;

    /// <summary>
    /// Represents the maximum <see cref="UBinarySize"/> value.
    /// </summary>
    public static readonly UBinarySize MaxValue = (UBinarySize)ulong.MaxValue;

    /// <summary>
    /// Gets the number of bytes represented by this instance.
    /// </summary>
    /// <value>
    /// The value of this instance, in bytes.
    /// </value>
    public ulong Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    /// <summary>
    /// Gets the value of this instance in kibibytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional kibibytes.
    /// </value>
    public double AsKibi => Value / (double)Kibi;

    /// <summary>
    /// Gets the value of this instance in mebibytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional mebibytes.
    /// </value>
    public double AsMebi => Value / (double)Mebi;

    /// <summary>
    /// Gets the value of this instance in gibibytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional gibibytes.
    /// </value>
    public double AsGibi => Value / (double)Gibi;

    /// <summary>
    /// Gets the value of this instance in tebibytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional tebibytes.
    /// </value>
    public double AsTebi => Value / (double)Tebi;

    /// <summary>
    /// Gets the value of this instance in pebibytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional pebibytes.
    /// </value>
    public double AsPebi => Value / (double)Pebi;

    /// <summary>
    /// Gets the value of this instance in exbibytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional exbibytes.
    /// </value>
    public double AsExbi => Value / (double)Exbi;

    /// <summary>
    /// Returns a <see cref="UBinarySize"/> that represents the specified number of kibibytes.
    /// </summary>
    /// <param name="value">A number of kibibytes.</param>
    /// <returns>An object that represents <paramref name="value"/>.</returns>
    /// <exception cref="OverflowException">
    /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN" qualifyHint="true"/>.
    /// </exception>
    public static UBinarySize FromKibi(double value) => FromScale(value, Kibi);

    /// <summary>
    /// Returns a <see cref="UBinarySize"/> that represents the specified number of mebibytes.
    /// </summary>
    /// <param name="value">A number of mebibytes.</param>
    /// <returns>An object that represents <paramref name="value"/>.</returns>
    /// <exception cref="OverflowException">
    /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN" qualifyHint="true"/>.
    /// </exception>
    public static UBinarySize FromMebi(double value) => FromScale(value, Mebi);

    /// <summary>
    /// Returns a <see cref="UBinarySize"/> that represents the specified number of gibibytes.
    /// </summary>
    /// <param name="value">A number of gibibytes.</param>
    /// <returns>An object that represents <paramref name="value"/>.</returns>
    /// <exception cref="OverflowException">
    /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN" qualifyHint="true"/>.
    /// </exception>
    public static UBinarySize FromGibi(double value) => FromScale(value, Gibi);

    /// <summary>
    /// Returns a <see cref="UBinarySize"/> that represents the specified number of tebibytes.
    /// </summary>
    /// <param name="value">A number of tebibytes.</param>
    /// <returns>An object that represents <paramref name="value"/>.</returns>
    /// <exception cref="OverflowException">
    /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN" qualifyHint="true"/>.
    /// </exception>
    public static UBinarySize FromTebi(double value) => FromScale(value, Tebi);

    /// <summary>
    /// Returns a <see cref="UBinarySize"/> that represents the specified number of pebibytes.
    /// </summary>
    /// <param name="value">A number of pebibytes.</param>
    /// <returns>An object that represents <paramref name="value"/>.</returns>
    /// <exception cref="OverflowException">
    /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN" qualifyHint="true"/>.
    /// </exception>
    public static UBinarySize FromPebi(double value) => FromScale(value, Pebi);

    /// <summary>
    /// Returns a <see cref="UBinarySize"/> that represents the specified number of exbibytes.
    /// </summary>
    /// <param name="value">A number of exbibytes.</param>
    /// <returns>An object that represents <paramref name="value"/>.</returns>
    /// <exception cref="OverflowException">
    /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN" qualifyHint="true"/>.
    /// </exception>
    public static UBinarySize FromExbi(double value) => FromScale(value, Exbi);

    /// <summary>
    /// Parses a span of characters into a <see cref="UBinarySize"/> structure.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how
    /// multi-byte units in <paramref name="s"/> are interpreted.
    /// </param>
    /// <param name="style">
    /// A bitwise combination of <see cref="NumberStyles"/> values that indicates the style elements
    /// that can be present in <paramref name="s"/>. A typical value is
    /// <see cref="NumberStyles.Number" qualifyHint="true"/>.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="options"/> is not valid.
    /// </exception>
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> is not representable as a <see cref="UBinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   Depending on the value of <paramref name="options"/>, the use of ulong units may also be
    ///   allowed: "byte", "kilobyte", "kibibyte", "megabyte", "mebibyte", "gigabyte", "gibibyte",
    ///   "terabyte", "tebibyte", "petabyte", "pebibyte", "exabyte", or "exbibyte". The "byte"
    ///   suffix may be ommitted, and may also be the plural "bytes", regardless of whether the
    ///   number is actually plural or not.
    /// </para>
    /// <para>
    ///   The units listed above are the default, invariant units based on the English language.
    ///   You can parse localized units by using the <see cref="BinaryUnitInfo"/> class with the
    ///   <paramref name="provider"/> parameter, either directly or together with a <see cref="CultureInfo"/>
    ///   object through the <see cref="CultureInfoExtensions.WithBinaryUnitInfo" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The case of the units in <paramref name="s"/> is ignored by default. Use the
    ///   <see cref="BinaryUnitInfo.CompareOptions" qualifyHint="true"/> property to customize how
    ///   units are matched.
    /// </para>
    /// <para>
    ///   The size of 1 KiB always equals 1024 bytes, and 1 MiB is  1,048,576 bytes, and so on.
    /// </para>
    /// <para>
    ///   By default, this method also treats 1 KB as 1,024 bytes, identical to 1 KiB, and 1 MB
    ///   equals 1 MiB equals 1,048,576 bytes, and so on.
    /// </para>
    /// <para>
    ///   When <paramref name="options"/> includes the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/>
    ///   flag, the SI prefixes (those without an 'i') are treated as powers of ten, so that 1 kB equals
    ///   1,000 bytes, 1 MB == 1,000,000 bytes, and so on. The IEC prefixes are unchanged, and
    ///   remain based on powers of two.
    /// </para>
    /// <para>
    ///   The value of <paramref name="options"/> is not stored in the instance, and will therefore
    ///   not affect the output when converting back to a string. Use an appropriate format string
    ///   with the <see cref="ToString(string?, IFormatProvider?)"/> method if you wish to use
    ///   decimal prefixes there as well.
    /// </para>
    /// </remarks>
    public static UBinarySize Parse(ReadOnlySpan<char> s, BinarySizeOptions options = BinarySizeOptions.Default, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
        => new(checked((ulong)FormatHelper.Parse(s, options, style, provider)));

    /// <summary>
    /// Parses a span of characters into a <see cref="UBinarySize"/> structure.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> is not representable as a <see cref="UBinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   The units listed above are the default, invariant units based on the English language.
    ///   You can parse localized units by using the <see cref="BinaryUnitInfo"/> class with the
    ///   <paramref name="provider"/> parameter, either directly or together with a <see cref="CultureInfo"/>
    ///   object through the <see cref="CultureInfoExtensions.WithBinaryUnitInfo" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The case of the units in <paramref name="s"/> is ignored by default. Use the
    ///   <see cref="BinaryUnitInfo.CompareOptions" qualifyHint="true"/> property to customize how
    ///   units are matched.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    ///   method.
    /// </para>
    /// </remarks>
    public static UBinarySize Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, NumberStyles.Number, provider);

    /// <summary>
    /// Tries to parse a span of characters into a <see cref="UBinarySize"/> structure.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how
    /// multi-byte units in <paramref name="s"/> are interpreted.
    /// </param>
    /// <param name="style">
    /// A bitwise combination of <see cref="NumberStyles"/> values that indicates the style elements
    /// that can be present in <paramref name="s"/>. A typical value is
    /// <see cref="NumberStyles.Number" qualifyHint="true"/>.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the result of successfully parsing <paramref name="s"/>,
    /// or an undefined value on failure.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="options"/> is not valid.
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out UBinarySize result)
    {
        var success = FormatHelper.TryParse(s, options, style, provider, out var value);
        if (value is > ulong.MaxValue or < ulong.MinValue)
        {
            result = default;
            return false;
        }

        result = new UBinarySize(unchecked((ulong)value));
        return success;
    }

    /// <summary>
    /// Tries to parse a span of characters into a <see cref="UBinarySize"/> structure.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the result of successfully parsing <paramref name="s"/>,
    /// or an undefined value on failure.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out UBinarySize)"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The units listed above are the default, invariant units based on the English language.
    ///   You can parse localized units by using the <see cref="BinaryUnitInfo"/> class with the
    ///   <paramref name="provider"/> parameter, either directly or together with a <see cref="CultureInfo"/>
    ///   object through the <see cref="CultureInfoExtensions.WithBinaryUnitInfo" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The case of the units in <paramref name="s"/> is ignored by default. Use the
    ///   <see cref="BinaryUnitInfo.CompareOptions" qualifyHint="true"/> property to customize how
    ///   units are matched.
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UBinarySize result)
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, provider, out result);

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out UBinarySize)"/>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out UBinarySize)"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The units listed above are the default, invariant units based on the English language.
    ///   You can parse localized units by using the <see cref="BinaryUnitInfo"/> class with the
    ///   <see cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out UBinarySize)"/> method.
    /// </para>
    /// <para>
    ///   The case of the units in <paramref name="s"/> is ignored by default. Use the
    ///   <see cref="BinaryUnitInfo.CompareOptions" qualifyHint="true"/> property to customize how
    ///   units are matched.
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, out UBinarySize result)
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, null, out result);

    /// <summary>
    /// Tries to parse a string into a <see cref="UBinarySize"/> structure.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how
    /// multi-byte units in <paramref name="s"/> are interpreted.
    /// </param>
    /// <param name="style">
    /// A bitwise combination of <see cref="NumberStyles"/> values that indicates the style elements
    /// that can be present in <paramref name="s"/>. A typical value is
    /// <see cref="NumberStyles.Number" qualifyHint="true"/>.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the result of successfully parsing <paramref name="s"/>,
    /// or an undefined value on failure.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="options"/> is not valid.
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out UBinarySize result)
#else
    public static bool TryParse(string? s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out UBinarySize result)
#endif
    {
        if (s == null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), options, style, provider, out result);
    }

    /// <summary>
    /// Tries to parse a string into a <see cref="UBinarySize"/> structure.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the result of successfully parsing <paramref name="s"/>,
    /// or an undefined value on failure.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="TryParse(string, BinarySizeOptions, NumberStyles, IFormatProvider?, out UBinarySize)"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The units listed above are the default, invariant units based on the English language.
    ///   You can parse localized units by using the <see cref="BinaryUnitInfo"/> class with the
    ///   <paramref name="provider"/> parameter, either directly or together with a <see cref="CultureInfo"/>
    ///   object through the <see cref="CultureInfoExtensions.WithBinaryUnitInfo" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The case of the units in <paramref name="s"/> is ignored by default. Use the
    ///   <see cref="BinaryUnitInfo.CompareOptions" qualifyHint="true"/> property to customize how
    ///   units are matched.
    /// </para>
    /// </remarks>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UBinarySize result)
#else
    public static bool TryParse(string? s, IFormatProvider? provider, out UBinarySize result)
#endif
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, provider, out result);

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out UBinarySize)"/>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out UBinarySize)"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The units listed above are the default, invariant units based on the English language.
    ///   You can parse localized units by using the <see cref="BinaryUnitInfo"/> class with the
    ///   <see cref="TryParse(string, IFormatProvider?, out UBinarySize)"/> method.
    /// </para>
    /// <para>
    ///   The case of the units in <paramref name="s"/> is ignored by default. Use the
    ///   <see cref="BinaryUnitInfo.CompareOptions" qualifyHint="true"/> property to customize how
    ///   units are matched.
    /// </para>
    /// </remarks>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, out UBinarySize result)
#else
    public static bool TryParse(string? s, out UBinarySize result)
#endif
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, null, out result);

    /// <summary>
    /// Parses a string into a <see cref="UBinarySize"/> structure.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how
    /// multi-byte units in <paramref name="s"/> are interpreted.
    /// </param>
    /// <param name="style">
    /// A bitwise combination of <see cref="NumberStyles"/> values that indicates the style elements
    /// that can be present in <paramref name="s"/>. A typical value is
    /// <see cref="NumberStyles.Number" qualifyHint="true"/>.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="options"/> is not valid.
    /// </exception>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> is not representable as a <see cref="UBinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
    public static UBinarySize Parse(string s, BinarySizeOptions options = BinarySizeOptions.Default, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        if (s == null)
        {
            throw new ArgumentNullException(nameof(s));
        }

        return Parse(s.AsSpan(), options, style, provider);
    }

    /// <inheritdoc cref="Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The units listed above are the default, invariant units based on the English language.
    ///   You can parse localized units by using the <see cref="BinaryUnitInfo"/> class with the
    ///   <paramref name="provider"/> parameter, either directly or together with a <see cref="CultureInfo"/>
    ///   object through the <see cref="CultureInfoExtensions.WithBinaryUnitInfo" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// <para>
    ///   The case of the units in <paramref name="s"/> is ignored by default. Use the
    ///   <see cref="BinaryUnitInfo.CompareOptions" qualifyHint="true"/> property to customize how
    ///   units are matched.
    /// </para>
    /// </remarks>
    public static UBinarySize Parse(string s, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, NumberStyles.Number, provider);

#if NET6_0_OR_GREATER

    /// <inheritdoc/>
    /// <remarks>
    /// See the <see cref="ToString(string?, IFormatProvider?)"/> method for the possible values of
    /// <paramref name="format"/>.
    /// </remarks>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => FormatHelper.TryFormat(Value, Value, destination, out charsWritten, format, provider);

#endif

    /// <summary>
    /// Formats the value of the current <see cref="UBinarySize"/> instance using the specified
    /// format.
    /// </summary>
    /// <param name="format">
    /// The format to use, or <see langword="null"/> to use the default format.
    /// </param>
    /// <param name="formatProvider">
    /// The provider to use to format the value, or <see langword="null"/> to obtain the numeric
    /// format information from the current locale setting of the operating system.
    /// </param>
    /// <returns>The value of the current instance in the specified format.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   If <paramref name="format"/> is <see langword="null"/>, an empty string, or "G" (the
    ///   general format specifier), the value will be formatted using the largest binary prefix in
    ///   which it can be represented without fractions, with the suffix "iB", and a space before
    ///   the unit. For example, "2 TiB", or "512 B". It is equivalent to using " AiB".
    /// </para>
    /// <para>
    ///   Otherwise, the value of <paramref name="format"/> must be either a multiple-byte unit by
    ///   itself, or a
    ///   <see href="https://learn.microsoft.com/dotnet/standard/base-types/standard-numeric-format-strings">standard numeric format string</see>
    ///   or <see href="https://learn.microsoft.com/dotnet/standard/base-types/custom-numeric-format-strings">custom numeric format string</see>
    ///   followed by a multiple-byte unit.
    /// </para>
    /// <para>
    ///   The multiple-byte unit can be one of the following:
    /// </para>
    /// <list type="table">
    ///   <listheader>
    ///     <term>Format string</term>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <term>B</term>
    ///     <description>
    ///       The output will be formatted as raw bytes, with the suffix "B", e.g. "512B".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>K[i][B], M[i][B], G[i][B], T[i][B], P[i][B], E[i][B]</term>
    ///     <description>
    ///       The output will be formatted as kibibytes, mebibytes, gibibytes, tebibytes, pebibytes,
    ///       or exbibytes respectively, with an optional 'i' for IEC units, and an optional 'B'.
    ///       For these units, SI prefixes without the 'i' character are treated as binary prefixes,
    ///       so 1 KB equals 1 KiB equals 1,024 bytes, and so on. For example, "1.5KiB", or "2Mi" or
    ///       "42TB".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>k[B], m[B], g[B], t[B], p[B], e[B]</term>
    ///     <description>
    ///       With a lower case prefix, the output will be formatted as decimal kilobytes,
    ///       megabytes, gigabytes, terabytes, petabytes, or exabytes respectively, followed by an
    ///       optional 'B'. In this case, 1 kB equals 1,000 bytes, 1 MB equals 1,000,000 bytes, and
    ///       so on. The unit prefix will be capitalized in the output, except for "k" which should
    ///       be lower case as an SI prefix. For example, "1.5kB", or "2M" or "42TB".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>A[i][B]</term>
    ///     <description>
    ///       Automatically select the largest prefix in which the value can be represented without
    ///       fractions, optionally followed by an 'i' and/or a 'B'. For example, 1,572,864 bytes
    ///       would be formatted as "1536KiB" ("AiB"), "1536Ki" ("Ai"), "1536KB" ("AB"), or "1536K"
    ///       ("A").
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>a[B]</term>
    ///     <description>
    ///       Automatically select the largest decimal prefix in which the value can be represented without
    ///       fractions, optionally followed by a 'B'. For example, 1,500,000 bytes would be formatted
    ///       as "1500kB" ("aB"), or "1500k" ("a").
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>S[i][B]</term>
    ///     <description>
    ///       Automatically select the largest prefix where the value is at least 1, allowing the
    ///       use of fractional values, optionally followed by an 'i' and/or a 'B'. For example,
    ///       1,572,864 bytes would be formatted as "1.5MiB" ("SiB"), "1.5Mi" ("Si"), "1.5MB" ("SB")
    ///       or "1.5M" ("S").
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>s[B]</term>
    ///     <description>
    ///       Automatically select the largest decimal prefix where the value is at least 1,
    ///       allowing the use of fractional values, optionally followed by a 'B'. For
    ///       example, 1,500,000 bytes would be formatted as "1.5MB" (sB), or "1.5M" (s).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>
    ///       byte, K[i]byte, M[i]byte, G[i]byte, T[i]byte, P[i]byte, E[i]byte, A[i]byte,
    ///       S[i]byte
    ///     </term>
    ///     <description>
    ///       Format the output using the specified or automatic unit, using the ulong form of the
    ///       unit (e.g. "kilobytes" or "mebibytes"). IEC prefixes will be used if the 'i' is
    ///       present, and all prefixes are treated as binary prefixes.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>
    ///       kbyte, mbyte, gbyte, tbyte, pbyte, ebyte, abyte, sbyte
    ///     </term>
    ///     <description>
    ///       Format the output using the specified or automatic SI unit, using the ulong form of the
    ///       unit (e.g. "kilobytes" or "megabytes"), and using decimal prefixes.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    ///   Any of the above multi-byte units may follow a numeric format string; for example,
    ///   "#,##0.# SiB".
    /// </para>
    /// <para>
    ///   If a multi-byte unit is surrounded by white space, this will be preserved in the output.
    ///   For example, " KB" can be used to format the value 512 as "0.5 KB".
    /// </para>
    /// <note>
    ///   Since "G" by itself is the general format specifier, it cannot be used to format as
    ///   gibibytes; use "GG" instead for this purpose. Using " G" with leading white space or a
    ///   number format will work correctly.
    /// </note>
    /// <para>
    ///   The actual units used when formatting are determined by the <see cref="BinaryUnitInfo"/>
    ///   object provided by the <paramref name="formatProvider"/>, or the value of the
    ///   <see cref="BinaryUnitInfo.InvariantInfo" qualifyHint="true"/> property if the provider
    ///   doesn't support this type.
    /// </para>
    /// <para>
    ///   The properties defining abbreviated units, such as <see cref="BinaryUnitInfo.ShortByte" qualifyHint="true"/>,
    ///   <see cref="BinaryUnitInfo.ShortKibi" qualifyHint="true"/> or <see cref="BinaryUnitInfo.ShortKilo" qualifyHint="true"/>
    ///   are used for all format strings, except those that end in "byte", which use the
    ///   full units, defined by properties such as <see cref="BinaryUnitInfo.LongByte" qualifyHint="true"/>,
    ///   <see cref="BinaryUnitInfo.LongKibi" qualifyHint="true"/> or <see cref="BinaryUnitInfo.LongKilo" qualifyHint="true"/>.
    /// </para>
    /// <para>
    ///   The <see cref="BinaryUnitInfo.ShortByte" qualifyHint="true"/> and <see cref="BinaryUnitInfo.LongByte" qualifyHint="true"/>
    ///   property are only used if the value, when scaled to the prefix, is exactly one. For
    ///   example, 1 B, 1 KiB, 1 PB, etc. Otherwise, the <see cref="BinaryUnitInfo.ShortBytes" qualifyHint="true"/>
    ///   and <see cref="BinaryUnitInfo.LongBytes" qualifyHint="true"/> properties are used, which
    ///   provide the plural versions. For the abbreviated units in the default, English-language
    ///   version, these values are the same (both "B"), but for the full units they are different
    ///   ("byte" and "bytes").
    /// </para>
    /// <para>
    ///   If the value is not exactly one, but is rounded to one by the number format used, the
    ///   <see cref="BinaryUnitInfo.LongByte" qualifyHint="true"/> and
    ///   <see cref="BinaryUnitInfo.LongBytes" qualifyHint="true"/> properties are still used.
    ///   For example, a value of 1.01 kibibytes, when using a format string of "0.# Sibyte", would be
    ///   formatted as "1 kibibytes", using the plural version of the unit.
    /// </para>
    /// </remarks>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
        => FormatHelper.GetString(Value, Value, format, formatProvider);

    /// <summary>
    /// Returns a string representation of the current value, using default formatting.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> that represents this instance.
    /// </returns>
    /// <remarks>
    /// <para>
    ///   Default formatting is equivalent to using the format string " AiB" with the
    ///   <see cref="ToString(string?, IFormatProvider?)"/> method.
    /// </para>
    /// </remarks>
    public override string ToString() => ToString(null, null);

    /// <inheritdoc/>
#if NET6_0_OR_GREATER
    public override bool Equals([NotNullWhen(true)] object? obj)
#else
    public override bool Equals(object? obj)
#endif
        => obj is UBinarySize size && Equals(size);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified
    /// <see cref="UBinarySize"/> value.
    /// </summary>
    /// <param name="other">
    /// The <see cref="UBinarySize"/> value to compare to this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(UBinarySize other) => Value.Equals(other.Value);

    /// <summary>
    /// Compares this instance to a specified <see cref="UBinarySize"/> instance and returns an
    /// indication of their relative values.
    /// </summary>
    /// <param name="other">A <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <inheritdoc/>
    /// </returns>
    public int CompareTo(UBinarySize other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is UBinarySize size)
        {
            return CompareTo(size);
        }

        throw new ArgumentException(Properties.Resources.ValueNotByteSize, nameof(obj));
    }

    XmlSchema? IXmlSerializable.GetSchema() => null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        var stringValue = reader.ReadElementContentAsString();
        var newValue = Parse(stringValue, CultureInfo.InvariantCulture);
        unsafe
        {
            // I don't like this, but it's the only way to use a readonly struct with XML
            // serialization as far as I can tell.
            fixed (UBinarySize* self = &this)
            {
                *self = newValue;
            }
        }
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
        writer.WriteString(ToString(null, CultureInfo.InvariantCulture));
    }

    private static UBinarySize FromScale(double value, ulong scale)
    {
        if (double.IsNaN(value))
        {
            throw new ArgumentException(Properties.Resources.ValueIsNaN, nameof(value));
        }

        return (UBinarySize)checked((ulong)(value * scale));
    }
}
