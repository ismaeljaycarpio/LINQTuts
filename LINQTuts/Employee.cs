using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQTuts
{
    class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Position> Positions { get; set; }
    }

    class Position
    {
        public string Title { get; set; }
        public int Level { get; set; }
    }
}
