using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ookii;

internal class CultureInfoWrapper : CultureInfo
{
    private readonly CultureInfo _culture;
    private readonly BinaryUnitInfo _binaryUnitInfo;

    public CultureInfoWrapper(CultureInfo culture, BinaryUnitInfo binaryUnitInfo)
        : base(culture.LCID)
    {
        _culture = culture;
        _binaryUnitInfo = binaryUnitInfo;
    }

    public override Calendar Calendar => _culture.Calendar;

    public override object Clone()
    {
        return new CultureInfoWrapper((CultureInfo)_culture.Clone(), (BinaryUnitInfo)_binaryUnitInfo.Clone());
    }

    public override CompareInfo CompareInfo => _culture.CompareInfo;

    public override DateTimeFormatInfo DateTimeFormat
    {
        get => _culture.DateTimeFormat;
        set => _culture.DateTimeFormat = value;
    }

    public override string DisplayName => _culture.DisplayName;

    public override string EnglishName => _culture.EnglishName;

    public override bool IsNeutralCulture => _culture.IsNeutralCulture;

    public override int KeyboardLayoutId => _culture.KeyboardLayoutId;

    public override int LCID => _culture.LCID;

    public override string Name => _culture.Name;

    public override string NativeName => _culture.NativeName;

    public override NumberFormatInfo NumberFormat 
    {
        get => _culture.NumberFormat;
        set => _culture.NumberFormat = value; 
    }

    public override Calendar[] OptionalCalendars => _culture.OptionalCalendars;

    public override CultureInfo Parent => _culture.Parent;

    public override TextInfo TextInfo => _culture.TextInfo;

    public override string ThreeLetterISOLanguageName => _culture.ThreeLetterISOLanguageName;

    public override string ThreeLetterWindowsLanguageName => _culture.ThreeLetterWindowsLanguageName;

    public override string TwoLetterISOLanguageName => _culture.TwoLetterISOLanguageName;

    public override bool Equals(
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        object? value) => _culture.Equals(value);

    public override object? GetFormat(Type? formatType)
    {
        if (formatType == typeof(BinaryUnitInfo))
        {
            return _binaryUnitInfo;
        }

        return _culture.GetFormat(formatType);
    }

    public override int GetHashCode() => _culture.GetHashCode();

    public override string ToString() => _culture.ToString();
}
