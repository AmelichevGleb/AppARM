using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AppARM.Scripts
{
    //____________________СЦЕНАРИИ КОТОРЫЕ РАБОТАЮТ ИНАЧЕ :) ___________________________________
    public class Script
    {
       public void AllCommand()
        {
            CommandPing();
            CommandStart(2);
            CommandStartFull();

        }
        
        //пинг 
        private void CommandPing()
        {
            string nameFile = "ping.xml";
            //xml для команды пинг
            XDocument xdoc = new XDocument(
                new XElement("command",
                    new XElement("action", "ping")));
            xdoc.Save(nameFile);
            Console.WriteLine(nameFile +  " saved");
        }
        //ответ на пинг 
        private void AnswerCommandPing()
        {
            XDocument xdoc = XDocument.Load("ping.xml");

            if (xdoc is not null)
            {
                foreach (XElement per in xdoc.Elements("command"))
                {

                    XElement? action = per.Element("action");
                    XAttribute? age = per.Attribute("code");
                    XElement? age1 = per.Element("result");
                    Console.WriteLine($"Company: {action?.Value}");
                    Console.WriteLine($"Age: {age1?.Value}");
                    Console.WriteLine($"Age: {age1?.Attribute("code").Value}");
                    Console.WriteLine();
                }

            }
        }

        /*
        //o	0 соответствует успеху (узел добавлен в разрешённые, АПУ-Ц в Дежурном режиме),
        o	1 указывает на отсутствие узла, отправившего запрос, в списке разрешенных узлов,
        o	2 идет входящее оповещение от П-166 федерального уровня
        o	3 идет входящее оповещение
        o	4 идет исходящее оповещение
        */


        private void CommandStart(int _numberScripts)
        {
            string nameFile = "start.xml";
            if (_numberScripts >= 0 && _numberScripts <= 6)
            //Команда запуска сценария оповещения
            {
                XDocument xdoc = new XDocument(
                new XElement("command",
                new XElement("action", "start"),
                new XElement("parameters",
                new XElement("scenario", _numberScripts),
                 new XElement("audio", "external"))));
                xdoc.Save(nameFile);
                Console.WriteLine(nameFile + " saved");
            }
            else
            {
                Console.WriteLine(nameFile + " not saved");
            }
        }

        private void ParserCommandstart()
        {
            XDocument xdoc = XDocument.Load("start.xml");

            if (xdoc is not null)
            {
                foreach (XElement per in xdoc.Elements("command"))
                {
                    XElement? action = per.Element("action");
                    Console.WriteLine($"Company: {action?.Value}");
                    XElement? age = per.Element("parameters");
                    XElement? age1 = age.Element("scenario");
                    Console.WriteLine($"scenario: {age1?.Value}");
                    XElement? age2 = age.Element("audio");
                    Console.WriteLine($"audio: {age2?.Value}");                 
                    Console.WriteLine();
                }
            }
        }


        private void AnswerCommandStart()
        {
            XDocument xdoc = XDocument.Load("Start1.xml");
            if (xdoc is not null)
            {
                foreach (XElement per in xdoc.Elements("answer"))
                {
                    XElement? action = per.Element("action");
                    Console.WriteLine($"action: {action?.Value}");
                    XElement? age1 = per.Element("result");
                    Console.WriteLine($"Age: {age1?.Value}");
                    Console.WriteLine($"Age: {age1?.Attribute("code").Value}");
                    var t = per.Element("details");
                    Console.WriteLine($"total = :{t?.Attribute("total").Value}");
                    Console.WriteLine($"terminals = :{t?.Element("terminals").Value}");
                    Console.WriteLine($"consoles = :{t?.Element("consoles").Value}");



                }
            }
        }

        public int CountSeparator(string _str)
        {
            int count = 0; 
            foreach (char c in _str) { if (c == ';')  count++; }
            return count;   
        }

       
        public Tuple<string, string, string, bool> ParserElement(string _str)
        {
            //числа это true
            string[] element = _str.Split(';'); // Разделение на коллекцию слов
            int number;

            if (int.TryParse(element[0],out number))
            {
                //если числа 
                if (CountSeparator(_str) == 1)
                {
                    return Tuple.Create(element[0], element[1], "-20", true );
                }

                else { return  Tuple.Create(element[0], element[1], element[2], true); }
            }
            else 
            {
                //если символы false 
                if (CountSeparator(_str) == 1)
                {
                    return Tuple.Create(element[0], element[1], "-20", false);
                }

                else { return Tuple.Create(element[0], element[1], element[2], false); }
            }
        }


        /*
         <?xml version="1.0" encoding="utf-8" ?>
            <answer>
                <action>start</action>
                <result code="0">Сценарий успешно запущен</result>
                <details total="100">
                    <terminals>1;4;8;</terminals>
                    <consoles>5;48</consoles>
                </details>
            </answer>
         */

        private void CommandStartFull()
        {
            string nameFile = "startFull.xml";
            //Команда запуска оповещения из набора сеанса
            XDocument xdoc = new XDocument(
            new XElement("command",
            new XElement("action", "start"),
            new XElement("parameters",
            new XElement("seance",
            new XAttribute("startmode", "1"),
            new XElement("types", "srn; rtu; apu"),
            new XElement("consoles", "78;102"),
            new XElement("terminals", "12;45;105"),
            new XElement("p160command", 3),
            new XElement("sirenmode", "discontinuous")
            ))));
            xdoc.Save(nameFile);
            Console.WriteLine(nameFile + " saved");

            /*
             <?xml version="1.0" encoding="utf-8" ?>
            <answer>
                <action>start</action>
                 <result total="100" code="0">Сеанс успешно запущен</result>
            </answer>

             */
        }


    }
}
