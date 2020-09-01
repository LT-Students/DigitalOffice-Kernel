using NUnit.Framework;
using System.Text.Json;

namespace LT.DigitalOffice.Kernel.UnitTestLibrary
{
    public static class SerializerAssert
    {
        public static void AreEqual(object expected, object result)
        {
            var expectedJson = JsonSerializer.Serialize(expected);
            var resultJson = JsonSerializer.Serialize(result);

            Assert.AreEqual(expectedJson, resultJson);
        }

        public static void AreNotEqual(object expected, object result)
        {
            string expectedJson = JsonSerializer.Serialize(expected);
            string resultJson = JsonSerializer.Serialize(result);

            Assert.AreNotEqual(expectedJson, resultJson);
        }
    }
}