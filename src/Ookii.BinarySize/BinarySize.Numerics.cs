#if NET7_0_OR_GREATER

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Ookii;

partial struct BinarySize : IBinaryNumber<BinarySize>, ISignedNumber<BinarySize>, IMinMaxValue<BinarySize>
{
    static BinarySize INumberBase<BinarySize>.One => 1;

    static int INumberBase<BinarySize>.Radix => 2;

    static BinarySize INumberBase<BinarySize>.Zero => Zero;

    static BinarySize ISignedNumber<BinarySize>.NegativeOne => -1;

    static BinarySize INumberBase<BinarySize>.Abs(BinarySize value) => long.Abs(value.Value);

    static bool INumberBase<BinarySize>.IsCanonical(BinarySize value) => true;

    static bool INumberBase<BinarySize>.IsComplexNumber(BinarySize value) => false;

    static bool INumberBase<BinarySize>.IsEvenInteger(BinarySize value) => long.IsEvenInteger(value.Value);

    static bool INumberBase<BinarySize>.IsFinite(BinarySize value) => true;

    static bool INumberBase<BinarySize>.IsImaginaryNumber(BinarySize value) => false;

    static bool INumberBase<BinarySize>.IsInfinity(BinarySize value) => false;

    static bool INumberBase<BinarySize>.IsInteger(BinarySize value) => true;

    static bool INumberBase<BinarySize>.IsNaN(BinarySize value) => false;

    static bool INumberBase<BinarySize>.IsNegative(BinarySize value) => long.IsNegative(value.Value);

    static bool INumberBase<BinarySize>.IsNegativeInfinity(BinarySize value) => false;

    static bool INumberBase<BinarySize>.IsNormal(BinarySize value) => true;

    static bool INumberBase<BinarySize>.IsOddInteger(BinarySize value) => long.IsOddInteger(value.Value);

    static bool INumberBase<BinarySize>.IsPositive(BinarySize value) => long.IsPositive(value.Value);

    static bool INumberBase<BinarySize>.IsPositiveInfinity(BinarySize value) => false;

    static bool IBinaryNumber<BinarySize>.IsPow2(BinarySize value) => long.IsPow2(value.Value);

    static bool INumberBase<BinarySize>.IsRealNumber(BinarySize value) => true;

    static bool INumberBase<BinarySize>.IsSubnormal(BinarySize value) => false;

    static bool INumberBase<BinarySize>.IsZero(BinarySize value) => value.Value == 0;

    static BinarySize IBinaryNumber<BinarySize>.Log2(BinarySize value) => long.Log2(value.Value);

    static BinarySize INumberBase<BinarySize>.MaxMagnitude(BinarySize x, BinarySize y) => long.MaxMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.MaxMagnitudeNumber(BinarySize x, BinarySize y)
        => long.MaxMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.MinMagnitude(BinarySize x, BinarySize y) => long.MinMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.MinMagnitudeNumber(BinarySize x, BinarySize y)
        => long.MinMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, style, provider);

    static BinarySize INumberBase<BinarySize>.Parse(string s, NumberStyles style, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, style, provider);

    static bool INumberBase<BinarySize>.TryConvertFromChecked<TOther>(TOther value, out BinarySize result)
        => TryConvertFrom(value, out result);

    static bool INumberBase<BinarySize>.TryConvertFromSaturating<TOther>(TOther value, out BinarySize result)
        => TryConvertFrom(value, out result);

    static bool INumberBase<BinarySize>.TryConvertFromTruncating<TOther>(TOther value, out BinarySize result)
        => TryConvertFrom(value, out result);

    static bool INumberBase<BinarySize>.TryConvertToChecked<TOther>(BinarySize value, [MaybeNullWhen(false)] out TOther result)
        => TryConvertTo(value, out result);

    static bool INumberBase<BinarySize>.TryConvertToSaturating<TOther>(BinarySize value, [MaybeNullWhen(false)] out TOther result)
        => TryConvertTo(value, out result);

    static bool INumberBase<BinarySize>.TryConvertToTruncating<TOther>(BinarySize value, [MaybeNullWhen(false)] out TOther result)
        => TryConvertTo(value, out result);

    static bool INumberBase<BinarySize>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
        => TryParse(s, BinarySizeOptions.Default, style, provider, out result);

    static bool INumberBase<BinarySize>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
        => TryParse(s, BinarySizeOptions.Default, style, provider, out result);

    static BinarySize IAdditiveIdentity<BinarySize, BinarySize>.AdditiveIdentity => Zero;

    static BinarySize IMultiplicativeIdentity<BinarySize, BinarySize>.MultiplicativeIdentity => 1;

    static BinarySize IMinMaxValue<BinarySize>.MaxValue => MaxValue;

    static BinarySize IMinMaxValue<BinarySize>.MinValue => MinValue;

    private static bool TryConvertFrom<TOther>(TOther value, out BinarySize result)
        where TOther : INumberBase<TOther>
    {
        // For simplicity, we only support conversion from long.
        // N.B. I would delegate this to INumberBase<long>.TryConvertFrom*, but those methods are
        //      protected so that's not possible.
        if (typeof(TOther) == typeof(long))
        {
            result = (long)(object)value;
            return true;
        }

        result = default;
        return false;
    }

    private static bool TryConvertTo<TOther>(BinarySize value, [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        // For simplicity, we only support conversion to long.
        // N.B. I would delegate this to INumberBase<long>.TryConvertTo*, but those methods are
        //      protected so that's not possible.
        if (typeof(TOther) == typeof(long))
        {
            result = (TOther)(object)value.Value;
            return true;
        }

        result = default;
        return false;
    }
}

#endif
