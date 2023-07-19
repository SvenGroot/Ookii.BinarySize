# Configuration sample

This sample demonstrates how to use the `BinarySize` structure in a JSON configuration file that is
consumed using the `ConfigurationBuilder` class. The configuration file is deserialized into a
custom [`Settings`](Settings.cs) class, which uses both the `BinarySize` and `IecBinarySize`
structures to demonstrate their differences in this context.

The application itself simply displays the configuration values. Using the default
[appsettings.json](appsettings.json) file, the output will look like this.

```text
The Size setting is 128 MiB
The IecSize setting is 122.1 MiB
```

Both values were parsed from the string "128MB" in the configuration file, but `BinarySize` reads
that as 134,217,728 bytes, and `IecBinarySize` reads that as 128,000,000 bytes. Both values are
formatted using IEC units for display in [Program.cs](Program.cs). If you change the configuration
values to e.g. "128MiB" (using an IEC unit), you'll see that both values are the same.

Besides JSON configuration files, both `BinarySize` and `IecBinarySize` can be serialized using the
`JsonSerializer` class, as well as the `XmlSerializer` and `DataContractSerializer`. It also works
in XML configuration sections.

This makes it easy to use `BinarySize` to configure buffer sizes, cache sizes, or any other kind of
size you want to be configurable, allowing users to use human-readable values to alter the
configuration.
