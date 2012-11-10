using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net_ocad_lib
{
    public class BaseSymbol
    {
        public int SymbolNumber { get; set; }
        public bool OrientedNorth { get; set; }
        public int Extent { get; set; }
        public string Description { get; set; }
        public byte[] Icon{ get; set; }
    }

    public class PointSymbol : BaseSymbol
    {
        public SymbolElement[] SymbolElements { get; set; }
    }
    public class LineSymbol : BaseSymbol
    {

    }
    public class AreaSymbol : BaseSymbol
    {

    }
    public class LineTextSymbol : BaseSymbol
    {

    }
    public class TextSymbol : BaseSymbol
    {

    }

    public class RectangleSymbol : BaseSymbol
    {

    }

    public class CmykColor
    {
        public byte cyan { get; set; }
        public byte magenta { get; set; }
        public byte yellow { get; set; }
        public byte black { get; set; }
    }

    public class SymbolElement
    {
        public CmykColor color { get; set; }
        public Coordinate[] Coordinates { get; set; }
    }

    public class LineSymbolElement : SymbolElement
    {
    }

    public class AreaSymbolElement : SymbolElement
    {
    }

    public class CircleSymbolElement : SymbolElement
    {
    }

    public class DotSymbolElement : SymbolElement
    {
    }

    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public static Coordinate FromOcadVal(int xval, int yval)
        {
            Coordinate c = new Coordinate();
            c.X = xval >> 8;
            c.Y = yval >> 8;
            return c;
        }
    }
}
