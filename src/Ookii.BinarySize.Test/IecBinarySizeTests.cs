using System.Globalization;
using System.Text.Json;

namespace Ookii.Test;

[TestClass]
public class IecIecBinarySizeTests
{
    [TestMethod]
    public void TestParse()
    {
        Assert.AreEqual(new IecBinarySize(123000000), IecBinarySize.Parse("123MB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new IecBinarySize(128974848), IecBinarySize.Parse("123MiB", CultureInfo.InvariantCulture));
        Assert.AreEqual(new IecBinarySize(123000000), IecBinarySize.Parse("123M", CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void TestTryParse()
    {
        Assert.IsTrue(IecBinarySize.TryParse("123MB", CultureInfo.InvariantCulture, out var result));
        Assert.AreEqual((IecBinarySize)123000000, result);
        Assert.IsTrue(IecBinarySize.TryParse("123MiB", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual((IecBinarySize)128974848, result);
        Assert.IsTrue(IecBinarySize.TryParse("123M", CultureInfo.InvariantCulture, out result));
        Assert.AreEqual((IecBinarySize)123000000, result);
        Assert.IsFalse(IecBinarySize.TryParse("asdf", CultureInfo.InvariantCulture, out _));
    }

    [TestMethod]
    public void TestToString()
    {
        var size = new IecBinarySize(BinarySize.FromMebi(123));
        Assert.AreEqual(size.ToString(null, null), size.ToString());
        Assert.AreEqual("123MB", size.ToString("MB", CultureInfo.InvariantCulture));
    }

#if NET6_0_OR_GREATER

    [TestMethod]
    public void TestTryFormat()
    {
        var destination = new char[20];
        var size = new IecBinarySize(BinarySize.FromMebi(123));
        Assert.IsTrue(size.TryFormat(destination, out int charsWritten, "MB".AsSpan(), CultureInfo.InvariantCulture));
        Assert.AreEqual("123MB", destination.AsSpan(0, charsWritten).ToString());
    }

#endif

    class IecSerializationTest
    {
        public IecBinarySize Size { get; set; }
    }

    [TestMethod]
    public void TestJsonSerialization()
    {
        var json = JsonSerializer.Serialize(new IecSerializationTest() { Size = BinarySize.FromMebi(1.5) });
        Assert.AreEqual("{\"Size\":\"1536 KiB\"}", json);
        var result = JsonSerializer.Deserialize<IecSerializationTest>(json)!;
        Assert.AreEqual((IecBinarySize)BinarySize.FromMebi(1.5), result.Size);

        result = JsonSerializer.Deserialize<IecSerializationTest>("{\"Size\":\"1536kB\"}")!;
        Assert.AreEqual((IecBinarySize)1536000, result.Size);
    }
}
