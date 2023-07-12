namespace Ookii;

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
    public static BinarySize Sum(this IEnumerable<BinarySize> source)
        => source.Sum(s => s.Value);

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER

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
    public static ValueTask<BinarySize> SumAsync(this IAsyncEnumerable<BinarySize> source, CancellationToken cancellationToken = default)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return SumAsyncCore(source, cancellationToken);
    }

    private static async ValueTask<BinarySize> SumAsyncCore(IAsyncEnumerable<BinarySize> source, CancellationToken cancellationToken)
    {
        // Do this manually to avoid taking a dependency on System.Linq.Async.
        var sum = BinarySize.Zero;
        await foreach (var value in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            checked
            {
                sum += value;
            }
        }

        return sum;
    }

#endif
}
