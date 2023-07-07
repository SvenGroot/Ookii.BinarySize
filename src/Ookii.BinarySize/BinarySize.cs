﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Ookii;

/// <summary>
/// Provides formatting, parsing and scaling for a value using binary units (e.g. MB).
/// </summary>
[TypeConverter(typeof(BinarySizeConverter))]
public readonly partial struct BinarySize : IEquatable<BinarySize>, IComparable<BinarySize>, IComparable, IFormattable
#if NET7_0_OR_GREATER
    , ISpanParsable<BinarySize>
#endif
{
    /// <summary>
    /// The size of a kilobyte, 1024 bytes.
    /// </summary>
    public const long Kibi = 1024L;
    /// <summary>
    /// The size of a megabyte, 1048576 bytes.
    /// </summary>
    public const long Mebi = 1024L * 1024L;
    /// <summary>
    /// The size of a gigabyte, 1073741824 bytes.
    /// </summary>
    public const long Gibi = 1024L * 1024L * 1024L;
    /// <summary>
    /// The size of a TeraByte, 1099511627776 bytes.
    /// </summary>
    public const long Tebi = 1024L * 1024L * 1024L * 1024L;
    /// <summary>
    /// The size of a PetaByte, 1125899906842624 bytes.
    /// </summary>
    public const long Pebi = 1024L * 1024L * 1024L * 1024L * 1024L;


    private static readonly char[] _numbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    private static readonly char[] _scalingChars = new[] { 'A', 'K', 'M', 'G', 'T', 'P' };
    private static readonly ReadOnlyMemory<char> _scalingCharsNoAuto = _scalingChars.AsMemory(1);
    private static readonly long[] _scalingFactors = new[] { Kibi, Mebi, Gibi, Tebi, Pebi };

    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySize"/> structure with the specified
    /// value.
    /// </summary>
    /// <param name="value">The size, in bytes.</param>
    public BinarySize(long value)
    {
        Value = value;
    }

    public static BinarySize Zero => default;

    /// <summary>
    /// Gets the value of this instance, in bytes.
    /// </summary>
    /// <value>
    /// The value of this instance, in bytes.
    /// </value>
    public long Value { get; }

    /// <summary>
    /// Gets the value of this instance in kilobytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional kilobytes (<see cref="Value"/> / <see cref="Kibi"/>).
    /// </value>
    public double AsKibi => Value / (double)Kibi;

    /// <summary>
    /// Gets the value of this instance in megabytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional megabytes (<see cref="Value"/> / <see cref="Mebi"/>)
    /// </value>
    public double AsMebi => Value / (double)Mebi;

    /// <summary>
    /// Gets the value of this instance in gigabytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional gigabytes (<see cref="Value"/> / <see cref="Gibi"/>)
    /// </value>
    public double AsGibi => Value / (double)Gibi;

    /// <summary>
    /// Gets the value of this instance in terabytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional terabytes (<see cref="Value"/> / <see cref="Tebi"/>)
    /// </value>
    public double AsTebi => Value / (double)Tebi;

    /// <summary>
    /// Gets the value of this instance in petabytes.
    /// </summary>
    /// <value>
    /// The value of this instance in whole and fractional petabytes (<see cref="Value"/> / <see cref="Pebi"/>)
    /// </value>
    public double AsPebi => Value / (double)Pebi;

    public static BinarySize Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        if (s.IsEmpty)
        {
            return Zero;
        }

        var factor = TrimSuffix(ref s);
#if NET6_0_OR_GREATER
        var size = decimal.Parse(s, style, provider);
#else
        var size = decimal.Parse(s.ToString(), style, provider);
#endif

        return new BinarySize(checked((long)(size * factor)));
    }

    public static BinarySize Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => Parse(s, NumberStyles.Number, provider);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
    {
        if (s.IsEmpty)
        {
            result = Zero;
            return true;
        }

        var factor = TrimSuffix(ref s);
#if NET6_0_OR_GREATER
        var success = decimal.TryParse(s, NumberStyles.Number, provider, out var size);
#else
        var success = decimal.TryParse(s.ToString(), NumberStyles.Number, provider, out var size);
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
            // I couldn't find a way to handle overflow without exceptions.
            result = default;
            return false;
        }
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BinarySize result)
        => TryParse(s, NumberStyles.Number, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, out BinarySize result)
        => TryParse(s, NumberStyles.Number, null, out result);

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

#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BinarySize result)
#else
    public static bool TryParse(string? s, IFormatProvider? provider, out BinarySize result)
#endif
        => TryParse(s, NumberStyles.Number, provider, out result);

#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, out BinarySize result)
#else
    public static bool TryParse(string? s, out BinarySize result)
