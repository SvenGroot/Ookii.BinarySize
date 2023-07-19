namespace Ookii;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/> for use with the <see cref="BinarySize"/>
/// type.
/// </summary>
/// <threadsafety instance="false" static="true"/>
public static class EnumerableExtensions
{
    /// <summary>
    /// Computes the sum of a sequence of <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue" qualifyHint="true"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   This method returns zero if <paramref name="source"/> contains no elements.
    /// </para>
    /// </remarks>
    public static BinarySize Sum(this IEnumerable<BinarySize> source)
        => (BinarySize)source.Sum(s => s.Value);

    /// <summary>
    /// Computes the sum of a sequence of <see cref="BinarySize"/> values that are obtained by
    /// invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">
    /// A sequence of values to calculate the sum of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue" qualifyHint="true"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   This method returns zero if <paramref name="source"/> contains no elements.
    /// </para>
    /// </remarks>
    public static BinarySize Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, BinarySize> selector)
        => (BinarySize)source.Sum(s => selector(s).Value);

    /// <summary>
    /// Computes the sum of a sequence of nullable <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue" qualifyHint="true"/>.
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
    /// Computes the sum of a sequence of nullable <see cref="BinarySize"/> values that are obtained
    /// by invoking a transform function on each element of the input sequence..
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">
    /// A sequence of values to calculate the sum of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The sum of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum is larger than <see cref="BinarySize.MaxValue" qualifyHint="true"/>.
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

    /// <summary>
    /// Computes the average of a sequence of <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of <see cref="BinarySize"/> values to calculate the average of.
    /// </param>
    /// <returns>The average of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="source"/> contains no elements.
    /// </exception>
    public static BinarySize Average(this IEnumerable<BinarySize> source)
        => (BinarySize)source.Average(s => (decimal)s.Value);

    /// <summary>
    /// Computes the average of a sequence of <see cref="BinarySize"/> values that are obtained by
    /// invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">
    /// A sequence of values to calculate the average of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The average of the values in the sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="source"/> contains no elements.
    /// </exception>
    public static BinarySize Average<TSource>(this IEnumerable<TSource> source, Func<TSource, BinarySize> selector)
        => (BinarySize)source.Average(s => (decimal)selector(s).Value);

    /// <summary>
    /// Computes the average of a sequence of nullable <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <see cref="BinarySize"/> values to calculate the average of.
    /// </param>
    /// <returns>
    /// The average of the values in the sequence, or <see langword="null"/> if <paramref name="source"/>
    /// is empty or contains only values that are <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public static BinarySize? Average(this IEnumerable<BinarySize?> source)
        => (BinarySize?)(long?)source.Average(s => (decimal?)s?.Value);

    /// <summary>
    /// Computes the average of a sequence of nullable <see cref="BinarySize"/> values that are
    /// obtained by invoking a transform function on each element of the input sequence..
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">
    /// A sequence of values to calculate the average of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>
    /// The average of the values in the sequence, or <see langword="null"/> if <paramref name="source"/>
    /// is empty or contains only values that are <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public static BinarySize? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, BinarySize?> selector)
        => (BinarySize?)(long?)source.Average(s => (decimal?)selector(s)?.Value);
}
