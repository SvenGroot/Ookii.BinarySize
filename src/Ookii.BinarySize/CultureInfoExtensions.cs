using System.Globalization;

namespace Ookii;

/// <summary>
/// Provides extension methods for the <see cref="CultureInfo"/> class.
/// </summary>
/// <threadsafety instance="false" static="true"/>
public static class CultureInfoExtensions
{
    /// <summary>
    /// Extends an existing <see cref="CultureInfo"/> object with a <see cref="BinaryUnitInfo"/>
    /// object.
    /// </summary>
    /// <param name="culture">The <see cref="CultureInfo"/> to extend.</param>
    /// <param name="binaryUnitInfo">The <see cref="BinaryUnitInfo"/> to add to <paramref name="culture"/>.</param>
    /// <returns>
    /// An object that wraps <paramref name="culture"/>, returning all the same information, but
    /// whose <see cref="IFormatProvider"/> implementation can also return the value of
    /// <paramref name="binaryUnitInfo"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="culture"/> or <paramref name="binaryUnitInfo"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   Use this method if you wish to use a custom culture, other than the current value of the
    ///   <see cref="CultureInfo.CurrentCulture"/> property, along with a custom <see cref="BinaryUnitInfo"/>
    ///   object.
    /// </para>
    /// </remarks>
    public static CultureInfo WithBinaryUnitInfo(this CultureInfo culture, BinaryUnitInfo binaryUnitInfo)
    {
        if (culture == null)
        {
            throw new ArgumentNullException(nameof(culture));
        }

        if (binaryUnitInfo == null)
        {
            throw new ArgumentNullException(nameof(binaryUnitInfo));
        }

        return new CultureInfoWrapper(culture, binaryUnitInfo);
    }
}
