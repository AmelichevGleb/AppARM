﻿using AppARM.Structure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppARM.Structure;
using static AppARM.Structure.ElementDataBase;

namespace UnitTest
{
    [TestClass]
    public class UnitStruct
    {
        [TestMethod]
        public void TestEmptyList()
        {
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            Assert.AreEqual(0, element.Count);
        }
        [TestMethod]
        public void TestAddelement()
        {
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3","4","5","6");
            Assert.AreEqual(1, element.Count);
        }
        [TestMethod]
        public void TestAddelement2()
        {
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            Assert.AreEqual(6, element.Count);
        }
        [TestMethod]
        public void TestFindExistElement()
        {
            //Проверка по id
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "2", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "3", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "4", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "5", "2", "3", "4", "5", "6");


            var result = wDB.ExistById(element, "5");
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestFindNotExistElement()
        {
            //Проверка по id (нет элемента)
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");


            var result = wDB.ExistById(element, "10");
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void TestFindExistElement1()
        {
            //проверка по IP (есть)
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "192.168.0.10", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "Kaluga", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");

            var result = wDB.ExistByIP(element, "192.168.0.10");
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestFindNotExistElement1()
        {
            //проверка по IP (нет)
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");

            var result = wDB.ExistByIP(element, "192.168.0.10");
            //Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void TestFindExistElement2()
        {
            //проверка по локации (есть)
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "Kaluga", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            var result = wDB.ExistByLocation(element, "Kalugas");
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void TestFindNotExistElement2()
        {
            //проверка по локации (нет)
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");

            var result = wDB.ExistByLocation(element, "Kaluga");
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void TestSearchRecordById_1()
        {
           (string, string, string,string , string , string) test = ("3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "2", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            wDB.AddNewElement(element, "4", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "5", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "6", "2", "3", "4", "5", "6");

            var t = wDB.SearchById(element, "3");
            Assert.AreEqual(test.Item1, t.Item1);
            Assert.AreEqual(test.Item2, t.Item2);
            Assert.AreEqual(test.Item3, t.Item3);
            Assert.AreEqual(test.Item4, t.Item4);
            Assert.AreEqual(test.Item5, t.Item5);
            Assert.AreEqual(test.Item6, t.Item6);

        }
        [TestMethod]
        public void TestSearchRecordById_2()
        {
            (string, string, string, string, string, string) test = ("3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "2", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            wDB.AddNewElement(element, "4", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "5", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "6", "2", "3", "4", "5", "6");


            var t = wDB.SearchById(element, "-1");

            Assert.AreEqual(null, t.Item1);
            Assert.AreEqual(null, t.Item2);
            Assert.AreEqual(null, t.Item3);
            Assert.AreEqual(null, t.Item4);
            Assert.AreEqual(null, t.Item5);
            Assert.AreEqual(null, t.Item6);
        }

        [TestMethod]
        public void TestSearchRecordByIP()
        {
            (string, string, string, string, string, string) test = ("3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "2", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            wDB.AddNewElement(element, "4", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "5", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "6", "2", "3", "4", "5", "6");

            var t = wDB.SearchByIP(element, "192.168.1.21");
            Assert.AreEqual(null, t.Item1);
            Assert.AreEqual(null, t.Item2);
            Assert.AreEqual(null, t.Item3);
            Assert.AreEqual(null, t.Item4);
            Assert.AreEqual(null, t.Item5);
            Assert.AreEqual(null, t.Item6);

        }
        [TestMethod]
        public void TestSearchRecordByLocation()
        {
            (string, string, string, string, string, string) test = ("3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "2", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            wDB.AddNewElement(element, "4", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "5", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "6", "2", "3", "4", "5", "6");

            var t = wDB.SearchByLocation(element, "Kaluga");
            Assert.AreEqual(test.Item1, t.Item1);
            Assert.AreEqual(test.Item2, t.Item2);
            Assert.AreEqual(test.Item3, t.Item3);
            Assert.AreEqual(test.Item4, t.Item4);
            Assert.AreEqual(test.Item5, t.Item5);
            Assert.AreEqual(test.Item6, t.Item6);
        }
        [TestMethod]
        public void TestSearchRecordByText_1()
        {
            (string, string, string, string, string, string) test = ("3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            wDB.AddNewElement(element, "1", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "2", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "3", "192.168.1.1", "Kaluga", "42.245", "23.1241", "Null");
            wDB.AddNewElement(element, "4", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "5", "2", "3", "4", "5", "6");
            wDB.AddNewElement(element, "6", "2", "3", "4", "5", "6");

            var t = wDB.SearchByLocation(element, "Kalugas");
            Assert.AreEqual(null, t.Item1);
            Assert.AreEqual(null, t.Item2);
            Assert.AreEqual(null,t.Item3);
            Assert.AreEqual(null, t.Item4);
            Assert.AreEqual(null, t.Item5);
            Assert.AreEqual(null, t.Item6);
        }
       
    }
}
