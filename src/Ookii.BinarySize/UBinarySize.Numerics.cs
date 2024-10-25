#if NET7_0_OR_GREATER

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Ookii;

partial struct UBinarySize : IBinaryNumber<UBinarySize>, IMinMaxValue<UBinarySize>
{
    static UBinarySize INumberBase<UBinarySize>.One => (UBinarySize)1;

    static int INumberBase<UBinarySize>.Radix => 2;

    static UBinarySize INumberBase<UBinarySize>.Zero => Zero;

    static UBinarySize INumberBase<UBinarySize>.Abs(UBinarySize value) => value;

    static bool INumberBase<UBinarySize>.IsCanonical(UBinarySize value) => true;

    static bool INumberBase<UBinarySize>.IsComplexNumber(UBinarySize value) => false;

    static bool INumberBase<UBinarySize>.IsEvenInteger(UBinarySize value) => ulong.IsEvenInteger(value.Value);

    static bool INumberBase<UBinarySize>.IsFinite(UBinarySize value) => true;

    static bool INumberBase<UBinarySize>.IsImaginaryNumber(UBinarySize value) => false;

    static bool INumberBase<UBinarySize>.IsInfinity(UBinarySize value) => false;

    static bool INumberBase<UBinarySize>.IsInteger(UBinarySize value) => true;

    static bool INumberBase<UBinarySize>.IsNaN(UBinarySize value) => false;

    static bool INumberBase<UBinarySize>.IsNegative(UBinarySize value) => false;

    static bool INumberBase<UBinarySize>.IsNegativeInfinity(UBinarySize value) => false;

    static bool INumberBase<UBinarySize>.IsNormal(UBinarySize value) => true;

    static bool INumberBase<UBinarySize>.IsOddInteger(UBinarySize value) => ulong.IsOddInteger(value.Value);

    static bool INumberBase<UBinarySize>.IsPositive(UBinarySize value) => true;

    static bool INumberBase<UBinarySize>.IsPositiveInfinity(UBinarySize value) => false;

    static bool IBinaryNumber<UBinarySize>.IsPow2(UBinarySize value) => ulong.IsPow2(value.Value);

    static bool INumberBase<UBinarySize>.IsRealNumber(UBinarySize value) => true;

    static bool INumberBase<UBinarySize>.IsSubnormal(UBinarySize value) => false;

    static bool INumberBase<UBinarySize>.IsZero(UBinarySize value) => value.Value == 0;

    static UBinarySize IBinaryNumber<UBinarySize>.Log2(UBinarySize value) => (UBinarySize)ulong.Log2(value.Value);

    static UBinarySize INumberBase<UBinarySize>.MaxMagnitude(UBinarySize x, UBinarySize y)
        => (UBinarySize)Math.Max(x.Value, y.Value);

    static UBinarySize INumberBase<UBinarySize>.MaxMagnitudeNumber(UBinarySize x, UBinarySize y)
        => (UBinarySize)Math.Max(x.Value, y.Value);

    static UBinarySize INumberBase<UBinarySize>.MinMagnitude(UBinarySize x, UBinarySize y)
        => (UBinarySize)Math.Min(x.Value, y.Value);

    static UBinarySize INumberBase<UBinarySize>.MinMagnitudeNumber(UBinarySize x, UBinarySize y)
        => (UBinarySize)Math.Min(x.Value, y.Value);

    static UBinarySize INumberBase<UBinarySize>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, style, provider);

    static UBinarySize INumberBase<UBinarySize>.Parse(string s, NumberStyles style, IFormatProvider? provider)
        => Parse(s, BinarySizeOptions.Default, style, provider);

    static bool INumberBase<UBinarySize>.TryConvertFromChecked<TOther>(TOther value, out UBinarySize result)
    {
        if (typeof(TOther) == typeof(ulong))
        {
            result = (UBinarySize)(ulong)(object)value;
            return true;
        }

        if (TryConvertFromCheckedHelper(value, out ulong longValue))
        {
            result = (UBinarySize)longValue;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<UBinarySize>.TryConvertFromSaturating<TOther>(TOther value, out UBinarySize result)
    {
        if (typeof(TOther) == typeof(ulong))
        {
            result = (UBinarySize)(ulong)(object)value;
            return true;
        }

        if (TryConvertFromSaturatingHelper(value, out ulong longValue))
        {
            result = (UBinarySize)longValue;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<UBinarySize>.TryConvertFromTruncating<TOther>(TOther value, out UBinarySize result)
    {
        if (typeof(TOther) == typeof(ulong))
        {
            result = (UBinarySize)(ulong)(object)value;
            return true;
        }

        if (TryConvertFromTruncatingHelper(value, out ulong longValue))
        {
            result = (UBinarySize)longValue;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<UBinarySize>.TryConvertToChecked<TOther>(UBinarySize value, [MaybeNullWhen(false)] out TOther result)
    {
        var longValue = value.Value;
        if (typeof(TOther) == typeof(ulong))
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

    static bool INumberBase<UBinarySize>.TryConvertToSaturating<TOther>(UBinarySize value, [MaybeNullWhen(false)] out TOther result)
    {
        var longValue = value.Value;
        if (typeof(TOther) == typeof(ulong))
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

    static bool INumberBase<UBinarySize>.TryConvertToTruncating<TOther>(UBinarySize value, [MaybeNullWhen(false)] out TOther result)
    {
        var longValue = value.Value;
        if (typeof(TOther) == typeof(ulong))
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

    static bool INumberBase<UBinarySize>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out UBinarySize result)
        => TryParse(s, BinarySizeOptions.Default, style, provider, out result);

    static bool INumberBase<UBinarySize>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out UBinarySize result)
        => TryParse(s, BinarySizeOptions.Default, style, provider, out result);

    static UBinarySize IAdditiveIdentity<UBinarySize, UBinarySize>.AdditiveIdentity => Zero;

    static UBinarySize IMultiplicativeIdentity<UBinarySize, UBinarySize>.MultiplicativeIdentity => (UBinarySize)1;

    static UBinarySize IMinMaxValue<UBinarySize>.MaxValue => MaxValue;

    static UBinarySize IMinMaxValue<UBinarySize>.MinValue => MinValue;

    /// <summary>
    /// Negates a <see cref="UBinarySize"/> value.
    /// </summary>
    /// <param name="value">The value to negate.</param>
    /// <returns>
    /// The negation of <paramref name="value"/>.
    /// </returns>
    static UBinarySize IUnaryNegationOperators<UBinarySize, UBinarySize>.operator -(UBinarySize value) => (UBinarySize)(0UL - value.Value);

    private static bool TryConvertFrom<TOther>(TOther value, out UBinarySize result)
        where TOther : INumberBase<TOther>
    {
        // For simplicity, we only support conversion from ulong.
        // N.B. I would delegate this to INumberBase<ulong>.TryConvertFrom*, but those methods are
        //      protected so that's not possible.
        if (typeof(TOther) == typeof(ulong))
        {
            result = (UBinarySize)(ulong)(object)value;
            return true;
        }



        result = default;
        return false;
    }

    private static bool TryConvertTo<TOther>(UBinarySize value, [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        // For simplicity, we only support conversion to ulong.
        // N.B. I would delegate this to INumberBase<ulong>.TryConvertTo*, but those methods are
        //      protected so that's not possible.
        if (typeof(TOther) == typeof(ulong))
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
