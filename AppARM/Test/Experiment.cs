using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Media;
using System.Net;
using System.IO;

namespace AppARM.Test
{
    internal class Experiment
    {
        public class TestExperient
        {
            //https://habr.com/ru/articles/544592/
            //VUY8nxlgMDZ531jgudhbWpBUc4zrV0ja
            //пароль dhbWpBUc

            //---------------------------------------------------------
            public void Get1()
            {
                var url = "https://google.com";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);


                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

                Console.WriteLine(httpResponse.StatusCode);

            }

            public void Post1()
            {
                var url = "https://google.com";

//                var user = new User("John Doe", "gardener");
 //               var json = JsonSerializer.Serialize(user);
   //             byte[] byteArray = Encoding.UTF8.GetBytes(json);
                
                
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";

                httpRequest.ContentType = "application/json";
                httpRequest.Headers["Content-Length"] = "0";


                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

                Console.WriteLine(httpResponse.StatusCode);
            }


            private static string POST(string Url, string Data)
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.GetEncoding(1251).GetBytes(Data);
                req.ContentLength = sentData.Length;
                System.IO.Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);

                //Кодировка указывается в зависимости от кодировки ответа сервера
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                string Out = String.Empty;
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    Out += str;
                    count = sr.Read(read, 0, 256);
                }
                return Out;
            }


            private static string GET(string Url, string Data)
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(Url + "?" + Data);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                string Out = sr.ReadToEnd();
                sr.Close();
                return Out;
            }
            public async void Test1()


            {
                HttpMessageHandler handler = new HttpClientHandler();

                Console.WriteLine("Приложение начало работу");
                for (int i = 0; i < 10; i++)
                {
                    using (var client = new HttpClient())
                    {
                        using var result = await client.GetAsync("https://google.com");
                        Console.WriteLine(result.StatusCode);
                    }
                }
                Console.WriteLine("Приложение завершило работу");
            }
            static HttpClient httpClient = new HttpClient();

            public static string Get1 (string url)
            {
                var web = new WebClient();
                return web.DownloadString(url);
            }
            public void Test2()
            {
                String url = "https://httpbin.org/get";

                String content = Get1(url);

                Console.WriteLine(content);
            }
        }
        
        class MyTable
        {
            public MyTable(int _Id, string _Name, string _Age)
            {
                this.Id = _Id;
                this.Name = _Name;
                this.Age = _Age;

            }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }

        }
      
        public void Test1()
        {
            List<MyTable> result = new List<MyTable>(3);
            List<MyTable> result1 = new List<MyTable>(3);

            result.Add(new MyTable(1, "22", "22"));
            result.Add(new MyTable(2, "22", "22"));
            result.Add(new MyTable(3, "22", "22"));
            result.Add(new MyTable(4, "22", "22"));

            result1.Add(new MyTable(1, "22", "22"));
            result1.Add(new MyTable(2, "23", "22"));
            result1.Add(new MyTable(3, "22", "22"));
            result1.Add(new MyTable(4, "22", "22"));
            
            var res = result.Except(result1);
            Console.WriteLine(res);
        }
    }
}
