using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarthogInterface
{
    public class Command
    {
        public string Name { get; set; }
        public string DeviceID { get; set; }
        public string Button { get; set; }
        public string Rule { get; set; }
        public string LastOutput { get; set; }
        public string CurrentOutput { get; set; }

        public Command(string n)
        {
            Name = n;
        }

        public Command(string n, string i, string b, string r)
        {
            Name = n;
            DeviceID = i;
            Button = b;
            Rule = r;
        }
    }
}
