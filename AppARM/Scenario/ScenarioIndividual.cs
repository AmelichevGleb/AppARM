using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Net.Http;
using System.Net.Sockets;
=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
using System.Text;
using System.Threading.Tasks;

namespace AppARM.Scenario
{
    internal class ScenarioIndividual
    {
<<<<<<< HEAD
        byte[] sendMassive;
        public byte[] CommandGroup(bool _type, string _typeScenario)
        {
=======
        public string CommandGroup(bool _type, string _typeScenario)
        {

>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            string soundCheck = "internal";
            if (_type == true) { soundCheck = "external"; }
            var command = new StringBuilder();
            command.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\" ?><command>\n<action>start</action>\n" +
               "<parameters>\n<scenario>{0}</scenario>\n<audio>{1}<audio/>\n</parameters>\n</command>", _typeScenario, soundCheck);

            Console.WriteLine(command.ToString());
<<<<<<< HEAD



            return SendReceive(Convert.ToString(command));
        }
        public byte[] SendReceive(string _command)
        {
            // перезапись 1-го массива установа первых 4-х байт
            byte[] massive1 = Encoding.Default.GetBytes(_command);
            byte[] massive2 = new byte[massive1.Length + 4];
            Int32 datasize = massive1.Length;

            Console.WriteLine(massive1.Length);
            for (int i = 3; i >= 0; i--)
            {
                massive2[i] = (byte)(datasize % 256);
                datasize = datasize / 256;
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(massive2));
            for (int j = 4; j < massive2.Length; j++)
            {
                massive2[j] = massive1[j - 4];
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(massive2));
            sendMassive = massive2;
            return sendMassive;


=======
            return Convert.ToString(command);
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
        }
    }
}
