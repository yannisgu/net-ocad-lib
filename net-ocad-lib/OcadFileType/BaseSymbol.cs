using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net_ocad_lib
{
    /*
     *  TBaseSym = record
    Size: SmallInt;            {Size of the symbol in bytes. This
                               depends on the type and the
                               number of subsymbols.}
    Sym: SmallInt;             {Symbol number. This is 10 times
                               the value which appears on the
                               screen (1010 for 101.0)}
    Otp: SmallInt;             {Object type
                                 1: Point symbol
                                 2: Line symbol or Line text
                                    symbol
                                 3: Area symbol
                                 4: Text symbol
                                 5: Rectangle symbol}
    SymTp: byte;               {Symbol type
                                 1: for Line text and text
                                    symbols
                                 0: for all other symbols}
    Flags: byte;               {OCAD 6/7: must be 0
                                OCAD 8: bit flags
                                  1: not oriented to north (inverted for
                                     better compatibility)
                                  2: Icon is compressed}
    Extent: SmallInt;          {Extent how much the rendered
                               symbols can reach outside the
                               coordinates of an object with
                               this symbol.
                               For a point object it tells
                               how far away from the coordinates
                               of the object anything of the
                               point symbol can appear.}
    Selected: boolean;         {Symbol is selected in the symbol
                               box}
    Status: byte;              {Status of the symbol
                                 0: Normal
                                 1: Protected
                                 2: Hidden}
    Res2: SmallInt;
    Res3: SmallInt;
    FilePos: longint;          {File position, not used in the
                               file, only when loaded in
                               memory. Value in the file is
                               not defined.}
    Cols: TColors;             {Set of the colors used in this
                               symbol. TColors is an array of
                               32 bytes, where each bit
                               represents 1 of the 256 colors.
                                 TColors = set of 0..255;
                               The color with the number 0 in
                               the color table appears as the
                               lowest bit in the first byte of
                               the structure.}
    Description: string [31];  {The description of the symbol}
    IconBits: array[0..263] of byte;
                               {the icon can be uncompressed (16-bit colors)
                               or compressed (256 color palette) depending
                               on the Flags field.
                               In OCAD 6/7 it is always uncompressed}
  end;*/

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
}
