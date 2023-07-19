using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ookii;

/// <summary>
/// Provides a wrapper around the <see cref="BinarySize"/> structure that always uses
/// <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> when parsing values
/// from a string.
/// </summary>
/// <remarks>
/// <para>
///   Use this type if you need the parsing behavior to be different, and cannot pass a custom
///   <see cref="BinarySizeOptions"/> value. For example, when the value is part of a serialized
///   data type.
/// </para>
/// <para>
///   This structure only provides parsing and formatting functionality. For all other functions
///   of the <see cref="BinarySize"/> structure, access the <see cref="Value"/> property.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
[TypeConverter(typeof(IecBinarySizeConverter))]
[JsonConverter(typeof(IecBinarySizeJsonConverter))]
[Serializable]
public readonly struct IecBinarySize : IFormattable, IXmlSerializable
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
    /// <summary>
    /// Parses a span of characters into an <see cref="IecBinarySize"/> structure.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static IecBinarySize Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => BinarySize.Parse(s, BinarySizeOptions.UseIecStandard, NumberStyles.Number, provider);

    /// <inheritdoc cref="BinarySize.Parse(string, IFormatProvider?)"/>
    /// <summary>
    /// Parses a string into an <see cref="IecBinarySize"/> structure.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static IecBinarySize Parse(string s, IFormatProvider? provider)
        => Parse(s.AsSpan(), provider);

    /// <inheritdoc cref="BinarySize.TryParse(ReadOnlySpan{char}, out BinarySize)"/>
    /// <summary>
    /// Tries to parse a span of characters into an <see cref="IecBinarySize"/> structure.
    /// </summary>
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
    /// <summary>
    /// Tries to parse a string into an <see cref="IecBinarySize"/> structure.
    /// </summary>
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
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method. Format strings with upper-case unit letters will still use powers of two for SI
    ///   units; you must use lower-case unit letters to format with powers of ten.
    /// </para>
    /// </remarks>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => Value.ToString(format, formatProvider);

    /// <inheritdoc cref="BinarySize.ToString()"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.ToString()" qualifyHint="true"/>
    ///   method, with the same default format.
    /// </para>
    /// </remarks>
    public override string ToString() => Value.ToString();

#if NET6_0_OR_GREATER

    /// <inheritdoc cref="BinarySize.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="BinarySize.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)" qualifyHint="true"/>
    ///   method. Format strings with upper-case unit letters will still use powers of two for SI
    ///   units; you must use lower-case unit letters to format with powers of ten.
    /// </para>
    /// </remarks>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format, provider);

#endif

    XmlSchema? IXmlSerializable.GetSchema() => null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        var stringValue = reader.ReadElementContentAsString();
        var newValue = Parse(stringValue, CultureInfo.InvariantCulture);
        unsafe
        {
            // I don't like this, but it's the only way to use a readonly struct with XML
            // serialization as far as I can tell.
            fixed (IecBinarySize* self = &this)
            {
                *self = newValue;
            }
        }
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
        writer.WriteString(ToString(null, CultureInfo.InvariantCulture));
    }

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

    /// <summary>
    /// Performs an explicit conversion from <see cref="IecBinarySize"/> to <see cref="long"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The value of the <see cref="Value"/> property.
    /// </returns>
    public static explicit operator long(IecBinarySize value) => value.Value.Value;

    /// <summary>
    /// Performs an explicit conversion from <see cref="long"/> to <see cref="IecBinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// An <see cref="IecBinarySize"/> where the <see cref="Value"/> property equals
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator IecBinarySize(long value) => new(value);
}
