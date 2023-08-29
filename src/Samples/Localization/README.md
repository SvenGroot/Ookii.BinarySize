# Localization sample

This sample demonstrates how to customize the units used when formatting `BinarySize` values by
using the `BinaryUnitInfo` class. This class lets you change how units are displayed and parsed, in
both their full and abbreviated forms.

The sample asks for a value, and prints that value using full and abbreviated SI and IEC units,
using three cultures: the current culture, with the default, English-language units; French culture
and units; and Japanese culture and units.

Below is an example execution of this application:

```text
Enter a value using a binary suffix (KB, MiB, etc.): 128.5MB

Current culture, default (English) units:
Full unit: 128.5 megabytes
Full units (IEC): 128.5 mebibytes
Abbreviated unit: 128.5 MB
Abbreviated unit (IEC): 128.5 MiB

French culture and units:
Full unit: 128,5 méga-octets
Full units (IEC): 128,5 mébi-octets
Abbreviated unit: 128,5 Mo
Abbreviated unit (IEC): 128,5 Mio

Japanese culture and units:
Full unit: 128.5 メガバイト
Full units (IEC): 128.5 メビバイト
Abbreviated unit: 128.5 MB
Abbreviated unit (IEC): 128.5 MiB
```

For more information, see the [Localization documentation](../../../README.md#localization).
