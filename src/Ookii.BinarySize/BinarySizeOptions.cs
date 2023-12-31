﻿using System.Globalization;

namespace Ookii;

/// <summary>
/// Determines how multiple-byte units are interpreted when parsing a string into a
/// <see cref="BinarySize"/> value, when passed to the
/// <see cref="BinarySize.Parse(string, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
/// and <see cref="BinarySize.TryParse(string?, BinarySizeOptions, NumberStyles, IFormatProvider?, out BinarySize)" qualifyHint="true"/>
/// methods.
/// </summary>
/// <remarks>
/// <para>
///   This enumeration supports a bitwise combination of its member values.
/// </para>
/// </remarks>
[Flags]
public enum BinarySizeOptions
{
    /// <summary>
    /// Use the default interpretation, where 1 KB equals 1 KiB equals 1,024 bytes, and 1 MB equals
    /// 1 MiB equals 1,048,576 bytes, and so on. Only abbreviated (short) units are allowed.
    /// </summary>
    Default = 0,
    /// <summary>
    /// Use the interpretation suggested by the IEC standard, where 1 kB equals 1,000 bytes, 1 KiB
    /// equals 1,024 bytes, 1 MB equals 1,000,000 bytes, 1 MiB equals 1,048,576 bytes, and so on.
    /// </summary>
    UseIecStandard = 0x1,
    /// <summary>
    /// Allow the use of unabbreviated (long) units, as defined by the <see cref="BinaryUnitInfo"/>
    /// class. Typically, these are units written out fully such as "kilobyte". If the
    /// <see cref="AllowLongUnitsOnly"/> flag is not present, both short and long units are
    /// accepted.
    /// </summary>
    AllowLongUnits = 0x2,
    /// <summary>
    /// Only allow the use of unabbreviated (long) units, as defined by the
    /// <see cref="BinaryUnitInfo"/> class. Typically, these are units written out fully such as
    /// "kilobyte". Abbreviated (short) units will not be accepted. Implies
    /// <see cref="AllowLongUnits"/>.
    /// </summary>
    AllowLongUnitsOnly = 0x4,
}
