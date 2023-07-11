using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
///   This structure uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1 MB"
///   == "1 MiB" == 1048576 bytes, and so forth. Treating the regular SI prefixes as decimal unit
///   prefixes is not currently supported.
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

    private const long AutoFactor = -1;
    private const long ShortestFactor = -2;

    private static readonly char[] _scalingChars =   new[] { 'P',  'T',  'G',  'M',  'K', 'A',        'S' };
    private static readonly long[] _scalingFactors = new[] { Pebi, Tebi, Gibi, Mebi, Kibi, AutoFactor, ShortestFactor };

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
    ///   This method uses the definition that "1 KB" == 1024 bytes, identical to "1 KiB", and "1
    ///   MB" == "1 MiB" == 1048576 bytes, and so forth. Treating the regular SI prefixes as decimal
    ///   unit prefixes is not currently supported.
    /// </para>
    /// </remarks>
    public static BinarySize Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        if (s.IsEmpty)
        {
            return Zero;
        }

        var result = TrimSuffix(s, false);
#if NET6_0_OR_GREATER
        var size = decimal.Parse(result.Trimmed, style, provider);
#else
        var size = decimal.Parse(result.Trimmed.ToString(), style, provider);
#endif

        return new BinarySize(checked((long)(size * result.Factor)));
    }

    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)"/>
    public static BinarySize Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => Parse(s, NumberStyles.Number, provider);

    /// <summary>
    /// Tries to parse a span of characters into a <see cref="BinarySize"/> structure.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
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
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
    {
        if (s.IsEmpty)
        {
            result = Zero;
            return true;
        }

        var trim = TrimSuffix(s, false);
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

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?, out BinarySize)"/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BinarySize result)
        => TryParse(s, NumberStyles.Number, provider, out result);

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?, out BinarySize)"/>
    public static bool TryParse(ReadOnlySpan<char> s, out BinarySize result)
        => TryParse(s, NumberStyles.Number, null, out result);

    /// <summary>
    /// Tries to parse a string into a <see cref="BinarySize"/> structure.
    /// </summary>
    /// <param name="s">The string to parse.</param>
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
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
#else
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
#endif
    {
        if (s == null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), style, provider, out result);
    }

    /// <inheritdoc cref="TryParse(string?, NumberStyles, IFormatProvider?, out BinarySize)"/>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BinarySize result)
#else
    public static bool TryParse(string? s, IFormatProvider? provider, out BinarySize result)
#endif
        => TryParse(s, NumberStyles.Number, provider, out result);

    /// <inheritdoc cref="TryParse(string?, NumberStyles, IFormatProvider?, out BinarySize)"/>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, out BinarySize result)
#else
    public static bool TryParse(string? s, out BinarySize result)
