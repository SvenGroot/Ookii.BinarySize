using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ookii;

partial struct UBinarySize
{
    #region Methods

    /// <summary>
    /// Returns the sum of two <see cref="UBinarySize"/> values.
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
    public static UBinarySize Add(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value + right.Value);

    /// <summary>
    /// Subtracts one <see cref="UBinarySize"/> value from another.
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
    public static UBinarySize Subtract(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value - right.Value);

    /// <summary>
    /// Returns the product of two <see cref="UBinarySize"/> values.
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
    public static UBinarySize Multiply(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value * right.Value);

    /// <summary>
    /// Divides one <see cref="UBinarySize"/> value by another.
    /// </summary>
    /// <param name="left">The value to be divided.</param>
    /// <param name="right">The value to divide by.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize Divide(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value / right.Value);

    /// <summary>
    /// Returns the remainder of dividing one <see cref="UBinarySize"/> value by another.
    /// </summary>
    /// <param name="left">The value to be divided.</param>
    /// <param name="right">The value to divide by.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize Remainder(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value % right.Value);

    #endregion

    #region Comparison

    /// <summary>
    /// Determines whether two specified <see cref="UBinarySize"/> values are the same.
    /// </summary>
    /// <param name="left">The first <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(UBinarySize left, UBinarySize right) => left.Value == right.Value;

    /// <summary>
    /// Determines whether two specified <see cref="UBinarySize"/> values are different.
    /// </summary>
    /// <param name="left">The first <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
    /// of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(UBinarySize left, UBinarySize right) => left.Value != right.Value;

    /// <summary>
    /// Determines whether a <see cref="UBinarySize"/> value is the same as a <see cref="ulong"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="ulong"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(UBinarySize left, ulong right) => left.Value == right;

    /// <summary>
    /// Determines whether a <see cref="UBinarySize"/> value is different from a <see cref="ulong"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="ulong"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the different from the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(UBinarySize left, ulong right) => left.Value != right;

    /// <summary>
    /// Determines whether a <see cref="ulong"/> value is the same as a <see cref="UBinarySize"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="ulong"/> to compare.</param>
    /// <param name="right">The <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(ulong left, UBinarySize right) => left == right.Value;

    /// <summary>
    /// Determines whether a <see cref="ulong"/> value is different from a <see cref="UBinarySize"/>
    /// value.
    /// </summary>
    /// <param name="left">The <see cref="ulong"/> to compare.</param>
    /// <param name="right">The <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is the different from the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(ulong left, UBinarySize right) => left != right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is less than another
    /// <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(UBinarySize left, UBinarySize right) => left.Value < right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is less than a
    /// <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="ulong"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(UBinarySize left, ulong right) => left.Value < right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="ulong"/> is less than a
    /// <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="ulong"/> to compare.</param>
    /// <param name="right">The <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(ulong left, UBinarySize right) => left < right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is less than or
    /// equal to another <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref
    /// name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(UBinarySize left, UBinarySize right) => left.Value <= right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is less than or
    /// equal to a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="ulong"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(UBinarySize left, ulong right) => left.Value <= right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="ulong"/> is less than or
    /// equal to a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="ulong"/> to compare.</param>
    /// <param name="right">The <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the
    /// value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(ulong left, UBinarySize right) => left <= right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is greater than
    /// another <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(UBinarySize left, UBinarySize right) => left.Value > right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is greater than a
    /// <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="ulong"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(UBinarySize left, ulong right) => left.Value > right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="ulong"/> is greater than a
    /// <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="ulong"/> to compare.</param>
    /// <param name="right">The <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(ulong left, UBinarySize right) => left > right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is greater than or
    /// equal to another <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The second <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than or equal to
    /// <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(UBinarySize left, UBinarySize right) => left.Value >= right.Value;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="UBinarySize"/> is greater than or
    /// equal to a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The <see cref="UBinarySize"/> to compare.</param>
    /// <param name="right">The <see cref="ulong"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to
    /// the value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(UBinarySize left, ulong right) => left.Value >= right;

    /// <summary>
    /// Returns a value indicating whether a specified <see cref="ulong"/> is greater than or
    /// equal to a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The <see cref="ulong"/> to compare.</param>
    /// <param name="right">The <see cref="UBinarySize"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to
    /// the value of <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(ulong left, UBinarySize right) => left >= right.Value;

    #endregion

    #region Addition

    /// <summary>
    /// Adds two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator +(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value + right.Value);

    /// <summary>
    /// Adds a <see cref="UBinarySize"/> value to a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator +(UBinarySize left, ulong right) => (UBinarySize)(left.Value + right);

    /// <summary>
    /// Adds a <see cref="ulong"/> value to a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator +(ulong left, UBinarySize right) => (UBinarySize)(left + right.Value);

    /// <summary>
    /// Adds two <see cref="UBinarySize"/> values in a checked context.
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
    public static UBinarySize operator checked +(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value + right.Value);

