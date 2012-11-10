using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using net_ocad_lib.Loaders;

namespace net_ocad_lib
{
    public class OcadFile : IOcadFile
    {
        public static IOcadFile OpenFile(string filename)
        {
            int majorversion = OcadFileLoader.GetFileMajorVersion(filename);
            if (majorversion != 10 && majorversion > 8)
                throw new ApplicationException("OCAD files of version " + majorversion + " is not supported!");

            IOcadFileLoader loader = net_ocad_lib.Loaders.OcadFileLoader.GetLoaderForVersion(majorversion);
            IOcadFile file = loader.Load(filename);


            return file;
        }

        public BaseSymbol[] Symbols { get; set; }
        public ObjectIndex[] ObjectIndexes { get; set; }
    }
}
