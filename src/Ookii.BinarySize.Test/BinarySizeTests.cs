using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

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
        Assert.AreEqual(new BinarySize(123), BinarySize.Parse("123 B", CultureInfo.InvariantCulture));
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

        // Negative
        Assert.AreEqual(new BinarySize(-126464), BinarySize.Parse("-123.5KB", CultureInfo.InvariantCulture));

        // Explicit culture test:
        Assert.AreEqual(new BinarySize(126464), BinarySize.Parse("123.5KB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new BinarySize(126464), BinarySize.Parse("123,5KB", new CultureInfo("nl-NL")));
        // Test version without provider uses current culture (weak test but it'll do)
        string size = string.Format(CultureInfo.CurrentCulture, "{0:0.0}KB", 123.5);
        Assert.AreEqual(new BinarySize(126464), BinarySize.Parse(size));

        // Empty span
        Assert.AreEqual((BinarySize)0, BinarySize.Parse(ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void TestTryParse()
    {
        Assert.IsTrue(BinarySize.TryParse("123", CultureInfo.InvariantCulture, out var result));
        Assert.AreEqual(new BinarySize(123), result);
        Assert.IsTrue(BinarySize.TryParse("123B", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new BinarySize(123), result);
        Assert.IsTrue(BinarySize.TryParse("123PB", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new BinarySize(138485688541642752), result);
        Assert.IsTrue(BinarySize.TryParse("123PiB", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new BinarySize(138485688541642752), result);
        Assert.IsTrue(BinarySize.TryParse("123P", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new BinarySize(138485688541642752), result);
        Assert.IsTrue(BinarySize.TryParse(" 123 PB ", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new BinarySize(138485688541642752), result);

        // Negative
        Assert.IsTrue(BinarySize.TryParse("-123.5KB", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new BinarySize(-126464), result);

        // Explicit culture test:
        Assert.IsTrue(BinarySize.TryParse("123,5KB", new CultureInfo("nl-NL"), out result));
        Assert.AreEqual(new BinarySize(126464), result);

        // Test version without provider uses current culture (weak test but it'll do)
        string size = string.Format(CultureInfo.CurrentCulture, "{0:0.0}KB", 123.5);
        Assert.IsTrue(BinarySize.TryParse(size, out result));
        Assert.AreEqual(new BinarySize(126464), result);

        // Empty span
        Assert.IsTrue(BinarySize.TryParse(ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new BinarySize(0), result);

        // Invalid number.
        Assert.IsFalse(BinarySize.TryParse("asdf", CultureInfo.InvariantCulture, out _));

        // Overflow.
        Assert.IsFalse(BinarySize.TryParse("1234EB", CultureInfo.InvariantCulture, out _));
    }

    [TestMethod]
    public void TestParseDecimal()
    { 
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
        Assert.AreEqual("123456789012345678B", target.ToString("B", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012345678 B", target.ToString(" B", CultureInfo.InvariantCulture));
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
        Assert.AreEqual("1KB", ((BinarySize)1024).ToString("Kb", CultureInfo.CurrentCulture));
        Assert.AreEqual("1KiB", ((BinarySize)1024).ToString("kIb", CultureInfo.CurrentCulture));
        Assert.AreEqual("1KB", ((BinarySize)1024).ToString("Ab", CultureInfo.CurrentCulture));
        Assert.AreEqual("1.5KiB", ((BinarySize)1536).ToString("sIb", CultureInfo.CurrentCulture));

        // Negative
        Assert.AreEqual("-2048KiB", BinarySize.FromMebi(-2).ToString("KiB", CultureInfo.CurrentCulture));
        Assert.AreEqual("-2MiB", BinarySize.FromMebi(-2).ToString("AiB", CultureInfo.CurrentCulture));
        Assert.AreEqual("-1.5KiB", ((BinarySize)(-1536)).ToString("SiB", CultureInfo.CurrentCulture));

        var unitInfo = new BinaryUnitInfo()
        {
            ShortKilo = "L",
            ShortDecimalKilo = "l",
            ShortKibi = "Lj",
            ShortByte = "C",
            ShortBytes = "Cs",
            ShortConnector = "-",
        };

        // Test custom format provider
        Assert.AreEqual("2 Lj-Cs", BinarySize.FromKibi(2).ToString(" KiB", unitInfo));
        Assert.AreEqual("1 Lj-C", BinarySize.FromKibi(1).ToString(" KiB", unitInfo));
        Assert.AreEqual("1 L-C", BinarySize.FromKibi(1).ToString(" KB", unitInfo));
        Assert.AreEqual("1 l-C", ((BinarySize)1000).ToString(" kB", unitInfo));

        // Test IFormattable/ISpanFormattable
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

        target = new BinarySize(1234000);
        Assert.AreEqual("1234kB", target.ToString("aB", CultureInfo.InvariantCulture));
        Assert.AreEqual("1234000 B", target.ToString(" aiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("1.234MB", target.ToString("sB", CultureInfo.InvariantCulture));
        Assert.AreEqual("1.1768341064453125MiB", target.ToString("siB", CultureInfo.InvariantCulture));
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

        var unitInfo = new BinaryUnitInfo()
        {
            ShortKilo = "L",
            ShortDecimalKilo = "l",
            ShortKibi = "Lj",
            ShortByte = "C",
            ShortBytes = "Cs",
            ShortConnector = "-",
        };

        // Test custom format provider
        Assert.IsTrue(BinarySize.FromKibi(2).TryFormat(destination.AsSpan(), out charsWritten, " KiB".AsSpan(), unitInfo));
        Assert.AreEqual("2 Lj-Cs", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(BinarySize.FromKibi(1).TryFormat(destination.AsSpan(), out charsWritten, " KiB".AsSpan(), unitInfo));
        Assert.AreEqual("1 Lj-C", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(BinarySize.FromKibi(1).TryFormat(destination.AsSpan(), out charsWritten, " KB".AsSpan(), unitInfo));
        Assert.AreEqual("1 L-C", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(((BinarySize)1000).TryFormat(destination.AsSpan(), out charsWritten, " kB".AsSpan(), unitInfo));
        Assert.AreEqual("1 l-C", destination.AsSpan(0, charsWritten).ToString());

        // Decimal
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "kb".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126.464kB", destination.AsSpan(0, charsWritten).ToString());
    }

#endif

    [TestMethod]
    public void TestComparison()
    {
        Assert.AreEqual(new BinarySize(123), new BinarySize(123));
        Assert.AreNotEqual(new BinarySize(123), new BinarySize(124));
        Assert.IsTrue(new BinarySize(123).Equals(new BinarySize(123)));
        Assert.IsFalse(new BinarySize(123).Equals(new BinarySize(124)));
        Assert.IsTrue(new BinarySize(123) == new BinarySize(123));
        Assert.IsTrue(new BinarySize(123) == 123);
        Assert.IsTrue(123 == new BinarySize(123));
        Assert.IsFalse(new BinarySize(123) == new BinarySize(124));
        Assert.IsFalse(new BinarySize(123) == 124);
        Assert.IsFalse(124 == new BinarySize(123));
        Assert.IsTrue(new BinarySize(123) != new BinarySize(124));
        Assert.IsTrue(new BinarySize(123) != 124);
        Assert.IsTrue(124 != new BinarySize(123));
        Assert.IsFalse(new BinarySize(123) != new BinarySize(123));
        Assert.IsFalse(new BinarySize(123) != 123);
        Assert.IsFalse(123 != new BinarySize(123));
        Assert.IsTrue(new BinarySize(123) < new BinarySize(124));
        Assert.IsFalse(new BinarySize(123) < new BinarySize(123));
        Assert.IsFalse(new BinarySize(124) < new BinarySize(123));
        Assert.IsTrue(123 < new BinarySize(124));
        Assert.IsFalse(123 < new BinarySize(123));
        Assert.IsFalse(124 < new BinarySize(123));
        Assert.IsTrue(new BinarySize(123) < 124);
        Assert.IsFalse(new BinarySize(123) < 123);
        Assert.IsFalse(new BinarySize(124) < 123);
        Assert.IsTrue(new BinarySize(123) <= new BinarySize(124));
        Assert.IsTrue(new BinarySize(123) <= new BinarySize(123));
        Assert.IsFalse(new BinarySize(124) <= new BinarySize(123));
        Assert.IsTrue(123 <= new BinarySize(124));
        Assert.IsTrue(123 <= new BinarySize(123));
        Assert.IsFalse(124 <= new BinarySize(123));
        Assert.IsTrue(new BinarySize(123) <= 124);
        Assert.IsTrue(new BinarySize(123) <= 123);
        Assert.IsFalse(new BinarySize(124) <= 123);
        Assert.IsFalse(new BinarySize(123) > new BinarySize(124));
        Assert.IsFalse(new BinarySize(123) > new BinarySize(123));
        Assert.IsTrue(new BinarySize(124) > new BinarySize(123));
        Assert.IsFalse(123 > new BinarySize(124));
        Assert.IsFalse(123 > new BinarySize(123));
        Assert.IsTrue(124 > new BinarySize(123));
        Assert.IsFalse(new BinarySize(123) > 124);
        Assert.IsFalse(new BinarySize(123) > 123);
        Assert.IsTrue(new BinarySize(124) > 123);
        Assert.IsFalse(new BinarySize(123) >= new BinarySize(124));
        Assert.IsTrue(new BinarySize(123) >= new BinarySize(123));
        Assert.IsTrue(new BinarySize(124) >= new BinarySize(123));
        Assert.IsFalse(123 >= new BinarySize(124));
        Assert.IsTrue(123 >= new BinarySize(123));
        Assert.IsTrue(124 >= new BinarySize(123));
        Assert.IsFalse(new BinarySize(123) >= 124);
        Assert.IsTrue(new BinarySize(123) >= 123);
        Assert.IsTrue(new BinarySize(124) >= 123);

        Assert.AreEqual(-1, new BinarySize(123).CompareTo(new BinarySize(124)));
        Assert.AreEqual(0, new BinarySize(123).CompareTo(new BinarySize(123)));
        Assert.AreEqual(1, new BinarySize(124).CompareTo(new BinarySize(123)));
    }

    [TestMethod]
    public void TestArithmeticOperations()
    {
        var value1 = 123L;
        var value2 = 321L;
        var size1 = (BinarySize)value1;
        var size2 = (BinarySize)value2;

        Assert.AreEqual((BinarySize)(value1 + value2), size1 + size2);
        Assert.AreEqual((BinarySize)(value1 + value2), size1 + value2);
        Assert.AreEqual((BinarySize)(value1 + value2), value1 + size2);
        Assert.AreEqual((BinarySize)(value1 - value2), size1 - size2);
        Assert.AreEqual((BinarySize)(value1 - value2), size1 - value2);
        Assert.AreEqual((BinarySize)(value1 - value2), value1 - size2);
        Assert.AreEqual((BinarySize)(value1 * value2), size1 * size2);
        Assert.AreEqual((BinarySize)(value1 * value2), value1 * size2);
        Assert.AreEqual((BinarySize)(value1 * value2), size1 * value2);
        Assert.AreEqual((BinarySize)(value2 / value1), size2 / size1);
        Assert.AreEqual((BinarySize)(value2 / value1), value2 / size1);
        Assert.AreEqual((BinarySize)(value2 / value1), size2 / value1);
        Assert.AreEqual((BinarySize)(value2 % value1), size2 % size1);
        Assert.AreEqual((BinarySize)(value2 % value1), value2 % size1);
        Assert.AreEqual((BinarySize)(value2 % value1), size2 % value1);
        Assert.AreEqual((BinarySize)(value2++), size2++);
        Assert.AreEqual((BinarySize)(--value2), --size2);
        Assert.AreEqual((BinarySize)(-value2), -size2);
        Assert.AreEqual((BinarySize)(+value2), +size2);

        Assert.AreEqual((BinarySize)(value1 + value2), BinarySize.Add(size1, size2));
        Assert.AreEqual((BinarySize)(value1 - value2), BinarySize.Subtract(size1, size2));
        Assert.AreEqual((BinarySize)(value1 * value2), BinarySize.Multiply(size1, size2));
        Assert.AreEqual((BinarySize)(value2 / value1), BinarySize.Divide(size2, size1));
        Assert.AreEqual((BinarySize)(value2 % value1), BinarySize.Remainder(size2, size1));
        Assert.AreEqual((BinarySize)(-value2), BinarySize.Negate(size2));
    }

    [TestMethod]
    public void TestCheckedArithmeticOperations()
    {
        checked
        {
            var value1 = 123L;
            var value2 = 321L;
            var size1 = (BinarySize)value1;
            var size2 = (BinarySize)value2;

            Assert.AreEqual((BinarySize)(value1 + value2), size1 + size2);
            Assert.AreEqual((BinarySize)(value1 + value2), size1 + value2);
            Assert.AreEqual((BinarySize)(value1 + value2), value1 + size2);
            Assert.AreEqual((BinarySize)(value1 - value2), size1 - size2);
            Assert.AreEqual((BinarySize)(value1 - value2), size1 - value2);
            Assert.AreEqual((BinarySize)(value1 - value2), value1 - size2);
            Assert.AreEqual((BinarySize)(value1 * value2), size1 * size2);
            Assert.AreEqual((BinarySize)(value1 * value2), value1 * size2);
            Assert.AreEqual((BinarySize)(value1 * value2), size1 * value2);
            Assert.AreEqual((BinarySize)(value2 / value1), size2 / size1);
            Assert.AreEqual((BinarySize)(value2 / value1), value2 / size1);
            Assert.AreEqual((BinarySize)(value2 / value1), size2 / value1);
            Assert.AreEqual((BinarySize)(value2 % value1), size2 % size1);
            Assert.AreEqual((BinarySize)(value2 % value1), value2 % size1);
            Assert.AreEqual((BinarySize)(value2 % value1), size2 % value1);
            Assert.AreEqual((BinarySize)(value2++), size2++);
            Assert.AreEqual((BinarySize)(--value2), --size2);
            Assert.AreEqual((BinarySize)(-value2), -size2);
            Assert.AreEqual((BinarySize)(+value2), +size2);

            Assert.ThrowsException<OverflowException>(() => BinarySize.MaxValue + BinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => long.MaxValue + BinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => BinarySize.MaxValue + long.MaxValue);
            Assert.ThrowsException<OverflowException>(() => BinarySize.MinValue - BinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => long.MinValue - BinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => BinarySize.MinValue - long.MaxValue);
            Assert.ThrowsException<OverflowException>(() => BinarySize.MaxValue * BinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => long.MaxValue * BinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => BinarySize.MaxValue * long.MaxValue);
            size1 = BinarySize.MaxValue;
            Assert.ThrowsException<OverflowException>(() => size1++);
            size1 = BinarySize.MinValue;
            Assert.ThrowsException<OverflowException>(() => size1--);
        }
    }

    [TestMethod]
    public void TestBinaryOperations()
    {
        var value1 = 123L;
        var value2 = 321L;
        var size1 = (BinarySize)value1;
        var size2 = (BinarySize)value2;

        Assert.AreEqual((BinarySize)(value1 >> 2), size1 >> 2);
        Assert.AreEqual((BinarySize)(value1 << 2), size1 << 2);
        Assert.AreEqual((BinarySize)(-123L >> 2), (BinarySize)(-123) >> 2);
        Assert.AreEqual((BinarySize)(-123L >>> 2), (BinarySize)(-123) >>> 2);
        Assert.AreEqual((BinarySize)(value1 & value2), size1 & size2);
        Assert.AreEqual((BinarySize)(value1 & value2), value1 & size2);
        Assert.AreEqual((BinarySize)(value1 & value2), size1 & value2);
        Assert.AreEqual((BinarySize)(value1 | value2), size1 | size2);
        Assert.AreEqual((BinarySize)(value1 | value2), value1 | size2);
        Assert.AreEqual((BinarySize)(value1 | value2), size1 | value2);
        Assert.AreEqual((BinarySize)(value2 ^ value1), size2 ^ size1);
        Assert.AreEqual((BinarySize)(value2 ^ value1), value2 ^ size1);
        Assert.AreEqual((BinarySize)(value2 ^ value1), size2 ^ value1);
        Assert.AreEqual((BinarySize)(~value1), ~size1);
    }

    [TestMethod]
    public void TestSum()
    {
        var values = new[] { (BinarySize)5, (BinarySize)6, (BinarySize)7, (BinarySize)8 };
        var sum = values.Sum();
        Assert.AreEqual((BinarySize)26, sum);

        var values2 = new BinarySize?[] { (BinarySize)5, (BinarySize)6, null, (BinarySize)7, (BinarySize)8 };
        var sum2 = values2.Sum();
        Assert.AreEqual((BinarySize)26, sum2);

        var values3 = new[] { "5", "6", "7", "8" };
        var sum3 = values3.Sum(v => BinarySize.Parse(v));
        Assert.AreEqual((BinarySize)26, sum3);

        var converter = TypeDescriptor.GetConverter(typeof(BinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" };
        var sum4 = values4.Sum(v => (BinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((BinarySize)26, sum4);

        Assert.AreEqual(BinarySize.Zero, Enumerable.Empty<BinarySize>().Sum());
        Assert.AreEqual(BinarySize.Zero, new[] { (BinarySize?)null }.Sum());
    }

    [TestMethod]
    public async Task TestSumAsync()
    {
        var values = new[] { (BinarySize)5, (BinarySize)6, (BinarySize)7, (BinarySize)8 }.ToAsyncEnumerable();
        var sum = await values.SumAsync();
        Assert.AreEqual((BinarySize)26, sum);

        var values2 = new BinarySize?[] { (BinarySize)5, (BinarySize)6, null, (BinarySize)7, (BinarySize)8 }.ToAsyncEnumerable();
        var sum2 = await values2.SumAsync();
        Assert.AreEqual((BinarySize)26, sum2);

        var values3 = new[] { "5", "6", "7", "8" }.ToAsyncEnumerable();
        var sum3 = await values3.SumAsync(v => BinarySize.Parse(v));
        Assert.AreEqual((BinarySize)26, sum3);

        var converter = TypeDescriptor.GetConverter(typeof(BinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" }.ToAsyncEnumerable();
        var sum4 = await values4.SumAsync(v => (BinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((BinarySize)26, sum4);

        Assert.AreEqual(BinarySize.Zero, await AsyncEnumerable.Empty<BinarySize>().SumAsync());
        Assert.AreEqual(BinarySize.Zero, await new[] { (BinarySize?)null }.ToAsyncEnumerable().SumAsync());
    }

    [TestMethod]
    public void TestAverage()
    {
        // Average truncates, so the result is 6.
        var values = new[] { (BinarySize)5, (BinarySize)6, (BinarySize)7, (BinarySize)8 };
        var average = values.Average();
        Assert.AreEqual((BinarySize)6, average);

        var values2 = new BinarySize?[] { (BinarySize)5, (BinarySize)6, null, (BinarySize)7, (BinarySize)8 };
        var average2 = values2.Average();
        Assert.AreEqual((BinarySize)6, average2);

        var values3 = new[] { "5", "6", "7", "8" };
        var average3 = values3.Average(v => BinarySize.Parse(v));
        Assert.AreEqual((BinarySize)6, average3);

        var converter = TypeDescriptor.GetConverter(typeof(BinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" };
        var average4 = values4.Average(v => (BinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((BinarySize)6, average4);

        Assert.IsNull(Enumerable.Empty<BinarySize?>().Average());
        Assert.IsNull(new[] { (BinarySize?)null }.Average());
    }

    [TestMethod]
    public async Task TestAverageAsync()
    {
        // Average truncates, so the result is 6.
        var values = new[] { (BinarySize)5, (BinarySize)6, (BinarySize)7, (BinarySize)8 }.ToAsyncEnumerable();
        var average = await values.AverageAsync();
        Assert.AreEqual((BinarySize)6, average);

        var values2 = new BinarySize?[] { (BinarySize)5, (BinarySize)6, null, (BinarySize)7, (BinarySize)8 }.ToAsyncEnumerable();
        var average2 = await values2.AverageAsync();
        Assert.AreEqual((BinarySize)6, average2);

        var values3 = new[] { "5", "6", "7", "8" }.ToAsyncEnumerable();
        var average3 = await values3.AverageAsync(v => BinarySize.Parse(v));
        Assert.AreEqual((BinarySize)6, average3);

        var converter = TypeDescriptor.GetConverter(typeof(BinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" }.ToAsyncEnumerable();
        var average4 = await values4.AverageAsync(v => (BinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((BinarySize)6, average4);

        Assert.IsNull(await AsyncEnumerable.Empty<BinarySize?>().AverageAsync());
        Assert.IsNull(await new[] { (BinarySize?)null }.ToAsyncEnumerable().AverageAsync());
    }

    [TestMethod]
    public void TestXmlSerialization()
    {
        var serializer = new XmlSerializer(typeof(SerializationTest));
        using var writer = new StringWriter();
        serializer.Serialize(writer, new SerializationTest() { Value = 10, Size = BinarySize.FromMebi(1.5) });
        var actual = writer.ToString();
        Assert.AreEqual(_expectedXml, actual);

        using var reader = new StringReader(actual);
        var result = (SerializationTest)serializer.Deserialize(reader)!;
        Assert.AreEqual(BinarySize.FromMebi(1.5), result.Size);
        Assert.AreEqual(10, result.Value);
    }

    [TestMethod]
    public void TestDataContractSerialization()
    {
        var serializer = new DataContractSerializer(typeof(SerializationTest));
        using var writer = new StringWriter();
        using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });
        serializer.WriteObject(xmlWriter, new SerializationTest() { Value = 10, Size = BinarySize.FromMebi(1.5) });
        xmlWriter.Flush();
        var actual = writer.ToString();
        Assert.AreEqual(_expectedDataContract, actual);

        using var reader = new StringReader(actual);
        using var xmlReader = XmlReader.Create(reader);
        var result = (SerializationTest)serializer.ReadObject(xmlReader)!;
        Assert.AreEqual(BinarySize.FromMebi(1.5), result.Size);
        Assert.AreEqual(10, result.Value);
    }

    [TestMethod]
    public void TestJsonSerialization()
    {
        var json = JsonSerializer.Serialize(new SerializationTest() { Value = 10, Size = BinarySize.FromMebi(1.5) });
        Assert.AreEqual("{\"Size\":\"1536 KiB\",\"Value\":10}", json);
        var result = JsonSerializer.Deserialize<SerializationTest>(json)!;
        Assert.AreEqual(BinarySize.FromMebi(1.5), result.Size);
        Assert.AreEqual(10, result.Value);
    }

#if NET6_0_OR_GREATER

    private static readonly string _expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<SerializationTest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Size>1536 KiB</Size>
  <Value>10</Value>
</SerializationTest>".ReplaceLineEndings();

#else

    private static readonly string _expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<SerializationTest xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Size>1536 KiB</Size>
  <Value>10</Value>
</SerializationTest>".ReplaceLineEndings();

#endif

    private static readonly string _expectedDataContract = @"<?xml version=""1.0"" encoding=""utf-16""?>
<SerializationTest xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/Ookii.Test"">
  <Size>1536 KiB</Size>
  <Value>10</Value>
</SerializationTest>".ReplaceLineEndings();
}
