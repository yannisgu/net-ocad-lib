﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net_ocad_lib.OcadFileType;

namespace net_ocad_lib
{
    public interface IOcadFile
    {
        BaseSymbol[] Symbols { get; set; }
        ColorInfo[] Colors { get; set; }
        MapObject[] Objects { get; set; }
    }
}
