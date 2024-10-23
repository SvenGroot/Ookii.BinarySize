using Ookii.Common;
using System.Globalization;
using System.Text;

namespace Ookii;

static class FormatHelper
{
    #region Nested types

    private ref struct SuffixInfo
    {
        public ReadOnlySpan<char> Trimmed { get; set; }
        public ReadOnlySpan<char> Whitespace { get; set; }
        public ReadOnlySpan<char> Trailing { get; set; }
        public long Factor { get; set; }
        public bool HasIecChar { get; set; }
        public bool HasByteChar { get; set; }
        public bool UseDecimal { get; set; }
        public bool IsLong { get; set; }
    }

    private ref struct FactorInfo
    {
        public ReadOnlySpan<char> Value { get; set; }
        public long Factor { get; set; }
        public CultureInfo Culture { get; set; }
        public CompareOptions CompareOptions { get; set; }

        public bool TrimSuffix(string suffix)
        {
            if (Value.StripSuffix(suffix.AsSpan(), Culture, CompareOptions).TryGetValue(out var result))
            {
                Value = result;
                return true;
            }

            return false;
        }

        public bool CheckUnitPrefix(string unit, long factor)
        {
            if (TrimSuffix(unit))
            {
                Factor = factor;
                return true;
            }

            return false;
        }
    }

    #endregion

    // Decimal factors used internally.
    private const long Kilo = 1000;
    private const long Mega = 1000 * Kilo;
    private const long Giga = 1000 * Mega;
    private const long Tera = 1000 * Giga;
    private const long Peta = 1000 * Tera;
    private const long Exa = 1000 * Peta;

    private const long AutoFactor = -1;
    private const long ShortestFactor = -2;
    private const long DecimalAutoFactor = -3;
    private const long DecimalShortestFactor = -4;

    private static readonly char[] _scalingChars = new[] { 'E',  'P',  'T',  'G',  'M',  'K',
                                                             'e',  'p',  't',  'g',  'm',  'k', 'A', 'S', 'a', 's' };
    private static readonly long[] _scalingFactors = new[] { BinarySize.Exbi, BinarySize.Pebi, BinarySize.Tebi, BinarySize.Gibi, BinarySize.Mebi, BinarySize.Kibi,
                                                             Exa,  Peta, Tera, Giga, Mega, Kilo, AutoFactor, ShortestFactor, DecimalAutoFactor, DecimalShortestFactor };

    public static decimal Parse(ReadOnlySpan<char> s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider)
    {
        ValidateOptions(options);
        if (s.IsEmpty)
        {
            return 0;
        }

        var factor = ParseUnit(ref s, provider, options);
#if NET6_0_OR_GREATER
        var size = decimal.Parse(s, style, provider);
#else
        var size = decimal.Parse(s.ToString(), style, provider);
#endif

        return size * factor;
    }

    public static bool TryParse(ReadOnlySpan<char> s, BinarySizeOptions options, NumberStyles style, IFormatProvider? provider, out decimal result)
    {
        ValidateOptions(options);
        if (s.IsEmpty)
        {
            result = 0;
            return true;
        }

        var factor = ParseUnit(ref s, provider, options);
#if NET6_0_OR_GREATER
        var success = decimal.TryParse(s, style, provider, out var size);
#else
        var success = decimal.TryParse(s.ToString(), style, provider, out var size);
#endif

        if (!success)
        {
            result = default;
            return false;
        }

        try
        {
            result = size * factor;
            return true;
        }
        catch (OverflowException)
        {
            // I couldn't find a good way to handle overflow without exceptions.
            result = 0;
            return false;
        }
    }

#if NET6_0_OR_GREATER

    public static bool TryFormat(ulong absoluteValue, decimal value, Span<char> destination, out int charsWritten,
        ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var suffix = ParseFormat(format, absoluteValue, ref value);
        if (!value.TryFormat(destination, out charsWritten, suffix.Trimmed, provider))
        {
            return false;
        }

        var unitInfo = GetUnitInfo(provider);
        var scale = GetUnitScale(suffix, unitInfo);

        if (!destination.TryAppend(ref charsWritten, suffix.Whitespace))
        {
            return false;
        }

        if (scale != null)
        {
            if (!destination.TryAppend(ref charsWritten, scale.AsSpan()))
            {
                return false;
            }

            var connector = suffix.IsLong ? unitInfo.LongConnector : unitInfo.ShortConnector;
            if (suffix.HasByteChar && !destination.TryAppend(ref charsWritten, connector))
            {
                return false;
            }
        }

        // Note: this does not account for the possibility that a not-exactly-one value is
        // rounded to 1 by the number format.
        string unit;
        if (suffix.IsLong)
        {
            unit = value == 1 ? unitInfo.LongByte : unitInfo.LongBytes;
        }
        else
        {
            unit = value == 1 ? unitInfo.ShortByte : unitInfo.ShortBytes;
        }

        return (!suffix.HasByteChar || destination.TryAppend(ref charsWritten, unit)) &&
            destination.TryAppend(ref charsWritten, suffix.Trailing);
    }

#endif