    /// <summary>
    /// Adds a <see cref="UBinarySize"/> value to a <see cref="ulong"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator checked +(UBinarySize left, ulong right) => (UBinarySize)checked(left.Value + right);

    /// <summary>
    /// Adds a <see cref="ulong"/> value to a <see cref="UBinarySize"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of adding <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator checked +(ulong left, UBinarySize right) => (UBinarySize)checked(left + right.Value);

    #endregion

    #region Subtraction

    /// <summary>
    /// Subtracts two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static UBinarySize operator -(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value - right.Value);

    /// <summary>
    /// Subtracts a <see cref="ulong"/> value from a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static UBinarySize operator -(UBinarySize left, ulong right) => (UBinarySize)(left.Value - right);

    /// <summary>
    /// Subtracts a <see cref="UBinarySize"/> value from a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static UBinarySize operator -(ulong left, UBinarySize right) => (UBinarySize)(left - right.Value);

    /// <summary>
    /// Subtracts two <see cref="UBinarySize"/> values in a checked context.
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
    public static UBinarySize operator checked -(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value - right.Value);

    /// <summary>
    /// Subtracts a <see cref="ulong"/> value from a <see cref="UBinarySize"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static UBinarySize operator checked  -(UBinarySize left, ulong right) => (UBinarySize)checked(left.Value - right);

    /// <summary>
    /// Subtracts a <see cref="UBinarySize"/> value from a <see cref="ulong"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of subtracting <paramref name="right"/> from <paramref name="left"/>.
    /// </returns>
    public static UBinarySize operator checked -(ulong left, UBinarySize right) => (UBinarySize)checked(left - right.Value);

    #endregion

    #region Multiplication

    /// <summary>
    /// Multiplies two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator *(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value * right.Value);

    /// <summary>
    /// Multiplies a <see cref="UBinarySize"/> value by a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator *(UBinarySize left, ulong right) => (UBinarySize)(left.Value * right);

    /// <summary>
    /// Multiplies a <see cref="ulong"/> value by a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator *(ulong left, UBinarySize right) => (UBinarySize)(left * right.Value);

    /// <summary>
    /// Multiplies two <see cref="UBinarySize"/> values in a checked context.
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
    public static UBinarySize operator checked *(UBinarySize left, UBinarySize right) => (UBinarySize)checked(left.Value * right.Value);

    /// <summary>
    /// Multiplies a <see cref="UBinarySize"/> value by a <see cref="ulong"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator checked  *(UBinarySize left, ulong right) => (UBinarySize)checked(left.Value * right);

    /// <summary>
    /// Multiplies a <see cref="ulong"/> value by a <see cref="UBinarySize"/> in a checked context.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of multiplying <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static UBinarySize operator checked  *(ulong left, UBinarySize right) => (UBinarySize)checked(left * right.Value);

    #endregion

    #region Division

    /// <summary>
    /// Divides two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize operator /(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value / right.Value);

    /// <summary>
    /// Divides a <see cref="UBinarySize"/> value by a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize operator /(UBinarySize left, ulong right) => (UBinarySize)(left.Value / right);

    /// <summary>
    /// Divides a <see cref="ulong"/> value by a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The result of dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize operator /(ulong left, UBinarySize right) => (UBinarySize)(left / right.Value);

    #endregion

    #region Remainder

    /// <summary>
    /// Returns the remainder after dividing two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize operator %(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value % right.Value);

    /// <summary>
    /// Returns the remainder after dividing a <see cref="UBinarySize"/> value by a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize operator %(UBinarySize left, ulong right) => (UBinarySize)(left.Value % right);

    /// <summary>
    /// Returns the remainder after dividing a <see cref="ulong"/> value by a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// The remainder after dividing <paramref name="left"/> by <paramref name="right"/>.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="right"/> is zero.
    /// </exception>
    public static UBinarySize operator %(ulong left, UBinarySize right) => (UBinarySize)(left % right.Value);

    #endregion

    #region Unary

    /// <summary>
    /// Returns the specified instance of <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="value">The value to return.</param>
    /// <returns>
    /// The value of <paramref name="value"/>.
    /// </returns>
    public static UBinarySize operator +(UBinarySize value) => value;

    /// <summary>
    /// Increments a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="value">The value to increment.</param>
    /// <returns>
    /// The result of incrementing <paramref name="value"/>.
    /// </returns>
    public static UBinarySize operator ++(UBinarySize value) => (UBinarySize)(value.Value + 1);

    /// <summary>
    /// Increments a <see cref="UBinarySize"/> in a checked context.
    /// </summary>
    /// <param name="value">The value to increment.</param>
    /// <returns>
    /// The result of incrementing <paramref name="value"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is greater than <see cref="MaxValue"/>.
    /// </exception>
    public static UBinarySize operator checked ++(UBinarySize value) => (UBinarySize)checked(value.Value + 1);

