﻿namespace Ookii;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/> for use with the <see cref="BinarySize"/>
/// type.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Computes the sum of the sequence of <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   This method returns zero if <paramref name="source"/> contains no elements.
    /// </para>
    /// </remarks>
    public static BinarySize Sum(this IEnumerable<BinarySize> source)
        => (BinarySize)source.Sum(s => s.Value);

    /// <summary>
    /// Computes the sum of the sequence of <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   This method returns zero if <paramref name="source"/> contains no elements.
    /// </para>
    /// </remarks>
    public static BinarySize Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, BinarySize> selector)
        => (BinarySize)source.Sum(s => selector(s).Value);

    /// <summary>
    /// Computes the sum of the sequence of nullable <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   Items in <paramref name="source"/> that are <see langword="null"/> are excluded from the
    ///   computation of the sum. This method returns zero if <paramref name="source"/> contains no
    ///   elements or all elements are <see langword="null"/>.
    /// </para>
    /// </remarks>
    public static BinarySize? Sum(this IEnumerable<BinarySize?> source)
        => (BinarySize?)source.Sum(s => s?.Value);

    /// <summary>
    /// Computes the sum of the sequence of nullable <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   Items in <paramref name="source"/> that are <see langword="null"/> are excluded from the
    ///   computation of the sum. This method returns zero if <paramref name="source"/> contains no
    ///   elements or all elements are <see langword="null"/>.
    /// </para>
    /// </remarks>
    public static BinarySize? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, BinarySize?> selector)
        => (BinarySize?)source.Sum(s => selector(s)?.Value);
}
