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
    /// Returns a new <see cref="BinarySize"/> object whose value is the sum of the specified <see cref="BinarySize"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns>A new object that represents the value of this instance plus <paramref name="value"/>.</returns>
    public static BinarySize Add(BinarySize size1, BinarySize size2) => checked(size1.Value + size2.Value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the difference of the specified <see cref="BinarySize"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to subtract.</param>
    /// <returns>A new object that represents the value of this instance minus <paramref name="value"/>.</returns>
    public static BinarySize Subtract(BinarySize size1, BinarySize size2) => checked(size1.Value - size2.Value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the product of the specified <see cref="Int64"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to multiply by.</param>
    /// <returns>A new object that represents the value of this instance times <paramref name="value"/>.</returns>
    public static BinarySize Multiply(BinarySize size1, BinarySize size2) => checked(size1.Value * size2.Value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the division of the specified <see cref="Int64"/> object and this instance.
    /// </summary>
    /// <param name="value">The value to divide by.</param>
    /// <returns>A new object that represents the value of this instance divided by <paramref name="value"/>.</returns>
    public static BinarySize Divide(BinarySize size1, BinarySize size2) => checked(size1.Value / size2.Value);

    public static BinarySize Remainder(BinarySize size1, BinarySize size2) => checked(size1.Value % size2.Value);

    /// <summary>
    /// Returns a new <see cref="BinarySize"/> object whose value is the negation of this instance.
    /// </summary>
    /// <returns>A new object that represents the negated value of this instance.</returns>
    public static BinarySize Negate(BinarySize size) => -size.Value;

    /// <summary>
    /// Determines whether two specified <see cref="BinarySize"/> values are the same.
    /// </summary>
    /// <param name="left">A <see cref="BinarySize"/>.</param>
    /// <param name="right">A <see cref="BinarySize"/>.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(BinarySize left, BinarySize right) => left.Value == right.Value;

    /// <summary>
    /// Determines whether two specified <see cref="BinarySize"/> values are different.
    /// </summary>
    /// <param name="left">A <see cref="BinarySize"/>.</param>
    /// <param name="right">A <see cref="BinarySize"/>.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(BinarySize left, BinarySize right) => left.Value != right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is less than another <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">A <see cref="BinarySize"/>.</param>
    /// <param name="right">A <see cref="BinarySize"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(BinarySize left, BinarySize right) => left.Value < right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is less than or equal to another <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">A <see cref="BinarySize"/>.</param>
    /// <param name="right">A <see cref="BinarySize"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(BinarySize left, BinarySize right) => left.Value <= right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is greater than another <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">A <see cref="BinarySize"/>.</param>
    /// <param name="right">A <see cref="BinarySize"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(BinarySize left, BinarySize right) => left.Value > right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is greater than or equal to another <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">A <see cref="BinarySize"/>.</param>
    /// <param name="right">A <see cref="BinarySize"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(BinarySize left, BinarySize right) => left.Value >= right.Value;

    /// <summary>
    /// Implements the operator +.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static BinarySize operator +(BinarySize left, BinarySize right) => left.Value + right.Value;

    /// <summary>
    /// Implements the operator -.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static BinarySize operator -(BinarySize left, BinarySize right) => left.Value - right.Value;

    /// <summary>
    /// Implements the operator *.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static BinarySize operator *(BinarySize left, BinarySize right) => left.Value * right.Value;

    /// <summary>
    /// Implements the operator /.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static BinarySize operator /(BinarySize left, BinarySize right) => left.Value / right.Value;

    public static BinarySize operator %(BinarySize left, BinarySize right) => left.Value % right.Value;

    /// <summary>
    /// Implements the unary operator -.
    /// </summary>
    /// <param name="value">The operand.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static BinarySize operator -(BinarySize value) => -value.Value;

    public static BinarySize operator +(BinarySize value) => value.Value;

    public static BinarySize operator >>(BinarySize value, int shift) => value.Value >> shift;

    public static BinarySize operator <<(BinarySize value, int shift) => value.Value << shift;

    /// <summary>
    /// Performs an explicit conversion from <see cref="Ookii.Jumbo.BinarySize"/> to <see cref="System.Int16"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static explicit operator long(BinarySize value) => value.Value;

    /// <summary>
    /// Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="Ookii.Jumbo.BinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator BinarySize(long value) => new(value);
}
