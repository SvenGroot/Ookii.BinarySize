using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ookii;

/// <summary>
/// Provides a wrapper around the <see cref="UBinarySize"/> structure that always uses
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
///   of the <see cref="UBinarySize"/> structure, access the <see cref="Value"/> property.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
[TypeConverter(typeof(UIecBinarySizeConverter))]
[JsonConverter(typeof(UIecBinarySizeJsonConverter))]
[Serializable]
[CLSCompliant(false)]
public readonly struct UIecBinarySize : IFormattable, IXmlSerializable
#if NET6_0_OR_GREATER
    , ISpanFormattable
#endif
#if NET7_0_OR_GREATER
    , ISpanParsable<UIecBinarySize>
#endif
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UIecBinarySize"/> structure.
    /// </summary>
    /// <param name="value">The size in bytes.</param>
    public UIecBinarySize(ulong value)
    {
        Value = (UBinarySize)value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIecBinarySize"/> structure.
    /// </summary>
    /// <param name="value">The size in bytes.</param>
    public UIecBinarySize(UBinarySize value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the <see cref="UBinarySize"/> value that is wrapped by this instance.
    /// </summary>
    /// <value>
    /// The wrapped value.
    /// </value>
    public UBinarySize Value { get; }

    /// <inheritdoc cref="UBinarySize.Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    /// <summary>
    /// Parses a span of characters into an <see cref="UIecBinarySize"/> structure.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="UBinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static UIecBinarySize Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => UBinarySize.Parse(s, BinarySizeOptions.UseIecStandard, NumberStyles.Number, provider);

    /// <inheritdoc cref="UBinarySize.Parse(string, IFormatProvider?)"/>
    /// <summary>
    /// Parses a string into an <see cref="UIecBinarySize"/> structure.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="UBinarySize.Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static UIecBinarySize Parse(string s, IFormatProvider? provider)
        => Parse(s.AsSpan(), provider);

    /// <inheritdoc cref="UBinarySize.TryParse(ReadOnlySpan{char}, out UBinarySize)"/>
    /// <summary>
    /// Tries to parse a span of characters into an <see cref="UIecBinarySize"/> structure.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="UBinarySize.TryParse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?, out UBinarySize)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UIecBinarySize result)
    {
        var success = UBinarySize.TryParse(s, BinarySizeOptions.UseIecStandard, NumberStyles.Number, provider, out UBinarySize size);
        result = size;
        return success;
    }

    /// <inheritdoc cref="UBinarySize.TryParse(string, out UBinarySize)"/>
    /// <summary>
    /// Tries to parse a string into an <see cref="UIecBinarySize"/> structure.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="UBinarySize.TryParse(string, BinarySizeOptions, NumberStyles, IFormatProvider?, out UBinarySize)" qualifyHint="true"/>
    ///   method with the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
    /// </para>
    /// </remarks>
#if NET6_0_OR_GREATER
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UIecBinarySize result)
#else
    public static bool TryParse(string? s, IFormatProvider? provider, out UIecBinarySize result)
#endif
        => TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc cref="UBinarySize.ToString(string?, IFormatProvider?)"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="UBinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method. Format strings with upper-case unit letters will still use powers of two for SI
    ///   units; you must use lower-case unit letters to format with powers of ten.
    /// </para>
    /// </remarks>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => Value.ToString(format, formatProvider);

    /// <inheritdoc cref="UBinarySize.ToString()"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="UBinarySize.ToString()" qualifyHint="true"/>
    ///   method, with the same default format.
    /// </para>
    /// </remarks>
    public override string ToString() => Value.ToString();

#if NET6_0_OR_GREATER

    /// <inheritdoc cref="UBinarySize.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
    /// <remarks>
    /// <para>
    ///   This method behaves identical to the <see cref="UBinarySize.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)" qualifyHint="true"/>
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
            fixed (UIecBinarySize* self = &this)
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
    /// Performs an implicit conversion from <see cref="UIecBinarySize"/> to <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The value of the <see cref="Value"/> property.
    /// </returns>
    public static implicit operator UBinarySize(UIecBinarySize value) => value.Value;

    /// <summary>
    /// Performs an implicit conversion from <see cref="UBinarySize"/> to <see cref="UIecBinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// An <see cref="UIecBinarySize"/> where the <see cref="Value"/> property equals
    /// <paramref name="value"/>.
    /// </returns>
    public static implicit operator UIecBinarySize(UBinarySize value) => new(value);

    /// <summary>
    /// Performs an explicit conversion from <see cref="UIecBinarySize"/> to <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The value of the <see cref="Value"/> property.
    /// </returns>
    public static explicit operator ulong(UIecBinarySize value) => value.Value.Value;

    /// <summary>
    /// Performs an explicit conversion from <see cref="ulong"/> to <see cref="UIecBinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// An <see cref="UIecBinarySize"/> where the <see cref="Value"/> property equals
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator UIecBinarySize(ulong value) => new(value);
}
