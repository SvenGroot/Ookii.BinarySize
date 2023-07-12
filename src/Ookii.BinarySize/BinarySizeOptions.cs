using System.Globalization;

namespace Ookii;

/// <summary>
/// <para>
///   Determines how byte unit suffixes are interpreted when passed to the
///   <see cref="BinarySize.Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
///   and <see cref="BinarySize.TryParse(string?, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)" qualifyHint="true"/>
///   methods.
/// </para>
/// <para>
///   This enumeration supports a bitwise combination of its member values.
/// </para>
/// </summary>
[Flags]
public enum BinarySizeOptions
{
    /// <summary>
    /// Use the default interpretation, where 1 KB equals 1 KiB equals 1,024 bytes, and 1 MB equals
    /// 1 MiB equals 1,048,576 bytes, and so forth.
    /// </summary>
    Default,
    /// <summary>
    /// Use the interpretation suggested by the IEC standard, where 1 kB equals 1,000 bytes, and
    /// 1 KiB equals 1,024 bytes, and 1 MB equals 1,000,000 bytes, and 1 MiB equals 1,048,576 bytes,
    /// and so forth.
    /// </summary>
    UseIecStandard
}
