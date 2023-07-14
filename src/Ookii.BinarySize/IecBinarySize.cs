using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Ookii;

/// <summary>
/// Provides a wrapper around the <see cref="BinarySize"/> structure that uses
/// <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> by default when parsing values
/// from a string.
/// </summary>
/// <remarks>
/// <para>
///   Use this type if you need the default conversion behavior to be different, and cannot use a
///   <see cref="TypeConverterAttribute"/> attribute to use the <see cref="BinarySizeIecConverter"/>
///   class.
/// </para>
/// <para>
///   This structure only provides parsing and formatting functionality. For all other functions
///   of the <see cref="BinarySize"/> structure, access the <see cref="Value"/> property.
/// </para>
/// </remarks>
[TypeConverter(typeof(IecBinarySizeConverter))]
public readonly struct IecBinarySize : IFormattable
#if NET6_0_OR_GREATER
    , ISpanFormattable
#endif
#if NET7_0_OR_GREATER
    , ISpanParsable<IecBinarySize>
#endif
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IecBinarySize"/> structure.
    /// </summary>
    /// <param name="value">The size in bytes.</param>
    public IecBinarySize(long value)
    {
        Value = (BinarySize)value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IecBinarySize"/> structure.
    /// </summary>
    /// <param name="value">The size in bytes.</param>
    public IecBinarySize(BinarySize value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the <see cref="BinarySize"/> value that is wrapped by this instance.
    /// </summary>
    /// <value>
    /// The wrapped value.
    /// </value>
    public BinarySize Value { get; }

    /// <inheritdoc cref="BinarySize.Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static IecBinarySize Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => BinarySize.Parse(s, BinarySizeOptions.UseIecStandard, NumberStyles.Number, provider);

    /// <inheritdoc cref="BinarySize.Parse(string, IFormatProvider?)"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static IecBinarySize Parse(string s, IFormatProvider? provider)
        => Parse(s.AsSpan(), provider);

    /// <inheritdoc cref="BinarySize.TryParse(ReadOnlySpan{char}, out BinarySize)"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IecBinarySize result)
    {
        var success = BinarySize.TryParse(s, BinarySizeOptions.UseIecStandard, NumberStyles.Number, provider, out BinarySize size);
        result = size;
        return success;
    }

    /// <inheritdoc cref="BinarySize.TryParse(string, out BinarySize)"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.TryParse(string, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out IecBinarySize result)
#else
    public static bool TryParse(string? s, IFormatProvider? provider, out IecBinarySize result)
#endif
        => TryParse(s.AsSpan(), provider, out result);
        
    /// <inheritdoc cref="BinarySize.ToString(string?, IFormatProvider?)"/>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => Value.ToString(format, formatProvider);

    /// <inheritdoc cref="BinarySize.ToString()"/>
    public override string ToString() => Value.ToString();

#if NET6_0_OR_GREATER

    /// <inheritdoc cref="BinarySize.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format, provider);

#endif

    /// <summary>
    /// Performs an implicit conversion from <see cref="IecBinarySize"/> to <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The value of the <see cref="Value"/> property.
    /// </returns>
    public static implicit operator BinarySize(IecBinarySize value) => value.Value;

    /// <summary>
    /// Performs an implicit conversion from <see cref="BinarySize"/> to <see cref="IecBinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// An <see cref="IecBinarySize"/> where the <see cref="Value"/> property equals
    /// <paramref name="value"/>.
    /// </returns>
    public static implicit operator IecBinarySize(BinarySize value) => new(value);

}
