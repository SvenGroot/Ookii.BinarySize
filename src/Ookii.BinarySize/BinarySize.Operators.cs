using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ookii;

partial struct BinarySize
{
    #region Methods

    /// <summary>
    /// Returns the sum of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The first value to add.</param>
    /// <param name="right">The second value to add.</param>
    /// <returns>
    /// The sum of the <paramref name="left"/> and <paramref name="right"/> parameters.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    public static BinarySize Add(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value + right.Value);

    /// <summary>
    /// Subtracts one <see cref="BinarySize"/> value from another.
    /// </summary>
    /// <param name="left">The value to subtract from.</param>
    /// <param name="right">The value to subtract.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    public static BinarySize Subtract(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value - right.Value);

    /// <summary>
    /// Returns the product of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The first value to multiply.</param>
    /// <param name="right">The second value to multiply.</param>
    /// <returns>
    /// The product of the <paramref name="left"/> and <paramref name="right"/> parameters.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    public static BinarySize Multiply(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value * right.Value);

    /// <summary>
    /// Divides one <see cref="BinarySize"/> value by another.
    /// </summary>
    /// <param name="left">The value to be divided.</param>
    /// <param name="right">The value to divide by.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize Divide(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value / right.Value);

    /// <summary>
    /// Returns the remainder of dividing one <see cref="BinarySize"/> value by another.
    /// </summary>
    /// <param name="left">The value to be divided.</param>
    /// <param name="right">The value to divide by.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize Remainder(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value % right.Value);

    /// <summary>
    /// Returns the negation of a <see cref="BinarySize"/> value.
    /// </summary>
    /// <param name="value">The value to negate.</param>
    /// <returns>The negation of <paramref name="value"/>.</returns>
    public static BinarySize Negate(BinarySize value) => (BinarySize)(-value.Value);

    #endregion

    #region Comparison

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
    /// Determines whether a <see cref="BinarySize"/> value is the same as a <see cref="long"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="long"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(BinarySize left, long right) => left.Value == right;

    /// <summary>
    /// Determines whether a <see cref="BinarySize"/> value is different from a <see cref="long"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="long"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the different from the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(BinarySize left, long right) => left.Value != right;

    /// <summary>
    /// Determines whether a <see cref="long"/> value is the same as a <see cref="BinarySize"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="long"/> to compare.</param>
    /// <param name="right">The <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(long left, BinarySize right) => left == right.Value;

    /// <summary>
    /// Determines whether a <see cref="long"/> value is different from a <see cref="BinarySize"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="long"/> to compare.</param>
    /// <param name="right">The <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the different from the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(long left, BinarySize right) => left != right.Value;

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
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is less than a
    /// <see cref="long"/>.
    /// </summary>
    /// <param name="left">The <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="long"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(BinarySize left, long right) => left.Value < right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="long"/> is less than a
    /// <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="long"/> to compare.</param>
    /// <param name="right">The <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(long left, BinarySize right) => left < right.Value;

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
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is less than or
    /// equal to a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="long"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(BinarySize left, long right) => left.Value <= right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="long"/> is less than or
    /// equal to a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="long"/> to compare.</param>
    /// <param name="right">The <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(long left, BinarySize right) => left <= right.Value;

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
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is greater than a
    /// <see cref="long"/>.
    /// </summary>
    /// <param name="left">The <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="long"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(BinarySize left, long right) => left.Value > right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="long"/> is greater than a
    /// <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="long"/> to compare.</param>
    /// <param name="right">The <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(long left, BinarySize right) => left > right.Value;

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
    /// Returns a value indicating whether a specified <see cref="BinarySize"/> is greater than or
    /// equal to a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The <see cref="BinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="long"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to
    /// the value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(BinarySize left, long right) => left.Value >= right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="long"/> is greater than or
    /// equal to a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="long"/> to compare.</param>
    /// <param name="right">The <see cref="BinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to
    /// the value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(long left, BinarySize right) => left >= right.Value;

    #endregion

    #region Addition

    /// <summary>
    /// Adds two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator +(BinarySize left, BinarySize right) => (BinarySize)(left.Value + right.Value);

    /// <summary>
    /// Adds a <see cref="BinarySize"/> value to a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator +(BinarySize left, long right) => (BinarySize)(left.Value + right);

    /// <summary>
    /// Adds a <see cref="long"/> value to a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator +(long left, BinarySize right) => (BinarySize)(left + right.Value);

