using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii;

/// <summary>
/// Converts the <see cref="UIecBinarySize"/> structure to or from JSON.
/// </summary>
/// <remarks>
/// <para>
///   This class is used to serialize <see cref="UIecBinarySize"/> values when using the
///   <see cref="JsonSerializer"/> class.
/// </para>
/// <para>
///   <see cref="UIecBinarySize"/> values are serialized to JSON as strings, allowing the use of
///   values with binary size suffixes.
/// </para>
/// <para>
///   When deserializing from JSON, this converter will interpret SI prefixes as based on powers of
///   ten, so "1kB" equals 1,000 bytes, "1MB" equals 1,000,000 bytes, and so on. IEC prefixes are
///   still based on powers of two.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
[CLSCompliant(false)]
public class UIecBinarySizeJsonConverter : JsonConverter<UIecBinarySize>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySizeJsonConverter"/> class.
    /// </summary>
    public UIecBinarySizeJsonConverter()
    { 
    }

    /// <inheritdoc/>
    public override UIecBinarySize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        if (stringValue == null)
        {
            return default;
        }

        try
        {
            return UIecBinarySize.Parse(stringValue, CultureInfo.InvariantCulture);
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
    public override void Write(Utf8JsonWriter writer, UIecBinarySize value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(null, CultureInfo.InvariantCulture));
}
