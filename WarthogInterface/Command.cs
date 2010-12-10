using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigBuilder
{
    class Command
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Rule { get; set; }

        public Command(string n)
        {
            Name = n;
        }

        public Command(string n, string i, string r)
        {
            Name = n;
            ID = i;
            Rule = r;
        }
    }
}
