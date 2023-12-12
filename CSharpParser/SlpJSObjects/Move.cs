using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.SlpJSObjects
{
    public class Move
    {
        public int playerIndex { get; set; }
        public int frame { get; set; }
        public int hitCount { get; set; }
        public double damage { get; set; }
    }
}
