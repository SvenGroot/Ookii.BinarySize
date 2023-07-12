using System.ComponentModel;

namespace Ookii;

/// <summary>
/// Provides an alternative <see cref="TypeConverter"/> for the <see cref="BinarySize"/> structure
/// that interprets SI prefixes as decimal and IEC prefixes as powers of two.
/// </summary>
/// <remarks>
/// <para>
///   This converter uses the <see cref="BinarySizeOptions.UseIecStandard" qualifyHint="true"/>
///   flag when parsing from a string, so it will converter 1 kB to 1000 bytes, and 1 KiB to 1024
///   bytes. This is in contrast to the default type converter for <see cref="BinarySize"/>, which
///   uses <see cref="BinarySizeOptions.Default" qualifyHint="true"/>.
/// </para>
/// <para>
///   Use the <see cref="TypeConverterAttribute"/> attribute on an individual property to use this
///   converter instead of the default one in contexts that support that, such as properties used in
///   XAML.
/// </para>
/// </remarks>
public class BinarySizeIecConverter : BinarySizeConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySizeIecConverter"/> class.
    /// </summary>
    public BinarySizeIecConverter()
        : base(BinarySizeOptions.UseIecStandard)
    {
    }
}
