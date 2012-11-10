using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using net_ocad_lib.OcadFileType;

namespace net_ocad_lib.Loaders
{
    public class OcadFileLoader
    {
        public static IOcadFileLoader GetLoaderForVersion(int ocadVersion)
        {
            if (ocadVersion <= 8)
                return new Ocad8Loader();
            else if (ocadVersion == 10)
                return new Ocad10Loader();
            else
                throw new ApplicationException("No loader for OCAD version : " + ocadVersion);
        }

        public static int GetFileMajorVersion(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            Type t = typeof(OcadFileType.Ocad10FileHeader);
            byte[] data = new byte[System.Runtime.InteropServices.Marshal.SizeOf(t)];
            ConvertHelpers.ReadArrayFromStream(fs, data);
            fs.Close();

            short mark = BitConverter.ToInt16(data, 0);
            if (mark != 0x0cad)
                throw new IOException("File " + filename + " is not recognized as an OCAD file!");

            OcadFileType.Ocad10FileHeader header = ConvertHelpers.FromByteArray<OcadFileType.Ocad10FileHeader>(data);
            return header.Version;
        }

        public static List<int> ReadSymbolIndexes<T>(FileStream fs, int firstSymbolIndexBlock)
        {
            fs.Seek(firstSymbolIndexBlock, SeekOrigin.Begin);

            List<int> symbolIndexes = new List<int>();
            bool more = false;
            do
            {
                var symIdx = ConvertHelpers.FromStream<T>(fs) as ISymbolIndexBlock;
                foreach (var idx in symIdx.SymbolPositions())
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

            return symbolIndexes;
        }
    }
}
