using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii;

/// <summary>
/// Converts the <see cref="BinarySize"/> structure to or from JSON.
/// </summary>
/// <remarks>
/// <para>
///   <see cref="BinarySize"/> values are serialized to JSON as strings, allowing the use of values
///   with binary size suffixes.
/// </para>
/// </remarks>
public class BinarySizeJsonConverter : JsonConverter<BinarySize>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySizeJsonConverter"/> class.
    /// </summary>
    public BinarySizeJsonConverter()
    { 
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySizeJsonConverter"/> class with the specified
    /// options.
    /// </summary>
    /// <param name="options">
    /// A bitwise combination of <see cref="BinarySizeOptions"/> values that indicates how units
    /// are interpreted when converting from a string.
    /// </param>
    /// <remarks>
    /// <para>
    ///   When converting from JSON, the <paramref name="options"/> will be passed to the
    ///   <see cref="BinarySize.Parse(ReadOnlySpan{char}, BinarySizeOptions, NumberStyles, IFormatProvider?)" qualifyHint="true"/>
    ///   method.
    /// </para>
    /// </remarks>
    public BinarySizeJsonConverter(BinarySizeOptions options)
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
    public override BinarySize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        if (stringValue == null)
        {
            return default;
        }

        try
        {
            return BinarySize.Parse(stringValue, Options, NumberStyles.Number, CultureInfo.InvariantCulture);
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
    public override void Write(Utf8JsonWriter writer, BinarySize value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(null, CultureInfo.InvariantCulture));
}
