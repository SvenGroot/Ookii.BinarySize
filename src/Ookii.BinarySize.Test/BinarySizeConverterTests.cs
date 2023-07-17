using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Globalization;

namespace Ookii.Test;

[TestClass]
public class BinarySizeConverterTests
{
    private static readonly Type[] PrimitiveTypes = { typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short),
        typeof(ushort), typeof(sbyte), typeof(byte), typeof(double), typeof(float) };

    [TestMethod]
    public void TestCanConvert()
    {
        TypeConverter converter = new BinarySizeConverter();
        Assert.IsTrue(converter.CanConvertFrom(typeof(string)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(IecBinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(string)));
        Assert.IsTrue(converter.CanConvertTo(typeof(IecBinarySize)));
        foreach (var type in PrimitiveTypes)
        {
            Assert.IsTrue(converter.CanConvertTo(type));
        }

        converter = new BinarySizeIecConverter();
        Assert.IsTrue(converter.CanConvertFrom(typeof(string)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(IecBinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(string)));
        Assert.IsTrue(converter.CanConvertTo(typeof(IecBinarySize)));
        foreach (var type in PrimitiveTypes)
        {
            Assert.IsTrue(converter.CanConvertTo(type));
        }

        converter = new IecBinarySizeConverter();
        Assert.IsTrue(converter.CanConvertFrom(typeof(string)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(BinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(string)));
        Assert.IsTrue(converter.CanConvertTo(typeof(BinarySize)));
        foreach (var type in PrimitiveTypes)
        {
            Assert.IsTrue(converter.CanConvertTo(type));
        }
    }

    [TestMethod]
    public void TestConversion()
    {
        var converter = TypeDescriptor.GetConverter(typeof(BinarySize));
        Assert.IsInstanceOfType(converter, typeof(BinarySizeConverter));
        Assert.AreEqual(BinarySizeOptions.Default, ((BinarySizeConverter)converter).Options);
        var target = new BinarySize(125952);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KB"));
        Assert.AreEqual("123 KiB", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
        target = new(129499136);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123.5MB"));
        Assert.AreEqual("126464 KiB", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));

        Assert.AreEqual((IecBinarySize)129499136, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(IecBinarySize)));
        Assert.AreEqual(129499136.0, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(double)));
    }

    [TestMethod]
    public void TestConversionOptions()
    {
        var converter = new BinarySizeConverter(BinarySizeOptions.UseIecStandard);
        Assert.AreEqual(BinarySizeOptions.UseIecStandard, converter.Options);
        var target = new BinarySize(123000);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KB"));
        Assert.AreEqual("123000 B", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
        target = new(125952);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KiB"));
        Assert.AreEqual("123 KiB", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
    }

    [TestMethod]
    public void TestIecConverter()
    {
        var converter = new BinarySizeIecConverter();
        Assert.AreEqual(BinarySizeOptions.UseIecStandard, converter.Options);
        var target = new BinarySize(123000);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KB"));
        Assert.AreEqual("123000 B", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
        target = new(125952);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KiB"));
        Assert.AreEqual("123 KiB", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
    }

    [TestMethod]
    public void TestIecBinarySizeConverter()
    {
        var converter = TypeDescriptor.GetConverter(typeof(IecBinarySize));
        Assert.IsInstanceOfType(converter, typeof(IecBinarySizeConverter));
        var target = new IecBinarySize(123000);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KB"));
        Assert.AreEqual("123000 B", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));
        target = new(125952);
        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, "123KiB"));
        Assert.AreEqual("123 KiB", converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(string)));

        Assert.AreEqual((BinarySize)125952, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(BinarySize)));
        Assert.AreEqual(125952.0, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(double)));
    }
}