    public static string GetString(ulong absoluteValue, decimal value, string? format, IFormatProvider? formatProvider = null)
    {
        var suffix = ParseFormat(format.AsSpan(), absoluteValue, ref value);
        var result = new StringBuilder((format?.Length ?? 0) + 16);
        result.Append(value.ToString(suffix.Trimmed.ToString(), formatProvider));
        result.Append(suffix.Whitespace);
        var unitInfo = GetUnitInfo(formatProvider);
        var scale = GetUnitScale(suffix, unitInfo);
        if (scale != null)
        {
            result.Append(scale);
            if (suffix.HasByteChar)
            {
                result.Append(suffix.IsLong ? unitInfo.LongConnector : unitInfo.ShortConnector);
            }
        }

        if (suffix.HasByteChar)
        {
            // Note: this does not account for the possibility that a not-exactly-one value is
            // rounded to 1 by the number format.
            if (suffix.IsLong)
            {
                result.Append(value == 1 ? unitInfo.LongByte : unitInfo.LongBytes);
            }
            else
            {
                result.Append(value == 1 ? unitInfo.ShortByte : unitInfo.ShortBytes);
            }
        }

        result.Append(suffix.Trailing);
        return result.ToString();
    }


    private static long ParseUnit(ref ReadOnlySpan<char> value, IFormatProvider? provider, BinarySizeOptions options)
    {
        var unitInfo = GetUnitInfo(provider);
        var factor = new FactorInfo()
        {
            Value = value.TrimEnd(),
            Factor = 1,
            Culture = (provider as CultureInfo) ?? CultureInfo.CurrentCulture,
            CompareOptions = unitInfo.CompareOptions,
        };

        var unitFound = false;
        var withConnector = factor.Value;
        if (options.HasFlag(BinarySizeOptions.AllowLongUnits) || options.HasFlag(BinarySizeOptions.AllowLongUnitsOnly))
        {
            unitFound = factor.TrimSuffix(unitInfo.LongBytes) || factor.TrimSuffix(unitInfo.LongByte);
            if (unitFound)
            {
                withConnector = factor.Value;
                factor.TrimSuffix(unitInfo.LongConnector);
            }
        }

        if (!unitFound && !options.HasFlag(BinarySizeOptions.AllowLongUnitsOnly))
        {
            if (factor.TrimSuffix(unitInfo.ShortBytes) || factor.TrimSuffix(unitInfo.ShortByte))
            {
                withConnector = factor.Value;
                factor.TrimSuffix(unitInfo.ShortConnector);
            }
        }

        var prefixFound = false;
        var useDecimal = options.HasFlag(BinarySizeOptions.UseIecStandard);
        if (options.HasFlag(BinarySizeOptions.AllowLongUnits) || options.HasFlag(BinarySizeOptions.AllowLongUnitsOnly))
        {
            prefixFound = factor.CheckUnitPrefix(unitInfo.LongKibi, BinarySize.Kibi)
                || factor.CheckUnitPrefix(unitInfo.LongMebi, BinarySize.Mebi)
                || factor.CheckUnitPrefix(unitInfo.LongGibi, BinarySize.Gibi)
                || factor.CheckUnitPrefix(unitInfo.LongTebi, BinarySize.Tebi)
                || factor.CheckUnitPrefix(unitInfo.LongPebi, BinarySize.Pebi)
                || factor.CheckUnitPrefix(unitInfo.LongExbi, BinarySize.Exbi)
                || factor.CheckUnitPrefix(unitInfo.LongKilo, useDecimal ? Kilo : BinarySize.Kibi)
                || factor.CheckUnitPrefix(unitInfo.LongMega, useDecimal ? Mega : BinarySize.Mebi)
                || factor.CheckUnitPrefix(unitInfo.LongMega, useDecimal ? Mega : BinarySize.Mebi)
                || factor.CheckUnitPrefix(unitInfo.LongGiga, useDecimal ? Giga : BinarySize.Gibi)
                || factor.CheckUnitPrefix(unitInfo.LongTera, useDecimal ? Tera : BinarySize.Tebi)
                || factor.CheckUnitPrefix(unitInfo.LongPeta, useDecimal ? Peta : BinarySize.Pebi)
                || factor.CheckUnitPrefix(unitInfo.LongExa, useDecimal ? Exa : BinarySize.Exbi);
        }

        if (!prefixFound && !options.HasFlag(BinarySizeOptions.AllowLongUnitsOnly))
        {
            prefixFound = factor.CheckUnitPrefix(unitInfo.ShortKibi, BinarySize.Kibi)
                || factor.CheckUnitPrefix(unitInfo.ShortMebi, BinarySize.Mebi)
                || factor.CheckUnitPrefix(unitInfo.ShortGibi, BinarySize.Gibi)
                || factor.CheckUnitPrefix(unitInfo.ShortTebi, BinarySize.Tebi)
                || factor.CheckUnitPrefix(unitInfo.ShortPebi, BinarySize.Pebi)
                || factor.CheckUnitPrefix(unitInfo.ShortExbi, BinarySize.Exbi)
                || factor.CheckUnitPrefix(unitInfo.ShortKilo, useDecimal ? Kilo : BinarySize.Kibi)
                || factor.CheckUnitPrefix(unitInfo.ShortMega, useDecimal ? Mega : BinarySize.Mebi)
                || factor.CheckUnitPrefix(unitInfo.ShortMega, useDecimal ? Mega : BinarySize.Mebi)
                || factor.CheckUnitPrefix(unitInfo.ShortGiga, useDecimal ? Giga : BinarySize.Gibi)
                || factor.CheckUnitPrefix(unitInfo.ShortTera, useDecimal ? Tera : BinarySize.Tebi)
                || factor.CheckUnitPrefix(unitInfo.ShortPeta, useDecimal ? Peta : BinarySize.Pebi)
                || factor.CheckUnitPrefix(unitInfo.ShortExa, useDecimal ? Exa : BinarySize.Exbi)
                || factor.CheckUnitPrefix(unitInfo.ShortDecimalKilo, useDecimal ? Kilo : BinarySize.Kibi);
        }

        if (prefixFound)
        {
            value = factor.Value;
        }
        else
        {
            // Don't remove the connector if there was no recognized prefix before it.
            value = withConnector;
        }

        return factor.Factor;
    }

