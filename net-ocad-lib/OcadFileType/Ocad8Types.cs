using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

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
    internal class Ocad8FileHeader
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
    internal class Ocad8SymbolIndexBlock
    {
        public int NextSymbolIndexBlock { get; set; }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public int[] SymbolPosition;
    }

}
