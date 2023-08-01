using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
/// Represents a quantity of bytes, supporting formatting and parsing using units with binary
/// prefixes such as "KB" or "KiB".
/// </summary>
/// <remarks>
/// <para>
///   The underlying value is stored as a <see cref="long"/>, as a whole number of bytes. Scaling
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
///   <see cref="ToString(string?, IFormatProvider?)"/> method. The <see cref="IecBinarySize"/>
///   structure provides a wrapper that defaults to the behavior of the
///   <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
[TypeConverter(typeof(BinarySizeConverter))]
[JsonConverter(typeof(BinarySizeJsonConverter))]
[Serializable]
public readonly partial struct BinarySize : IEquatable<BinarySize>, IComparable<BinarySize>, IComparable, IFormattable, IXmlSerializable
#if NET6_0_OR_GREATER
    , ISpanFormattable
#endif
#if NET7_0_OR_GREATER
    , ISpanParsable<BinarySize>
#endif
{
    #region Nested types

    private ref struct SuffixInfo
    {
        public ReadOnlySpan<char> Trimmed { get; set; }
        public ReadOnlySpan<char> Whitespace { get; set; }
        public ReadOnlySpan<char> Trailing { get; set; }
        public long Factor { get; set; }
        public bool HasIecChar { get; set; }
        public bool HasByteChar { get; set; }
        public bool UseDecimal { get; set; }
    }

    #endregion

    /// <summary>
    /// The size of a kibibyte (binary kilobyte); 1,024 bytes.
    /// </summary>
    public const long Kibi = 1024L;

    /// <summary>
    /// The size of a mebibyte (binary megabyte); 1,048,576 bytes.
    /// </summary>
    public const long Mebi = 1024L * Kibi;

    /// <summary>
    /// The size of a gibibyte (binary gigabyte); 1,073,741,824 bytes.
    /// </summary>
    public const long Gibi = 1024L * Mebi;

    /// <summary>
    /// The size of a tebibyte (binary terabyte); 1,099,511,627,776 bytes.
    /// </summary>
    public const long Tebi = 1024L * Gibi;

    /// <summary>
    /// The size of a pebibyte (binary petabyte); 1,125,899,906,842,624 bytes.
    /// </summary>
    public const long Pebi = 1024L * Tebi;

    /// <summary>
    /// The size of an exbibyte (binary exabyte); 1,152,921,504,606,846,976 bytes.
    /// </summary>
    public const long Exbi = 1024L * Pebi;

    // Decimal factors used internally.
    private const long Kilo = 1000;
    private const long Mega = 1000 * Kilo;
    private const long Giga = 1000 * Mega;
    private const long Tera = 1000 * Giga;
    private const long Peta = 1000 * Tera;
    private const long Exa = 1000 * Peta;

    private const long AutoFactor = -1;
    private const long ShortestFactor = -2;
    private const long DecimalAutoFactor = -3;
    private const long DecimalShortestFactor = -4;

    private static readonly char[] _scalingChars = new[] { 'E',  'P',  'T',  'G',  'M',  'K',
                                                             'e',  'p',  't',  'g',  'm',  'k', 'A', 'S', 'a', 's' };
    private static readonly long[] _scalingFactors = new[] { Exbi, Pebi, Tebi, Gibi, Mebi, Kibi,
                                                             Exa,  Peta, Tera, Giga, Mega, Kilo, AutoFactor, ShortestFactor, DecimalAutoFactor, DecimalShortestFactor };

    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySize"/> structure with the specified
    /// value.
    /// </summary>
    /// <param name="value">The size in bytes.</param>
    public BinarySize(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets a <see cref="BinarySize"/> instance with a value of zero bytes.
    /// </summary>
    public static readonly BinarySize Zero = default;

    /// <summary>
    /// Represents the minimum <see cref="BinarySize"/> value.
    /// </summary>
    public static readonly BinarySize MinValue = (BinarySize)long.MinValue;

    /// <summary>
    /// Represents the maximum <see cref="BinarySize"/> value.
    /// </summary>
    public static readonly BinarySize MaxValue = (BinarySize)long.MaxValue;

    /// <summary>
    /// Gets the number of bytes represented by this instance.
    /// </summary>
    /// <value>
    /// The value of this instance, in bytes.
    /// </value>
    public long Value
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
    /// Returns a <see cref="BinarySize"/> that represents the specified number of kibibytes.
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
    public static BinarySize FromKibi(double value) => FromScale(value, Kibi);

    /// <summary>
    /// Returns a <see cref="BinarySize"/> that represents the specified number of mebibytes.
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
    public static BinarySize FromMebi(double value) => FromScale(value, Mebi);

    /// <summary>
    /// Returns a <see cref="BinarySize"/> that represents the specified number of gibibytes.
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
    public static BinarySize FromGibi(double value) => FromScale(value, Gibi);

    /// <summary>
    /// Returns a <see cref="BinarySize"/> that represents the specified number of tebibytes.
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
    public static BinarySize FromTebi(double value) => FromScale(value, Tebi);

    /// <summary>
    /// Returns a <see cref="BinarySize"/> that represents the specified number of pebibytes.
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
    public static BinarySize FromPebi(double value) => FromScale(value, Pebi);

    /// <summary>
    /// Returns a <see cref="BinarySize"/> that represents the specified number of exbibytes.
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
    public static BinarySize FromExbi(double value) => FromScale(value, Exbi);

    /// <summary>
    /// Parses a span of characters into a <see cref="BinarySize"/> structure.
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
    /// <paramref name="s"/> is not representable as a <see cref="BinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and the casing of the unit and any surrounding whitespace is ignored.
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
    public static BinarySize Parse(ReadOnlySpan<char> s, BinarySizeOptions options = BinarySizeOptions.Default, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        ValidateOptions(options);
        if (s.IsEmpty)
        {
            return Zero;
        }

        var factor = ParseUnit(ref s, provider, options);
#if NET6_0_OR_GREATER
        var size = decimal.Parse(s, style, provider);
#else
        var size = decimal.Parse(s.ToString(), style, provider);
#endif

        return new BinarySize(checked((long)(size * factor)));
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="BinarySize"/> structure.
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
    /// <paramref name="s"/> is not representable as a <see cref="BinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "KiB",
    ///   "MB", "MiB", "GB", "GiB", "TB", "TiB", "PB", "PiB", "EB", or "EiB". The "B" may be
    ///   omitted, and the casing of the unit and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    ///   method.
    /// </para>
    /// </remarks>
    public static BinarySize Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, NumberStyles.Number, provider);

    /// <summary>
    /// Tries to parse a span of characters into a <see cref="BinarySize"/> structure.
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
    public static bool TryParse(ReadOnlySpan<char> s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out BinarySize result)
    {
        ValidateOptions(options);
        if (s.IsEmpty)
        {
            result = Zero;
            return true;
        }

        var factor = ParseUnit(ref s, provider, options);
#if NET6_0_OR_GREATER
        var success = decimal.TryParse(s, style, provider, out var size);
#else
        var success = decimal.TryParse(s.ToString(), style, provider, out var size);
#endif

        if (!success)
        {
            result = default;
            return false;
        }

        try
        {
            result = new BinarySize(checked((long)(size * factor)));
            return true;
        }
        catch (OverflowException)
        {
            // I couldn't find a good way to handle overflow without exceptions.
            result = default;
            return false;
        }
    }

    /// <summary>
    /// Tries to parse a span of characters into a <see cref="BinarySize"/> structure.
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
    ///   omitted, and the casing of the unit and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)"/>
    ///   method.
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BinarySize result)
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, provider, out result);

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out BinarySize)"/>
    public static bool TryParse(ReadOnlySpan<char> s, out BinarySize result)
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, null, out result);

    /// <summary>
    /// Tries to parse a string into a <see cref="BinarySize"/> structure.
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
    public static bool TryParse([NotNullWhen(true)] string? s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out BinarySize result)