    /// <summary>
    /// Adds two <see cref="BinarySize"/> values in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    public static BinarySize operator checked +(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value + right.Value);

    /// <summary>
    /// Adds a <see cref="BinarySize"/> value to a <see cref="long"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator checked +(BinarySize left, long right) => (BinarySize)checked(left.Value + right);

    /// <summary>
    /// Adds a <see cref="long"/> value to a <see cref="BinarySize"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator checked +(long left, BinarySize right) => (BinarySize)checked(left + right.Value);

    #endregion

    #region Subtraction

    /// <summary>
    /// Subtracts two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static BinarySize operator -(BinarySize left, BinarySize right) => (BinarySize)(left.Value - right.Value);

    /// <summary>
    /// Subtracts a <see cref="long"/> value from a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static BinarySize operator -(BinarySize left, long right) => (BinarySize)(left.Value - right);

    /// <summary>
    /// Subtracts a <see cref="BinarySize"/> value from a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static BinarySize operator -(long left, BinarySize right) => (BinarySize)(left - right.Value);

    /// <summary>
    /// Subtracts two <see cref="BinarySize"/> values in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    public static BinarySize operator checked -(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value - right.Value);

    /// <summary>
    /// Subtracts a <see cref="long"/> value from a <see cref="BinarySize"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static BinarySize operator checked  -(BinarySize left, long right) => (BinarySize)checked(left.Value - right);

    /// <summary>
    /// Subtracts a <see cref="BinarySize"/> value from a <see cref="long"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static BinarySize operator checked -(long left, BinarySize right) => (BinarySize)checked(left - right.Value);

    #endregion

    #region Multiplication

    /// <summary>
    /// Multiplies two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator *(BinarySize left, BinarySize right) => (BinarySize)(left.Value * right.Value);

    /// <summary>
    /// Multiplies a <see cref="BinarySize"/> value by a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator *(BinarySize left, long right) => (BinarySize)(left.Value * right);

    /// <summary>
    /// Multiplies a <see cref="long"/> value by a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator *(long left, BinarySize right) => (BinarySize)(left * right.Value);

    /// <summary>
    /// Multiplies two <see cref="BinarySize"/> values in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/> or less than
    /// <see cref="MinValue"/>.
    /// </exception>
    public static BinarySize operator checked *(BinarySize left, BinarySize right) => (BinarySize)checked(left.Value * right.Value);

    /// <summary>
    /// Multiplies a <see cref="BinarySize"/> value by a <see cref="long"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator checked  *(BinarySize left, long right) => (BinarySize)checked(left.Value * right);

    /// <summary>
    /// Multiplies a <see cref="long"/> value by a <see cref="BinarySize"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static BinarySize operator checked  *(long left, BinarySize right) => (BinarySize)checked(left * right.Value);

    #endregion

    #region Division

    /// <summary>
    /// Divides two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize operator /(BinarySize left, BinarySize right) => (BinarySize)(left.Value / right.Value);

    /// <summary>
    /// Divides a <see cref="BinarySize"/> value by a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize operator /(BinarySize left, long right) => (BinarySize)(left.Value / right);

    /// <summary>
    /// Divides a <see cref="long"/> value by a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize operator /(long left, BinarySize right) => (BinarySize)(left / right.Value);

    #endregion

    #region Remainder

    /// <summary>
    /// Returns the remainder after dividing two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize operator %(BinarySize left, BinarySize right) => (BinarySize)(left.Value % right.Value);

    /// <summary>
    /// Returns the remainder after dividing a <see cref="BinarySize"/> value by a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize operator %(BinarySize left, long right) => (BinarySize)(left.Value % right);

    /// <summary>
    /// Returns the remainder after dividing a <see cref="long"/> value by a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static BinarySize operator %(long left, BinarySize right) => (BinarySize)(left % right.Value);

    #endregion

    #region Unary

    /// <summary>
    /// Negates a <see cref="BinarySize"/> value.
    /// </summary>
    /// <param name="value">The value to negate.</param>
    /// <returns>
    /// The negation of <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator -(BinarySize value) => (BinarySize)(-value.Value);

    /// <summary>
    /// Returns the specified instance of <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value to return.</param>
    /// <returns>
    /// The value of <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator +(BinarySize value) => value;

    /// <summary>
    /// Increments a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value to increment.</param>
    /// <returns>
    /// The result of incrementing <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator ++(BinarySize value) => (BinarySize)(value.Value + 1);

