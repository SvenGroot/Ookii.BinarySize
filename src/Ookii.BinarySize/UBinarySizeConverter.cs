using System.ComponentModel;
using System.Globalization;

namespace Ookii;

/// <summary>
/// Converts a <see cref="UBinarySize"/> object from one data type to another. Access this class
/// through the <see cref="TypeDescriptor"/> class.
/// </summary>
/// <remarks>
/// <para>
///   This <see cref="TypeConverter"/> is able to convert from a <see cref="string"/> to a
///   <see cref="UBinarySize"/>, using the same rules as the
///   <see cref="UBinarySize.Parse(string, IFormatProvider?)" qualifyHint="true"/> method.
/// </para>
/// <para>
///   It can also convert from a <see cref="UBinarySize"/> to a <see cref="string"/> using the
///   default format used by the <see cref="UBinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
///   method. It also supports conversion from a <see cref="UBinarySize"/> to any primitive type,
///   and between <see cref="UBinarySize"/> and <see cref="UIecBinarySize"/>.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
public class UBinarySizeConverter : TypeConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UBinarySizeConverter"/> class.
    /// </summary>
    public UBinarySizeConverter()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UBinarySizeConverter"/> class with the specified
    /// options.
    /// </summary>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units
    /// are interpreted when converting from a string.
    /// </param>
    /// <remarks>
    /// <para>
    ///   When converting from a string, the <paramref name="options"/> will be passed to the
    ///   <see cref="UBinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// </remarks>
    public UBinarySizeConverter(BinarySizeOptions options)
    {
        Options = options;
    }

    /// <summary>
    /// Gets a value that indicates how units are interpreted when converting from a string.
    /// </summary>
    /// <value>
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values.
    /// </value>
    public BinarySizeOptions Options { get; }

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || sourceType == typeof(UIecBinarySize) || sourceType == typeof(BinarySize)
        || sourceType == typeof(IecBinarySize) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => (destinationType != null && destinationType.IsPrimitive) || destinationType == typeof(UIecBinarySize)
            || destinationType == typeof(IecBinarySize) || destinationType == typeof(BinarySize)
            || base.CanConvertTo(context, destinationType);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            string stringValue => UBinarySize.Parse(stringValue, Options, provider: culture),
            UIecBinarySize sizeValue => sizeValue.Value,
            BinarySize sizeValue => new UBinarySize((ulong)sizeValue.Value),
            IecBinarySize sizeValue => new UBinarySize((ulong)sizeValue.Value.Value),
            _ => base.ConvertFrom(context, culture, value)
        };
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is UBinarySize size)
        {
            if (destinationType == typeof(string))
            {
                return size.ToString(null, culture);
            }

            if (destinationType == typeof(UIecBinarySize))
            {
                return new UIecBinarySize(size);
            }

            if (destinationType == typeof(ulong))
            {
                return size.Value;
            }

            if (destinationType == typeof(BinarySize))
            {
                return new BinarySize((long)size.Value);
            }

            if (destinationType == typeof(IecBinarySize))
            {
                return new IecBinarySize((long)size.Value);
            }

            if (destinationType.IsPrimitive)
            {
                return Convert.ChangeType(size.Value, destinationType, culture);
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
