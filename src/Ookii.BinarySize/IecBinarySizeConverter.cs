using System;
using System.ComponentModel;
using System.Globalization;

namespace Ookii;

/// <summary>
/// Converts an <see cref="IecBinarySize"/> object from one data type to another. Access this class
/// through the <see cref="TypeDescriptor"/> object.
/// </summary>
/// <remarks>
/// <para>
///   This is the default type converter for the <see cref="IecBinarySize"/> structure. It performs
///   conversion using the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/> flag.
///   It supports the same conversions as the <see cref="BinarySizeConverter"/> class, but for the
///   <see cref="IecBinarySize"/> structure.
/// </para>
/// <para>
///   Unlike the <see cref="BinarySizeIecConverter"/> class, which is intended for use with the
///   <see cref="TypeConverterAttribute"/> on properties that use a <see cref="BinarySize"/>
///   structure, this converter instead returns a <see cref="IecBinarySize"/> instance and is only
///   intended for use with that type.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
public class IecBinarySizeConverter : TypeConverter
{
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
            return IecBinarySize.Parse(stringValue, culture);
        }

        if (value is BinarySize sizeValue)
        {
            return new IecBinarySize(sizeValue);
        }

        return base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is IecBinarySize size)
        {
            if (destinationType == typeof(string))
            {
                return size.ToString(null, culture);
            }

            if (destinationType == typeof(long))
            {
                return size.Value.Value;
            }

            if (destinationType == typeof(BinarySize))
            {
                return size.Value;
            }

            if (destinationType.IsPrimitive)
            {
                return Convert.ChangeType(size.Value.Value, destinationType, culture);
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
