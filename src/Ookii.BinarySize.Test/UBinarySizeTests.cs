using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
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
public class UBinarySizeTests
{
    [TestMethod]
    public void TestValue()
    {
        Assert.AreEqual(0ul, UBinarySize.Zero.Value);
        Assert.AreEqual(ulong.MinValue, UBinarySize.MinValue.Value);
        Assert.AreEqual(ulong.MaxValue, UBinarySize.MaxValue.Value);

        var size = new UBinarySize(512);
        Assert.AreEqual(512ul, size.Value);
        Assert.AreEqual(0.5, size.AsKibi);

        size = UBinarySize.FromKibi(2.5);
        Assert.AreEqual(2_560ul, size.Value);
        Assert.AreEqual(2.5, size.AsKibi);

        size = UBinarySize.FromMebi(2.5);
        Assert.AreEqual(2_621_440ul, size.Value);
        Assert.AreEqual(2.5, size.AsMebi);

        size = UBinarySize.FromGibi(2.5);
        Assert.AreEqual(2_684_354_560ul, size.Value);
        Assert.AreEqual(2.5, size.AsGibi);

        size = UBinarySize.FromTebi(2.5);
        Assert.AreEqual(2_748_779_069_440ul, size.Value);
        Assert.AreEqual(2.5, size.AsTebi);

        size = UBinarySize.FromPebi(2.5);
        Assert.AreEqual(2_814_749_767_106_560ul, size.Value);
        Assert.AreEqual(2.5, size.AsPebi);

        size = UBinarySize.FromExbi(2.5);
        Assert.AreEqual(2_882_303_761_517_117_440ul, size.Value);
        Assert.AreEqual(2.5, size.AsExbi);
    }

    [TestMethod]
    public void TestFromNegative()
    {
        Assert.ThrowsException<OverflowException>(() => UBinarySize.FromKibi(-1));
        Assert.ThrowsException<OverflowException>(() => UBinarySize.FromMebi(-1));
        Assert.ThrowsException<OverflowException>(() => UBinarySize.FromGibi(-1));
        Assert.ThrowsException<OverflowException>(() => UBinarySize.FromTebi(-1));
        Assert.ThrowsException<OverflowException>(() => UBinarySize.FromPebi(-1));
        Assert.ThrowsException<OverflowException>(() => UBinarySize.FromExbi(-1));
    }

