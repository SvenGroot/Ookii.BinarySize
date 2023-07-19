using Ookii;

namespace Configuration;

// This class is the strongly typed class used to read the settings from appsettings.json using
// the configuration binder. It will use the default TypeConverter for each property to convert it
// from a string value, which lets us use values like "64 MiB" in the configuration file, making it
// easy for a user to alter the configuration.
internal class Settings
{
    public BinarySize Size { get; set; }

    public IecBinarySize IecSize { get; set; }
}
