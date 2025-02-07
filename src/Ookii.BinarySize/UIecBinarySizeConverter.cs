using System.ComponentModel;
using System.Globalization;

namespace Ookii;

/// <summary>
/// Converts an <see cref="UIecBinarySize"/> object from one data type to another. Access this class
/// through the <see cref="TypeDescriptor"/> class.
/// </summary>
/// <remarks>
/// <para>
///   This is the default type converter for the <see cref="UIecBinarySize"/> structure. It supports
///   the same conversions as the <see cref="UBinarySizeConverter"/> class, but for the
///   <see cref="UIecBinarySize"/> structure.
/// </para>
/// <para>
///   When converting from a string, this converter will interpret SI prefixes as based on powers of
///   ten, so "1kB" equals 1,000 bytes, "1MB" equals 1,000,000 bytes, and so on. IEC prefixes are
///   still based on powers of two.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
public class UIecBinarySizeConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || sourceType == typeof(UBinarySize) || sourceType == typeof(BinarySize)
            || sourceType == typeof(IecBinarySize) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => (destinationType != null && destinationType.IsPrimitive) || destinationType == typeof(UBinarySize)
            || destinationType == typeof(BinarySize) || destinationType == typeof(IecBinarySize)
            || base.CanConvertTo(context, destinationType);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            string stringValue => UIecBinarySize.Parse(stringValue, culture),
            UBinarySize sizeValue => new UIecBinarySize(sizeValue),
            BinarySize sizeValue => new UIecBinarySize((ulong)sizeValue.Value),
            IecBinarySize sizeValue => new UIecBinarySize((ulong)sizeValue.Value.Value),
            _ => base.ConvertFrom(context, culture, value)
        };
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is UIecBinarySize size)
        {
            if (destinationType == typeof(string))
            {
                return size.ToString(null, culture);
            }

            if (destinationType == typeof(ulong))
            {
                return size.Value.Value;
            }

            if (destinationType == typeof(UBinarySize))
            {
                return size.Value;
            }

            if (destinationType == typeof(IecBinarySize))
            {
                return new IecBinarySize((long)size.Value.Value);
            }

            if (destinationType == typeof(BinarySize))
            {
                return new BinarySize((long)size.Value.Value);
            }

            if (destinationType.IsPrimitive)
            {
                return Convert.ChangeType(size.Value.Value, destinationType, culture);
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
