using AppARM.PostgresSQL;
using AppARM.Structure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static AppARM.Structure.ElementDataBase;
using System.Collections.Generic;
using static AppARM.Structure.ElementDataBaseAlarm;
using AppARM.Parser;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitAlarmDB
    {

        private const string serverBD = "localhost";
        private const string portBD = "5432";
        private const string userBD = "postgres";
        private const string passwordBD = "111111";


        [TestMethod]
        public void TestСonnectDB()
        {
            //Создание базы данных
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            var status = db.ConnectDataBase();
            Assert.AreEqual(true, status);
        }
        [TestMethod]
        public void TestCreateDBAlarm()
        {
            //Создание тестовой базы данных
            string testName = "UnitTestAlarm";
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            var result = db.CreateTableAlarm(testName);
            Assert.AreEqual(true, result);
            db.DeleteTable(testName);
        }
        [TestMethod]
        public void TestInsertDBAlarm()
        {
            //Тестовое добавление записи 
            WorkElementDBAlarm wDB = new WorkElementDBAlarm();
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBaseAlarm> element = new List<ElementDataBaseAlarm>();
            db.CreateTableAlarm("Test_alarm");
            db.InsertDataBase("Test_alarm", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            var t = db.GetDataBase("Test_alarm");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)));
                Console.WriteLine("{0} {1} {2} {3} {4}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4));
            }
            Assert.AreEqual(1, element.Count);
            Assert.IsTrue(db.DeleteTable("Test_alarm"));
            db.DeleteTable("Test_alarm");
        }
        [TestMethod]
        public void TestDeleteDBAlarm()
        {
            //Тестовое удаление записи
            WorkElementDBAlarm wDB = new WorkElementDBAlarm();
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBaseAlarm> element = new List<ElementDataBaseAlarm>();
            db.CreateTableAlarm("Test_alarm_Delete");
            db.InsertDataBase("Test_alarm_Delete", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.InsertDataBase("Test_alarm_Delete", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.DeleteValueDB("Test_alarm_Delete", 1);
            var result = db.CountDataInBD("Test_alarm_Delete");
            db.DeleteTable("Test_alarm_Delete");
            Assert.AreEqual(1, Convert.ToInt32(result));
        }

        [TestMethod]
        public void TestLastIdAlarm()
        {
            //Тестовое получение последнего ID
            WorkElementDBAlarm wDB = new WorkElementDBAlarm();
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBaseAlarm> element = new List<ElementDataBaseAlarm>();
            db.CreateTableAlarm("Test_alarm_LastId");
            db.InsertDataBase("Test_alarm_LastId", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.InsertDataBase("Test_alarm_LastId", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.InsertDataBase("Test_alarm_LastId", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.InsertDataBase("Test_alarm_LastId", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            var result = db.GetLastID("Test_alarm_LastId");
            db.DeleteTable("Test_alarm_LastId");
            Assert.AreEqual(4, Convert.ToInt32(result));

        }

        [TestMethod]
        public void TestClearTable()
        {
            //тестовое очистка таблицы
            WorkElementDBAlarm wDB = new WorkElementDBAlarm();
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBaseAlarm> element = new List<ElementDataBaseAlarm>();
            db.CreateTableAlarm("Test_alarm_ClearTable");
            db.InsertDataBase("Test_alarm_ClearTable", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.InsertDataBase("Test_alarm_ClearTable", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.InsertDataBase("Test_alarm_ClearTable", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.InsertDataBase("Test_alarm_ClearTable", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            db.ClearTable("Test_alarm_ClearTable");
            var result = db.CountDataInBD("Test_alarm_ClearTable");
            db.DeleteTable("Test_alarm_ClearTable");
            Assert.AreEqual(0, Convert.ToInt32(result));
        }

        [TestMethod]
        public void TestUpdateTable()
        {
            //Тестовое обновление данных в базе данных
            WorkElementDBAlarm wDB = new WorkElementDBAlarm();
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBaseAlarm> element = new List<ElementDataBaseAlarm>();

            db.CreateTableAlarm("Test_update_Alarm");
            db.InsertDataBase("Test_update_Alarm", "'192.168.1.1'", "4", "'Path'", "'127.0.0.1;127.0.0.1'");
            Assert.IsTrue(db.UpdateElementDataBase("Test_update_Alarm", "1", "192.168.1.4", "3", "Pat!h", "127.0.0.2;127.0.0.2"));
            var t = db.GetDataBase("Test_update_Alarm");
            while (t.Read())
            {
                wDB.AddNewElement(element, Convert.ToString(t.GetInt32(0)), Convert.ToString(t.GetString(1)), Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)));
                Console.WriteLine("{0} {1} {2} {3} {4}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4));
            }
            db.DeleteTable("Test_update_Alarm");
            var t1 = wDB.SearchById(element, "1");
            Assert.AreEqual("192.168.1.4", t1.Item2);
            Assert.AreEqual("3", t1.Item3);
            Assert.AreEqual("Pat!h", t1.Item4);
            Assert.AreEqual("127.0.0.2;127.0.0.2", t1.Item5);
        }

        [TestMethod]
        public void TestSelect_1()
        {
            ParserAll parcer = new ParserAll();
            //вывести сценарий и кого оповестить
            WorkElementDBAlarm wDB = new WorkElementDBAlarm();
            AlarmDB db = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            List<ElementDataBaseAlarm> element = new List<ElementDataBaseAlarm>();

            var t = db.GettingListOfDevices("alarmdb", "'126.126.126.126'");
            parcer.MassiveIPDanger(t.Item1);
        }

    }
}