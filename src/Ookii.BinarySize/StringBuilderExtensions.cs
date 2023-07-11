using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ookii;

static class StringBuilderExtensions
{
#if !NET6_0_OR_GREATER
    public static StringBuilder Append(this StringBuilder sb, ReadOnlySpan<char> value)
    {
        sb.Append(value.ToString());
        return sb;
    }
#endif
}
