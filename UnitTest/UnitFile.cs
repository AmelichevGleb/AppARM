using AppARM.FilesLogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Threading;


namespace UnitTest
{
    [TestClass]
    public class UnitFile
    {

        Files files = new Files();
        [TestMethod]
        public void TestCreateNewFile()
        {
            string nameFile = "test.txt";
            //проверка функции на создание файла
            files.CreateFile(nameFile);
            Assert.AreEqual(true, File.Exists(nameFile));
           
        }
        [TestMethod]
        public void TestExistFile()
        {
            //проверка функии наличие файла
            string nameFile = "test.txt";
            var status = files.CheckFile(nameFile);
            Assert.AreEqual(true, status);
            files.DeleteFile(nameFile);
        }
        [TestMethod]
        public void TestNotExistFile()
        {
            //проверка функии наличие файла
            string nameFile = "test1.txt";
            var status = files.CheckFile(nameFile);
            Assert.AreEqual(false, status);
            files.DeleteFile(nameFile);
        }
        [TestMethod]
        public void TestDeleteFile()
        {
            //удаление необходимого файла
            string nameFile = "test12.txt";
            files.CreateFile(nameFile);
            var status = files.DeleteFile(nameFile);
            Assert.AreEqual(false, files.CheckFile(nameFile));
            files.DeleteFile(nameFile);

        }

        [TestMethod]
        public void Test1()
        {
            //проверка что файл пустой
            string nameFile = "test123.txt";
            files.CreateFile(nameFile);
            Assert.AreEqual(true, files.CheckNullFile(nameFile));
            files.DeleteFile(nameFile);
        }
        [TestMethod]
        public void Test2()
        {
            //проверка что файл не пустой 
            string nameFile = "test124.txt";
            files.CreateFile(nameFile);
            files.ReadFile(nameFile,"sss");
            Assert.AreEqual(false, files.CheckNullFile(nameFile));
            files.DeleteFile(nameFile);
        }
        [TestMethod]
        public void Test3()
        {
            //проверка записи в файл
            string nameFile = "test125.txt";
            files.DeleteFile(nameFile);
            files.CreateFile(nameFile);
            files.ReadFile(nameFile, "sss");
            var str = files.ShowDatainFile(nameFile);
            Assert.AreEqual("sss\n", str);
            files.DeleteFile(nameFile);
        }
        [TestMethod]
        public void Test4()
        {
            //Запись двух сторк 
            string nameFile = "test126.txt";
            files.DeleteFile(nameFile);
            files.CreateFile(nameFile);
            files.ReadFile(nameFile,"sssss");
            files.ReadFile(nameFile, "sssss1");
            var str = files.ShowDatainFile(nameFile);
            Assert.AreEqual("sssss\nsssss1\n", str);
            files.DeleteFile(nameFile);
        }
        [TestMethod]
        public void test5()
        {
            string nameFile = "logs.txt";
            files.DeleteFile(nameFile);
            files.CreateFile(nameFile);
            files.ReadFile("sssss",true);
            var str = files.ShowDatainFile(nameFile);
            Assert.AreEqual("- " + DateTime.Now + " " +"sssss" +"\n", str);
        }
    }
}