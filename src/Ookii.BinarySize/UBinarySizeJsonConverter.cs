using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii;

/// <summary>
/// Converts the <see cref="UBinarySize"/> structure to or from JSON.
/// </summary>
/// <remarks>
/// <para>
///   This class is used to serialize <see cref="UBinarySize"/> values when using the
///   <see cref="JsonSerializer"/> class.
/// </para>
/// <para>
///   <see cref="UBinarySize"/> values are serialized to JSON as strings, allowing the use of values
///   with binary size suffixes.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
[CLSCompliant(false)]
public class UBinarySizeJsonConverter : JsonConverter<UBinarySize>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UBinarySizeJsonConverter"/> class.
    /// </summary>
    public UBinarySizeJsonConverter()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UBinarySizeJsonConverter"/> class with the specified
    /// options.
    /// </summary>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units
    /// are interpreted when converting from JSON.
    /// </param>
    /// <remarks>
    /// <para>
    ///   When converting from JSON, the <paramref name="options"/> will be passed to the
    ///   <see cref="UBinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// </remarks>
    public UBinarySizeJsonConverter(BinarySizeOptions options)
    {
        Options = options;
    }

    /// <summary>
    /// Gets a value that indicates how units are interpreted when converting from a string.
    /// </summary>
    /// <value>
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values.
    /// </value>
    public BinarySizeOptions Options { get; }

    /// <inheritdoc/>
    public override UBinarySize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        if (stringValue == null)
        {
            return default;
        }

        try
        {
            return UBinarySize.Parse(stringValue, Options, NumberStyles.Number, CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
            throw new JsonException(null, ex);
        }
        catch (OverflowException ex)
        {
            throw new JsonException(null, ex);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, UBinarySize value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(null, CultureInfo.InvariantCulture));
}
