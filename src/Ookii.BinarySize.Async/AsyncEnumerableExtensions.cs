﻿namespace Ookii;

/// <summary>
/// Provides extension methods for <see cref="IAsyncEnumerable{T}"/> for use with the <see cref="BinarySize"/>
/// type.
/// </summary>
public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// Asynchronously computes the sum of the sequence of <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <param name="cancellationToken">
    /// The optional cancellation token to be used for canceling the sequence at any time.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The result of the task is the sum of the
    /// values in the sequence.
    /// </returns>
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
    public static async ValueTask<BinarySize> SumAsync(this IAsyncEnumerable<BinarySize> source, CancellationToken cancellationToken = default)
    {
        var result = await source.SumAsync(s => s.Value, cancellationToken);
        return (BinarySize)result;
    }

    /// <summary>
    /// Asynchronously computes the sum of the sequence of <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="cancellationToken">
    /// The optional cancellation token to be used for canceling the sequence at any time.
    /// </param>
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
    public static async ValueTask<BinarySize> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, BinarySize> selector, CancellationToken cancellationToken = default)
    {
        var result = await source.SumAsync(s => selector(s).Value, cancellationToken);
        return (BinarySize)result;
    }

    /// <summary>
    /// Asynchronously computes the sum of the sequence of nullable <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <param name="cancellationToken">
    /// The optional cancellation token to be used for canceling the sequence at any time.
    /// </param>
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
    public static async ValueTask<BinarySize?> SumAsync(this IAsyncEnumerable<BinarySize?> source, CancellationToken cancellationToken = default)
    {
        var result = await source.SumAsync(s => s?.Value, cancellationToken);
        return (BinarySize?)result;
    }

    /// <summary>
    /// Asynchronously computes the sum of the sequence of nullable <see cref="BinarySize"/> values.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <see cref="BinarySize"/> values to calculate the sum of.
    /// </param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="cancellationToken">
    /// The optional cancellation token to be used for canceling the sequence at any time.
    /// </param>
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
    public static async ValueTask<BinarySize?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, BinarySize?> selector, CancellationToken cancellationToken = default)
    {
        var result = await source.SumAsync(s => selector(s)?.Value, cancellationToken);
        return (BinarySize?)result;
    }
}
