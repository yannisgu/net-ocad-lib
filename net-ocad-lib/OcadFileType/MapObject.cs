using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net_ocad_lib.OcadFileType
{
    public class MapObject
    {
        public Coordinate[] Coordinates { get; set; }
        public int SymbolNumber { get; set; }
    }

    public class PointObject : MapObject
    {
        public double Angle { get; set; }
    }

    public class LineObject : MapObject
    {
    }
}
