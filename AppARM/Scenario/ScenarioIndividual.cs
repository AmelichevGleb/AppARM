using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.Scenario
{
    internal class ScenarioIndividual
    {
        public string CommandGroup(bool _type, string _typeScenario)
        {

            string soundCheck = "internal";
            if (_type == true) { soundCheck = "external"; }
            var command = new StringBuilder();
            command.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\" ?><command>\n<action>start</action>\n" +
               "<parameters>\n<scenario>{0}</scenario>\n<audio>{1}<audio/>\n</parameters>\n</command>", _typeScenario, soundCheck);

            Console.WriteLine(command.ToString());
            return Convert.ToString(command);
        }
    }
}
