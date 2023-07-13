using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

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
///   KiB", and "1 MB" == "1 MiB" == 1048576 bytes, and so forth. This behavior can be changed using
///   the see <see cref="BinarySizeOptions"/> enumeration, and formatting strings used with the
///   <see cref="ToString(string?, IFormatProvider?)"/> method.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
[TypeConverter(typeof(BinarySizeConverter))]
public readonly partial struct BinarySize : IEquatable<BinarySize>, IComparable<BinarySize>, IComparable, IFormattable
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
        public char ScaleChar { get; set; }
        public bool HasIecChar { get; set; }
        public bool HasByteChar { get; set; }
    }

    #endregion

    /// <summary>
    /// The size of a kibibyte, 1024 bytes.
    /// </summary>
    public const long Kibi = 1024L;

    /// <summary>
    /// The size of a mebibyte, 1,048,576 bytes.
    /// </summary>
    public const long Mebi = 1024L * Kibi;

    /// <summary>
    /// The size of a gibibyte, 1,073,741,824 bytes.
    /// </summary>
    public const long Gibi = 1024L * Mebi;

    /// <summary>
    /// The size of a tebibyte, 1,099,511,627,776 bytes.
    /// </summary>
    public const long Tebi = 1024L * Gibi;

    /// <summary>
    /// The size of a pebibyte, 1,125,899,906,842,624 bytes.
    /// </summary>
    public const long Pebi = 1024L * Tebi;

    /// <summary>
    /// The size of an exbibyte, 1,152,921,504,606,846,976 bytes.
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
    private const char IecChar = 'i';
    private const char ByteChar = 'B';

    private static readonly char[] _scalingChars =   new[] { 'E',  'P',  'T',  'G',  'M',  'K',
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
    public static readonly BinarySize MinValue = long.MinValue;

    /// <summary>
    /// Represents the maximum <see cref="BinarySize"/> value.
    /// </summary>
    public static readonly BinarySize MaxValue = long.MaxValue;

    /// <summary>
    /// Gets the number of bytes represented by this instance.
    /// </summary>
    /// <value>
    /// The value of this instance, in bytes.
    /// </value>
    public long Value { get; }

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
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units in
    /// <paramref name="s"/> are interpreted.
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
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> is not representable as a <see cref="long"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "MB",
    ///   "GB", "TB", or "PB".
    /// </para>
    /// <para>
    ///   The "B" itself is optional, and the input may also use IEC binary units such as "KiB" or
    ///   "MiB", etc. The case of the units does not matter.
    /// </para>
    /// <para>
    ///   The size of 1 KiB always equals 1024 bytes, and 1 MiB is  1,048,576 bytes, and so forth.
    /// </para>
    /// <para>
    ///   By default, this method also treats 1 KB as 1,024 bytes, identical to 1 KiB, and 1 MB
    ///   equals 1 MiB equals 1,048,576 bytes, and so forth.
    /// </para>
    /// <para>
    ///   When <paramref name="options"/> includes the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/>
    ///   flag, the SI prefixes (without an 'i') are treated as decimal, so that 1 kB equals 1,000
    ///   bytes, 1 MB == 1,000,000 bytes, and so forth. The IEC prefixes are unchanged, and remain
    ///   based on powers of two.
    /// </para>
    /// <para>
    ///   The value of <paramref name="options"/> will not affect the output when converting back
    ///   to a string, so use an appropriate format string with the <see cref="ToString(string?, IFormatProvider?)"/>
    ///   method if you wish to use decimal prefixes there as well.
    /// </para>
    /// </remarks>
    public static BinarySize Parse(ReadOnlySpan<char> s, BinarySizeOptions options = BinarySizeOptions.Default, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        if (s.IsEmpty)
        {
            return Zero;
        }

        var result = TrimSuffix(s, options);
#if NET6_0_OR_GREATER
        var size = decimal.Parse(result.Trimmed, style, provider);
#else
        var size = decimal.Parse(result.Trimmed.ToString(), style, provider);
#endif

        return new BinarySize(checked((long)(size * result.Factor)));
    }

    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "MB",
    ///   "GB", "TB", "PB", or "EB".
    /// </para>
    /// <para>
    ///   The "B" itself is optional, and the input may also use IEC binary units such as "KiB" or
    ///   "MiB", etc. The case of the units does not matter.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so forth. To use the IEC definition where SI prefixes
    ///   are treated as decimal, use the <see cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
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
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units in
    /// <paramref name="s"/> are interpreted.
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
    /// <remarks>
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out BinarySize result)
    {
        if (s.IsEmpty)
        {
            result = Zero;
            return true;
        }

        var trim = TrimSuffix(s, options);
#if NET6_0_OR_GREATER
        var success = decimal.TryParse(trim.Trimmed, style, provider, out var size);
#else
        var success = decimal.TryParse(trim.Trimmed.ToString(), style, provider, out var size);
#endif

        if (!success)
        {
            result = default;
            return false;
        }

        try
        {
            result = new BinarySize(checked((long)(size * trim.Factor)));
            return true;
        }
        catch (OverflowException)
        {
            // I couldn't find a good way to handle overflow without exceptions.
            result = default;
            return false;
        }
    }

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)"/>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "MB",
    ///   "GB", "TB", "PB", or "EB".
    /// </para>
    /// <para>
    ///   The "B" itself is optional, and the input may also use IEC binary units such as "KiB" or
    ///   "MiB", etc. The case of the units does not matter.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so forth. To use the IEC definition where SI prefixes
    ///   are treated as decimal, use the <see cref="TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)"/>
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
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units in
    /// <paramref name="s"/> are interpreted.
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

    /// <inheritdoc cref="TryParse(string?, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)"/>
    /// <remarks>
    /// <para>
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "MB",
    ///   "GB", "TB", or "PB".
    /// </para>
    /// <para>
    ///   The "B" itself is optional, and the input may also use IEC binary units such as "KiB" or
    ///   "MiB", etc. The case of the units does not matter.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so forth. To use the IEC definition where SI prefixes
    ///   are treated as decimal, use the <see cref="TryParse(string, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)"/>
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
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units in
    /// <paramref name="s"/> are interpreted.
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
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> is not representable as a <see cref="long"/>.
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
    ///   The input must contain a number, followed by one of the following units: "B", "KB", "MB",
    ///   "GB", "TB", "PB", or "EB".
    /// </para>
    /// <para>
    ///   The "B" itself is optional, and the input may also use IEC binary units such as "KiB" or
    ///   "MiB", etc. The case of the units does not matter.
    /// </para>
    /// <para>
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so forth. To use the IEC definition where SI prefixes
    ///   are treated as decimal, use the <see cref="Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
    ///   method.
    /// </para>
    /// </remarks>
    public static BinarySize Parse(string s, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, NumberStyles.Number, provider);

