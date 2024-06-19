using AppARM.PostgresSQL;
using AppARM.TestXML;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using AppARM.Structure;
using static AppARM.Structure.ElementDataBase;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTest
{
    [TestClass]

    public class UnitTestDataBase
    {
        private const string serverBD = "localhost";
        private const string portBD = "5432";
        private const string userBD = "postgres";
        private const string passwordBD = "111111";

        [TestMethod]
        public void TestConnectBD()
        {
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(true, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadIP()
        {
            DataBaseMeteostation db = new DataBaseMeteostation("192.168.1.23", portBD, userBD, passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadPORT()
        {
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, "2444", userBD, passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadUser()
        {
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, "Tester", passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadPassword()
        {
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, "323232");
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestCreateTable()
        {
            string testName = "UnitTest";
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            var result = db.CreateTable(testName);
            Assert.AreEqual(true, result);
            db.DeleteTable(testName);
        }
        
        [TestMethod]
        public void TestDeleteTable()
        {
            string testName = "UnitTest";
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.CreateTable(testName);          
            db.DeleteTable(testName);
            var result = db.DeleteTable(testName);
            Assert.AreEqual(false, result);
        }
        
        [TestMethod]
        public void TestGetTable() {
            //база данных не существует
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            var t = db.GetDataBaseShort("test144");
            Assert.IsNull(t);           
        }

        [TestMethod]
        public void TestGetTable1()
        {
            
            //база данных существует
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.CreateTable("test1");
            var t = db.GetDataBaseShort("test1");
            Assert.IsNotNull(t);
        }
        
        [TestMethod]
        public void TestGetTable2()
        {
            //база данных существует (1 тестовый элемент)
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.ClearTable("test1");
            var t = db.GetDataBaseShort("test1");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            Assert.AreEqual(0,element.Count);
        }
       
        [TestMethod]
        public void TestGetTable3()
        {
            //база данных создаетcя + 2 тестовые записи

            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.CreateTable("Test_2");
            db.InsertDataBase("Test_2", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12" , "'null'");
            db.InsertDataBase("Test_2", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            var t = db.GetDataBaseShort("Test_2");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            Assert.AreEqual(2, element.Count);
            db.DeleteTable("Test_2");
        }

        [TestMethod]
        public void TestDropTable_1()
        {
            //удаление таблицы существующей таблицы
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            Assert.IsTrue(db.CreateTable("Toster"));
            Assert.IsTrue(db.DropTable("Toster"));
        }

        [TestMethod]
        public void TestDropTable_2()
        {
            //удаление таблицы которой нет
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            Assert.IsFalse(db.DropTable("Toster"));
        }

        [TestMethod]
        public void TestInsertDataBase()
        {
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.CreateTable("Test5");
            db.InsertDataBase("Test5", "'192.168.1.1'", "'8080'", "'kaluga'", "15.12", "26.12", "'null'");
            var t = db.GetDataBaseShort("Test5");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            Assert.AreEqual(1, element.Count);
            Assert.IsTrue(db.DeleteTable("Test5"));
        }

        [TestMethod]
        public void TestClearDataBase()
        {
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.CreateTable("Test12");
            db.InsertDataBase("Test12", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'"); 
            Assert.AreEqual(true, db.ClearTable("Test12"));
            var t = db.GetDataBaseShort("Test12");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            Assert.AreEqual(0, element.Count);
            Assert.IsTrue(db.DeleteTable("Test12"));
        }
        
        [TestMethod]
        public void TestDeleteValueDataBase()
        {
            //удаление данных по id
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.DeleteTable("Test124");
            db.CreateTable("Test124");
            db.InsertDataBase("Test124", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            db.InsertDataBase("Test124", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            db.DeleteValueDB("Test124", 1);
            var t = db.GetDataBaseShort("Test124");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            Assert.AreEqual(1, element.Count);
            Assert.IsTrue(db.DeleteTable("Test124"));
        }
        
        [TestMethod]
        public void TestUpdate_1()
        {
            //пустые входные параметры ВОПРОС
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.DeleteTable("Test1234");
            db.CreateTable("Test1234");
            db.InsertDataBase("Test124", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            Assert.IsFalse(db.UpdateElementDataBase("Test1234", "1", null, null,null,null,null,null));
            db.DeleteTable("Test1234");
        }
        
        [TestMethod]
        public void TestUpdate_2()
        {

            //поменять только IP

            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.DeleteTable("Test1234");
            db.CreateTable("Test1234");
            db.InsertDataBase("Test1234", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            Assert.IsTrue(db.UpdateElementDataBase("Test1234", "1" ,"192.168.1.2", "8080", "kaluga", "15.12", "26.12", "null"));
            var t = db.GetDataBaseShort("Test1234");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            var t1 = wDB.SearchById(element, "1");
            Assert.AreEqual("192.168.1.2", t1.Item2);
            db.DeleteTable("Test1234");
        }
        
        [TestMethod]
        public void TestUpdate_3()
        {
            //поменять только Port
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.CreateTable("Test1234");
            db.InsertDataBase("Test1234", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            Assert.IsTrue(db.UpdateElementDataBase("Test1234", "1", "192.168.1.1", "8081", "kaluga", "15.12", "26.12", "null"));
            var t = db.GetDataBaseShort("Test1234");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            var t1 = wDB.SearchById(element, "1");
            Assert.AreEqual("8081", t1.Item3);
            db.DeleteTable("Test1234");
        }
        
        [TestMethod]
        public void TestUpdate_4()
        {
            //поменять оба поля
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.CreateTable("Test1234");
            db.InsertDataBase("Test1234", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            Assert.IsTrue(db.UpdateElementDataBase("Test1234", "1", "192.168.1.4", "8082", "kaluga", "15.12", "26.12", "null"));
            var t = db.GetDataBaseShort("Test1234");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            }
            var t1 = wDB.SearchById(element, "1");
            Assert.AreEqual("192.168.1.4", t1.Item2);
            Assert.AreEqual("8082", t1.Item3);
            db.DeleteTable("Test1234");
        }

        [TestMethod]
        public void TestGetLastID()
        {
            //проверка последнего id - в заполненой 
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.CreateTable("LastID");
            db.InsertDataBase("LastID", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            db.InsertDataBase("LastID", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'");
            int id = db.GetLastID("LastID");
            Assert.AreEqual(id, 2);
            db.DeleteTable("LastID");
        }
        
        [TestMethod]
        public void TestGetNullLastID()
        {
            //последний id в пустой таблице
            WorkElementDBMeteo wDB = new WorkElementDBMeteo();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            db.CreateTable("NullID");
            int id = db.GetLastID("LastID");
            Assert.AreEqual(id, -1);
            db.DeleteTable("LastID");
        }

        [TestMethod]
        public void TestReturnRow()
        {
            //вернуть Ip метеостанции стоящей рядом с датчиком ЧС
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            var t = db.returnIp4CDevice("meteostation", "127.0.0.1");
            Assert.AreEqual(t.Item1, "127.0.0.1");
        }

        [TestMethod]
        public void TestReturnAll()
        {
            string str = "";
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            var t = db.GetDataBaseShort("meteostation");
            while (t.Read())
            {
                str += Convert.ToString(t.GetInt32(0)) + ';' + t.GetString(1) + ';' + t.GetString(2) + ';' + t.GetString(3) + ';' + t.GetString(4) + ';' + t.GetString(5) + ';' + t.GetString(6) + '\n';
                /*
                               "4; 127.0.0.1; 11000; Kaluga; 55.73011; 37.59985; 127.0.0.1\n" +
                               "2; 127.0.0.2; 11000; Kaluga; 55.72732; 37.62766; 127.0.0.2\n" +
                               "3; 127.0.0.2; 11000; Kaluga; 55.74905; 37.58955; 127.0.0.2\n" +
                               "1; 127.0.0.2; 11000; Kaluga; 55.76025; 37.63298; 127.0.0.2\n" +
                               "6; 127.0.0.2; 11000; Kaluga; 55.75473; 37.58968; 127.0.0.12\n" +
                               "5; 127.0.0.2; 11000; Kaluga; 55.75658; 37.60152; 127.0.0.11\n" +
                               "7; 127.0.0.2; 11000; Kaluga; 55.72885; 37.66637; 127.0.0.23\n" +
                               "8; 127.0.0.2; 11000; Kaluga; 55.71759; 37.62277; 127.0.0.123\n";
                */

                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5), t.GetString(6));
            
            }
            Console.WriteLine("Итоговая строка \n{0}", str);

        }

        [TestMethod]
        public void TestReturnLongitude()
        {
            //вернуть Ip метеостанции стоящей рядом с датчиком ЧС
            DataBaseMeteostation db = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            var t = db.returnLongitudeMeteostation("meteostation", "195.124.51.12");
            Assert.AreEqual(t, "69.0000");
        }

        //returnIp4CDevice
    }
}

        