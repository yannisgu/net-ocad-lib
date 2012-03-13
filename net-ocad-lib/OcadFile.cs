using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace net_ocad_lib
{
    public class OcadFile
    {
        public static OcadFile OpenFile(string filename)
        {
            int majorversion = DetectFiletype(filename);
            if (majorversion != 10 && majorversion > 8)
                throw new ApplicationException("OCAD files of version " + majorversion + " is not supported!");

            if (majorversion == 10)
            {

                ReadOcad10File(filename);
            }
            else if (majorversion <= 8)
            {
                ReadOcad8File(filename);
            }


            return null;
        }

        private static void ReadOcad8File(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            //Read header
            OcadFileType.Ocad8FileHeader header = ConvertHelpers.FromStream<OcadFileType.Ocad8FileHeader>(fs);

            fs.Seek(header.FirstSymbolIndexBlock, SeekOrigin.Begin);
            //Read symbolindexes
            List<int> symbolIndexes = new List<int>();
            bool more = false;
            do
            {
                OcadFileType.Ocad8SymbolIndexBlock symIdx = ConvertHelpers.FromStream<OcadFileType.Ocad8SymbolIndexBlock>(fs);
                foreach (var idx in symIdx.SymbolPosition)
                {
                    if (idx > 0)
                        symbolIndexes.Add(idx);
                }

                if (symIdx.NextSymbolIndexBlock > 0)
                {
                    more = true;
                    fs.Seek(symIdx.NextSymbolIndexBlock, SeekOrigin.Begin);
                }

            } while (more);

            fs.Close();
        }



        private static void ReadOcad10File(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            //Read header
            OcadFileType.Ocad10FileHeader header = ConvertHelpers.FromStream<OcadFileType.Ocad10FileHeader>(fs);

            fs.Seek(header.FirstSymbolIndexBlock, SeekOrigin.Begin);
            //Read symbolindexes
            List<OcadFileType.Ocad10SymbolIndexBlock> symbolIndexes = new List<OcadFileType.Ocad10SymbolIndexBlock>();
            bool more = false;
            do
            {
                OcadFileType.Ocad10SymbolIndexBlock symIdx = ConvertHelpers.FromStream<OcadFileType.Ocad10SymbolIndexBlock>(fs);
                symbolIndexes.Add(symIdx);
                if (symIdx.NextSymbolIndexBlock > 0)
                {
                    more = true;
                    fs.Seek(symIdx.NextSymbolIndexBlock, SeekOrigin.Begin);
                }

            } while (more);

            fs.Close();
        }

        private static int DetectFiletype(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            Type t = typeof(OcadFileType.Ocad10FileHeader);
            byte[] data = new byte[System.Runtime.InteropServices.Marshal.SizeOf(t)];
            ConvertHelpers.ReadArrayFromStream(fs, data);
            fs.Close();

            short mark = BitConverter.ToInt16(data, 0);
            if (mark != 0x0cad)
                throw new ApplicationException("File " + filename + " is not recognized as an OCAD file!");

            OcadFileType.Ocad10FileHeader header = ConvertHelpers.FromByteArray<OcadFileType.Ocad10FileHeader>(data);
            return header.Version;
        }


    }
}
