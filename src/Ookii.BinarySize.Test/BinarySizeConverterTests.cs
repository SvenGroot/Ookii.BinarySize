using System.ComponentModel;
using System.Globalization;

namespace Ookii.Test;

[TestClass]
public class BinarySizeConverterTests
{
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
}
