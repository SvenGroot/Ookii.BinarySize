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

    public static bool TryAppend(this Span<char> destination, ref int index, char source)
    {
        if (destination.Length <= index)
        {
            return false;
        }

        destination[index] = source;
        ++index;
        return true;
    }
}
