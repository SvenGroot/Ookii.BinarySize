using System;
using System.ComponentModel;
using System.Globalization;

namespace Ookii;

/// <summary>
/// Converts a <see cref="BinarySize"/> object from one data type to another. Access this class
/// through the <see cref="TypeDescriptor"/> object.
/// </summary>
/// <remarks>
/// <para>
///   This <see cref="TypeConverter"/> is able to convert from a <see cref="string"/> to a
///   <see cref="BinarySize"/>, using the same rules as the
///   <see cref="BinarySize.Parse(string, IFormatProvider?)" qualifyHint="true"/> method.
/// </para>
/// <para>
///   It can also convert from a <see cref="BinarySize"/> to a <see cref="string"/> using the
///   default format used by the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
///   method. It also supports conversion from a <see cref="BinarySize"/> to any primitive type.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
public class BinarySizeConverter : TypeConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySizeConverter"/> class.
    /// </summary>
    public BinarySizeConverter()
    { 
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySizeConverter"/> class with the specified
    /// options.
    /// </summary>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units
    /// are interpreted when converting from a string.
    /// </param>
    /// <remarks>
    /// <para>
    ///   When converting from a string, the <paramref name="options"/> will be passed to the
    ///   <see cref="BinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// </remarks>
    public BinarySizeConverter(BinarySizeOptions options)
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
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => (destinationType != null && destinationType.IsPrimitive) || base.CanConvertTo(context, destinationType);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            return BinarySize.Parse(stringValue, Options, NumberStyles.Number, culture);
        }

        if (value is IecBinarySize sizeValue)
        {
            return sizeValue.Value;
        }

        return base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is BinarySize size)
        {
            if (destinationType == typeof(string))
            {
                return size.ToString(null, culture);
            }

            if (destinationType == typeof(IecBinarySize))
            {
                return new IecBinarySize(size);
            }

            if (destinationType == typeof(long))
            {
                return size.Value;
            }

            if (destinationType.IsPrimitive)
            {
                return Convert.ChangeType(size.Value, destinationType, culture);
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
