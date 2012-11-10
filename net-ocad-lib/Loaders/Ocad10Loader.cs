using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using net_ocad_lib.OcadFileType;

namespace net_ocad_lib.Loaders
{
    public class Ocad10Loader : OcadFileLoader, IOcadFileLoader
    {
        public IOcadFile Load(string filename)
        {
            IOcadFile file = null;

            FileStream fs = File.OpenRead(filename);

            //Read header
            OcadFileType.Ocad10FileHeader header = ConvertHelpers.FromStream<OcadFileType.Ocad10FileHeader>(fs);
            //Read symbolindexes
            List<int> symIndexes = ReadSymbolIndexes<Ocad10SymbolIndexBlock>(fs, header.FirstSymbolIndexBlock);
            fs.Close();

            return file;
        }

        public BaseSymbol ReadSymbol(FileStream fs, int symFilePosition)
        {
            return null;
        }
        
    }
}