    private static SuffixInfo TrimFormatSuffix(ReadOnlySpan<char> value)
    {
        SuffixInfo result = new()
        {
            Factor = 1,
        };

        var trimmed = value.TrimEnd();
        result.Trailing = value.Slice(trimmed.Length);
        if (trimmed.Length == 0)
        {
            return result;
        }

        // Suffix can use "byte" to indicate long units.
        if (trimmed.StripSuffix("byte".AsSpan(), StringComparison.OrdinalIgnoreCase).TryGetValue(out var newValue))
        {
            trimmed = newValue;
            result.HasByteChar = true;
            result.IsLong = true;
        }
        else if (trimmed.StripSuffix("b".AsSpan(), StringComparison.OrdinalIgnoreCase).TryGetValue(out newValue))
        {
            trimmed = newValue;
            result.HasByteChar = true;
        }

        if (trimmed.Length == 0)
        {
            return result;
        }

        var index = trimmed.Length - 1;
        var ch = trimmed[index];

        // The 'i' is only counted as an IEC char if there's a valid scale prefix before it.
        if (trimmed.Length > 1 && ch is 'I' or 'i')
        {
            result.HasIecChar = true;
            --index;
        }

        ch = trimmed[index];
        var prefixes = _scalingChars.AsSpan();
        if (result.HasIecChar)
        {
            // Force the use of binary regardless of case if 'i' is present.
            ch = char.ToUpperInvariant(ch);
        }

        var scaleIndex = prefixes.IndexOf(ch);
        if (scaleIndex < 0)
        {
            // No scale prefix before the 'i', so don't count it as an IEC char.
            result.HasIecChar = false;
        }
        else
        {
            trimmed = trimmed.Slice(0, index);
            result.Factor = _scalingFactors[scaleIndex];
        }

        // Remove any whitespace between the number and the unit.
        result.Trimmed = trimmed.TrimEnd();
        result.Whitespace = trimmed.Slice(result.Trimmed.Length);
        return result;
    }