    /// <summary>
    /// Decrements a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="value">The value to decrement.</param>
    /// <returns>
    /// The result of decrementing <paramref name="value"/>.
    /// </returns>
    public static UBinarySize operator --(UBinarySize value) => (UBinarySize)(value.Value - 1);

    /// <summary>
    /// Decrements a <see cref="UBinarySize"/> in a checked context.
    /// </summary>
    /// <param name="value">The value to decrement.</param>
    /// <returns>
    /// The result of decrementing <paramref name="value"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The result of the operation is less than <see cref="MinValue"/>.
    /// </exception>
    public static UBinarySize operator checked --(UBinarySize value) => (UBinarySize)checked(value.Value - 1);

    #endregion

    #region Shift

    /// <summary>
    /// Shifts the bits of a <see cref="UBinarySize"/> to the right.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> right by <paramref name="shift"/> bits.
    /// </returns>
    public static UBinarySize operator >>(UBinarySize value, int shift) => (UBinarySize)(value.Value >> shift);

    /// <summary>
    /// Shifts the bits of a <see cref="UBinarySize"/> to the right in an unsigned manner.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> right by <paramref name="shift"/> bits
    /// without considering the sign.
    /// </returns>
    public static UBinarySize operator >>>(UBinarySize value, int shift) => (UBinarySize)(value.Value >>> shift);

    /// <summary>
    /// Shifts the bits of a <see cref="UBinarySize"/> to the left.
    /// </summary>
    /// <param name="value">The value to shift.</param>
    /// <param name="shift">The number of bits to shift by.</param>
    /// <returns>
    /// The result of shifting <paramref name="value"/> left by <paramref name="shift"/> bits.
    /// </returns>
    public static UBinarySize operator <<(UBinarySize value, int shift) => (UBinarySize)(value.Value << shift);

    #endregion

    #region Bitwise

    /// <summary>
    /// Computes the ones-complement representation of a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="value">The value for which to compute the ones-complement.</param>
    /// <returns>
    /// The ones-complement of <paramref name="value"/>.
    /// </returns>
    public static UBinarySize operator ~(UBinarySize value) => (UBinarySize)(~value.Value);

    /// <summary>
    /// Computes the bitwise-and of two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>and</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>and</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator &(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value & right.Value);

    /// <summary>
    /// Computes the bitwise-and of a <see cref="UBinarySize"/> value and a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The value to <c>and</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>and</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator &(UBinarySize left, ulong right) => (UBinarySize)(left.Value & right);

    /// <summary>
    /// Computes the bitwise-and of a <see cref="ulong"/> value and a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The value to <c>and</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>and</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator &(ulong left, UBinarySize right) => (UBinarySize)(left & right.Value);

    /// <summary>
    /// Computes the bitwise-or of two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>or</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>or</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator |(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value | right.Value);

    /// <summary>
    /// Computes the bitwise-or of a <see cref="UBinarySize"/> value and a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The value to <c>or</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>or</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator |(UBinarySize left, ulong right) => (UBinarySize)(left.Value | right);

    /// <summary>
    /// Computes the bitwise-or of a <see cref="ulong"/> value and a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The value to <c>or</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>or</c> with <paramref name="left"/>.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator |(ulong left, UBinarySize right) => (UBinarySize)(left | right.Value);

    /// <summary>
    /// Computes the exclusive-or of two <see cref="UBinarySize"/> values.
    /// </summary>
    /// <param name="left">The value to <c>xor</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>xor</c> with <paramref name="left"/>.</param>
    /// <returns>The exclusive-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator ^(UBinarySize left, UBinarySize right) => (UBinarySize)(left.Value ^ right.Value);

    /// <summary>
    /// Computes the exclusive-or of a <see cref="UBinarySize"/> value and a <see cref="ulong"/>.
    /// </summary>
    /// <param name="left">The value to <c>xor</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>xor</c> with <paramref name="left"/>.</param>
    /// <returns>The exclusive-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator ^(UBinarySize left, ulong right) => (UBinarySize)(left.Value ^ right);

    /// <summary>
    /// Computes the exclusive-or of a <see cref="ulong"/> value and a <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="left">The value to <c>xor</c> with <paramref name="right"/>.</param>
    /// <param name="right">The value to <c>xor</c> with <paramref name="left"/>.</param>
    /// <returns>The exclusive-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static UBinarySize operator ^(ulong left, UBinarySize right) => (UBinarySize)(left ^ right.Value);

    #endregion

    #region Conversion

    /// <summary>
    /// Performs an explicit conversion from <see cref="UBinarySize"/> to <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The value of the <see cref="Value"/> property.
    /// </returns>
    public static explicit operator ulong(UBinarySize value) => value.Value;

    /// <summary>
    /// Performs an implicit conversion from <see cref="ulong"/> to <see cref="UBinarySize"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// A <see cref="UBinarySize"/> where the <see cref="Value"/> property equals
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator UBinarySize(ulong value) => new(value);

    #endregion
}
