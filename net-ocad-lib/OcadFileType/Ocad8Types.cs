using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace net_ocad_lib.OcadFileType
{


    //TFileHeader = record
    //  OCADMark: SmallInt;        {3245 (hex 0cad)}
    //  SectionMark: SmallInt;     {OCAD 6: 0
    //                              OCAD 7: 7
    //                              OCAD 8: 2 for normal files
    //                                      3 for course setting files}
    //  Version: SmallInt;         {6 for OCAD 6, 7 for OCAD 7, 8 for OCAD 8}
    //  Subversion: SmallInt;      {number of subversion (0 for 6.00,
    //                             1 for 6.01 etc.)}
    //  FirstSymBlk: longint;      {file position of the first symbol
    //                             block}
    //  FirstIdxBlk: longint;      {file position of the first index
    //                             block}
    //  SetupPos: longint;         {file position of the setup record }
    //  SetupSize: longint;        {size (in bytes) of the setup record}
    //  InfoPos: longint;          {file position of the file
    //                             information. The file information is
    //                             stored as a zero-terminated string with
    //                             up to 32767 characters + terminating
    //                             zero}
    //  InfoSize: longint;         {size (in bytes) of the file
    //                             information}
    //  FirstStIndexBlk: longint;  {OCAD 8 only. file position of the first
    //                             string index block}
    //  Reserved2: longint;
    //  Reserved3: longint;
    //  Reserved4: longint;
    //end;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Ocad8FileHeader
    {
        public short OCADMark { get; set; }
        public short SectionMark { get; set; }
        public short Version { get; set; }
        public short Subversion { get; set; }
        public int FirstSymbolIndexBlock { get; set; }
        public int ObjectIndexBlock { get; set; }
        public int SetupPos { get; set; }
        public int SetupSize { get; set; }
        public int InfoPos { get; set; }
        public int InfoSize { get; set; }
        public int FirstStringIndexBlk { get; set; }
        public int Reserved2 { get; set; }
        public int Reserved3 { get; set; }
        public int Reserved4 { get; set; }
    }

    //  TSymbolBlock = record
    //  NextBlock: longint;        {file position of the next symbol
    //                             block. 0 if this is the last
    //                             symbol block.}
    //  FilePos: array[0..255] of longint;
    //                             {file position of up to 256
    //                             symbols. 0 if there is no symbol
    //                             for this index.}
    //end;
    [StructLayout( LayoutKind.Sequential, Pack = 1)]
    public class Ocad8SymbolIndexBlock : ISymbolIndexBlock
    {
        public int NextSymbolIndexBlock { get; set; }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public int[] m_SymbolPosition;

        public int[] SymbolPositions()
        {
            return m_SymbolPosition;
        }
    }
    
  //    TSymHeader = record
  //  nColors: SmallInt;         {Number of colors defined}
  //  nColorSep: SmallInt;       {Number or color separations
  //                             defined}
  //  CyanFreq: SmallInt;        {Halftone frequency of the
  //                             Cyan color separation. This
  //                             is 10 times the value entered
  //                             in the CMYK Separations dialog
  //                             box.}
  //  CyanAng: SmallInt;         {Halftone angle of the cyan
  //                             color separation. This is 10 times
  //                             the value entered in the CMYK
  //                             separations dialog box.}
  //  MagentaFreq: SmallInt;     {dito for magenta}
  //  MagentaAng: SmallInt;      {dito for magenta}
  //  YellowFreq: SmallInt;      {dito for yellow}
  //  YellowAng: SmallInt;       {dito for yellow}
  //  BlackFreq: SmallInt;       {dito for black}
  //  BlackAng: SmallInt;        {dito for black}
  //  Res1: SmallInt;
  //  Res2: SmallInt;
  //  aColorInfo: array [0..255] of TColorInfo;
  //                             {the TColorInfo structure is
  //                             explained below}
  //  aColorSep: array [0..31] of TColorSep;
  //                             {the TColorSep structure is
  //                             explained below. Note that only
  //                             24 color separations are allowed.
  //                             The rest is reserved for future
  //                             use.}
  //end;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Ocad8SymHeader
    {
        public short nColors { get; set; }
        public short nColorSep { get; set; }
        public short CyanFreq { get; set; }
        public short CyanAng { get; set; }
        public short MagentaFreq { get; set; }
        public short MagentaAng { get; set; }
        public short YellowFreq { get; set; }
        public short YellowAng { get; set; }
        public short BlackFreq { get; set; }
        public short BlackAng { get; set; }
        public short Res1 { get; set; }
        public short Res2 { get; set; }

        [MarshalAs(UnmanagedType.ByValArray,  SizeConst = 256)]
        public Ocad8ColorInfo[] aColorInfo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Ocad8ColorSep[] aColorSep;


        public static Ocad8SymHeader ReadFromStream(Stream s)
        {
            Ocad8SymHeader symh = new Ocad8SymHeader();
            
            //Type t = symh.GetType();
            //byte[] data = new byte[Marshal.SizeOf(symh)];
            //ConvertHelpers.ReadArrayFromStream(s, data);

            symh.nColors = FileIOHelpers.ReadInt16FromStream(s);
            symh.nColorSep = FileIOHelpers.ReadInt16FromStream(s);

            symh.aColorInfo = new Ocad8ColorInfo[symh.nColors];
            symh.aColorSep = new Ocad8ColorSep[symh.nColorSep];

            symh.CyanFreq = FileIOHelpers.ReadInt16FromStream(s);
            symh.CyanAng = FileIOHelpers.ReadInt16FromStream(s);
            symh.MagentaFreq = FileIOHelpers.ReadInt16FromStream(s);
            symh.MagentaAng = FileIOHelpers.ReadInt16FromStream(s);
            symh.YellowFreq = FileIOHelpers.ReadInt16FromStream(s);
            symh.YellowAng = FileIOHelpers.ReadInt16FromStream(s);
            symh.BlackFreq = FileIOHelpers.ReadInt16FromStream(s);
            symh.BlackAng = FileIOHelpers.ReadInt16FromStream(s);
            symh.Res1 = FileIOHelpers.ReadInt16FromStream(s);
            symh.Res2 = FileIOHelpers.ReadInt16FromStream(s);

            for (int i = 0; i < Math.Min(256, (int)symh.nColors); i++)
            {
                symh.aColorInfo[i] = new Ocad8ColorInfo();
                symh.aColorInfo[i].ColorNum = FileIOHelpers.ReadInt16FromStream(s);
                symh.aColorInfo[i].Reserved = FileIOHelpers.ReadInt16FromStream(s);

                symh.aColorInfo[i].Color = new Ocad8Cmyk();
                symh.aColorInfo[i].Color.cyan = FileIOHelpers.ReadBytesFromStream(s, 1)[0];
                symh.aColorInfo[i].Color.magenta = FileIOHelpers.ReadBytesFromStream(s, 1)[0];
                symh.aColorInfo[i].Color.yellow= FileIOHelpers.ReadBytesFromStream(s, 1)[0];
                symh.aColorInfo[i].Color.black = FileIOHelpers.ReadBytesFromStream(s, 1)[0];

                symh.aColorInfo[i].ColorName = FileIOHelpers.ReadDelphiStringFromStream(s, 31);
                symh.aColorInfo[i].SepPercentage = FileIOHelpers.ReadBytesFromStream(s, 32);
            }

            return symh;            
        }

    }

  //   TColorInfo = record
  //  ColorNum: SmallInt;        {Color number. This number is
  //                             used in the symbols when
  //                             referring a color.}
  //  Reserved: SmallInt;
  //  Color: TCmyk;              {Color value. The structure
  //                             is explained below.}
  //  ColorName: string[31];     {Description of the color}
  //  SepPercentage: array [0..31] of byte;
  //                             {Definition how the color
  //                             appears in the different spot
  //                             color separations.
  //                               0..200: 2 times the separation
  //                                 percentage as it appears
  //                                 in the Color dialog box (to
  //                                 allow half percents)
  //                               255: the color does not
  //                                 appear in the corresponding
  //                                 color separation (empty field
  //                                 in the color dialog box)}
  //end;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Ocad8ColorInfo
    {
        public short ColorNum { get; set; }
        public short Reserved { get; set; }
        public Ocad8Cmyk Color { get; set; }

        [MarshalAs(UnmanagedType.AnsiBStr, SizeConst = 31)]
        public string ColorName;
        
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=32)]
        public byte[] SepPercentage;
    }

  //  TCmyk = record
  //  cyan: byte;                {2 times the cyan value as it
  //                             appears in the Define Color dialog
  //                             box (to allow half percents)}
  //  magenta: byte;             {dito for magenta}
  //  yellow: byte;              {dito for yellow}
  //  black: byte;               {dito for black}
  //end;
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public class Ocad8Cmyk
    {
        public byte cyan { get; set; }
        public byte magenta { get; set; }
        public byte yellow { get; set; }
        public byte black { get; set; }
    }

  //  TColorSep = record
  //  SepName: string[15];       {Name of the color separation}
  //  Color: TCmyk;              {0 in OCAD 6, CMYK value of
  //                             the separation in OCAD 7.
  //                             This value is only used in
  //                             the AI (Adobe Illustrator) export}
  //  RasterFreq: SmallInt;      {10 times the halfton frequency
  //                             as it appears in the Color
  //                             Separation dialog box.}
  //  RasterAngle: SmallInt;     {10 times the halftone angle
  //                             as it appears in the Color
  //                             Separation dialog box.}
  //end;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Ocad8ColorSep
    {
        [MarshalAs(UnmanagedType.AnsiBStr, SizeConst = 15)]
        public string SepName;
        public Ocad8Cmyk Color { get; set; }
        public short RasterFreq { get; set; }
        public short RasterAngle { get; set; }
    }


}