#endif
        => TryParse(s, NumberStyles.Number, null, out result);

    public static BinarySize Parse(string value, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return Parse(value.AsSpan(), style, provider);
    }

    /// <summary>
    /// Converts the string representation of a byte size in a specified culture-specific format into a <see cref="BinarySize"/> structure.
    /// </summary>
    /// <param name="value">A string containing a number to convert. This string may use a suffix indicating a binary multiple (B, KB, KiB, K, MB, MiB, M, GB, GiB, G, TB, TiB, T, PB, PiB, or P).</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value" />. May be <see langword="null"/> to use the current culture.</param>
    /// <returns>A <see cref="BinarySize"/> instance that is the equivalent of <paramref name="value"/>.</returns>
    public static BinarySize Parse(string value, IFormatProvider? provider)
        => Parse(value, NumberStyles.Number, provider);

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    /// <remarks>
    /// <para>
    ///   The value of <paramref name="format"/> must be a string containing a numeric format string followed by a binary unit, or either one of both. If no numeric
    ///   format is present, the default is used. If no binary unit is specified, the raw value in bytes is used.
    /// </para>
    /// <para>
    ///   The first character of the binary suffix indicates the scaling factor. This can be one of the normal binary prefixes K, M, G, T, or P. The value A (auto) indicates that
    ///   the scaling factor should be automatically determined as the largest factor in which this value can be precisely represented with no decimals. The value S (short)
    ///   indicates that the scaling factor should be automatically determined as the largest possible scaling factor in which this value can be represented with the scaled
    ///   value being at least 1. Using S may lead to rounding so while this is appropriate for some display scenarios, it is not appropriate if the precise value must be preserved.
    /// </para>
    /// <para>
    ///   The binary prefix can be followed by either B or iB, which will be included in the unit of the output.
    /// </para>
    /// <para>
    ///   The casing of the binary unit will be preserved as in the format string. Any whitespace that surrounding the binary unit will be preserved.
    /// </para>
    /// </remarks>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return BinarySizeFormatter.Format(this, format, formatProvider);
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    /// <remarks>
    /// <para>
    ///   The value of <paramref name="format"/> must be a string containing a numeric format string followed by a binary unit, or either one of both. If no numeric
    ///   format is present, the default is used. If no binary unit is specified, the raw value in bytes is used.
    /// </para>
    /// <para>
    ///   The first character of the binary suffix indicates the scaling factor. This can be one of the normal binary prefixes K, M, G, T, or P. The value A (auto) indicates that
    ///   the scaling factor should be automatically determined as the largest factor in which this value can be precisely represented with no decimals. The value S (short)
    ///   indicates that the scaling factor should be automatically determined as the largest possible scaling factor in which this value can be represented with the scaled
    ///   value being at least 1. Using S may lead to rounding so while this is appropriate for some display scenarios, it is not appropriate if the precise value must be preserved.
    /// </para>
    /// <para>
    ///   The binary prefix can be followed by either B or iB to indicate the the unit formatting.
    /// </para>
    /// <para>
    ///   The casing of the binary unit will be preserved as in the format string. Any whitespace that surrounding the binary unit will be preserved.
    /// </para>
    /// </remarks>
    public string ToString(string? format)
    {
        return BinarySizeFormatter.Format(this, format, null);
    }


    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return BinarySizeFormatter.Format(this, null, null);
    }

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">The object to compare to this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> has the same value as this instance; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is BinarySize)
            return Equals((BinarySize)obj);
        else
            return false;
    }

    /// <summary>
    /// Returns the hash code for this <see cref="BinarySize"/>.
    /// </summary>
    /// <returns>The hash code for this <see cref="BinarySize"/>.</returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }


    #region IEquatable<ByteSize> Members

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="BinarySize"/> value.
    /// </summary>
    /// <param name="other">The <see cref="BinarySize"/> value to compare to this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(BinarySize other)
    {
        return object.Equals(Value, other.Value);
    }

    #endregion

    #region IComparable<ByteSize> Members

    /// <summary>
    /// Compares this instance to a specified <see cref="BinarySize"/> and returns an indication of their relative values.
    /// </summary>
    /// <param name="other">A <see cref="BinarySize"/> to compare.</param>
    /// <returns>Less than zero if this instance is less than <paramref name="other"/>, zero if this instance is equal to <paramref name="other"/>, or greater than zero if this instance is greater than <paramref name="other"/>.</returns>
    public int CompareTo(BinarySize other)
    {
        return Value.CompareTo(other.Value);
    }

    #endregion

    #region IComparable Members

    /// <summary>
    /// Compares this instance to a specified object and returns an indication of their relative values.
    /// </summary>
    /// <param name="obj">An object to compare.</param>
    /// <returns>Less than zero if this instance is less than <paramref name="obj"/>, zero if this instance is equal to <paramref name="obj"/>, or greater than zero if this instance is greater than <paramref name="obj"/> or <paramref name="obj"/> is <see langword="null"/>.</returns>
    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;
        else if (obj is BinarySize)
            return CompareTo((BinarySize)obj);
        else
            throw new ArgumentException("The specified value is not a ByteSize.", nameof(obj));
    }

    #endregion

    internal static long GetUnitScalingFactor(string unit)
    {
        return unit.ToUpperInvariant() switch
        {
            "B" => 1,
            "KB" or "KIB" or "K" => Kibi,
            "MB" or "MIB" or "M" => Mebi,
            "GB" or "GIB" or "G" => Gibi,
            "TB" or "TIB" or "T" => Tebi,
            "PB" or "PIB" or "P" => Pebi,
            _ => throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "Unrecognized unit {0}.", unit), nameof(unit)),
        };
    }

    private static long TrimSuffix(ref ReadOnlySpan<char> value)
    {
        value = value.TrimEnd();
        if (value.Length == 0)
        {
            return 1;
        }

        // Suffix can use B.
        if (value[value.Length - 1] is 'B' or 'b')
        {
            value = value.Slice(0, value.Length - 1);
        }

        if (value.Length == 0)
        {
            return 1;
        }

        // Suffix can use I only if it's preceded by another character.
        var index = value.Length - 1;
        if (value.Length > 1 && value[index] is 'I' or 'i')
        {
            --index;
        }

        var scaleIndex = _scalingCharsNoAuto.Span.IndexOf(value[index]);
        if (scaleIndex < 0)
        {
            return 1;
        }

        value = value.Slice(0, index);
        return _scalingFactors[scaleIndex];
    }
}