#else
    public static bool TryParse(string? s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out BinarySize result)
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
    /// Tries to parse a string into a <see cref="BinarySize"/> structure.
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
    ///   omitted, and the casing of the unit and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="TryParse(string, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)"/>
    ///   method.
    /// </para>
    /// </remarks>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BinarySize result)
#else
    public static bool TryParse(string? s, IFormatProvider? provider, out BinarySize result)
#endif
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, provider, out result);

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out BinarySize)"/>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, out BinarySize result)
#else
    public static bool TryParse(string? s, out BinarySize result)
#endif
        => TryParse(s, BinarySizeOptions.Default, NumberStyles.Number, null, out result);

    /// <summary>
    /// Parses a string into a <see cref="BinarySize"/> structure.
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
    /// <paramref name="s"/> is not representable as a <see cref="BinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
    public static BinarySize Parse(string s, BinarySizeOptions options = BinarySizeOptions.Default, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
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
    ///   omitted, and the casing of the unit and any surrounding whitespace is ignored.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so on. To use the IEC standard where SI prefixes
    ///   are treated as powers of ten, use the <see cref="Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    ///   method.
    /// </para>
    /// </remarks>
    public static BinarySize Parse(string s, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, NumberStyles.Number, provider);

#if NET6_0_OR_GREATER

    /// <inheritdoc/>
    /// <remarks>
    /// See the <see cref="ToString(string?, IFormatProvider?)"/> method for the possible values of
    /// <paramref name="format"/>.
    /// </remarks>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var suffix = ParseFormat(format, out var scaledValue);
        if (!scaledValue.TryFormat(destination, out charsWritten, suffix.Trimmed, provider))
        {
            return false;
        }

        var unitInfo = GetUnitInfo(provider);
        var scale = GetUnitScale(suffix, unitInfo);

        if (!destination.TryAppend(ref charsWritten, suffix.Whitespace))
        {
            return false;
        }

        if (scale != null)
        {
            if (!destination.TryAppend(ref charsWritten, scale.AsSpan()))
            {
                return false;
            }

            if (suffix.HasByteChar && !destination.TryAppend(ref charsWritten, unitInfo.ShortConnector.AsSpan()))
            {
                return false;
            }
        }

        var unit = scaledValue == 1 ? unitInfo.ShortByte : unitInfo.ShortBytes;

        // Note: this does not account for the possibility that a not-exactly-one value is
        // rounded to 1 by the number format.
        return (!suffix.HasByteChar || destination.TryAppend(ref charsWritten, unit)) &&
            destination.TryAppend(ref charsWritten, suffix.Trailing);
    }

