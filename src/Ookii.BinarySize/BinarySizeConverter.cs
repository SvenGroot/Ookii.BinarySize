using System;
using System.ComponentModel;
using System.Globalization;

namespace Ookii;

/// <summary>
/// Converts a <see cref="BinarySize"/> object from one data type to another. Access this class through the <see cref="TypeDescriptor"/> object.
/// </summary>
public class BinarySizeConverter : TypeConverter
{
    /// <summary>
    /// Determines if this converter can convert an object in the given source type to the native type of the converter.
    /// </summary>
    /// <param name="context">A formatter context. This object can be used to get additional information about the environment this converter is being called from. This may be <see langword="null"/>, so you should always check. Also, properties on the context object may also return <see langword="null"/>. </param>
    /// <param name="sourceType">The type you want to convert from.</param>
    /// <returns><see langword="true"/> if this object can perform the conversion; otherwise, <see langword="false"/>.</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <summary>
    /// Gets a value indicating whether this converter can convert an object to the given destination type using the context. 
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> object that provides a format context.</param>
    /// <param name="destinationType">A <see cref="Type"/> object that represents the type you want to convert to.</param>
    /// <returns><see langword="true"/> if this object can perform the conversion; otherwise, <see langword="false"/>.</returns>
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        => (destinationType != null && destinationType.IsPrimitive) || base.CanConvertTo(context, destinationType);

    /// <summary>
    /// Converts the specified object to a <see cref="BinarySize"/> object.
    /// </summary>
    /// <param name="context">A formatter context. This object can be used to get additional information about the environment this converter is being called from. This may be <see langword="null"/>, so you should always check. Also, properties on the context object may also return <see langword="null"/>. </param>
    /// <param name="culture">An object that contains culture specific information, such as the language, calendar, and cultural conventions associated with a specific culture. It is based on the RFC 1766 standard.</param>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted object.</returns>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string stringValue)
        {
            return BinarySize.Parse(stringValue, culture);
        }

        return base.ConvertFrom(context, culture, value);
    }

    /// <summary>
    /// Converts the specified object to the specified type. 
    /// </summary>
    /// <param name="context">A formatter context. This object can be used to get additional information about the environment this converter is being called from. This may be <see langword="null"/>, so you should always check. Also, properties on the context object may also return <see langword="null"/>. </param>
    /// <param name="culture">An object that contains culture specific information, such as the language, calendar, and cultural conventions associated with a specific culture. It is based on the RFC 1766 standard.</param>
    /// <param name="value">The object to convert.</param>
    /// <param name="destinationType">The type to convert the object to.</param>
    /// <returns>The converted object.</returns>
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (value is BinarySize size)
        {
            if (destinationType == typeof(string))
            {
                return size.ToString(null, culture);
            }

            if (destinationType.IsPrimitive)
            {
                return Convert.ChangeType(size.Value, destinationType, culture);
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
