using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii;

/// <summary>
/// Converts the <see cref="IecBinarySize"/> structure to or from JSON.
/// </summary>
/// <remarks>
/// <para>
///   This class is used to serialize <see cref="IecBinarySize"/> values when using the
///   <see cref="JsonSerializer"/> class.
/// </para>
/// <para>
///   <see cref="IecBinarySize"/> values are serialized to JSON as strings, allowing the use of values
///   with binary size suffixes.
/// </para>
/// <para>
///   This converter uses the <see cref="IecBinarySize"/> structure, so when converting from JSON it
///   will converter 1 kB to 1,000 bytes, and 1 KiB to 1,024 bytes, and so on. This is in contrast
///   to the default <see cref="BinarySizeJsonConverter"/>, which would treat both as 1,024 bytes.
/// </para>
/// </remarks>
/// <threadsafety instance="true" static="true"/>
public class IecBinarySizeJsonConverter : JsonConverter<IecBinarySize>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySizeJsonConverter"/> class.
    /// </summary>
    public IecBinarySizeJsonConverter()
    { 
    }

    /// <inheritdoc/>
    public override IecBinarySize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        if (stringValue == null)
        {
            return default;
        }

        try
        {
            return IecBinarySize.Parse(stringValue, CultureInfo.InvariantCulture);
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
    public override void Write(Utf8JsonWriter writer, IecBinarySize value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(null, CultureInfo.InvariantCulture));
}
