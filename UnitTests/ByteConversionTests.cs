using ConvertLib;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ByteConversionTests
{
    public struct TestData<T>
    {
        public T b;
        public string expected;

        public TestData(T v1, string v2) : this()
        {
            this.b = v1;
            this.expected = v2;
        }
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void IntToHex()
    {
        TestData<int>[] dataArray = new[]
        {
            new TestData<int>(-2147483647, "-7FFFFFFF"),
            new TestData<int>(-214748364, "-CCCCCCC"),
            new TestData<int>(-21474836, "-147AE14"),
            new TestData<int>(-2147483, "-20C49B"),
            new TestData<int>(-214748, "-346DC"),
            new TestData<int>(-21474, "-53E2"),
            new TestData<int>(-2147, "-863"),
            new TestData<int>(-214, "-D6"),
            new TestData<int>(-21, "-15"),
            new TestData<int>(-2, "-2"),
            new TestData<int>(100, "64"),
            new TestData<int>(1000, "3E8"),
            new TestData<int>(10000, "2710"),
            new TestData<int>(100000, "186A0"),
        };

        foreach (TestData<int> data in dataArray)
        {
            Assert.That(BaseConvert.ToHex(data.b), Is.EqualTo(Convert.ToString(data.expected)));
        }
    }

    [Test]
    public void LongToHex()
    {
        TestData<long>[] dataArray = new[]
        {
            new TestData<long>(long.MaxValue, "7FFFFFFFFFFFFFFF"),
            new TestData<long>(-2147483647, "-7FFFFFFF"),
            new TestData<long>(-214748364, "-CCCCCCC"),
            new TestData<long>(-21474836, "-147AE14"),
            new TestData<long>(-2147483, "-20C49B"),
            new TestData<long>(-214748, "-346DC"),
            new TestData<long>(-21474, "-53E2"),
            new TestData<long>(-2147, "-863"),
            new TestData<long>(-214, "-D6"),
            new TestData<long>(-21, "-15"),
            new TestData<long>(-2, "-2"),
            new TestData<long>(100, "64"),
            new TestData<long>(1000, "3E8"),
            new TestData<long>(10000, "2710"),
            new TestData<long>(100000, "186A0"),
        };

        foreach (TestData<long> data in dataArray)
        {
            Assert.That(BaseConvert.ToHex(data.b), Is.EqualTo(Convert.ToString(data.expected)));
        }
    }

    [Test]
    public void ByteArrayToHex()
    {
        Assert.That(BaseConvert.StringToHex("Hello"), Is.EqualTo("48656C6C6F"));
    }
}
