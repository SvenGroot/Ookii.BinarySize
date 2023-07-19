#if NET7_0_OR_GREATER

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Ookii;

partial struct BinarySize : IBinaryNumber<BinarySize>, ISignedNumber<BinarySize>, IMinMaxValue<BinarySize>
{
    static BinarySize INumberBase<BinarySize>.One => (BinarySize)1;

    static int INumberBase<BinarySize>.Radix => 2;

    static BinarySize INumberBase<BinarySize>.Zero => Zero;

    static BinarySize ISignedNumber<BinarySize>.NegativeOne => (BinarySize)(-1);

    static BinarySize INumberBase<BinarySize>.Abs(BinarySize value) => (BinarySize)long.Abs(value.Value);

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

    static BinarySize IBinaryNumber<BinarySize>.Log2(BinarySize value) => (BinarySize)long.Log2(value.Value);

    static BinarySize INumberBase<BinarySize>.MaxMagnitude(BinarySize x, BinarySize y) => (BinarySize)long.MaxMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.MaxMagnitudeNumber(BinarySize x, BinarySize y)
        => (BinarySize)long.MaxMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.MinMagnitude(BinarySize x, BinarySize y) => (BinarySize)long.MinMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.MinMagnitudeNumber(BinarySize x, BinarySize y)
        => (BinarySize)long.MinMagnitude(x.Value, y.Value);

    static BinarySize INumberBase<BinarySize>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, style, provider);

    static BinarySize INumberBase<BinarySize>.Parse(string s, NumberStyles style, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, style, provider);

    static bool INumberBase<BinarySize>.TryConvertFromChecked<TOther>(TOther value, out BinarySize result)
    {
        if (typeof(TOther) == typeof(long))
        {
            result = (BinarySize)(long)(object)value;
            return true;
        }

        if (TryConvertFromCheckedHelper(value, out long longValue))
        {
            result = (BinarySize)longValue;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<BinarySize>.TryConvertFromSaturating<TOther>(TOther value, out BinarySize result)
    {
        if (typeof(TOther) == typeof(long))
        {
            result = (BinarySize)(long)(object)value;
            return true;
        }

        if (TryConvertFromSaturatingHelper(value, out long longValue))
        {
            result = (BinarySize)longValue;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<BinarySize>.TryConvertFromTruncating<TOther>(TOther value, out BinarySize result)
    {
        if (typeof(TOther) == typeof(long))
        {
            result = (BinarySize)(long)(object)value;
            return true;
        }

        if (TryConvertFromTruncatingHelper(value, out long longValue))
        {
            result = (BinarySize)longValue;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<BinarySize>.TryConvertToChecked<TOther>(BinarySize value, [MaybeNullWhen(false)] out TOther result)
    {
        var longValue = value.Value;
        if (typeof(TOther) == typeof(long))
        {
            result = (TOther)(object)longValue;
            return true;
        }

        if (TryConvertToCheckedHelper(longValue, out result))
        {
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<BinarySize>.TryConvertToSaturating<TOther>(BinarySize value, [MaybeNullWhen(false)] out TOther result)
    {
        var longValue = value.Value;
        if (typeof(TOther) == typeof(long))
        {
            result = (TOther)(object)longValue;
            return true;
        }

        if (TryConvertToSaturatingHelper(longValue, out result))
        {
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<BinarySize>.TryConvertToTruncating<TOther>(BinarySize value, [MaybeNullWhen(false)] out TOther result)
    {
        var longValue = value.Value;
        if (typeof(TOther) == typeof(long))
        {
            result = (TOther)(object)longValue;
            return true;
        }

        if (TryConvertToTruncatingHelper(longValue, out result))
        {
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<BinarySize>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
        => TryParse(s, BinarySizeOptions.Default, style, provider, out result);

    static bool INumberBase<BinarySize>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out BinarySize result)
        => TryParse(s, BinarySizeOptions.Default, style, provider, out result);

    static BinarySize IAdditiveIdentity<BinarySize, BinarySize>.AdditiveIdentity => Zero;

    static BinarySize IMultiplicativeIdentity<BinarySize, BinarySize>.MultiplicativeIdentity => (BinarySize)1;

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
            result = (BinarySize)(long)(object)value;
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

    // The various TryConvert functions can only be accessed through a generic type parameter, so
    // these helper functions let us use them.
    private static bool TryConvertFromCheckedHelper<T, TOther>(TOther value, [MaybeNullWhen(false)] out T result)
        where T : INumberBase<T>
        where TOther : INumberBase<TOther>
        => T.TryConvertFromChecked(value, out result);

    private static bool TryConvertFromSaturatingHelper<T, TOther>(TOther value, [MaybeNullWhen(false)] out T result)
        where T : INumberBase<T>
        where TOther : INumberBase<TOther>
        => T.TryConvertFromSaturating(value, out result);

    private static bool TryConvertFromTruncatingHelper<T, TOther>(TOther value, [MaybeNullWhen(false)] out T result)
        where T : INumberBase<T>
        where TOther : INumberBase<TOther>
        => T.TryConvertFromTruncating(value, out result);

    private static bool TryConvertToCheckedHelper<T, TOther>(T value, [MaybeNullWhen(false)] out TOther result)
        where T : INumberBase<T>
        where TOther : INumberBase<TOther>
        => T.TryConvertToChecked(value, out result);

    private static bool TryConvertToSaturatingHelper<T, TOther>(T value, [MaybeNullWhen(false)] out TOther result)
        where T : INumberBase<T>
        where TOther : INumberBase<TOther>
        => T.TryConvertToSaturating(value, out result);

    private static bool TryConvertToTruncatingHelper<T, TOther>(T value, [MaybeNullWhen(false)] out TOther result)
        where T : INumberBase<T>
        where TOther : INumberBase<TOther>
        => T.TryConvertToTruncating(value, out result);
}

#endif
