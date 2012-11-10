using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using net_ocad_lib.OcadFileType;

namespace net_ocad_lib.Loaders
{
    public class Ocad8Loader : OcadFileLoader, IOcadFileLoader
    {
        public IOcadFile Load(string filename)
        {
            IOcadFile file = new OcadFile();

            FileStream fs = File.OpenRead(filename);
            //Read header
            OcadFileType.Ocad8FileHeader header = ConvertHelpers.FromStream<OcadFileType.Ocad8FileHeader>(fs);
            //Read SYmbolheader
            OcadFileType.Ocad8SymHeader symheader = OcadFileType.Ocad8SymHeader.ReadFromStream(fs);
            //Read symbolindexes
            List<int> symIndexes = ReadSymbolIndexes<Ocad8SymbolIndexBlock>(fs, header.FirstSymbolIndexBlock);
            file.Symbols = ReadSymbols(fs, symIndexes).ToArray();
            fs.Close();

            return file;
        }

        public static List<BaseSymbol>  ReadSymbols(FileStream fs, List<int> symIndexes)
        {
            List<BaseSymbol> symbols = new List<BaseSymbol>();
            foreach (var idx in symIndexes)
            {
                var bs = ReadSymbol(fs, idx);
                symbols.Add(bs);
            }
            return symbols;
        }

        public static BaseSymbol ReadSymbol(FileStream fs, int symFilePosition)
        {
            fs.Seek(symFilePosition, SeekOrigin.Begin);
            int dataSize = FileIOHelpers.ReadInt16FromStream(fs);
            
            int symbolNumber = FileIOHelpers.ReadInt16FromStream(fs);

            int objectType = FileIOHelpers.ReadInt16FromStream(fs);
            byte symType = FileIOHelpers.ReadBytesFromStream(fs, 1)[0];
            BaseSymbol bs = GetSymbolFromType(objectType, symType);

            bs.SymbolNumber = symbolNumber;

            byte flags = FileIOHelpers.ReadBytesFromStream(fs, 1)[0];
            bs.Extent = FileIOHelpers.ReadInt16FromStream(fs);

            bool selected = FileIOHelpers.ReadBoolFromStream(fs);
            byte status = FileIOHelpers.ReadBytesFromStream(fs, 1)[0];
            int res2 = FileIOHelpers.ReadInt16FromStream(fs);
            int res3 = FileIOHelpers.ReadInt16FromStream(fs);

            long filePos = FileIOHelpers.ReadInt32FromStream(fs);
            byte[] colors = FileIOHelpers.ReadBytesFromStream(fs, 32);
            bs.Description = FileIOHelpers.ReadDelphiStringFromStream(fs);
            bs.Icon = FileIOHelpers.ReadBytesFromStream(fs, 264);

            if (bs is PointSymbol)
                readPointSymbol(bs, fs);

            return bs;
        }

        private static void readPointSymbol(BaseSymbol bs, FileStream fs)
        {
            short dataSize = FileIOHelpers.ReadInt16FromStream(fs);
            short res = FileIOHelpers.ReadInt16FromStream(fs);
        }

        private static BaseSymbol GetSymbolFromType(int objectType, byte symType)
        {
            BaseSymbol bs = null;

            switch (objectType)
            {
                case 1:
                    bs = new PointSymbol();
                    break;
                case 2:
                    if (symType == 0x01)
                        bs = new LineTextSymbol();
                    else
                        bs = new LineSymbol();
                    break;
                case 3:
                    bs = new AreaSymbol();
                    break;
                case 4:
                    bs = new TextSymbol();
                    break;
                case 5:
                    bs = new RectangleSymbol();
                    break;
                default:
                    throw new ApplicationException("Unknown objectType: " + objectType);
            }
            return bs;
        }

        
    }
}
