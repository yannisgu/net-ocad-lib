using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net_ocad_lib.OcadFileType
{
    public interface ISymbolIndexBlock
    {
        int NextSymbolIndexBlock { get; set; }
        int[] SymbolPositions();
    }
}
