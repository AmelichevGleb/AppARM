using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace UnitTest
{
    [TestClass]
    public class UnitList
    {
        [TestMethod]
        public void TestMethod()
        {
            AppARM.TestXML.Apus apus = new AppARM.TestXML.Apus();
            Assert.AreEqual(apus.Part(1, 2), 0);
        }

        AppARM.Scripts.Script Script = new AppARM.Scripts.Script();
       
        [TestMethod]
        public void TestSeparator1()
        {
            //проверка подсчет символов ;
            string str = ";;";
            Assert.AreEqual(Script.CountSeparator(str), 2);
        }
        [TestMethod]
        public void TestSeparator2()
        {
            //проверка подсчет символов ; с элементами
            string str = "233;32;31;123";
            Assert.AreEqual(Script.CountSeparator(str), 3);
        }
        [TestMethod]
        public void TestSeparator3()
        {
            //проверка на отсутсвие символа ;
            string str = "asdasdasdsa";
            Assert.AreEqual(Script.CountSeparator(str), 0);
        }


        [TestMethod]
        public void TestSeparator5()
        {
            //проверка на содержание символов (3 элемента)
            string str = "строка;строка;строка";
            var a =  Script.ParserElement(str);
            Assert.AreEqual(a.Item4, false);
        }

        [TestMethod]
        public void TestSeparator6()
        {
            //проверка на содержание чисел (3 элемента)
            string str = "12;12;12";
            var a = Script.ParserElement(str);
            Assert.AreEqual(a.Item4, true);
        }
        [TestMethod]

        public void TestSeparator7()
        {
            //проверка на содержание символов (2 элемента)
            string str = "строка;строка";
            var a = Script.ParserElement(str);
            Assert.AreEqual(a.Item4, false);

        }

        [TestMethod]
        public void TestSeparator8()
        {
            //проверка на содержание чисел (2 элемента)
            string str = "12;12";
            var a = Script.ParserElement(str);
            Assert.AreEqual(a.Item4, true);
        }

        [TestMethod]
        public void TestSeparator9()
        {
            //проверка на содержание чисел (2 элемента)
            string str = "12;13";
            var a = Script.ParserElement(str);
            Assert.AreEqual(a.Item1, "12");
            Assert.AreEqual(a.Item2, "13");
            Assert.AreEqual(a.Item3, "-20");
            Assert.AreEqual(a.Item4, true);
        }

        [TestMethod]
        public void TestSeparator10()
        {
            //проверка на содержание чисел (3 элемента)
            string str = "12;13;14";
            var a = Script.ParserElement(str);
            Assert.AreEqual(a.Item1, "12");
            Assert.AreEqual(a.Item2, "13");
            Assert.AreEqual(a.Item3, "14");
            Assert.AreEqual(a.Item4, true);
        }

        [TestMethod]
        public void TestSeparator11()
        {
            //проверка на содержание cтрок (2 элемента)
            string str = "строка;test";
            var a = Script.ParserElement(str);
            Assert.AreEqual(a.Item1, "строка");
            Assert.AreEqual(a.Item2, "test");
            Assert.AreEqual(a.Item3, "-20");
            Assert.AreEqual(a.Item4, false);
        }

        [TestMethod]
        public void TestSeparator12()
        {
            //проверка на содержание cтрок (3 элемента)
            string str = "строка;test;проверка";
            var a = Script.ParserElement(str);
            Assert.AreEqual(a.Item1, "строка");
            Assert.AreEqual(a.Item2, "test");
            Assert.AreEqual(a.Item3, "проверка");
            Assert.AreEqual(a.Item4, false);
        }


    }
}
