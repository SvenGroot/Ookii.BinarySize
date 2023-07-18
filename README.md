# Ookii.BinarySize

Ookii.BinarySize is a modern library for parsing and displaying quantities of bytes using
human-readable representation.

It provides functionality to parse numeric values that end with a byte size unit, such as "512 B",
"3 MB", "10 M", or "4.5GiB", and format them for display in the same way, automatically choosing the
best size prefix, or using the one you specified depending on the format string.

- Supports units with SI prefixes ("KB", "MB", "GB", "TB", "PB", and "EB"), and the IEC prefixes.
  ("KiB", "MiB", "GiB", "TiB", "PiB", and "EiB"), and with and without the "B".
- Optionally use decimal SI prefixes.
- Fully customizable formatting, with automatic size prefix selection.
- Supports values up to approximately positive and negative 8 EiB, using Int64 (long) to store the
  value.
- Supports math and binary operators, and .Net 7 generic math.
- Supports .Net Standard 2.0, .Net Standard 2.1, and .Net 6.0 and up.
- Trim-friendly.