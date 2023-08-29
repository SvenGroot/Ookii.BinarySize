// See https://aka.ms/new-console-template for more information
using Ookii;
using System.Globalization;

// Set the output encoding so printing Japanese characters will work.
// Utf-8 should work in both Windows and Linux.
Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Enter a value using a binary suffix (KB, MiB, etc.): ");
var input = Console.ReadLine();

// You can also specify a custom culture/BinaryUnitInfo when parsing, but since we do not do so here
// the current culture with default units is used.
//
// You can also use long units (e.g. 128.5 megabytes) on the input because the
// BinarySizeOptions.AllowLongUnits flag is used.
if (!BinarySize.TryParse(input, BinarySizeOptions.AllowLongUnits, NumberStyles.Number, null, out var size))
{
    Console.WriteLine("The value is not a valid size.");
    return;
}

// First, we display the values using the default, invariant units. These are the English-language
// units.
Console.WriteLine();
Console.WriteLine("Current culture, default (English) units:");
PrintSize(size, CultureInfo.CurrentCulture);

// Create unit information for French. The BinaryUnitInfo class defaults to the English units, so
// only those elements that are different need to be set.
var unitInfo = new BinaryUnitInfo()
{
    LongByte = "octet",
    LongBytes = "octets",
    LongConnector = "-",
    LongMega = "méga",
    LongMebi = "mébi",
    LongTera = "téra",
    LongTebi = "tébi",
    LongPeta = "péta",
    LongPebi = "pébi",
    // For short (abbreivated) units, only "B" is replaced with "o" in French. The prefixes are the same.
    ShortByte = "o",
    ShortBytes = "o",
};

Console.WriteLine();
Console.WriteLine("French culture and units:");

// We want to use French number formatting as well as our custom BinaryUnitInfo, and an extension
// method is provided to combine them.
PrintSize(size, CultureInfo.GetCultureInfo("fr-FR").WithBinaryUnitInfo(unitInfo));

// Now create one for Japanese. Singular and plural byte(s) is the same in this case, and no changes
// are made to the short (abbreviated) units.
unitInfo = new BinaryUnitInfo()
{
    LongByte = "バイト",
    LongBytes = "バイト",
    LongKilo = "キロ",
    LongKibi = "キビ",
    LongMega = "メガ",
    LongMebi = "メビ",
    LongGiga = "ギガ",
    LongGibi = "ギビ",
    LongTera = "テラ",
    LongTebi = "テビ",
    LongPeta = "ペタ",
    LongPebi = "ペビ",
    LongExa = "エクサ",
    LongExbi = "エクスビ",
};

Console.WriteLine();
Console.WriteLine("Japanese culture and units:");
PrintSize(size, CultureInfo.GetCultureInfo("ja-JP").WithBinaryUnitInfo(unitInfo));

void PrintSize(BinarySize size, CultureInfo culture)
{
    // Format strings are unchanged regardless of culture or units.
    Write(culture, $"Full unit: {size:#.# Sbyte}");
    Write(culture, $"Full unit (IEC): {size:0.# Sibyte}");
    Write(culture, $"Abbreviated unit: {size:0.# SB}");
    Write(culture, $"Abbreviated unit (IEC): {size:0.# SiB}");
}

// This is just a helper to apply a custom culture to an interpolated string by forcing it to be
// implicitly converted to a FormattableString first.
void Write(CultureInfo culture, FormattableString format)
{
    Console.WriteLine(format.ToString(culture));
}
