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

    public static bool EndsWith(this ReadOnlySpan<char> value, string suffix, CompareInfo info, CompareOptions options) 
    {
        if (value.Length < suffix.Length)
        {
            return false;
        }

#if NET6_0_OR_GREATER
        return info.Compare(value.Slice(value.Length - suffix.Length), suffix.AsSpan(), options) == 0;
#else
        return info.Compare(value.Slice(value.Length - suffix.Length).ToString(), suffix, options) == 0;
#endif
    }

    public static bool TrimSuffix(ref ReadOnlySpan<char> value, string suffix, CompareInfo info, CompareOptions options)
    {
        var result = value.EndsWith(suffix, info, options);
        if (result)
        {
            value = value.Slice(0, value.Length - suffix.Length);
        }

        return result;
    }
}
