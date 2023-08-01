using System.Globalization;

namespace Ookii;

/// <summary>
/// Provides culture-specific information on how to format and parse multiple-byte units.
/// </summary>
/// <remarks>
/// <para>
///   The properties of this class are used by the <see cref="BinarySize"/> structure to determine
///   how to format and parse multiple-byte units, including their IEC and SI prefixes.
/// </para>
/// <para>
///   Only an invariant version of this structure is provided by default; you can create your own
///   to localize or customize the units. Pass a custom <see cref="BinaryUnitInfo"/> instance to
///   the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/> or
///   <see cref="BinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)"/>
///   method, or combine it with a <see cref="CultureInfo"/> class using the
///   <see cref="CultureInfoExtensions.WithBinaryUnitInfo(CultureInfo, Ookii.BinaryUnitInfo)" qualifyHint="true"/>
///   method.
/// </para>
/// </remarks>
public class BinaryUnitInfo : ICloneable, IFormatProvider
{
    private static readonly BinaryUnitInfo _invariantInfo = new() { IsReadOnly = true };
    private string _shortByte = "B";
    private string _shortBytes = "B";
    private string _shortConnector = "";
    private string _shortKibi = "Ki";
    private string _shortMebi = "Mi";
    private string _shortGibi = "Gi";
    private string _shortTebi = "Ti";
    private string _shortPebi = "Pi";
    private string _shortExbi = "Ei";
    private string _shortKilo = "K";
    private string _shortDecimalKilo = "k";
    private string _shortMega = "M";
    private string _shortGiga = "G";
    private string _shortTera = "T";
    private string _shortPeta = "P";
    private string _shortExa = "E";
    private string _longByte = "byte";
    private string _longBytes = "bytes";
    private string _longConnector = "";
    private string _longKibi = "kibi";
    private string _longMebi = "mebi";
    private string _longGibi = "gibi";
    private string _longTebi = "tebi";
    private string _longPebi = "pebi";
    private string _longExbi = "exbi";
    private string _longKilo = "kilo";
    private string _longMega = "mega";
    private string _longGiga = "giga";
    private string _longTera = "tera";
    private string _longPeta = "peta";
    private string _longExa = "exa";

    /// <summary>
    /// Gets a read-only <see cref="BinaryUnitInfo"/> object that is culture-independent (invariant).
    /// </summary>
    /// <value>
    /// A read-only object that is culture-independent (invariant).
    /// </value>
    /// <remarks>
    /// <para>
    ///   The <see cref="BinaryUnitInfo"/> object returned by this property represents the default,
    ///   English-language units.
    /// </para>
    /// </remarks>
    public static BinaryUnitInfo InvariantInfo => _invariantInfo;

    /// <summary>
    /// Gets a value that indicates whether this <see cref="BinaryUnitInfo"/> object is read-only.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the <see cref="BinaryUnitInfo"/> is read-only; otherwise,
    /// <see langword="false"/>.
    /// </value>
    public bool IsReadOnly { get; private set; }

