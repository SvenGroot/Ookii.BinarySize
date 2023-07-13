using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ookii;

partial struct BinarySize
{
    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the sum of the specified
    /// <see cref="BinarySize"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns>
    /// A new object that represents the value of this instance plus <paramref name="value"/>.
    /// </returns>
    public BinarySize Add(BinarySize value) => checked(Value + value.Value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the difference of the specified
    /// <see cref="BinarySize"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to subtract.</param>
    /// <returns>
    /// A new object that represents the value of this instance minus <paramref name="value"/>.
    /// </returns>
    public BinarySize Subtract(BinarySize value) => checked(Value - value.Value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the product of the specified
    /// <see cref="long"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to multiply by.</param>
    /// <returns>
    /// A new object that represents the value of this instance times <paramref name="value"/>.
    /// </returns>
    public BinarySize Multiply(long value) => checked(Value * value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the division of the specified
    /// <see cref="long"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to divide by.</param>
    /// <returns>
    /// A new object that represents the value of this instance divided by <paramref name="value"/>.
    /// </returns>
    public BinarySize Divide(long value) => checked(Value / value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the remainder of the division
    /// of the specified <see cref="long"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to divide by.</param>
    /// <returns>
    /// A new object that represents the remainder of the value of this instance divided by
    /// <paramref name="value"/>.
    /// </returns>
    public BinarySize Remainder(long value) => checked(Value % value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the negation of this instance.
    /// </summary>
    /// <returns>A new object that represents the negated value of this instance.</returns>
    public static BinarySize Negate(BinarySize size) => -size.Value;

    /// <summary>
    /// Determines whether two specified <see cref="BinarySize"/> values are the same.
    /// </summary>
    /// <param name="left">The first <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(BinarySize left, BinarySize right) => left.Value == right.Value;

    /// <summary>
    /// Determines whether two specified <see cref="BinarySize"/> values are different.
    /// </summary>
    /// <param name="left">The first <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
    /// of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(BinarySize left, BinarySize right) => left.Value != right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is less than another
    /// <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(BinarySize left, BinarySize right) => left.Value < right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is less than or
    /// equal to another <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref
    /// name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(BinarySize left, BinarySize right) => left.Value <= right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is greater than
    /// another <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(BinarySize left, BinarySize right) => left.Value > right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is greater than or
    /// equal to another <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than or equal to
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(BinarySize left, BinarySize right) => left.Value >= right.Value;

    /// <summary>
    /// Adds two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator +(BinarySize left, BinarySize right) => left.Value + right.Value;

    /// <summary>
    /// Adds two <see cref="BinarySize"/> values in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator checked +(BinarySize left, BinarySize right) => checked(left.Value + right.Value);

    /// <summary>
    /// Subtracts two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static BinarySize operator -(BinarySize left, BinarySize right) => left.Value - right.Value;

    /// <summary>
    /// Subtracts two <see cref="BinarySize"/> values in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static BinarySize operator checked -(BinarySize left, BinarySize right) => checked(left.Value - right.Value);

    /// <summary>
    /// Multiplies two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator *(BinarySize left, BinarySize right) => left.Value * right.Value;

    /// <summary>
    /// Multiplies two <see cref="BinarySize"/> values in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator checked *(BinarySize left, BinarySize right) => checked(left.Value * right.Value);

    /// <summary>
    /// Divides two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator /(BinarySize left, BinarySize right) => left.Value / right.Value;

    /// <summary>
    /// Returns the remainder after dividing two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator %(BinarySize left, BinarySize right) => left.Value % right.Value;

    /// <summary>
    /// Negates a <see cref="BinarySize"/> value.
    /// </summary>
    /// <param name="value">The value to negate.</param>
    /// <returns>
    /// The negation of <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator -(BinarySize value) => -value.Value;

    /// <summary>
    /// Returns the specified instance of <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value to return.</param>
    /// <returns>
    /// The value of <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator +(BinarySize value) => value.Value;

    /// <summary>
    /// Shifts the bits of a <see cref="BinarySize"/> to the right.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> right by <paramref name="shift"/> bits.
    /// </returns>
    public static BinarySize operator >>(BinarySize value, int shift) => value.Value >> shift;

    /// <summary>
    /// Shifts the bits of a <see cref="BinarySize"/> to the right in an unsigned manner.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> right by <paramref name="shift"/> bits
    /// without considering the sign.
    /// </returns>
    public static BinarySize operator >>>(BinarySize value, int shift) => value.Value >>> shift;

    /// <summary>
    /// Shifts the bits of a <see cref="BinarySize"/> to the left.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> left by <paramref name="shift"/> bits.
    /// </returns>
    public static BinarySize operator <<(BinarySize value, int shift) => value.Value << shift;

    /// <summary>
    /// Computes the ones-complement representation of a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value for which to compute the ones-complement.</param>
    /// <returns>
    /// The ones-complement of <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator ~(BinarySize value) => ~value.Value;

    /// <summary>
    /// Increments a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value to increment.</param>
    /// <returns>
    /// The result of incrementing <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator ++(BinarySize value) => value.Value + 1;

    /// <summary>
    /// Increments a <see cref="BinarySize"/> in a checked context.
    /// </summary>
    /// <param name="value">The value to increment.</param>
    /// <returns>
    /// The result of incrementing <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator checked ++(BinarySize value) => checked(value.Value + 1);

    /// <summary>
    /// Decrements a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value to decrement.</param>
    /// <returns>
    /// The result of decrementing <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator --(BinarySize value) => value.Value - 1;

    /// <summary>
    /// Decrements a <see cref="BinarySize"/> in a checked context.
    /// </summary>
    /// <param name="value">The value to decrement.</param>
    /// <returns>
    /// The result of decrementing <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator checked --(BinarySize value) => checked(value.Value - 1);

    /// <summary>
    /// Computes the bitwise-and of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>and</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>and</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator &(BinarySize left, BinarySize right) => left.Value & right.Value;

    /// <summary>
    /// Computes the bitwise-or of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>or</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>or</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator |(BinarySize left, BinarySize right) => left.Value | right.Value;

    /// <summary>
    /// Computes the exclusive-or of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>xor</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>xor</c> with <paramref name="left"/>.</param>
    /// <returns>The exclusive-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator ^(BinarySize left, BinarySize right) => left.Value ^ right.Value;

    /// <summary>
    /// Performs an explicit conversion from <see cref="BinarySize"/> to <see cref="long"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The value of the <see cref="Value"/> property.
    /// </returns>
    public static explicit operator long(BinarySize value) => value.Value;

    /// <summary>
    /// Performs an implicit conversion from <see cref="long"/> to <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// A <see cref="BinarySize"/> where the <see cref="Value"/> property equals
    /// <paramref name="value"/>.
    /// </returns>
    public static implicit operator BinarySize(long value) => new(value);
}
