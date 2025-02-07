using System.ComponentModel;
using System.Globalization;

namespace Ookii.Test;

[TestClass]
public class BinarySizeConverterTests
{
    private static readonly Type[] _primitiveTypes = { typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short),
        typeof(ushort), typeof(sbyte), typeof(byte), typeof(double), typeof(float) };

    [TestMethod]
    public void TestCanConvert()
    {
        TypeConverter converter = new BinarySizeConverter();
        Assert.IsTrue(converter.CanConvertFrom(typeof(string)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(IecBinarySize)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(UBinarySize)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(UIecBinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(string)));
        Assert.IsTrue(converter.CanConvertTo(typeof(IecBinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(UBinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(UIecBinarySize)));
        foreach (var type in _primitiveTypes)
        {
            Assert.IsTrue(converter.CanConvertTo(type));
        }

        converter = new IecBinarySizeConverter();
        Assert.IsTrue(converter.CanConvertFrom(typeof(string)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(BinarySize)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(UBinarySize)));
        Assert.IsTrue(converter.CanConvertFrom(typeof(UIecBinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(string)));
        Assert.IsTrue(converter.CanConvertTo(typeof(BinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(UBinarySize)));
        Assert.IsTrue(converter.CanConvertTo(typeof(UIecBinarySize)));
        foreach (var type in _primitiveTypes)
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

        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, (IecBinarySize)129499136));
        Assert.AreEqual((IecBinarySize)129499136, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(IecBinarySize)));
        Assert.AreEqual(129499136.0, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(double)));
    }

    [TestMethod]
    public void TestSignedUnsignedConversion()
    {
        var converter = TypeDescriptor.GetConverter(typeof(BinarySize));
        Assert.AreEqual((BinarySize)1234, converter.ConvertFrom(null, CultureInfo.InvariantCulture, (UBinarySize)1234));
        Assert.AreEqual((UBinarySize)1234, converter.ConvertTo(null, CultureInfo.InvariantCulture, (BinarySize)1234, typeof(UBinarySize)));
        Assert.AreEqual((BinarySize)1234, converter.ConvertFrom(null, CultureInfo.InvariantCulture, (UIecBinarySize)1234));
        Assert.AreEqual((UIecBinarySize)1234, converter.ConvertTo(null, CultureInfo.InvariantCulture, (BinarySize)1234, typeof(UIecBinarySize)));

        converter = TypeDescriptor.GetConverter(typeof(IecBinarySize));
        Assert.AreEqual((IecBinarySize)1234, converter.ConvertFrom(null, CultureInfo.InvariantCulture, (UBinarySize)1234));
        Assert.AreEqual((UBinarySize)1234, converter.ConvertTo(null, CultureInfo.InvariantCulture, (IecBinarySize)1234, typeof(UBinarySize)));
        Assert.AreEqual((IecBinarySize)1234, converter.ConvertFrom(null, CultureInfo.InvariantCulture, (UIecBinarySize)1234));
        Assert.AreEqual((UIecBinarySize)1234, converter.ConvertTo(null, CultureInfo.InvariantCulture, (IecBinarySize)1234, typeof(UIecBinarySize)));
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

        Assert.AreEqual(target, converter.ConvertFrom(null, CultureInfo.InvariantCulture, (BinarySize)125952));
        Assert.AreEqual((BinarySize)125952, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(BinarySize)));
        Assert.AreEqual(125952.0, converter.ConvertTo(null, CultureInfo.InvariantCulture, target, typeof(double)));
    }
}