#if NET6_0_OR_GREATER

    /// <inheritdoc/>
    /// <remarks>
    /// <inheritdoc cref="ToString(string?, IFormatProvider?)"/>
    /// </remarks>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var suffix = ParseFormat(format, out var scaledValue);
        if (!scaledValue.TryFormat(destination, out charsWritten, suffix.Trimmed, provider))
        {
            return false;
        }

        return destination.TryAppend(ref charsWritten, suffix.Whitespace) &&
            (suffix.ScaleChar == '\0' || destination.TryAppend(ref charsWritten, suffix.ScaleChar)) &&
            (!suffix.HasIecChar || destination.TryAppend(ref charsWritten, IecChar)) &&
            (!suffix.HasByteChar || destination.TryAppend(ref charsWritten, ByteChar)) &&
            destination.TryAppend(ref charsWritten, suffix.Trailing);
    }

#endif

    /// <inheritdoc/>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The value of <paramref name="format"/> must be either a byte unit, or a byte unit that follows either a
    ///   <see href="https://learn.microsoft.com/dotnet/standard/base-types/standard-numeric-format-strings">standard numeric format string</see>
    ///   or a <see href="https://learn.microsoft.com/dotnet/standard/base-types/custom-numeric-format-strings">custom numeric format string</see>.
    /// </para>
    /// <para>
    ///   The byte unit can be one of the following:
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
    ///     <term><em>empty</em>, or G</term>
    ///     <description>
    ///       This is the general format specifier. The value will be formatted using the largest
    ///       binary prefix in which it can be represented without fractions, the suffix "iB", and a
    ///       space before the unit. For example, "2 TiB", or "512 B". It is equivalent to using
    ///       " AiB".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>K[i][B], M[i][B], G[i][B], T[i][B], P[i][B], E[i][B]</term>
    ///     <description>
    ///       The output will be formatted as kibibytes, mebibytes, gibibytes, tebibytes, pebibytes,
    ///       or exibytes, with an optional 'i' for IEC units, and an optional 'B'. For these units,
    ///       SI prefixes without the 'i' character are still treated as binary prefixes, so 1 KB
    ///       equals 1024 bytes, and so forth. For example, "1.5KiB", or "2Mi" or "42TB". 
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>k[B], m[B], g[B], t[B], p[B], e[B]</term>
    ///     <description>
    ///       With a lower case prefix, the output will be formatted as decimal kilobytes,
    ///       megabytes, gigabytes, terabytes, petabytes, or exabytes, followed by an optional 'B'.
    ///       In this case, 1 KB equals 1000 bytes, and so forth. The prefix will be a capital
    ///       letter in the output, except for "k" which should be lower case as an SI prefix.
    ///       For example, "1.5kB", or "2M" or "42T".
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>A[i][B] or a[B]</term>
    ///     <description>
    ///       Automatically select the largest prefix in which the value can be represented without
    ///       fractions, optionally followed by an 'i' and/or a 'B'. The former variant uses binary
    ///       units, while the latter uses decimal. For example, 1,572,864 bytes would be formatted
    ///       as "1536KiB", "1536Ki", "1536KB", or "1536K"; if using decimal it would be "1572864B",
    ///       since there is no higher factor.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term>S[i][B] or s[B]</term>
    ///     <description>
    ///       Automatically select the largest prefix in which the value is at least 1, optionally
    ///       followed by an 'i' and/or a 'B'. The former variant uses binary units, while the
    ///       latter uses decimal. The output may contain decimals. For example, 1,572,864 bytes
    ///       would be formatted as "1.5MiB", "1.5Mi", "1.5MB" or "1.5M"; if using decimal it would
    ///       be "1.572864MB".
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    ///   Any of the above unit formats, except the general format specified, can be combined with a
    ///   numeric format string; for example, "#,##0.# SiB".
    /// </para>
    /// <para>
    ///   If a unit is preceded by white space, this will be preserved in the output. For example,
    ///   " KB" can be used to format the value 512 as "0.5 KB".
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
        if (suffix.ScaleChar != '\0')
        {
            result.Append(suffix.ScaleChar);
            if (suffix.HasIecChar)
            {
                result.Append(IecChar);
            }
        }

        if (suffix.HasByteChar)
        {
            result.Append(ByteChar);
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
    ///   Default formatting is equivalent to using the "AiB" format string with the
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

    private static SuffixInfo TrimSuffix(ReadOnlySpan<char> value, BinarySizeOptions? options)
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
        if (options is BinarySizeOptions o)
        {
            // This is not a format string; trim off the auto prefixes.
            prefixes = prefixes.Slice(0, prefixes.Length - 4);
            if (o.HasFlag(BinarySizeOptions.UseIecStandard) && !result.HasIecChar)
            {
                // No 'i' and IEC mode, so treat it as decimal.
                ch = char.ToLowerInvariant(ch);
            }
            else
            {
                // Default mode or an 'i', so treat it as binary.
                ch = char.ToUpperInvariant(ch);
            }
        }
        else if (result.HasIecChar)
        {
            // For format strings, force the use of binary regardless of case if 'i' is present.
            ch = char.ToUpperInvariant(ch);
        }

        var scaleIndex = prefixes.IndexOf(ch);
        if (scaleIndex < 0)
        {
            // No scale prefix before the 'i', so don't count it as an IEC char.
            result.HasIecChar = false;
            return result;
        }

        result.Trimmed = result.Trimmed.Slice(0, index);
        result.Factor = _scalingFactors[scaleIndex];
        result.ScaleChar = prefixes[scaleIndex];

        // Always use upper case prefix when formatting, except if decimal kilo.
        if (options == null && result.Factor != Kilo)
        {
            result.ScaleChar = char.ToUpperInvariant(result.ScaleChar);
        }

        // Remove any whitespace between the number and the unit.
        var trimmed = result.Trimmed.TrimEnd();
        result.Whitespace = result.Trimmed.Slice(trimmed.Length);
        result.Trimmed = trimmed;
        return result;
    }

    private (long, char) DetermineAutomaticScalingFactor(long autoFactor)
    {
        // Check all factors except the automatic ones.
        var factors = _scalingChars.AsSpan(0, _scalingChars.Length - 4);
        var (allowRounding, useDecimal) = autoFactor switch
        {
            AutoFactor => (false, false),
            ShortestFactor => (true, false),
            DecimalAutoFactor => (false, true),
            DecimalShortestFactor => (true, true),
            _ => throw new ArgumentException(null, nameof(autoFactor)), // Should never be reached
        };

        factors = useDecimal
            ? factors.Slice(factors.Length / 2)
            : factors.Slice(0, factors.Length / 2);

        for (int index = 0; index < factors.Length; ++index)
        {
            var factor = _scalingFactors[index];
            if (Value >= factor && (allowRounding || Value % factor == 0))
            {
                return (factor, _scalingChars[index]);
            }
        }

        return (1, '\0');
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
            suffix = TrimSuffix(format, null);
        }

        if (suffix.Factor < 0)
        {
            (suffix.Factor, suffix.ScaleChar) = DetermineAutomaticScalingFactor(suffix.Factor);
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

        return checked((long)(value * scale));
    }
}
