using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Ookii.Test;

[TestClass]
public class BinarySizeTests
{
    [TestMethod]
    public void TestValue()
    {
        Assert.AreEqual(0, BinarySize.Zero.Value);
        Assert.AreEqual(long.MinValue, BinarySize.MinValue.Value);
        Assert.AreEqual(long.MaxValue, BinarySize.MaxValue.Value);

        var size = new BinarySize(512);
        Assert.AreEqual(512, size.Value);
        Assert.AreEqual(0.5, size.AsKibi);

        size = BinarySize.FromKibi(2.5);
        Assert.AreEqual(2_560, size.Value);
        Assert.AreEqual(2.5, size.AsKibi);

        size = BinarySize.FromMebi(2.5);
        Assert.AreEqual(2_621_440, size.Value);
        Assert.AreEqual(2.5, size.AsMebi);

        size = BinarySize.FromGibi(2.5);
        Assert.AreEqual(2_684_354_560, size.Value);
        Assert.AreEqual(2.5, size.AsGibi);

        size = BinarySize.FromTebi(2.5);
        Assert.AreEqual(2_748_779_069_440, size.Value);
        Assert.AreEqual(2.5, size.AsTebi);

        size = BinarySize.FromPebi(2.5);
        Assert.AreEqual(2_814_749_767_106_560, size.Value);
        Assert.AreEqual(2.5, size.AsPebi);

        size = BinarySize.FromExbi(2.5);
        Assert.AreEqual(2_882_303_761_517_117_440, size.Value);
        Assert.AreEqual(2.5, size.AsExbi);
    }

