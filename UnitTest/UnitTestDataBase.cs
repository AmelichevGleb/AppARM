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
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(true, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadIP()
        {
            DataBase db = new DataBase("192.168.1.23", portBD, userBD, passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadPORT()
        {
            DataBase db = new DataBase(serverBD, "2444", userBD, passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadUser()
        {
            DataBase db = new DataBase(serverBD, portBD, "Tester", passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestConnectBD_BadPassword()
        {
            DataBase db = new DataBase(serverBD, portBD, userBD, "323232");
            var status = db.ConnectDataBase();
            Assert.AreEqual(false, status);
        }
        
        [TestMethod]
        public void TestCreateTable()
        {
            string testName = "UnitTest";
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            var result = db.CreateTableApy(testName);
            Assert.AreEqual(true, result);
            db.DeleteTable(testName);
        }
        
        [TestMethod]
        public void TestDeleteTable()
        {
            string testName = "UnitTest";
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            db.CreateTableApy(testName);          
            db.DeleteTable(testName);
            var result = db.DeleteTable(testName);
            Assert.AreEqual(false, result);
        }
        
        [TestMethod]
        public void TestGetTable() {
            //база данных не существует
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            var t = db.GetDataBaseShort("test144");
            Assert.IsNull(t);           
        }

        [TestMethod]
        public void TestGetTable1()
        {
            
            //база данных существует
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            var t = db.GetDataBaseShort("test1");
            Assert.IsNotNull(t);
        }
        
        [TestMethod]
        public void TestGetTable2()
        {
            //база данных существует (1 тестовый элемент)
            WorkElementDB wDB = new WorkElementDB();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
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
      
            WorkElementDB wDB = new WorkElementDB();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.CreateTableApy("Test_2");
            db.InsertDataBase("Test_2", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12" , "'null'", "'null'","'null'", "'null'");
            db.InsertDataBase("Test_2", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'");
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
            WorkElementDB wDB = new WorkElementDB();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            Assert.IsTrue(db.CreateTableApy("Toster"));
            Assert.IsTrue(db.DropTable("Toster"));
        }

        [TestMethod]
        public void TestDropTable_2()
        {
            //удаление таблицы которой нет
            WorkElementDB wDB = new WorkElementDB();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            Assert.IsFalse(db.DropTable("Toster"));
        }

        [TestMethod]
        public void TestInsertDataBase()
        {
            WorkElementDB wDB = new WorkElementDB();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.CreateTableApy("Test5");
            db.InsertDataBase("Test5", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'");
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
            WorkElementDB wDB = new WorkElementDB();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.CreateTableApy("Test12");
            db.InsertDataBase("Test12", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'"); 
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
            WorkElementDB wDB = new WorkElementDB();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBase> element = new List<ElementDataBase>();
            db.DeleteTable("Test124");
            db.CreateTableApy("Test124");
            db.InsertDataBase("Test124", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'");
            db.InsertDataBase("Test124", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'");
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
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            db.CreateTableApy("Test1234");
            db.InsertDataBase("Test124", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'");
            Assert.IsFalse(db.UpdateElementDataBase("Test1234", "1", null, null,null,null,null,null, null, null,null));
            db.DeleteTable("Test1234");
        }
        
        [TestMethod]
        public void TestUpdate_2()
        {
            //поменять только values
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
            db.CreateTableApy("Test1234");
            db.InsertDataBase("Test124", "'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'");
            Assert.IsTrue(db.UpdateElementDataBase("Test124","1" ,"'192.168.1.1'", "8080", "'kaluga'", "15.12", "26.12", "'null'", "'null'", "'null'", "'null'"));
            var t = db.GetDataBaseShort("Test1234");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)),
                Convert.ToString(t.GetString(5)), Convert.ToString(t.GetString(6)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
            }
            var t1 = wDB.SearchById(element, "1");
            Assert.AreEqual("23",t1.Item2);
            db.DeleteTable("Test1234");
        }
        
        [TestMethod]
        public void TestUpdate_3()
        {
            //поменять только values
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
          //  db.CreateTable("Test1234");
          //  db.InsertDataBase("Test1234", "192.168.1.24", "Kaluga", "22.1", "33.2", "text");
        //    Assert.IsTrue(db.UpdateElementDataBase("Test1234", "1", null, "33")); ;
            var t = db.GetDataBaseShort("Test1234");
            while (t.Read())
            {
            //    wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)),
  //  Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
            }
            var t1 = wDB.SearchById(element, "1");
            Assert.AreEqual("33", t1.Item3);
            db.DeleteTable("Test1234");
        }
        
        [TestMethod]
        public void TestUpdate_4()
        {
            //поменять оба поля
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
          //  db.CreateTable("Test1234");
          // db.InsertDataBase("Test1234", "22", "22", "22", "22", "22");
         //   Assert.IsTrue(db.UpdateElementDataBase("Test1234", "1", "22", "33")); ;
            var t = db.GetDataBaseShort("Test1234");
            while (t.Read())
           {
            //    wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)),
   //   Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)));
                Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
            }
            var t1 = wDB.SearchById(element, "1");
            Assert.AreEqual("22", t1.Item2);
            Assert.AreEqual("33", t1.Item3);
            db.DeleteTable("Test1234");
        }

        [TestMethod]
        public void TestGetLastID()
        {
            //проверка последнего id - в заполненой 
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
         //   db.CreateTable("LastID");
         //   db.InsertDataBase("LastID", "22", "22", "22", "22", "22");
         //   db.InsertDataBase("LastID", "22", "22", "22", "22", "22"); 
            int id = db.GetLastID("LastID");
            Assert.AreEqual(id, 2);
            db.DeleteTable("LastID");
        }
        
        [TestMethod]
        public void TestGetNullLastID()
        {
            //последний id в пустой таблице
            WorkElementDB wDB = new WorkElementDB();
            List<ElementDataBase> element = new List<ElementDataBase>();
            DataBase db = new DataBase(serverBD, portBD, userBD, passwordBD);
           // db.CreateTable("NullID");
            int id = db.GetLastID("LastID");
            Assert.AreEqual(id, -1);
            db.DeleteTable("LastID");
        }
    }
}

        