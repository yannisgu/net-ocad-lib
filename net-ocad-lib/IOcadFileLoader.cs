using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using net_ocad_lib.OcadFileType;

namespace net_ocad_lib
{
    public interface IOcadFileLoader
    {
        IOcadFile Load(string filename);
    }
}