    [TestMethod]
    public void TestParse()
    {
        Assert.AreEqual(new BinarySize(123), BinarySize.Parse("123", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123), BinarySize.Parse("123B", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(125952), BinarySize.Parse("123KB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(125952), BinarySize.Parse("123KiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(125952), BinarySize.Parse("123K", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(128974848), BinarySize.Parse("123MB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(128974848), BinarySize.Parse("123MiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(128974848), BinarySize.Parse("123M", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(132070244352), BinarySize.Parse("123GB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(132070244352), BinarySize.Parse("123GiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(132070244352), BinarySize.Parse("123G", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(135239930216448), BinarySize.Parse("123TB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(135239930216448), BinarySize.Parse("123TiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(135239930216448), BinarySize.Parse("123T", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(138485688541642752), BinarySize.Parse("123PB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(138485688541642752), BinarySize.Parse("123PiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(138485688541642752), BinarySize.Parse("123P", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(138485688541642752), BinarySize.Parse("123 PB ", CultureInfo.InvariantCulture)); // with some spaces.
        Assert.AreEqual(new BinarySize(6341068275337658368), BinarySize.Parse("5.5EB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(6341068275337658368), BinarySize.Parse("5.5EiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(6341068275337658368), BinarySize.Parse("5.5E", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(6341068275337658368), BinarySize.Parse("5.5 EB ", CultureInfo.InvariantCulture)); // with some spaces.

        // Explicit culture test:
        Assert.AreEqual(new BinarySize(126464), BinarySize.Parse("123.5KB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(126464), BinarySize.Parse("123,5KB", new CultureInfo("nl-NL")));
        // Test version without provider uses current culture (weak test but it'll do)
        string size = string.Format(CultureInfo.CurrentCulture, "{0:0.0}KB", 123.5);
        Assert.AreEqual(new BinarySize(126464), BinarySize.Parse(size));
    }

    [TestMethod]
    public void TestParseDecimal()
    { 
        // Decimal
        Assert.AreEqual(new BinarySize(123), BinarySize.Parse("123", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123), BinarySize.Parse("123B", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000), BinarySize.Parse("123KB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(125952), BinarySize.Parse("123KiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000), BinarySize.Parse("123K", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000), BinarySize.Parse("123MB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(128974848), BinarySize.Parse("123MiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000), BinarySize.Parse("123M", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000000), BinarySize.Parse("123GB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(132070244352), BinarySize.Parse("123GiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000000), BinarySize.Parse("123G", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000000000), BinarySize.Parse("123TB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(135239930216448), BinarySize.Parse("123TiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000000000), BinarySize.Parse("123T", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000000000000), BinarySize.Parse("123PB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(138485688541642752), BinarySize.Parse("123PiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000000000000), BinarySize.Parse("123P", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(123000000000000000), BinarySize.Parse("123 PB ", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture)); // with some spaces.
        Assert.AreEqual(new BinarySize(5500000000000000000), BinarySize.Parse("5.5EB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(6341068275337658368), BinarySize.Parse("5.5EiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(5500000000000000000), BinarySize.Parse("5.5E", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(5500000000000000000), BinarySize.Parse("5.5 EB ", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture)); // with some spaces.
    }

    [TestMethod]
    public void TestToString()
    {
        var target = new BinarySize(123456789012345678);
        Assert.AreEqual("123456789012345678 B", target.ToString(null, CultureInfo.InvariantCulture));
        Assert.AreEqual("120563270519868.826171875KB", target.ToString("KB", CultureInfo.InvariantCulture));
        Assert.AreEqual("120563270519868.826171875KiB", target.ToString("KiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("120563270519868.826171875K", target.ToString("K", CultureInfo.InvariantCulture));
        Assert.AreEqual("117737568867.05940055847167969MB", target.ToString("MB", CultureInfo.InvariantCulture));
        Assert.AreEqual("117737568867.05940055847167969MiB", target.ToString("MiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("117737568867.05940055847167969M", target.ToString("M", CultureInfo.InvariantCulture));
        Assert.AreEqual("114978094.59673769585788249969GB", target.ToString("GB", CultureInfo.InvariantCulture));
        Assert.AreEqual("114978094.59673769585788249969GiB", target.ToString("GiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("114978094.59673769585788249969G", target.ToString("GG", CultureInfo.InvariantCulture)); // "GG" to work around "G" being the general format specifier.
        Assert.AreEqual("112283.29550462665611121337861TB", target.ToString("TB", CultureInfo.InvariantCulture));
        Assert.AreEqual("112283.29550462665611121337861TiB", target.ToString("TiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("112283.29550462665611121337861T", target.ToString("T", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.65165576623696885860681505PB", target.ToString("PB", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.65165576623696885860681505PiB", target.ToString("PiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.65165576623696885860681505P", target.ToString("P", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.65165576623696885860681505  PB  ", target.ToString("  PB  ", CultureInfo.InvariantCulture)); // With whitespace
        Assert.AreEqual("0.1070816950842157899009832178EB", target.ToString("EB", CultureInfo.InvariantCulture));
        Assert.AreEqual("0.1070816950842157899009832178EiB", target.ToString("EiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("0.1070816950842157899009832178E", target.ToString("E", CultureInfo.InvariantCulture));

        // General format specifier
        Assert.AreEqual("123456789012345678 B", target.ToString("G", CultureInfo.InvariantCulture));

        // Explicit format test:
        Assert.AreEqual("109.7 PB", target.ToString("0.# PB", CultureInfo.InvariantCulture));

        // Explicit culture test:
        Assert.AreEqual("109,7PB", target.ToString("0.#PB", new CultureInfo("nl-NL")));
        Assert.AreEqual("109,65165576623696885860681505PB", target.ToString("PB", new CultureInfo("nl-NL")));

        // Current culture test:
        Assert.AreEqual(target.ToString("PB", CultureInfo.CurrentCulture), target.ToString("PB"));

        // Automatic units test:
        Assert.AreEqual("123B", ((BinarySize)123).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123B", ((BinarySize)123).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464B", ((BinarySize)126464).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5KB", ((BinarySize)126464).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464KB", ((BinarySize)129499136).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5MB", ((BinarySize)129499136).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464MB", ((BinarySize)132607115264).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5GB", ((BinarySize)132607115264).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464GB", ((BinarySize)135789686030336).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5TB", ((BinarySize)135789686030336).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464TB", ((BinarySize)139048638495064064).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5PB", ((BinarySize)139048638495064064).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012345678B", ((BinarySize)123456789012345678).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.7PB", ((BinarySize)123456789012345678).ToString("0.#SB", CultureInfo.InvariantCulture));

        // Test with different options:
        Assert.AreEqual("126464", ((BinarySize)126464).ToString("A", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5K", ((BinarySize)126464).ToString("S", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464B", ((BinarySize)126464).ToString("AiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5KiB", ((BinarySize)126464).ToString("SiB", CultureInfo.InvariantCulture));

        // Smaller than unit.
        Assert.AreEqual("0.5KB", ((BinarySize)512).ToString("KB", CultureInfo.InvariantCulture));

        // Zero
        Assert.AreEqual("0 B", BinarySize.Zero.ToString(null, CultureInfo.InvariantCulture));
        Assert.AreEqual("0B", BinarySize.Zero.ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("0KB", BinarySize.Zero.ToString("KB", CultureInfo.InvariantCulture));

        // Test defaults, should have same effect as AB.
        string expected = 126464.ToString() + " KiB";
        Assert.AreEqual(expected, ((BinarySize)129499136).ToString());
        Assert.AreEqual(expected, ((BinarySize)129499136).ToString(null, CultureInfo.CurrentCulture));
        Assert.AreEqual(expected, ((BinarySize)129499136).ToString(null, null));
        Assert.AreEqual(expected, ((BinarySize)129499136).ToString(""));
        Assert.AreEqual(expected, ((BinarySize)129499136).ToString("", null));

        // Case correction.
        Assert.AreEqual("1KB", ((BinarySize)1024).ToString("Kb"));
        Assert.AreEqual("1KiB", ((BinarySize)1024).ToString("kIb"));
        Assert.AreEqual("1KB", ((BinarySize)1024).ToString("Ab"));
        Assert.AreEqual("1.5KiB", ((BinarySize)1536).ToString("sIb"));

        // Test IFormattable
        Assert.AreEqual("test 109.7 PB test2", string.Format(CultureInfo.InvariantCulture, "test {0:0.# SB} test2", ((BinarySize)123456789012345678)));
    }

    [TestMethod]
    public void TestToStringDecimal()
    {
        // Lowercase means decimal unless 'i' is present.
        var target = new BinarySize(123456789012345678);
        Assert.AreEqual("123456789012345678 B", target.ToString(null, CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012345.678kB", target.ToString("kB", CultureInfo.InvariantCulture));
        Assert.AreEqual("120563270519868.826171875KiB", target.ToString("kiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012345.678k", target.ToString("k", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012.345678MB", target.ToString("mB", CultureInfo.InvariantCulture));
        Assert.AreEqual("117737568867.05940055847167969MiB", target.ToString("miB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012.345678M", target.ToString("m", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789.012345678GB", target.ToString("gB", CultureInfo.InvariantCulture));
        Assert.AreEqual("114978094.59673769585788249969GiB", target.ToString("giB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789.012345678G", target.ToString("g", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456.789012345678TB", target.ToString("tB", CultureInfo.InvariantCulture));
        Assert.AreEqual("112283.29550462665611121337861TiB", target.ToString("tiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456.789012345678T", target.ToString("t", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.456789012345678PB", target.ToString("pB", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.65165576623696885860681505PiB", target.ToString("piB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.456789012345678P", target.ToString("p", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.456789012345678  PB  ", target.ToString("  pB  ", CultureInfo.InvariantCulture)); // With whitespace
        Assert.AreEqual("0.123456789012345678EB", target.ToString("eB", CultureInfo.InvariantCulture));
        Assert.AreEqual("0.1070816950842157899009832178EiB", target.ToString("eiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("0.123456789012345678E", target.ToString("e", CultureInfo.InvariantCulture));
    }

#if NET6_0_OR_GREATER

        [TestMethod]
    public void TestTryFormat()
    {
        var size = new BinarySize(126464);
        var destination = new char[20];

        // Plenty of room.
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out int charsWritten, "SB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5KB", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "0.00 SiB ".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.50 KiB ", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "0".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126464", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "AiB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126464B", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "  KB  ".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5  KB  ", destination.AsSpan(0, charsWritten).ToString());

        // Exactly the right size.
        Assert.IsTrue(size.TryFormat(destination.AsSpan(0, 7), out charsWritten, "SB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5KB", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(0, 11), out charsWritten, "0.00 SiB ".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.50 KiB ", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(0, 6), out charsWritten, "0".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126464", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(0, 7), out charsWritten, "AiB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126464B", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(0, 11), out charsWritten, "  KB  ".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5  KB  ", destination.AsSpan(0, charsWritten).ToString());

        // Just too small.
        Assert.IsFalse(size.TryFormat(destination.AsSpan(0, 6), out _, "SB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(destination.AsSpan(0, 10), out _, "0.00 SiB ".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(destination.AsSpan(0, 5), out _, "0".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(destination.AsSpan(0, 6), out _, "AiB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(destination.AsSpan(0, 10), out _, "  KB  ".AsSpan(), CultureInfo.InvariantCulture));

        // Empty.
        Assert.IsFalse(size.TryFormat(default, out _, "SB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(default, out _, "0.00 SiB ".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(default, out _, "0".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(default, out _, "AiB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.IsFalse(size.TryFormat(default, out _, "  KB  ".AsSpan(), CultureInfo.InvariantCulture));

        // Case correction.
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "kIb".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5KiB", destination.AsSpan(0, charsWritten).ToString());

        // Decimal
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "kb".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126.464kB", destination.AsSpan(0, charsWritten).ToString());
    }

#endif

    [TestMethod]
    public void TestEquality()
    {
        Assert.AreEqual(new BinarySize(123), new BinarySize(123));
        Assert.AreNotEqual(new BinarySize(123), new BinarySize(124));
        Assert.IsTrue(new BinarySize(123) == new BinarySize(123));
        Assert.IsFalse(new BinarySize(123) == new BinarySize(124));
        Assert.IsTrue(new BinarySize(123) != new BinarySize(124));
        Assert.IsFalse(new BinarySize(123) != new BinarySize(123));
    }

    [TestMethod]
    public void TestTypeConverter()
    {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(BinarySize));
        var target = new BinarySize(125952);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KB"));
        Assert.AreEqual("123 KiB", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
        target = new(129499136);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123.5MB"));
        Assert.AreEqual("126464 KiB", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
    }
}
