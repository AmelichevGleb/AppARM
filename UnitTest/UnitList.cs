using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Security.Authenticode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitList
{
    [TestClass]       
    public class UnitList
    {
        [TestMethod]

        public void Test1()
        {
            string str = "127.0.0.1-4444;127.0.0.2-4444;";
            string input = "abc][rfd][5][,][.";
            string[] parts1 = str.Split(new string[] { ";" }, StringSplitOptions.None);
            string[] MassiveIP=  new string[parts1.Length-1];
            string[] MassivePort = new string[parts1.Length - 1];
            string str2;
            for (int i = 0; i < parts1.Length - 1; i++)
            {
                str2 = parts1[i];
                MassiveIP[i] = str2.Substring(0, str2.IndexOf('-'));
                MassivePort[i] = str2.Substring(str2.IndexOf('-') + 1);

                Console.WriteLine("IP = {0} Port = {1} ", MassiveIP[i], MassivePort[i]);
            }

        }
    }
}
