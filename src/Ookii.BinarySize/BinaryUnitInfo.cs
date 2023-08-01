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
///   method, or combine it with a <see cref="CultureInfo"/> class using the <see cref="TODO"/>
///   method.
/// </para>
/// </remarks>
public class BinaryUnitInfo : ICloneable, IFormatProvider
{
    private bool _isReadOnly;
    private static readonly BinaryUnitInfo _invariantInfo = new() { _isReadOnly = true };
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
    /// The property is being set to <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The property is being set and the <see cref="BinaryUnitInfo"/> object is read-only.
    /// </exception>
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
    /// Gets or sets a string that is inserted between a
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
    public string ShortConnector
    {
        get => _shortConnector;
        set
        {
            CheckReadOnly();
            _shortConnector = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortKibi
    {
        get => _shortKibi;
        set
        {
            CheckReadOnly();
            _shortKibi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortMebi
    {
        get => _shortMebi;
        set
        {
            CheckReadOnly();
            _shortMebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortGibi
    {
        get => _shortGibi;
        set
        {
            CheckReadOnly();
            _shortGibi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortTebi
    {
        get => _shortTebi;
        set
        {
            CheckReadOnly();
            _shortTebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortPebi
    {
        get => _shortPebi;
        set
        {
            CheckReadOnly();
            _shortPebi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortExbi
    {
        get => _shortExbi;
        set
        {
            CheckReadOnly();
            _shortExbi = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortKilo
    {
        get => _shortKilo;
        set
        {
            CheckReadOnly();
            _shortKilo = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    // When parsing, this will be interpreted as 1024 depending on flag.
    public string ShortDecimalKilo
    {
        get => _shortDecimalKilo;
        set
        {
            CheckReadOnly();
            _shortDecimalKilo = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortMega
    {
        get => _shortMega;
        set
        {
            CheckReadOnly();
            _shortMega = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortGiga
    {
        get => _shortGiga;
        set
        {
            CheckReadOnly();
            _shortGiga = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortTera
    {
        get => _shortTera;
        set
        {
            CheckReadOnly();
            _shortTera = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortPeta
    {
        get => _shortPeta;
        set
        {
            CheckReadOnly();
            _shortPeta = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public string ShortExa
    {
        get => _shortExa;
        set
        {
            CheckReadOnly();
            _shortExa = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public static BinaryUnitInfo ReadOnly(BinaryUnitInfo info)
    {
        if (info._isReadOnly)
        {
            return info;
        }

        var result = (BinaryUnitInfo)info.MemberwiseClone();
        result._isReadOnly = true;
        return result;
    }

    public object Clone()
    {
        var result = (BinaryUnitInfo)MemberwiseClone();
        result._isReadOnly = false;
        return result;
    }

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
        if (_isReadOnly)
        {
            throw new InvalidOperationException(Properties.Resources.BinaryUnitInfoReadOnly);
        }
    }
}
