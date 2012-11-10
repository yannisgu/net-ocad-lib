using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net_ocad_lib
{
    public interface IOcadFile
    {
        BaseSymbol[] Symbols { get; set; }
    }
}
