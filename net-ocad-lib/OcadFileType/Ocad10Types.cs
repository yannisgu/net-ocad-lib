using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace net_ocad_lib.OcadFileType
{


  //   TFileHeader = record         // Size = 48 Byte
  //  OCADMark: SmallInt;        // 3245 (hex 0cad)
  //  FileType: Byte;            // File type
  //                             //   0: normal map
  //                             //   1: course setting project
  //                             //   3: OCAD 8 course setting project
  //  FileStatus: Byte;          // Not used
  //  Version: SmallInt;         // 10
  //  Subversion: Byte;          // number of subversion (0 for 10.00, 1 for 10.1 etc.)
  //  SubSubversion: Byte;       // number of subsubversion (0 for 10.0.0, 1 for 10.0.1)
  //  FirstSymbolIndexBlock: integer;   // file position of the first symbol index block
  //  ObjectIndexBlk: integer;   // file position of object index block -> TObjectIndexBlock    // max 65536 * 256 objects
  //  Res0: integer;             // Not used
  //  Res1: integer;             // Not used
  //  Res2: longint;             // Not used
  //  Res3: longint;             // Not used
  //  FirstStringIndexBlk: longint;   // file position of string index block -> TStringIndexBlock    // max 256 strings
  //  FileNamePos: integer;      // file position of file name, used for temporary files only
  //  FileNameSize: integer;     // size of the file name, used for temporary files only
  //  Res4: longint;             // Not used
  //end;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Ocad10FileHeader
    {
        public short OCADMark { get; set; }
        public byte FileType { get; set; }
        public byte FileStatus { get; set; }
        public short Version { get; set; }
        public byte Subversion { get; set; }
        public int FirstSymbolIndexBlock { get; set; }
        public int ObjectIndexBlock { get; set; }
        public int Res0 { get; set; }
        public int Res1 { get; set; }
        public long Res2 { get; set; }
        public long Res3 { get; set; }
        public long FirstStringIndexBlk { get; set; }
        public int FileNamePos { get; set; }
        public int FileNameSize { get; set; }
        public long Res4 { get; set; }
    }

    //   TSymbolIndexBlock= record   // Size: 1028 Bytes
    // NextSymbolIndexBlock: integer;
    // SymbolPosition: array[0..255] of integer;
    // end;
    [StructLayout( LayoutKind.Sequential, Pack = 1)]
    public class Ocad10SymbolIndexBlock : ISymbolIndexBlock
    {
        public int NextSymbolIndexBlock { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public int[] m_SymbolPosition;

        public int[] SymbolPositions()
        {
            return m_SymbolPosition;
        }
    }

}
