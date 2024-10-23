using System.Globalization;

namespace Ookii;

static class SpanExtensions
{
    public static bool TryAppend(this Span<char> destination, ref int index, ReadOnlySpan<char> source)
    {
        if (!source.TryCopyTo(destination.Slice(index)))
        {
            return false;
        }

        index += source.Length;
        return true;
    }
}
