using AppARM.WeatherSokol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitByteWeathers
    {
        //Проверка направления ветра 



        [TestMethod]
        public void TestMethod()
        {
            byte[] bytes = new byte[40];
            ByteWeather byteWeather = new ByteWeather(null, null, 0, null, null, null, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8],
                        bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15], bytes[16], bytes[17], bytes[18], bytes[19], bytes[20], bytes[21], bytes[22], bytes[23],
                        bytes[24], bytes[25], bytes[26], bytes[27], bytes[28], false);
            Assert.AreEqual("С", byteWeather.HorizonPoints("127.0.0.1", 0));
        }
        [TestMethod]
        public void TestMethod1()
        {
            byte[] bytes = new byte[40];
            ByteWeather byteWeather = new ByteWeather(null, null, 0, null, null, null, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8],
                        bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15], bytes[16], bytes[17], bytes[18], bytes[19], bytes[20], bytes[21], bytes[22], bytes[23],
                        bytes[24], bytes[25], bytes[26], bytes[27], bytes[28], false);
            Assert.AreEqual("В", byteWeather.HorizonPoints("127.0.0.1", 90));
        }

        [TestMethod]
        public void TestMethod2()
        {
            byte[] bytes = new byte[40];
            ByteWeather byteWeather = new ByteWeather(null, null, 0, null, null, null, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8],
                        bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15], bytes[16], bytes[17], bytes[18], bytes[19], bytes[20], bytes[21], bytes[22], bytes[23],
                        bytes[24], bytes[25], bytes[26], bytes[27], bytes[28], false);
            Assert.AreEqual("СВ", byteWeather.HorizonPoints("127.0.0.1", 45));
        }
        [TestMethod]
        public void TestMethod3()
        {
            byte[] bytes = new byte[40];
            ByteWeather byteWeather = new ByteWeather(null, null, 0, null, null, null, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8],
                        bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15], bytes[16], bytes[17], bytes[18], bytes[19], bytes[20], bytes[21], bytes[22], bytes[23],
                        bytes[24], bytes[25], bytes[26], bytes[27], bytes[28], false);
            Assert.AreEqual("Ю", byteWeather.HorizonPoints("127.0.0.1", 180));
        }
    }
}