    /// <summary>
    /// Increments a <see cref="BinarySize"/> in a checked context.
    /// </summary>
    /// <param name="value">The value to increment.</param>
    /// <returns>
    /// The result of incrementing <paramref name="value"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/>.
    /// </exception>
    public static BinarySize operator checked ++(BinarySize value) => (BinarySize)checked(value.Value + 1);

    /// <summary>
    /// Decrements a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value to decrement.</param>
    /// <returns>
    /// The result of decrementing <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator --(BinarySize value) => (BinarySize)(value.Value - 1);

    /// <summary>
    /// Decrements a <see cref="BinarySize"/> in a checked context.
    /// </summary>
    /// <param name="value">The value to decrement.</param>
    /// <returns>
    /// The result of decrementing <paramref name="value"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is less than <see cref="MinValue"/>.
    /// </exception>
    public static BinarySize operator checked --(BinarySize value) => (BinarySize)checked(value.Value - 1);

    #endregion

    #region Shift

    /// <summary>
    /// Shifts the bits of a <see cref="BinarySize"/> to the right.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> right by <paramref name="shift"/> bits.
    /// </returns>
    public static BinarySize operator >>(BinarySize value, int shift) => (BinarySize)(value.Value >> shift);

    /// <summary>
    /// Shifts the bits of a <see cref="BinarySize"/> to the right in an unsigned manner.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> right by <paramref name="shift"/> bits
    /// without considering the sign.
    /// </returns>
    public static BinarySize operator >>>(BinarySize value, int shift) => (BinarySize)(value.Value >>> shift);

    /// <summary>
    /// Shifts the bits of a <see cref="BinarySize"/> to the left.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> left by <paramref name="shift"/> bits.
    /// </returns>
    public static BinarySize operator <<(BinarySize value, int shift) => (BinarySize)(value.Value << shift);

    #endregion

    #region Bitwise

    /// <summary>
    /// Computes the ones-complement representation of a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="value">The value for which to compute the ones-complement.</param>
    /// <returns>
    /// The ones-complement of <paramref name="value"/>.
    /// </returns>
    public static BinarySize operator ~(BinarySize value) => (BinarySize)(~value.Value);

    /// <summary>
    /// Computes the bitwise-and of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>and</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>and</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator &(BinarySize left, BinarySize right) => (BinarySize)(left.Value & right.Value);

    /// <summary>
    /// Computes the bitwise-and of a <see cref="BinarySize"/> value and a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The value to <c>and</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>and</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator &(BinarySize left, long right) => (BinarySize)(left.Value & right);

    /// <summary>
    /// Computes the bitwise-and of a <see cref="long"/> value and a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The value to <c>and</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>and</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator &(long left, BinarySize right) => (BinarySize)(left & right.Value);

    /// <summary>
    /// Computes the bitwise-or of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>or</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>or</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator |(BinarySize left, BinarySize right) => (BinarySize)(left.Value | right.Value);

    /// <summary>
    /// Computes the bitwise-or of a <see cref="BinarySize"/> value and a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The value to <c>or</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>or</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator |(BinarySize left, long right) => (BinarySize)(left.Value | right);

    /// <summary>
    /// Computes the bitwise-or of a <see cref="long"/> value and a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The value to <c>or</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>or</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator |(long left, BinarySize right) => (BinarySize)(left | right.Value);

    /// <summary>
    /// Computes the exclusive-or of two <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>xor</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>xor</c> with <paramref name="left"/>.</param>
    /// <returns>The exclusive-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator ^(BinarySize left, BinarySize right) => (BinarySize)(left.Value ^ right.Value);

    /// <summary>
    /// Computes the exclusive-or of a <see cref="BinarySize"/> value and a <see cref="long"/>.
    /// </summary>
    /// <param name="left">The value to <c>xor</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>xor</c> with <paramref name="left"/>.</param>
    /// <returns>The exclusive-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator ^(BinarySize left, long right) => (BinarySize)(left.Value ^ right);

    /// <summary>
    /// Computes the exclusive-or of a <see cref="long"/> value and a <see cref="BinarySize"/>.
    /// </summary>
    /// <param name="left">The value to <c>xor</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>xor</c> with <paramref name="left"/>.</param>
    /// <returns>The exclusive-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static BinarySize operator ^(long left, BinarySize right) => (BinarySize)(left ^ right.Value);

    #endregion

    #region Conversion

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
    public static explicit operator BinarySize(long value) => new(value);

    #endregion
}