#endif
        => TryParse(s, NumberStyles.Number, null, out result);

    /// <summary>
    /// Parses a string into a <see cref="BinarySize"/> structure.
    /// </summary>
    /// <param name="s">The string to parse.</param>
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
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)"/>
    /// </remarks>
    public static BinarySize Parse(string s, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        if (s == null)
        {
            throw new ArgumentNullException(nameof(s));
        }

        return Parse(s.AsSpan(), style, provider);
    }

    /// <inheritdoc cref="Parse(string, NumberStyles, IFormatProvider?)"/>
    public static BinarySize Parse(string s, IFormatProvider? provider)
        => Parse(s, NumberStyles.Number, provider);

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

        if (!suffix.Whitespace.TryCopyTo(destination.Slice(charsWritten)))
        {
            return false;
        }

        charsWritten += suffix.Whitespace.Length;
        if (suffix.ScaleChar != '\0')
        {
            if (destination.Length <= charsWritten)
            {
                return false;
            }

            destination[charsWritten] = suffix.ScaleChar;
            ++charsWritten;
        }

        if (!suffix.Trailing.TryCopyTo(destination.Slice(charsWritten)))
        {
            return false;
        }

        charsWritten += suffix.Trailing.Length;
        return true;
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
    ///   The byte unit is a binary or SI unit prefix, optionally followed by a "B". The prefix can
    ///   be any of "K", "M", "G", "T", or "P", optionally followed by an "i" to use IEC prefixes.
    ///   If any of these prefixes is present, the value will be shown using the appropriate
    ///   quantity, possibly using a floating point value if the value is not a whole number of
    ///   those units.
    /// </para>
    /// <para>
    ///   If the format is only a byte unit, it can be preceded by white space which will be
    ///   preserved in the output. If there is no white space before the unit, the unit is placed
    ///   immediately after the number.
    /// </para>
    /// <para>
    ///   For example, "MB", "PiB", " GB", "0.0 K" and "#,###.# Ki" are all valid formats, as is any
    ///   numeric format string by itself, such as "0.0", in which case the value is formatted as
    ///   a plain number of bytes without a unit.
    /// </para>
    /// <para>
    ///   Instead of a regular unit prefix, you can also use the "A" and "S" prefixes to
    ///   automatically determine the largest prefix that can be applied. With "A" (automatic),
    ///   the largest prefix that can represent the value as a whole number is used. With "S"
    ///   (shortest), the largest prefix where the quantity is at least one will be used, using
    ///   floating-point representation is necessary.
    /// </para>
    /// <para>
    ///   For example, a value of 1,572,864 with the format string "AB" would be formatted as
    ///   "1536KB", and with the format string "SB" as "1.5MB".
    /// </para>
    /// <para>
    ///   You can use "Ai" or "Si", optionally followed by a B, to use IEC binary prefixes while
    ///   automatically choosing the prefix. The "i" character will not be present in the output
    ///   if the chosen unit is bytes, without a prefix.
    /// </para>
    /// <para>
    ///   The case of the characters in the byte unit of the format string will be preserved in
    ///   the output.
    /// </para>
    /// <note>
    ///   The general format specifier "G" is supported, as required by the <see cref="IFormattable"/>
    ///   interface, and has the same effect as " AiB". To format a value using the byte unit "G",
    ///   use the specifier "GG".
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

    private static SuffixInfo TrimSuffix(ReadOnlySpan<char> value, bool allowAuto)
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
            result.Trailing = value.Slice(result.Trimmed.Length);
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
        if (!allowAuto)
        {
            prefixes = prefixes.Slice(0, prefixes.Length - 2);
        }

        var scaleIndex = prefixes.IndexOf(char.ToUpperInvariant(ch));
        if (scaleIndex < 0)
        {
            // No scale prefix before the 'i', so don't count it as an IEC char.
            result.HasIecChar = false;
            return result;
        }

        result.Trimmed = result.Trimmed.Slice(0, index);
        result.Trailing = value.Slice(index + 1);
        result.Factor = _scalingFactors[scaleIndex];
        result.ScaleChar = ch;

        // Remove any whitespace between the number and the unit.
        var trimmed = result.Trimmed.TrimEnd();
        result.Whitespace = result.Trimmed.Slice(trimmed.Length);
        result.Trimmed = trimmed;
        return result;
    }

    private (long, char) DetermineAutomaticScalingFactor(bool allowRounding)
    {
        // Check all factors except the automatic ones.
        for (int index = 0; index < _scalingFactors.Length - 2; ++index)
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
                Trailing = "iB".AsSpan(),
                HasIecChar = true,
            };
        }
        else
        {
            suffix = TrimSuffix(format, true);
        }

        if (suffix.Factor < 0)
        {
            var (factor, scaleChar) = DetermineAutomaticScalingFactor(suffix.Factor == ShortestFactor);
            suffix.Factor = factor;
            if (char.IsLower(suffix.ScaleChar))
            {
                suffix.ScaleChar = char.ToLowerInvariant(scaleChar);
            }
            else
            {
                suffix.ScaleChar = scaleChar;
            }
        }

        // Don't include the 'i' if there's no scale prefix.
        if (suffix.Factor == 1 && suffix.HasIecChar)
        {
            suffix.Trailing = suffix.Trailing.Slice(1);
        }

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
