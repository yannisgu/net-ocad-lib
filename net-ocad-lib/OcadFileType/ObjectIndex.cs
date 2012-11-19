using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net_ocad_lib
{
    public class ObjectIndex
    {
        public Coordinate LowerLeft { get; set; }
        public Coordinate UpperRight { get; set; }
        public int SymbolNumber { get; set; }
        public int FilePostition { get; set; }
    }
}