    /// <summary>
    /// Gets or sets the abbreviated version of the byte unit, singular.
    /// </summary>
    /// <value>
    /// The abbreviated singular byte unit. The default value is "B".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// The property is being set to <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The property is being set and the <see cref="BinaryUnitInfo"/> object is read-only.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   When formatting, the <see cref="ShortByte"/> property is only used if the value, when
    ///   scaled to the prefix, is exactly one. For example, 1 B, 1 KiB, 1 PB, etc.
    /// </para>
    /// <para>
    ///   If the value is not exactly one, but is rounded to one by the number format used, the
    ///   <see cref="ShortBytes"/> property is still used. For example, a value of 1.01 kibibytes,
    ///   when using a format string of "0.# SiB", would be formatted as "1 KB" using the plural
    ///   version of the unit.
    /// </para>
    /// <para>
    ///   In the default (invariant) information, the singular and plural abbreviated units are the
    ///   same.
    /// </para>
    /// </remarks>
    public string ShortByte
    {
        get => _shortByte;
        set
        {
            CheckReadOnly();
            _shortByte = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the bytes unit, plural.
    /// </summary>
    /// <value>
    /// The abbreviated plural bytes unit. The default value is "B".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortByte"/>
    /// </remarks>
    public string ShortBytes
    {
        get => _shortBytes;
        set
        {
            CheckReadOnly();
            _shortBytes = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets a string that can be used between an abbreviated prefix (e.g. <see cref="ShortKibi"/>
    /// and a unit (e.g. <see cref="ShortByte"/>).
    /// </summary>
    /// <value>
    /// The connector for abbreviated units. The default value is an empty string.
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The connector will be inserted when formatting a value if both a prefix and unit are
    ///   present. When parsing, the connector may be present between prefix and unit, but it is
    ///   always optional and parsing will not fail if it is not present.
    /// </para>
    /// </remarks>
    public string ShortConnector
    {
        get => _shortConnector;
        set
        {
            CheckReadOnly();
            _shortConnector = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the kibi prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "Ki".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   See the format string documentation for the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method for when this prefix will be used.
    /// </para>
    /// <para>
    ///   When parsing, a string containing this prefix will always be interpreted as powers of
    ///   two.
    /// </para>
    /// </remarks>
    public string ShortKibi
    {
        get => _shortKibi;
        set
        {
            CheckReadOnly();
            _shortKibi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the mebi prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "Me".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortKibi"/>
    /// </remarks>
    public string ShortMebi
    {
        get => _shortMebi;
        set
        {
            CheckReadOnly();
            _shortMebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the gibi prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "Gi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortKibi"/>
    /// </remarks>
    public string ShortGibi
    {
        get => _shortGibi;
        set
        {
            CheckReadOnly();
            _shortGibi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the tebi prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "Ti".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortKibi"/>
    /// </remarks>
    public string ShortTebi
    {
        get => _shortTebi;
        set
        {
            CheckReadOnly();
            _shortTebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the pebi prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "Pi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortKibi"/>
    /// </remarks>
    public string ShortPebi
    {
        get => _shortPebi;
        set
        {
            CheckReadOnly();
            _shortPebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the exbi prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "Ei".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortKibi"/>
    /// </remarks>
    public string ShortExbi
    {
        get => _shortExbi;
        set
        {
            CheckReadOnly();
            _shortExbi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the kilo prefix, when interpreted as powers of two
    /// (1,024 bytes).
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "K".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   When formatting, this prefix is used only when SI prefixes are treated as powers of two
    ///   (see the format string information for the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method). For a powers of ten version of the kilo prefix, the <see cref="ShortDecimalKilo"/>
    ///   property is used.
    /// </para>
    /// <para>
    ///   When parsing, whether this prefix is interpreted as powers of two or powers of ten depends
    ///   on the <see cref="BinarySizeOptions"/> value used.
    /// </para>
    /// </remarks>
    public string ShortKilo
    {
        get => _shortKilo;
        set
        {
            CheckReadOnly();
            _shortKilo = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the kilo prefix, when interpreted as powers of ten
    /// (1,000 bytes).
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "k".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   A distinct version of the powers of ten SI kilo prefix is provided because the SI prefixes
    ///   indicate that kilo should be a lower-case k, while when using kilo to represent 1,024
    ///   bytes, it is by convention written with an upper-case K. This is only the case for the
    ///   kilo prefix, so similar properties do not exist for other SI prefixes.
    /// </para>
    /// <para>
    ///   When formatting, this prefix is used only when SI prefixes are treated as powers of ten
    ///   (see the format string information for the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method). For a powers of two version of the kilo prefix, the <see cref="ShortKilo"/>
    ///   property is used.
    /// </para>
    /// <para>
    ///   When parsing, whether this prefix is interpreted as powers of two or powers of ten depends
    ///   on the <see cref="BinarySizeOptions"/> value used.
    /// </para>
    /// </remarks>
    public string ShortDecimalKilo
    {
        get => _shortDecimalKilo;
        set
        {
            CheckReadOnly();
            _shortDecimalKilo = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the mega prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "M".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   See the format string documentation for the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method for when this prefix will be used.
    /// </para>
    /// <para>
    ///   When parsing, whether this prefix is interpreted as powers of two or powers of ten depends
    ///   on the <see cref="BinarySizeOptions"/> value used.
    /// </para>
    /// </remarks>
    public string ShortMega
    {
        get => _shortMega;
        set
        {
            CheckReadOnly();
            _shortMega = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the giga prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "G".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortMega"/>
    /// </remarks>
    public string ShortGiga
    {
        get => _shortGiga;
        set
        {
            CheckReadOnly();
            _shortGiga = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the tera prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "T".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortMega"/>
    /// </remarks>
    public string ShortTera
    {
        get => _shortTera;
        set
        {
            CheckReadOnly();
            _shortTera = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the peta prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "P".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortMega"/>
    /// </remarks>
    public string ShortPeta
    {
        get => _shortPeta;
        set
        {
            CheckReadOnly();
            _shortPeta = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the abbreviated version of the exa prefix.
    /// </summary>
    /// <value>
    /// The abbreviated prefix. The default value is "E".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="ShortMega"/>
    /// </remarks>
    public string ShortExa
    {
        get => _shortExa;
        set
        {
            CheckReadOnly();
            _shortExa = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the byte unit, singular.
    /// </summary>
    /// <value>
    /// The full singular byte unit. The default value is "byte".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   When formatting, the <see cref="LongByte"/> property is only used if the value, when
    ///   scaled to the prefix, is exactly one. For example, 1 byte, 1 kibibyte, 1 petabyte, etc.
    /// </para>
    /// <para>
    ///   If the value is not exactly one, but is rounded to one by the number format used, the
    ///   <see cref="LongBytes"/> property is still used. For example, a value of 1.01 kibibytes,
    ///   when using a format string of "0.# Sibyte", would be formatted as "1 kibibytes", using the
    ///   plural version of the unit.
    /// </para>
    /// </remarks>
    public string LongByte
    {
        get => _longByte;
        set
        {
            CheckReadOnly();
            _longByte = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the bytes unit, plural.
    /// </summary>
    /// <value>
    /// The full plural bytes unit. The default value is "bytes".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongByte"/>
    /// </remarks>
    public string LongBytes
    {
        get => _longBytes;
        set
        {
            CheckReadOnly();
            _longBytes = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets a string that can be used between a full prefix (e.g. <see cref="LongKibi"/>
    /// and a unit (e.g. <see cref="LongByte"/>).
    /// </summary>
    /// <value>
    /// The connector for full units. The default value is an empty string.
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The connector will be inserted when formatting a value if both a prefix and unit are
    ///   present. When parsing, the connector may be present between prefix and unit, but it is
    ///   always optional and parsing will not fail if it is not present.
    /// </para>
    /// </remarks>
    public string LongConnector
    {
        get => _longConnector;
        set
        {
            CheckReadOnly();
            _longConnector = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the kibi prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "kibi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   See the format string documentation for the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method for when this prefix will be used.
    /// </para>
    /// <para>
    ///   When parsing, a string containing this prefix will always be interpreted as powers of
    ///   two. Long units are only allowed when parsing if the <see cref="BinarySizeOptions.AllowLongUnits" qualifyHint="true"/>
    ///   or <see cref="BinarySizeOptions.AllowLongUnitsOnly" qualifyHint="true"/> flag is used.
    /// </para>
    /// </remarks>
    public string LongKibi
    {
        get => _longKibi;
        set
        {
            CheckReadOnly();
            _longKibi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the mebi prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "mebi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKibi"/>
    /// </remarks>
    public string LongMebi
    {
        get => _longMebi;
        set
        {
            CheckReadOnly();
            _longMebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the gibi prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "gibi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKibi"/>
    /// </remarks>
    public string LongGibi
    {
        get => _longGibi;
        set
        {
            CheckReadOnly();
            _longGibi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the tebi prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "tebi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKibi"/>
    /// </remarks>
    public string LongTebi
    {
        get => _longTebi;
        set
        {
            CheckReadOnly();
            _longTebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the pebi prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "pebi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKibi"/>
    /// </remarks>
    public string LongPebi
    {
        get => _longPebi;
        set
        {
            CheckReadOnly();
            _longPebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the exbi prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "exbi".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKibi"/>
    /// </remarks>
    public string LongExbi
    {
        get => _longExbi;
        set
        {
            CheckReadOnly();
            _longExbi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the kilo prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "kilo".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <para>
    ///   See the format string documentation for the <see cref="BinarySize.ToString(string?, IFormatProvider?)" qualifyHint="true"/>
    ///   method for when this prefix will be used.
    /// </para>
    /// <para>
    ///   When parsing, whether this prefix is interpreted as powers of two or powers of ten depends
    ///   on the <see cref="BinarySizeOptions"/> value used. Long units are only allowed when
    ///   parsing if the <see cref="BinarySizeOptions.AllowLongUnits" qualifyHint="true"/> or
    ///   <see cref="BinarySizeOptions.AllowLongUnitsOnly" qualifyHint="true"/> flag is used.
    /// </para>
    /// </remarks>
    public string LongKilo
    {
        get => _longKilo;
        set
        {
            CheckReadOnly();
            _longKilo = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the mega prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "mega".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKilo"/>
    /// </remarks>
    public string LongMega
    {
        get => _longMega;
        set
        {
            CheckReadOnly();
            _longMega = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the giga prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "G".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKilo"/>
    /// </remarks>
    public string LongGiga
    {
        get => _longGiga;
        set
        {
            CheckReadOnly();
            _longGiga = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the tera prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "tera".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKilo"/>
    /// </remarks>
    public string LongTera
    {
        get => _longTera;
        set
        {
            CheckReadOnly();
            _longTera = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the peta prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "peta".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKilo"/>
    /// </remarks>
    public string LongPeta
    {
        get => _longPeta;
        set
        {
            CheckReadOnly();
            _longPeta = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Gets or sets the full version of the exa prefix.
    /// </summary>
    /// <value>
    /// The full prefix. The default value is "exa".
    /// </value>
    /// <exception cref="ArgumentNullException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <inheritdoc cref="ShortByte"/>
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="LongKilo"/>
    /// </remarks>
    public string LongExa
    {
        get => _longExa;
        set
        {
            CheckReadOnly();
            _longExa = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Creates a read-only copy of this <see cref="BinaryUnitInfo"/> object.
    /// </summary>
    /// <param name="info">The <see cref="BinaryUnitInfo"/> to make read-only.</param>
    /// <returns>
    /// An object with the same values as <paramref name="info"/>, but which is read-only.
    /// </returns>
    public static BinaryUnitInfo ReadOnly(BinaryUnitInfo info)
    {
        if (info.IsReadOnly)
        {
            return info;
        }

        var result = (BinaryUnitInfo)info.MemberwiseClone();
        result.IsReadOnly = true;
        return result;
    }

    /// <summary>
    /// Creates a shallow copy of the <see cref="BinaryUnitInfo"/> object.
    /// </summary>
    /// <returns>
    /// A new object copied from the original <see cref="BinaryUnitInfo"/> object.
    /// </returns>
    /// <remarks>
    /// The clone is writable even if the original <see cref="BinaryUnitInfo"/> object is read-only.
    /// </remarks>
    public object Clone()
    {
        var result = (BinaryUnitInfo)MemberwiseClone();
        result.IsReadOnly = false;
        return result;
    }

    /// <summary>
    /// Gets an object of the specified type that provides a formatting service.
    /// </summary>
    /// <param name="formatType">The <see cref="Type"/> of the formatting service.</param>
    /// <returns>
    /// The current <see cref="BinaryUnitInfo"/>, if <paramref name="formatType"/> is the
    /// <see cref="BinaryUnitInfo"/> class; otherwise, <see langword="null"/>.
    /// </returns>
    public object? GetFormat(Type? formatType)
    {
        if (formatType == typeof(BinaryUnitInfo))
        {
            return this;
        }

        return null;
    }

    private void CheckReadOnly()
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException(Properties.Resources.BinaryUnitInfoReadOnly);
        }
    }
}
