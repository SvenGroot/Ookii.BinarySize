using Ookii;

namespace Configuration;

internal class Settings
{
    public BinarySize Size { get; set; }

    // The configuration binder does not respect a TypeConverterAttribute on the property, so we
    // use IecBinarySize to get the alternative behavior.
    public IecBinarySize IecSize { get; set; }
}