#endif

    /// <summary>
    /// Formats the value of the current <see cref="BinarySize"/> instance using the specified
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
    ///       or exibytes respectively, with an optional 'i' for IEC units, and an optional 'B'. For
    ///       these units, SI prefixes without the 'i' character are treated as binary prefixes, so
    ///       1 KB equals 1 KiB equals 1,024 bytes, and so on. For example, "1.5KiB", or "2Mi" or "42TB". 
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>k[B], m[B], g[B], t[B], p[B], e[B]</term>
    ///     <description>
    ///       With a lower case prefix, the output will be formatted as decimal kilobytes,
    ///       megabytes, gigabytes, terabytes, petabytes, or exabytes respectively, followed by an
    ///       optional 'B'. In this case, 1 kB equals 1,000 bytes, 1 MB equals 1,000,000 bytes, and
    ///       so on. The unit prefix will be capitalized in the output, except for "k" which should
    ///       be lower case as an SI prefix. For example, "1.5kB", or "2M" or "42T".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>A[i][B]</term>
    ///     <description>
    ///       Automatically select the largest prefix in which the value can be represented without
    ///       fractions, optionally followed by an 'i' and/or a 'B'. The former variant uses binary
    ///       units, while the latter uses decimal. For example, 1,572,864 bytes would be formatted
    ///       as "1536KiB", "1536Ki", "1536KB", or "1536K"; if using decimal it would be "1572864B",
    ///       since there is no higher factor.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>a[B]</term>
    ///     <description>
    ///       Automatically select the largest decimal prefix in which the value can be represented without
    ///       fractions, optionally followed by a 'B'. For example, 1,500,000 bytes would be formatted
    ///       as "1500kB", or "1500k".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>S[i][B]</term>
    ///     <description>
    ///       Automatically select the largest prefix where the value is at least 1, allowing the
    ///       use of fractional values, optionally followed by an 'i' and/or a 'B'. For example,
    ///       1,572,864 bytes would be formatted as "1.5MiB", "1.5Mi", "1.5MB" or "1.5M".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>s[B]</term>
    ///     <description>
    ///       Automatically select the largest decimal prefix where the value is at least 1,
    ///       allowing the use of fractional values, optionally followed by a 'B'. For
    ///       example, 1,500,000 bytes would be formatted as "1.5MB", or "1.5M".
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
    ///   gibibytes; use "GG" instead for this purpose. Using "G" with leading white space or a
    ///   number format will work correctly.
    /// </note>
    /// </remarks>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        var suffix = ParseFormat(format.AsSpan(), out var scaledValue);
        var result = new StringBuilder((format?.Length ?? 0) + 16);
        result.Append(scaledValue.ToString(suffix.Trimmed.ToString(), formatProvider));
        result.Append(suffix.Whitespace);
        var unitInfo = GetUnitInfo(formatProvider);
        var scale = GetUnitScale(suffix, unitInfo);
        if (scale != null)
        {
            result.Append(scale);
            if (suffix.HasByteChar)
            {
                result.Append(unitInfo.ShortConnector);
            }
        }

        if (suffix.HasByteChar)
        {
            // Note: this does not account for the possibility that a not-exactly-one value is
            // rounded to 1 by the number format.
            result.Append(scaledValue == 1 ? unitInfo.ShortByte : unitInfo.ShortBytes);
        }

        result.Append(suffix.Trailing);
        return result.ToString();
    }

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
        => obj is BinarySize size && Equals(size);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified
    /// <see cref="BinarySize"/> value.
    /// </summary>
    /// <param name="other">
    /// The <see cref="BinarySize"/> value to compare to this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(BinarySize other) => Value.Equals(other.Value);

    /// <summary>
    /// Compares this instance to a specified <see cref="BinarySize"/> instance and returns an
    /// indication of their relative values.
    /// </summary>
    /// <param name="other">A <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <inheritdoc/>
    /// </returns>
    public int CompareTo(BinarySize other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is BinarySize size)
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
            fixed (BinarySize* self = &this)
            {
                *self = newValue;
            }
        }
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
        writer.WriteString(ToString(null, CultureInfo.InvariantCulture));
    }

    private static long ParseUnit(ref ReadOnlySpan<char> value, IFormatProvider? provider, BinarySizeOptions options)
    {
        var unitInfo = GetUnitInfo(provider);
        var compareInfo = provider is CultureInfo culture ? culture.CompareInfo : CultureInfo.CurrentCulture.CompareInfo;
        value = value.TrimEnd();
        _ = SpanExtensions.TrimSuffix(ref value, unitInfo.ShortByte, compareInfo, CompareOptions.IgnoreCase)
            || SpanExtensions.TrimSuffix(ref value, unitInfo.ShortBytes, compareInfo, CompareOptions.IgnoreCase);

        var withConnector = value;
        SpanExtensions.TrimSuffix(ref value, unitInfo.ShortConnector, compareInfo, CompareOptions.IgnoreCase);

        var useDecimal = options.HasFlag(BinarySizeOptions.UseIecStandard);
        long factor = 1;
        if (!(CheckUnit(ref value, unitInfo.ShortKibi, compareInfo, Kibi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortMebi, compareInfo, Mebi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortGibi, compareInfo, Gibi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortTebi, compareInfo, Tebi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortPebi, compareInfo, Pebi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortExbi, compareInfo, Exbi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortKilo, compareInfo, useDecimal ? Kilo : Kibi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortMega, compareInfo, useDecimal ? Mega : Mebi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortMega, compareInfo, useDecimal ? Mega : Mebi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortGiga, compareInfo, useDecimal ? Giga : Gibi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortTera, compareInfo, useDecimal ? Tera : Tebi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortPeta, compareInfo, useDecimal ? Peta : Pebi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortExa, compareInfo, useDecimal ? Exa : Exbi, ref factor)
            || CheckUnit(ref value, unitInfo.ShortDecimalKilo, compareInfo, useDecimal ? Kilo : Kibi, ref factor)))
        {
            // Don't remove the connector if there was no recognized character before it.
            value = withConnector;
        }

        return factor;
    }

    private static bool CheckUnit(ref ReadOnlySpan<char> value, string unit, CompareInfo info, long factor, ref long factorResult)
    {
        if (SpanExtensions.TrimSuffix(ref value, unit, info, CompareOptions.IgnoreCase))
        {
            factorResult = factor;
            return true;
        }

        return false;
    }

    private static SuffixInfo TrimFormatSuffix(ReadOnlySpan<char> value)
    {
        SuffixInfo result = new()
        {
            Trimmed = value.TrimEnd(),
            Factor = 1,
        };

        result.Trailing = value.Slice(result.Trimmed.Length);
        if (result.Trimmed.Length == 0)
        {
            return result;
        }

        // Suffix can use B.
        char ch = result.Trimmed[result.Trimmed.Length - 1];
        if (ch is 'B' or 'b')
        {
            result.Trimmed = result.Trimmed.Slice(0, result.Trimmed.Length - 1);
            result.HasByteChar = true;
        }

        if (result.Trimmed.Length == 0)
        {
            return result;
        }

        var index = result.Trimmed.Length - 1;
        ch = result.Trimmed[index];

        // The 'i' is only counted as an IEC char if there's a valid scale prefix before it.
        if (result.Trimmed.Length > 1 && ch is 'I' or 'i')
        {
            result.HasIecChar = true;
            --index;
        }

        ch = result.Trimmed[index];
        var prefixes = _scalingChars.AsSpan();
        if (result.HasIecChar)
        {
            // Force the use of binary regardless of case if 'i' is present.
            ch = char.ToUpperInvariant(ch);
        }

        var scaleIndex = prefixes.IndexOf(ch);
        if (scaleIndex < 0)
        {
            // No scale prefix before the 'i', so don't count it as an IEC char.
            result.HasIecChar = false;
        }
        else
        {
            result.Trimmed = result.Trimmed.Slice(0, index);
            result.Factor = _scalingFactors[scaleIndex];
        }

        // Remove any whitespace between the number and the unit.
        var trimmed = result.Trimmed.TrimEnd();
        result.Whitespace = result.Trimmed.Slice(trimmed.Length);
        result.Trimmed = trimmed;
        return result;
    }

    private long DetermineAutomaticScalingFactor(long autoFactor)
    {
        // Check all factors except the automatic ones.
        var (allowRounding, useDecimal) = autoFactor switch
        {
            AutoFactor => (false, false),
            ShortestFactor => (true, false),
            DecimalAutoFactor => (false, true),
            DecimalShortestFactor => (true, true),
            _ => throw new ArgumentException(null, nameof(autoFactor)), // Should never be reached
        };

        var factors = _scalingFactors.AsSpan(0, _scalingChars.Length - 4);
        if (useDecimal)
        {
            factors = factors.Slice(factors.Length / 2);
        }
        else
        {
            factors = factors.Slice(0, factors.Length / 2);
        }

        // Use the absolute value to select the correct unit for negative numbers.
        var value = Math.Abs(Value);
        for (int index = 0; index < factors.Length; ++index)
        {
            var factor = factors[index];
            if (value >= factor && (allowRounding || value % factor == 0))
            {
                return factor;
            }
        }

        return 1;
    }

    private SuffixInfo ParseFormat(ReadOnlySpan<char> format, out decimal scaledValue)
    {
        SuffixInfo suffix;
        if (format.IsEmpty || format == "G".AsSpan())
        {
            suffix = new()
            {
                Factor = AutoFactor,
                Whitespace = " ".AsSpan(),
                HasIecChar = true,
                HasByteChar = true,
            };
        }
        else
        {
            suffix = TrimFormatSuffix(format);
        }

        if (suffix.Factor < 0)
        {
            suffix.Factor = DetermineAutomaticScalingFactor(suffix.Factor);
        }

        // Don't include the 'i' if there's no scale prefix.
        suffix.HasIecChar = suffix.HasIecChar && suffix.Factor > 1;
        scaledValue = Value / (decimal)suffix.Factor;
        return suffix;
    }

    private static BinarySize FromScale(double value, long scale)
    {
        if (double.IsNaN(value))
        {
            throw new ArgumentException(Properties.Resources.ValueIsNaN, nameof(value));
        }

        return (BinarySize)checked((long)(value * scale));
    }

    private static void ValidateOptions(BinarySizeOptions options)
    {
        const BinarySizeOptions invalid = ~BinarySizeOptions.UseIecStandard;
        if ((options & invalid) != 0) 
        {
            throw new ArgumentException(Properties.Resources.InvalidOptions, nameof(options));
        }
    }

    private static BinaryUnitInfo GetUnitInfo(IFormatProvider? provider)
    {
        // Only check current culture if provider was not specified. If it was but has no unit info,
        // we always use the invariant info.
        provider ??= CultureInfo.CurrentCulture;
        return (BinaryUnitInfo?)provider.GetFormat(typeof(BinaryUnitInfo)) ?? BinaryUnitInfo.InvariantInfo;
    }

    private static string? GetUnitScale(SuffixInfo suffix, BinaryUnitInfo unitInfo)
    {
        if (suffix.HasIecChar)
        {
            return suffix.Factor switch
            {
                Kilo or Kibi => unitInfo.ShortKibi,
                Mega or Mebi => unitInfo.ShortMebi,
                Giga or Gibi => unitInfo.ShortGibi,
                Tera or Tebi => unitInfo.ShortTebi,
                Peta or Pebi => unitInfo.ShortPebi,
                Exa or Exbi => unitInfo.ShortExbi,
                _ => null,
            };
        }
        else
        {
            return suffix.Factor switch
            {
                Kilo => unitInfo.ShortDecimalKilo,
                Kibi => unitInfo.ShortKilo,
                Mega or Mebi => unitInfo.ShortMega,
                Giga or Gibi => unitInfo.ShortGiga,
                Tera or Tebi => unitInfo.ShortTera,
                Peta or Pebi => unitInfo.ShortPeta,
                Exa or Exbi => unitInfo.ShortExa,
                _ => null,
            };
        }
    }
}