    [TestMethod]
    public void TestParse()
    {
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123B", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123 B", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123KB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123KiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123K", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123MB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123MiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123M", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123GB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123GiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123G", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123TB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123TiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123T", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123PB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123PiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123P", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123 PB ", CultureInfo.InvariantCulture)); // with some spaces.
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5EB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5EiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5E", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5 EB ", CultureInfo.InvariantCulture)); // with some spaces.

        // Negative
        Assert.ThrowsException<OverflowException>(() => UBinarySize.Parse("-123.5KB", CultureInfo.InvariantCulture));

        // Explicit culture test:
        Assert.AreEqual(new UBinarySize(126464), UBinarySize.Parse("123.5KB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(126464), UBinarySize.Parse("123,5KB", new CultureInfo("nl-NL")));
        // Test version without provider uses current culture (weak test but it'll do)
        string size = string.Format(CultureInfo.CurrentCulture, "{0:0.0}KB", 123.5);
        Assert.AreEqual(new UBinarySize(126464), UBinarySize.Parse(size));

        // Empty span
        Assert.AreEqual((UBinarySize)0, UBinarySize.Parse(ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture));

        var unitInfo = new BinaryUnitInfo()
        {
            ShortKilo = "L",
            ShortDecimalKilo = "l",
            ShortKibi = "Lj",
            ShortByte = "C",
            ShortBytes = "Cs",
            ShortConnector = ":",
        };

        // Test custom format provider
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 Lj:Cs", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 Lj:C", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 LjC", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 L:C", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 LC", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 lc", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 ljc", BinarySizeOptions.UseIecStandard, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual((UBinarySize)2000, UBinarySize.Parse("2 lc", BinarySizeOptions.UseIecStandard, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual((UBinarySize)2, UBinarySize.Parse("2 C", BinarySizeOptions.UseIecStandard, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual((UBinarySize)2, UBinarySize.Parse("2 cs", BinarySizeOptions.UseIecStandard, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.ThrowsException<FormatException>(() => UBinarySize.Parse("2 :cs", BinarySizeOptions.UseIecStandard, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
    }

    [TestMethod]
    public void TestTryParse()
    {
        Assert.IsTrue(UBinarySize.TryParse("123", CultureInfo.InvariantCulture, out var result));
        Assert.AreEqual(new UBinarySize(123), result);
        Assert.IsTrue(UBinarySize.TryParse("123B", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(123), result);
        Assert.IsTrue(UBinarySize.TryParse("123PB", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse("123PiB", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse("123P", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse(" 123 PB ", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse(" 123 pb ", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);

        // Negative
        Assert.IsFalse(UBinarySize.TryParse("-123.5KB", CultureInfo.InvariantCulture, out result));

        // Explicit culture test:
        Assert.IsTrue(UBinarySize.TryParse("123,5KB", new CultureInfo("nl-NL"), out result));
        Assert.AreEqual(new UBinarySize(126464), result);

        // Test version without provider uses current culture (weak test but it'll do)
        string size = string.Format(CultureInfo.CurrentCulture, "{0:0.0}KB", 123.5);
        Assert.IsTrue(UBinarySize.TryParse(size, out result));
        Assert.AreEqual(new UBinarySize(126464), result);

        // Empty span
        Assert.IsTrue(UBinarySize.TryParse(ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(0), result);

        // Invalid number.
        Assert.IsFalse(UBinarySize.TryParse("asdf", CultureInfo.InvariantCulture, out _));

        // Overflow.
        Assert.IsFalse(UBinarySize.TryParse("1234EB", CultureInfo.InvariantCulture, out _));

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
        Assert.IsTrue(UBinarySize.TryParse("2 Lj-Cs", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo), out result));
        Assert.AreEqual(UBinarySize.FromKibi(2), result);
        Assert.IsTrue(UBinarySize.TryParse("2 LjC", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo), out result));
        Assert.AreEqual(UBinarySize.FromKibi(2), result);
        Assert.IsTrue(UBinarySize.TryParse("2 L-C", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo), out result));
        Assert.AreEqual(UBinarySize.FromKibi(2), result);
        Assert.IsTrue(UBinarySize.TryParse("2 LC", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo), out result));
        Assert.AreEqual(UBinarySize.FromKibi(2), result);
        Assert.IsTrue(UBinarySize.TryParse("2 lc", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo), out result));
        Assert.AreEqual(UBinarySize.FromKibi(2), result);
        Assert.IsTrue(UBinarySize.TryParse("2 ljc", CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo), out result));
        Assert.AreEqual(UBinarySize.FromKibi(2), result);
        Assert.IsTrue(UBinarySize.TryParse("2 lc", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo), out result));
        Assert.AreEqual((UBinarySize)2000, result);

        // Decimal
        Assert.IsTrue(UBinarySize.TryParse("123PB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(123000000000000000), result);
        Assert.IsTrue(UBinarySize.TryParse("123PiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse("123P", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(123000000000000000), result);

        // Long
        Assert.IsTrue(UBinarySize.TryParse("123petabytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse("123pebibyte", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse("123peta", BinarySizeOptions.AllowLongUnitsOnly, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(138485688541642752), result);
        Assert.IsTrue(UBinarySize.TryParse("123petabytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
        Assert.AreEqual(new UBinarySize(123000000000000000), result);

        // Flags
        Assert.IsFalse(UBinarySize.TryParse("123petabytes", CultureInfo.InvariantCulture, out result));
        Assert.IsFalse(UBinarySize.TryParse("123PB", BinarySizeOptions.AllowLongUnitsOnly, NumberStyles.Number, CultureInfo.InvariantCulture, out result));
    }

    [TestMethod]
    public void TestParseDecimal()
    { 
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123B", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000), UBinarySize.Parse("123KB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123KiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000), UBinarySize.Parse("123K", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000), UBinarySize.Parse("123MB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123MiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000), UBinarySize.Parse("123M", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000), UBinarySize.Parse("123GB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123GiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000), UBinarySize.Parse("123G", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000), UBinarySize.Parse("123TB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123TiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000), UBinarySize.Parse("123T", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000000), UBinarySize.Parse("123PB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123PiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000000), UBinarySize.Parse("123P", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000000), UBinarySize.Parse("123 PB ", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture)); // with some spaces.
        Assert.AreEqual(new UBinarySize(5500000000000000000), UBinarySize.Parse("5.5EB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5EiB", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(5500000000000000000), UBinarySize.Parse("5.5E", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(5500000000000000000), UBinarySize.Parse("5.5 EB ", BinarySizeOptions.UseIecStandard, NumberStyles.Number, CultureInfo.InvariantCulture)); // with some spaces.
    }

    [TestMethod]
    public void TestParseLong()
    {
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123 byte", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123 bytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kilobytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123Kilobytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kiloBytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kibibytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kilo", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123megabytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123mebibytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123mebi", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123gigabytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123gibibyte", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123gibi", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123terabyte", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123tebibytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123tera", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123petabytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123pebibytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123pebi", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123 pebibyte ", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture)); // with some spaces.
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5exabytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5exbibytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5exa", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));

        // Negative
        Assert.ThrowsException<OverflowException>(() => UBinarySize.Parse("-123.5kilobytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));

        var unitInfo = new BinaryUnitInfo()
        {
            LongKilo = "foo",
            LongKibi = "bar",
            LongByte = "bit",
            LongBytes = "bits",
            LongConnector = ":",
        };

        // Test custom format provider
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 foo:bits", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 foo:bit", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 barbit", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 bar", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 FOO:BIT", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 BAR:BIT", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 FOObit", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 BaRbIT", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual((UBinarySize)2000, UBinarySize.Parse("2 foobits", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual(UBinarySize.FromKibi(2), UBinarySize.Parse("2 barbits", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual((UBinarySize)2, UBinarySize.Parse("2 bits", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.AreEqual((UBinarySize)2, UBinarySize.Parse("2 bit", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));
        Assert.ThrowsException<FormatException>(() => UBinarySize.Parse("2 :bits", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, provider: CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo)));

        // Test flags.
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kilobytes", BinarySizeOptions.AllowLongUnitsOnly, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kb", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.ThrowsException<FormatException>(() => UBinarySize.Parse("2 kilobytes", CultureInfo.InvariantCulture));
        Assert.ThrowsException<FormatException>(() => UBinarySize.Parse("123kb", BinarySizeOptions.AllowLongUnitsOnly, NumberStyles.Number, CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void TestParseDecimalLong()
    {
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123 byte", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123), UBinarySize.Parse("123bytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000), UBinarySize.Parse("123kilobytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kibibytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000), UBinarySize.Parse("123kilo", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000), UBinarySize.Parse("123megabytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123mebibytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(128974848), UBinarySize.Parse("123mebi", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000), UBinarySize.Parse("123mega", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000), UBinarySize.Parse("123gigabytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(132070244352), UBinarySize.Parse("123gibibytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000), UBinarySize.Parse("123giga", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000), UBinarySize.Parse("123terabytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(135239930216448), UBinarySize.Parse("123tebibytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000), UBinarySize.Parse("123tera", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000000), UBinarySize.Parse("123petabytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(138485688541642752), UBinarySize.Parse("123pebibytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(123000000000000000), UBinarySize.Parse("123peta", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(5500000000000000000), UBinarySize.Parse("5.5exabytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(6341068275337658368), UBinarySize.Parse("5.5exbibytes", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
        Assert.AreEqual(new UBinarySize(5500000000000000000), UBinarySize.Parse("5.5exa", BinarySizeOptions.UseIecStandard | BinarySizeOptions.AllowLongUnits, NumberStyles.Number, CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void TestParseCompareOptions()
    {
        // Make comparisons case sensitive to test custom options.
        var unitInfo = new BinaryUnitInfo()
        {
            CompareOptions = CompareOptions.None,
        };

        var culture = CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo);
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123KB", culture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kB", culture)); // Allowed because of short decimal kilo
        Assert.ThrowsException<FormatException>(() => UBinarySize.Parse("123Kb", culture));
        Assert.AreEqual(new UBinarySize(125952), UBinarySize.Parse("123kilobytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, culture));
        Assert.ThrowsException<FormatException>(() => UBinarySize.Parse("123Kilobytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, culture));
        Assert.ThrowsException<FormatException>(() => UBinarySize.Parse("123kiloBytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, culture));

        Assert.IsTrue(UBinarySize.TryParse("123KB", culture, out UBinarySize result));
        Assert.IsTrue(UBinarySize.TryParse("123kB", culture, out result));
        Assert.IsFalse(UBinarySize.TryParse("123Kb", culture, out _));
        Assert.AreEqual(new UBinarySize(125952), result);
        Assert.IsTrue(UBinarySize.TryParse("123kilobytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, culture, out result));
        Assert.AreEqual(new UBinarySize(125952), result);
        Assert.IsFalse(UBinarySize.TryParse("123Kilobytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, culture, out _));
        Assert.IsFalse(UBinarySize.TryParse("123kiloBytes", BinarySizeOptions.AllowLongUnits, NumberStyles.Number, culture, out _));
    }

    [TestMethod]
    public void TestToString()
    {
        var target = new UBinarySize(123456789012345678);
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
        Assert.AreEqual("123B", ((UBinarySize)123).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123B", ((UBinarySize)123).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464B", ((UBinarySize)126464).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5KB", ((UBinarySize)126464).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464KB", ((UBinarySize)129499136).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5MB", ((UBinarySize)129499136).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464MB", ((UBinarySize)132607115264).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5GB", ((UBinarySize)132607115264).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464GB", ((UBinarySize)135789686030336).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5TB", ((UBinarySize)135789686030336).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464TB", ((UBinarySize)139048638495064064).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5PB", ((UBinarySize)139048638495064064).ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012345678B", ((UBinarySize)123456789012345678).ToString("AB", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.7PB", ((UBinarySize)123456789012345678).ToString("0.#SB", CultureInfo.InvariantCulture));

        // Test with different options:
        Assert.AreEqual("126464", ((UBinarySize)126464).ToString("A", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5K", ((UBinarySize)126464).ToString("S", CultureInfo.InvariantCulture));
        Assert.AreEqual("126464B", ((UBinarySize)126464).ToString("AiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5KiB", ((UBinarySize)126464).ToString("SiB", CultureInfo.InvariantCulture));

        // Smaller than unit.
        Assert.AreEqual("0.5KB", ((UBinarySize)512).ToString("KB", CultureInfo.InvariantCulture));

        // Zero
        Assert.AreEqual("0 B", UBinarySize.Zero.ToString(null, CultureInfo.InvariantCulture));
        Assert.AreEqual("0B", UBinarySize.Zero.ToString("SB", CultureInfo.InvariantCulture));
        Assert.AreEqual("0KB", UBinarySize.Zero.ToString("KB", CultureInfo.InvariantCulture));

        // Test defaults, should have same effect as AB.
        string expected = 126464.ToString() + " KiB";
        Assert.AreEqual(expected, ((UBinarySize)129499136).ToString());
        Assert.AreEqual(expected, ((UBinarySize)129499136).ToString(null, CultureInfo.CurrentCulture));
        Assert.AreEqual(expected, ((UBinarySize)129499136).ToString(null, null));
        Assert.AreEqual(expected, ((UBinarySize)129499136).ToString(""));
        Assert.AreEqual(expected, ((UBinarySize)129499136).ToString("", null));

        // Case correction.
        Assert.AreEqual("1KB", ((UBinarySize)1024).ToString("Kb", CultureInfo.InvariantCulture));
        Assert.AreEqual("1KiB", ((UBinarySize)1024).ToString("kIb", CultureInfo.InvariantCulture));
        Assert.AreEqual("1KB", ((UBinarySize)1024).ToString("Ab", CultureInfo.InvariantCulture));
        Assert.AreEqual("1.5KiB", ((UBinarySize)1536).ToString("sIb", CultureInfo.InvariantCulture));

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
        var culture = CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo);
        Assert.AreEqual("2 Lj-Cs", UBinarySize.FromKibi(2).ToString(" KiB", culture));
        Assert.AreEqual("1 Lj-C", UBinarySize.FromKibi(1).ToString(" KiB", culture));
        Assert.AreEqual("1 L-C", UBinarySize.FromKibi(1).ToString(" KB", culture));
        Assert.AreEqual("1 l-C", ((UBinarySize)1000).ToString(" kB", culture));
        Assert.AreEqual("5 Cs", ((UBinarySize)5).ToString(" sB", culture));
        Assert.AreEqual("1 C", ((UBinarySize)1).ToString(" sB", culture));

        // Test IFormattable/ISpanFormattable
        Assert.AreEqual("test 109.7 PB test2", string.Format(CultureInfo.InvariantCulture, "test {0:0.# SB} test2", ((UBinarySize)123456789012345678)));
    }

    [TestMethod]
    public void TestToStringDecimal()
    {
        // Lowercase means decimal unless 'i' is present.
        var target = new UBinarySize(123456789012345678);
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

        target = new UBinarySize(1234000);
        Assert.AreEqual("1234kB", target.ToString("aB", CultureInfo.InvariantCulture));
        Assert.AreEqual("1234000 B", target.ToString(" aiB", CultureInfo.InvariantCulture));
        Assert.AreEqual("1.234MB", target.ToString("sB", CultureInfo.InvariantCulture));
        Assert.AreEqual("1.1768341064453125MiB", target.ToString("siB", CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void TestToStringLong()
    {
        var target = new UBinarySize(123456789012345678);
        Assert.AreEqual("123456789012345678 bytes", target.ToString(" byte", CultureInfo.InvariantCulture));
        Assert.AreEqual("120563270519868.826171875kilobytes", target.ToString("KByte", CultureInfo.InvariantCulture));
        Assert.AreEqual("120563270519868.826171875kibibytes", target.ToString("Kibyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("117737568867.05940055847167969megabytes", target.ToString("Mbyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("117737568867.05940055847167969mebibytes", target.ToString("Mibyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("114978094.59673769585788249969gigabytes", target.ToString("Gbyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("114978094.59673769585788249969gibibytes", target.ToString("Gibyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("112283.29550462665611121337861terabytes", target.ToString("Tbyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("112283.29550462665611121337861tebibytes", target.ToString("Tibyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.65165576623696885860681505petabytes", target.ToString("Pbyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("109.65165576623696885860681505pebibytes", target.ToString("Pibyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("0.1070816950842157899009832178exabytes", target.ToString("Ebyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("0.1070816950842157899009832178exbibytes", target.ToString("Eibyte", CultureInfo.InvariantCulture));

        // Decimal
        Assert.AreEqual("123456789012345678 bytes", target.ToString(" byte", CultureInfo.InvariantCulture));
        Assert.AreEqual("123456789012345.678kilobytes", target.ToString("kbyte", CultureInfo.InvariantCulture));

        target = new UBinarySize(1234000);
        Assert.AreEqual("1234kilobytes", target.ToString("abyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("1234000 bytes", target.ToString(" aibyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("1.234megabytes", target.ToString("sbyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("1.1768341064453125mebibytes", target.ToString("sibyte", CultureInfo.InvariantCulture));

        // Automatic units test:
        Assert.AreEqual("126464 kilobytes", ((UBinarySize)129499136).ToString(" AByte", CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5 mebibytes", ((UBinarySize)129499136).ToString(" SIBYTE", CultureInfo.InvariantCulture));

        // Singular
        Assert.AreEqual("1 byte", ((UBinarySize)1).ToString(" Abyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("1 kilobyte", UBinarySize.FromKibi(1).ToString(" Kbyte", CultureInfo.InvariantCulture));

        // Zero
        Assert.AreEqual("0 bytes", UBinarySize.Zero.ToString(" byte", CultureInfo.InvariantCulture));
        Assert.AreEqual("0bytes", UBinarySize.Zero.ToString("Sbyte", CultureInfo.InvariantCulture));
        Assert.AreEqual("0kilobytes", UBinarySize.Zero.ToString("Kbyte", CultureInfo.InvariantCulture));

        var unitInfo = new BinaryUnitInfo()
        {
            LongKilo = "foo",
            LongKibi = "bar",
            LongByte = "bit",
            LongBytes = "bits",
            LongConnector = "-",
        };

        // Test custom format provider
        var culture = CultureInfo.InvariantCulture.WithBinaryUnitInfo(unitInfo);
        Assert.AreEqual("2 bar-bits", UBinarySize.FromKibi(2).ToString(" Kibyte", culture));
        Assert.AreEqual("1 bar-bit", UBinarySize.FromKibi(1).ToString(" Kibyte", culture));
        Assert.AreEqual("1 foo-bit", UBinarySize.FromKibi(1).ToString(" Kbyte", culture));
        Assert.AreEqual("1 foo-bit", ((UBinarySize)1000).ToString(" kbyte", culture));
        Assert.AreEqual("5 bits", ((UBinarySize)5).ToString(" sbyte", culture));
        Assert.AreEqual("1 bit", ((UBinarySize)1).ToString(" sbyte", culture));

        // Test IFormattable/ISpanFormattable
        Assert.AreEqual("test 109.7 petabytes test2", string.Format(CultureInfo.InvariantCulture, "test {0:0.# Sbyte} test2", ((UBinarySize)123456789012345678)));
    }


#if NET6_0_OR_GREATER

    [TestMethod]
    public void TestTryFormat()
    {
        var size = new UBinarySize(126464);
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

        // Long units.
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, " Sbyte".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123.5 kilobytes", destination.AsSpan(0, charsWritten).ToString());

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
            LongKilo = "foo",
            LongKibi = "bar",
            LongByte = "bit",
            LongBytes = "bits",
            LongConnector = "-",
        };

        // Test custom format provider
        Assert.IsTrue(UBinarySize.FromKibi(2).TryFormat(destination.AsSpan(), out charsWritten, " KiB".AsSpan(), unitInfo));
        Assert.AreEqual("2 Lj-Cs", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(UBinarySize.FromKibi(1).TryFormat(destination.AsSpan(), out charsWritten, " KiB".AsSpan(), unitInfo));
        Assert.AreEqual("1 Lj-C", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(UBinarySize.FromKibi(1).TryFormat(destination.AsSpan(), out charsWritten, " KB".AsSpan(), unitInfo));
        Assert.AreEqual("1 L-C", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(((UBinarySize)1000).TryFormat(destination.AsSpan(), out charsWritten, " kB".AsSpan(), unitInfo));
        Assert.AreEqual("1 l-C", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(UBinarySize.FromKibi(2).TryFormat(destination.AsSpan(), out charsWritten, " Kibyte".AsSpan(), unitInfo));
        Assert.AreEqual("2 bar-bits", destination.AsSpan(0, charsWritten).ToString());

        // Decimal
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, "kb".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126.464kB", destination.AsSpan(0, charsWritten).ToString());
        Assert.IsTrue(size.TryFormat(destination.AsSpan(), out charsWritten, " kbyte".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("126.464 kilobytes", destination.AsSpan(0, charsWritten).ToString());
    }

#endif

    [TestMethod]
    public void TestComparison()
    {
        Assert.AreEqual(new UBinarySize(123), new UBinarySize(123));
        Assert.AreNotEqual(new UBinarySize(123), new UBinarySize(124));
        Assert.IsTrue(new UBinarySize(123).Equals(new UBinarySize(123)));
        Assert.IsFalse(new UBinarySize(123).Equals(new UBinarySize(124)));
        Assert.IsTrue(new UBinarySize(123) == new UBinarySize(123));
        Assert.IsTrue(new UBinarySize(123) == 123);
        Assert.IsTrue(123 == new UBinarySize(123));
        Assert.IsFalse(new UBinarySize(123) == new UBinarySize(124));
        Assert.IsFalse(new UBinarySize(123) == 124);
        Assert.IsFalse(124 == new UBinarySize(123));
        Assert.IsTrue(new UBinarySize(123) != new UBinarySize(124));
        Assert.IsTrue(new UBinarySize(123) != 124);
        Assert.IsTrue(124 != new UBinarySize(123));
        Assert.IsFalse(new UBinarySize(123) != new UBinarySize(123));
        Assert.IsFalse(new UBinarySize(123) != 123);
        Assert.IsFalse(123 != new UBinarySize(123));
        Assert.IsTrue(new UBinarySize(123) < new UBinarySize(124));
        Assert.IsFalse(new UBinarySize(123) < new UBinarySize(123));
        Assert.IsFalse(new UBinarySize(124) < new UBinarySize(123));
        Assert.IsTrue(123 < new UBinarySize(124));
        Assert.IsFalse(123 < new UBinarySize(123));
        Assert.IsFalse(124 < new UBinarySize(123));
        Assert.IsTrue(new UBinarySize(123) < 124);
        Assert.IsFalse(new UBinarySize(123) < 123);
        Assert.IsFalse(new UBinarySize(124) < 123);
        Assert.IsTrue(new UBinarySize(123) <= new UBinarySize(124));
        Assert.IsTrue(new UBinarySize(123) <= new UBinarySize(123));
        Assert.IsFalse(new UBinarySize(124) <= new UBinarySize(123));
        Assert.IsTrue(123 <= new UBinarySize(124));
        Assert.IsTrue(123 <= new UBinarySize(123));
        Assert.IsFalse(124 <= new UBinarySize(123));
        Assert.IsTrue(new UBinarySize(123) <= 124);
        Assert.IsTrue(new UBinarySize(123) <= 123);
        Assert.IsFalse(new UBinarySize(124) <= 123);
        Assert.IsFalse(new UBinarySize(123) > new UBinarySize(124));
        Assert.IsFalse(new UBinarySize(123) > new UBinarySize(123));
        Assert.IsTrue(new UBinarySize(124) > new UBinarySize(123));
        Assert.IsFalse(123 > new UBinarySize(124));
        Assert.IsFalse(123 > new UBinarySize(123));
        Assert.IsTrue(124 > new UBinarySize(123));
        Assert.IsFalse(new UBinarySize(123) > 124);
        Assert.IsFalse(new UBinarySize(123) > 123);
        Assert.IsTrue(new UBinarySize(124) > 123);
        Assert.IsFalse(new UBinarySize(123) >= new UBinarySize(124));
        Assert.IsTrue(new UBinarySize(123) >= new UBinarySize(123));
        Assert.IsTrue(new UBinarySize(124) >= new UBinarySize(123));
        Assert.IsFalse(123 >= new UBinarySize(124));
        Assert.IsTrue(123 >= new UBinarySize(123));
        Assert.IsTrue(124 >= new UBinarySize(123));
        Assert.IsFalse(new UBinarySize(123) >= 124);
        Assert.IsTrue(new UBinarySize(123) >= 123);
        Assert.IsTrue(new UBinarySize(124) >= 123);

        Assert.AreEqual(-1, new UBinarySize(123).CompareTo(new UBinarySize(124)));
        Assert.AreEqual(0, new UBinarySize(123).CompareTo(new UBinarySize(123)));
        Assert.AreEqual(1, new UBinarySize(124).CompareTo(new UBinarySize(123)));
    }

    [TestMethod]
    public void TestArithmeticOperations()
    {
        unchecked
        {
            var value1 = 123UL;
            var value2 = 321UL;
            var size1 = (UBinarySize)value1;
            var size2 = (UBinarySize)value2;

            Assert.AreEqual((UBinarySize)(value1 + value2), size1 + size2);
            Assert.AreEqual((UBinarySize)(value1 + value2), size1 + value2);
            Assert.AreEqual((UBinarySize)(value1 + value2), value1 + size2);
            Assert.AreEqual((UBinarySize)(value1 - value2), size1 - size2);
            Assert.AreEqual((UBinarySize)(value1 - value2), value1 - size2);
            Assert.AreEqual((UBinarySize)(value1 - value2), value1 - size2);
            Assert.AreEqual((UBinarySize)(value1 * value2), size1 * size2);
            Assert.AreEqual((UBinarySize)(value1 * value2), value1 * size2);
            Assert.AreEqual((UBinarySize)(value1 * value2), size1 * value2);
            Assert.AreEqual((UBinarySize)(value2 / value1), size2 / size1);
            Assert.AreEqual((UBinarySize)(value2 / value1), value2 / size1);
            Assert.AreEqual((UBinarySize)(value2 / value1), size2 / value1);
            Assert.AreEqual((UBinarySize)(value2 % value1), size2 % size1);
            Assert.AreEqual((UBinarySize)(value2 % value1), value2 % size1);
            Assert.AreEqual((UBinarySize)(value2 % value1), size2 % value1);
            Assert.AreEqual((UBinarySize)(value2++), size2++);
            Assert.AreEqual((UBinarySize)(--value2), --size2);
            Assert.AreEqual((UBinarySize)(+value2), +size2);

            Assert.AreEqual((UBinarySize)(value1 + value2), UBinarySize.Add(size1, size2));
            Assert.AreEqual((UBinarySize)(value2 - value1), UBinarySize.Subtract(size2, size1));
            Assert.AreEqual((UBinarySize)(value1 * value2), UBinarySize.Multiply(size1, size2));
            Assert.AreEqual((UBinarySize)(value2 / value1), UBinarySize.Divide(size2, size1));
            Assert.AreEqual((UBinarySize)(value2 % value1), UBinarySize.Remainder(size2, size1));
        }
    }

    [TestMethod]
    public void TestCheckedArithmeticOperations()
    {
        checked
        {
            var value1 = 123UL;
            var value2 = 321UL;
            var size1 = (UBinarySize)value1;
            var size2 = (UBinarySize)value2;

            Assert.AreEqual((UBinarySize)(value1 + value2), size1 + size2);
            Assert.AreEqual((UBinarySize)(value1 + value2), size1 + value2);
            Assert.AreEqual((UBinarySize)(value1 + value2), value1 + size2);
            Assert.AreEqual((UBinarySize)(value2 - value1), size2 - size1);
            Assert.AreEqual((UBinarySize)(value2 - value1), size2 - value1);
            Assert.AreEqual((UBinarySize)(value2 - value1), value2 - size1);
            Assert.AreEqual((UBinarySize)(value1 * value2), size1 * size2);
            Assert.AreEqual((UBinarySize)(value1 * value2), value1 * size2);
            Assert.AreEqual((UBinarySize)(value1 * value2), size1 * value2);
            Assert.AreEqual((UBinarySize)(value2 / value1), size2 / size1);
            Assert.AreEqual((UBinarySize)(value2 / value1), value2 / size1);
            Assert.AreEqual((UBinarySize)(value2 / value1), size2 / value1);
            Assert.AreEqual((UBinarySize)(value2 % value1), size2 % size1);
            Assert.AreEqual((UBinarySize)(value2 % value1), value2 % size1);
            Assert.AreEqual((UBinarySize)(value2 % value1), size2 % value1);
            Assert.AreEqual((UBinarySize)(value2++), size2++);
            Assert.AreEqual((UBinarySize)(--value2), --size2);
            Assert.AreEqual((UBinarySize)(+value2), +size2);

            Assert.ThrowsException<OverflowException>(() => size1 - size2);
            Assert.ThrowsException<OverflowException>(() => UBinarySize.MaxValue + UBinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => ulong.MaxValue + UBinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => UBinarySize.MaxValue + ulong.MaxValue);
            Assert.ThrowsException<OverflowException>(() => UBinarySize.MinValue - UBinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => ulong.MinValue - UBinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => UBinarySize.MinValue - ulong.MaxValue);
            Assert.ThrowsException<OverflowException>(() => UBinarySize.MaxValue * UBinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => ulong.MaxValue * UBinarySize.MaxValue);
            Assert.ThrowsException<OverflowException>(() => UBinarySize.MaxValue * ulong.MaxValue);
            size1 = UBinarySize.MaxValue;
            Assert.ThrowsException<OverflowException>(() => size1++);
            size1 = UBinarySize.MinValue;
            Assert.ThrowsException<OverflowException>(() => size1--);
        }
    }

    [TestMethod]
    public void TestBinaryOperations()
    {
        var value1 = 123UL;
        var value2 = 321UL;
        var size1 = (UBinarySize)value1;
        var size2 = (UBinarySize)value2;

        Assert.AreEqual((UBinarySize)(value1 >> 2), size1 >> 2);
        Assert.AreEqual((UBinarySize)(value1 << 2), size1 << 2);
        Assert.AreEqual((UBinarySize)(value1 & value2), size1 & size2);
        Assert.AreEqual((UBinarySize)(value1 & value2), value1 & size2);
        Assert.AreEqual((UBinarySize)(value1 & value2), size1 & value2);
        Assert.AreEqual((UBinarySize)(value1 | value2), size1 | size2);
        Assert.AreEqual((UBinarySize)(value1 | value2), value1 | size2);
        Assert.AreEqual((UBinarySize)(value1 | value2), size1 | value2);
        Assert.AreEqual((UBinarySize)(value2 ^ value1), size2 ^ size1);
        Assert.AreEqual((UBinarySize)(value2 ^ value1), value2 ^ size1);
        Assert.AreEqual((UBinarySize)(value2 ^ value1), size2 ^ value1);
        Assert.AreEqual((UBinarySize)(~value1), ~size1);
    }

    [TestMethod]
    public void TestSum()
    {
        var values = new[] { (UBinarySize)5, (UBinarySize)6, (UBinarySize)7, (UBinarySize)8 };
        var sum = values.Sum();
        Assert.AreEqual((UBinarySize)26, sum);

        var values2 = new UBinarySize?[] { (UBinarySize)5, (UBinarySize)6, null, (UBinarySize)7, (UBinarySize)8 };
        var sum2 = values2.Sum();
        Assert.AreEqual((UBinarySize)26, sum2);

        var values3 = new[] { "5", "6", "7", "8" };
        var sum3 = values3.Sum(v => UBinarySize.Parse(v));
        Assert.AreEqual((UBinarySize)26, sum3);

        var converter = TypeDescriptor.GetConverter(typeof(UBinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" };
        var sum4 = values4.Sum(v => (UBinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((UBinarySize)26, sum4);

        Assert.AreEqual(UBinarySize.Zero, Enumerable.Empty<UBinarySize>().Sum());
        Assert.AreEqual(UBinarySize.Zero, new[] { (UBinarySize?)null }.Sum());
    }

    [TestMethod]
    public async Task TestSumAsync()
    {
        var values = new[] { (UBinarySize)5, (UBinarySize)6, (UBinarySize)7, (UBinarySize)8 }.ToAsyncEnumerable();
        var sum = await values.SumAsync();
        Assert.AreEqual((UBinarySize)26, sum);

        var values2 = new UBinarySize?[] { (UBinarySize)5, (UBinarySize)6, null, (UBinarySize)7, (UBinarySize)8 }.ToAsyncEnumerable();
        var sum2 = await values2.SumAsync();
        Assert.AreEqual((UBinarySize)26, sum2);

        var values3 = new[] { "5", "6", "7", "8" }.ToAsyncEnumerable();
        var sum3 = await values3.SumAsync(v => UBinarySize.Parse(v));
        Assert.AreEqual((UBinarySize)26, sum3);

        var converter = TypeDescriptor.GetConverter(typeof(UBinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" }.ToAsyncEnumerable();
        var sum4 = await values4.SumAsync(v => (UBinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((UBinarySize)26, sum4);

        Assert.AreEqual(UBinarySize.Zero, await AsyncEnumerable.Empty<UBinarySize>().SumAsync());
        Assert.AreEqual(UBinarySize.Zero, await new[] { (UBinarySize?)null }.ToAsyncEnumerable().SumAsync());
    }

    [TestMethod]
    public void TestAverage()
    {
        // Average truncates, so the result is 6.
        var values = new[] { (UBinarySize)5, (UBinarySize)6, (UBinarySize)7, (UBinarySize)8 };
        var average = values.Average();
        Assert.AreEqual((UBinarySize)6, average);

        var values2 = new UBinarySize?[] { (UBinarySize)5, (UBinarySize)6, null, (UBinarySize)7, (UBinarySize)8 };
        var average2 = values2.Average();
        Assert.AreEqual((UBinarySize)6, average2);

        var values3 = new[] { "5", "6", "7", "8" };
        var average3 = values3.Average(v => UBinarySize.Parse(v));
        Assert.AreEqual((UBinarySize)6, average3);

        var converter = TypeDescriptor.GetConverter(typeof(UBinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" };
        var average4 = values4.Average(v => (UBinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((UBinarySize)6, average4);

        Assert.IsNull(Enumerable.Empty<UBinarySize?>().Average());
        Assert.IsNull(new[] { (UBinarySize?)null }.Average());
    }

    [TestMethod]
    public async Task TestAverageAsync()
    {
        // Average truncates, so the result is 6.
        var values = new[] { (UBinarySize)5, (UBinarySize)6, (UBinarySize)7, (UBinarySize)8 }.ToAsyncEnumerable();
        var average = await values.AverageAsync();
        Assert.AreEqual((UBinarySize)6, average);

        var values2 = new UBinarySize?[] { (UBinarySize)5, (UBinarySize)6, null, (UBinarySize)7, (UBinarySize)8 }.ToAsyncEnumerable();
        var average2 = await values2.AverageAsync();
        Assert.AreEqual((UBinarySize)6, average2);

        var values3 = new[] { "5", "6", "7", "8" }.ToAsyncEnumerable();
        var average3 = await values3.AverageAsync(v => UBinarySize.Parse(v));
        Assert.AreEqual((UBinarySize)6, average3);

        var converter = TypeDescriptor.GetConverter(typeof(UBinarySize?));
        var values4 = new[] { "5", "6", "", "7", "8" }.ToAsyncEnumerable();
        var average4 = await values4.AverageAsync(v => (UBinarySize?)converter.ConvertFromInvariantString(v));
        Assert.AreEqual((UBinarySize)6, average4);

        Assert.IsNull(await AsyncEnumerable.Empty<UBinarySize?>().AverageAsync());
        Assert.IsNull(await new[] { (UBinarySize?)null }.ToAsyncEnumerable().AverageAsync());
    }

    [TestMethod]
    public void TestXmlSerialization()
    {
        var serializer = new XmlSerializer(typeof(USerializationTest));
        using var writer = new StringWriter();
        serializer.Serialize(writer, new USerializationTest() { Value = 10, Size = UBinarySize.FromMebi(1.5) });
        var actual = writer.ToString();
        Assert.AreEqual(_expectedXml, actual);

        using var reader = new StringReader(actual);
        var result = (USerializationTest)serializer.Deserialize(reader)!;
        Assert.AreEqual(UBinarySize.FromMebi(1.5), result.Size);
        Assert.AreEqual(10, result.Value);
    }

    [TestMethod]
    public void TestDataContractSerialization()
    {
        var serializer = new DataContractSerializer(typeof(USerializationTest));
        using var writer = new StringWriter();
        using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });
        serializer.WriteObject(xmlWriter, new USerializationTest() { Value = 10, Size = UBinarySize.FromMebi(1.5) });
        xmlWriter.Flush();
        var actual = writer.ToString();
        Assert.AreEqual(_expectedDataContract, actual);

        using var reader = new StringReader(actual);
        using var xmlReader = XmlReader.Create(reader);
        var result = (USerializationTest)serializer.ReadObject(xmlReader)!;
        Assert.AreEqual(UBinarySize.FromMebi(1.5), result.Size);
        Assert.AreEqual(10, result.Value);
    }

    [TestMethod]
    public void TestJsonSerialization()
    {
        var json = JsonSerializer.Serialize(new USerializationTest() { Value = 10, Size = UBinarySize.FromMebi(1.5) });
        Assert.AreEqual("{\"Size\":\"1536 KiB\",\"Value\":10}", json);
        var result = JsonSerializer.Deserialize<USerializationTest>(json)!;
        Assert.AreEqual(UBinarySize.FromMebi(1.5), result.Size);
        Assert.AreEqual(10, result.Value);
    }

#if NET6_0_OR_GREATER

    private static readonly string _expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<USerializationTest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Size>1536 KiB</Size>
  <Value>10</Value>
</USerializationTest>".ReplaceLineEndings();

#else

    private static readonly string _expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<USerializationTest xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Size>1536 KiB</Size>
  <Value>10</Value>
</USerializationTest>".ReplaceLineEndings();

#endif

    private static readonly string _expectedDataContract = @"<?xml version=""1.0"" encoding=""utf-16""?>
<USerializationTest xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/Ookii.Test"">
  <Size>1536 KiB</Size>
  <Value>10</Value>
</USerializationTest>".ReplaceLineEndings();
}
