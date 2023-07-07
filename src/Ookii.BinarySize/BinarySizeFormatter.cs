// Copyright (c) Sven Groot (Ookii.org)
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ookii;

static class BinarySizeFormatter
{
    private static readonly Regex _formatRegex = new Regex(@"(?<before>\s*)(?<prefix>[ASKMGTP])?(?<iec>i?)(?<after>B?\s*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static string Format(BinarySize value, string? format, IFormatProvider? provider)
    {
        string? before = null;
        string? realPrefix;
        string? after;
        string? numberFormat = null;
        long factor;

        // Must support the "G" specifier, required by IFormattable
        if (string.IsNullOrEmpty(format) || string.Equals(format, "g", StringComparison.OrdinalIgnoreCase))
        {
            factor = DetermineAutomaticScalingFactor(value, false, out realPrefix);
            after = "B";
        }
        else
        {
            var m = _formatRegex.Match(format);
            if (!m.Success)
                throw new FormatException("Invalid format string.");

            before = m.Groups["before"].Value;
            var prefix = m.Groups["prefix"].Success ? m.Groups["prefix"].Value : null;
            var iec = m.Groups["iec"].Value;
            after = m.Groups["after"].Value;
            numberFormat = format!.Substring(0, m.Index);

            if (prefix == null)
            {
                realPrefix = null;
                factor = 1;
            }
            else if (prefix == "A" || prefix == "a")
                factor = DetermineAutomaticScalingFactor(value, false, out realPrefix);
            else if (prefix == "S" || prefix == "s")
                factor = DetermineAutomaticScalingFactor(value, true, out realPrefix);
            else
            {
                realPrefix = prefix;
                factor = BinarySize.GetUnitScalingFactor(prefix);
            }

            if (realPrefix != null && char.IsLower(prefix, 0))
                realPrefix = realPrefix.ToLower(CultureInfo.CurrentCulture);

            if (factor > 1)
                realPrefix += iec;
        }

        return (value.Value / (decimal)factor).ToString(numberFormat, provider) + before + realPrefix + after;
    }

    private static long DetermineAutomaticScalingFactor(BinarySize value, bool allowRounding, out string prefix)
    {
        if (value >= BinarySize.Pebi && (allowRounding || value.Value % BinarySize.Pebi == 0))
        {
            prefix = "P";
            return BinarySize.Pebi;
        }
        else if (value >= BinarySize.Tebi && (allowRounding || value.Value % BinarySize.Tebi == 0))
        {
            prefix = "T";
            return BinarySize.Tebi;
        }
        else if (value >= BinarySize.Gibi && (allowRounding || value.Value % BinarySize.Gibi == 0))
        {
            prefix = "G";
            return BinarySize.Gibi;
        }
        else if (value >= BinarySize.Mebi && (allowRounding || value.Value % BinarySize.Mebi == 0))
        {
            prefix = "M";
            return BinarySize.Mebi;
        }
        else if (value >= BinarySize.Kibi && (allowRounding || value.Value % BinarySize.Kibi == 0))
        {
            prefix = "K";
            return BinarySize.Kibi;
        }
        else
        {
            prefix = "";
            return 1;
        }
    }
}
