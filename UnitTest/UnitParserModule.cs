using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AppARM.Parser;


namespace UnitTest
{
    [TestClass]
    public class UnitParserModule
    {
        //--------------------------Для массива байт--------------------------
       
        [TestMethod]
        //проверка индекса ; (есть элемент после последнего символа ; )
        public void TestMethod1()
        {
            ParserAll parser = new ParserAll();
            Assert.AreEqual(true, parser.NextElementByte("22;22"));
        }
        
        [TestMethod]
        //проверка индекса ; (есть элемент после последнего символа ; )
        public void TestMethod2()
        {
            ParserAll parser = new ParserAll();
            Assert.AreEqual(true, parser.NextElementByte("22;22;22;22"));
        }
        
        [TestMethod]
        //проверка индекса ; (нету элемента после последнего символа ; )
        public void TestMethod3()
        {  
            ParserAll parser = new ParserAll();
            Assert.AreEqual(false, parser.NextElementByte("22;"));
        }

        [TestMethod]
        //проверка подсчета кол-во элементов между разделителями 
        public void TestMethod4()
        {
            ParserAll parser = new ParserAll();
            Assert.AreEqual(3, parser.CountByteSend("22;22;22"));
        }
        
        [TestMethod]
        //проверка подсчета кол-во элементов между разделителями 
        public void TestMethod5()
        {
            ParserAll parser = new ParserAll();
            Assert.AreEqual(1, parser.CountByteSend("22"));
        }
       
        [TestMethod]
        //проверка подсчета кол-во элементов между разделителями 
        public void TestMethod6()
        {
            ParserAll parser = new ParserAll();
            Assert.AreEqual(1, parser.CountByteSend("22;"));
        }

        [TestMethod]
        //проверка заполения массива Int значениями
        public void TestMethod7()
        {   
            byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };  // правильный массив
            int[] test = new int[8] { 22, 22, 22, 0, 0, 0, 0, 0 };
            ParserAll parser = new ParserAll();
            int count = 0;
            var t = parser.AddMassiveByte("22;22;22");
            for (int i = 0; i < test.Length; i++)
            {
                if (t[i] == test[i])
                { count++; }
            }
            Assert.AreEqual(8, count);
        }

        [TestMethod]
        //проверка заполения массива Int значениями
        public void TestMethod8()
        {   
            byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };  // правильный массив
            int[] test = new int[8] { 22, 0, 0, 0, 0, 0, 0, 0 };
            ParserAll parser = new ParserAll();
            int count = 0;
            var t = parser.AddMassiveByte("22");
            for (int i = 0; i < test.Length; i++)
            {
                if (t[i] == test[i])
                { count++; }
            }
            Assert.AreEqual(8, count);

        }
        
        [TestMethod]
        //проверка заполения массива Int значениями
        public void TestMethod9()
        {   
            byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };  // правильный массив
            int[] test = new int[8] { 22, 22, 20, 20, 20, 20, 20, 20 };
            ParserAll parser = new ParserAll();
            int count = 0;
            var t = parser.AddMassiveByte("22;22;20;20;20;20;20;20");
            for (int i = 0; i < test.Length; i++)
            {
                if (t[i] == test[i])
                { count++; }
            }
            Assert.AreEqual(8, count);
        }

        [TestMethod]
        //проверка заполения массива byte значениями
        public void TestMethod10()
        {
            byte[] test = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };  // правильный массив
              ParserAll parser = new ParserAll();
            int count = 0;
            var t = parser.AddMassiveByte("1;3;0;0;0;90;197;241");
            for (int i = 0; i < test.Length; i++)
            {
                if (t[i] == test[i])
                { count++; }
            }
            Assert.AreEqual(8, count);
        }
    }

}