    private static long DetermineAutomaticScalingFactor(long autoFactor, ulong absoluteValue)
    {
        // Check all factors except the automatic ones.
        var (allowRounding, useDecimal) = autoFactor switch
        {
            AutoFactor => (false, false),
            ShortestFactor => (true, false),
            DecimalAutoFactor => (false, true),
            DecimalShortestFactor => (true, true),
            _ => throw new ArgumentException(null, nameof(autoFactor)), // Should never be reached
        };

        var factors = _scalingFactors.AsSpan(0, _scalingChars.Length - 4);
        if (useDecimal)
        {
            factors = factors.Slice(factors.Length / 2);
        }
        else
        {
            factors = factors.Slice(0, factors.Length / 2);
        }

        // Use the absolute value to select the correct unit for negative numbers.
        for (int index = 0; index < factors.Length; ++index)
        {
            var factor = factors[index];
            // TODO: Should _scalingFactors be ulong?
            if (absoluteValue >= (ulong)factor && (allowRounding || absoluteValue % (ulong)factor == 0))
            {
                return factor;
            }
        }

        return 1;
    }

    private static SuffixInfo ParseFormat(ReadOnlySpan<char> format, ulong absoluteValue, ref decimal scaledValue)
    {
        SuffixInfo suffix;
        if (format.IsEmpty || format == "G".AsSpan())
        {
            suffix = new()
            {
                Factor = AutoFactor,
                Whitespace = " ".AsSpan(),
                HasIecChar = true,
                HasByteChar = true,
            };
        }
        else
        {
            suffix = TrimFormatSuffix(format);
        }

        if (suffix.Factor < 0)
        {
            suffix.Factor = DetermineAutomaticScalingFactor(suffix.Factor, absoluteValue);
        }

        // Don't include the 'i' if there's no scale prefix.
        suffix.HasIecChar = suffix.HasIecChar && suffix.Factor > 1;
        scaledValue /= suffix.Factor;
        return suffix;
    }

    private static void ValidateOptions(BinarySizeOptions options)
    {
        const BinarySizeOptions invalid =
            ~(BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits | BinarySizeOptions.AllowLongUnitsOnly);

        if ((options & invalid) != 0)
        {
            throw new ArgumentException(Properties.Resources.InvalidOptions, nameof(options));
        }
    }

    private static BinaryUnitInfo GetUnitInfo(IFormatProvider? provider)
    {
        // Only check current culture if provider was not specified. If it was but has no unit info,
        // we always use the invariant info.
        provider ??= CultureInfo.CurrentCulture;
        return (BinaryUnitInfo?)provider.GetFormat(typeof(BinaryUnitInfo)) ?? BinaryUnitInfo.InvariantInfo;
    }

    private static string? GetUnitScale(SuffixInfo suffix, BinaryUnitInfo unitInfo)
    {
        if (suffix.IsLong)
        {
            if (suffix.HasIecChar)
            {
                return suffix.Factor switch
                {
                    Kilo or BinarySize.Kibi => unitInfo.LongKibi,
                    Mega or BinarySize.Mebi => unitInfo.LongMebi,
                    Giga or BinarySize.Gibi => unitInfo.LongGibi,
                    Tera or BinarySize.Tebi => unitInfo.LongTebi,
                    Peta or BinarySize.Pebi => unitInfo.LongPebi,
                    Exa or BinarySize.Exbi => unitInfo.LongExbi,
                    _ => null,
                };
            }

            return suffix.Factor switch
            {
                Kilo or BinarySize.Kibi => unitInfo.LongKilo,
                Mega or BinarySize.Mebi => unitInfo.LongMega,
                Giga or BinarySize.Gibi => unitInfo.LongGiga,
                Tera or BinarySize.Tebi => unitInfo.LongTera,
                Peta or BinarySize.Pebi => unitInfo.LongPeta,
                Exa or BinarySize.Exbi => unitInfo.LongExa,
                _ => null,
            };
        }

        if (suffix.HasIecChar)
        {
            return suffix.Factor switch
            {
                Kilo or BinarySize.Kibi => unitInfo.ShortKibi,
                Mega or BinarySize.Mebi => unitInfo.ShortMebi,
                Giga or BinarySize.Gibi => unitInfo.ShortGibi,
                Tera or BinarySize.Tebi => unitInfo.ShortTebi,
                Peta or BinarySize.Pebi => unitInfo.ShortPebi,
                Exa or BinarySize.Exbi => unitInfo.ShortExbi,
                _ => null,
            };
        }

        return suffix.Factor switch
        {
            Kilo => unitInfo.ShortDecimalKilo,
            BinarySize.Kibi => unitInfo.ShortKilo,
            Mega or BinarySize.Mebi => unitInfo.ShortMega,
            Giga or BinarySize.Gibi => unitInfo.ShortGiga,
            Tera or BinarySize.Tebi => unitInfo.ShortTera,
            Peta or BinarySize.Pebi => unitInfo.ShortPeta,
            Exa or BinarySize.Exbi => unitInfo.ShortExa,
            _ => null,
        };
    }

}
