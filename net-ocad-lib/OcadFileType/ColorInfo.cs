using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net_ocad_lib.OcadFileType
{
    public class ColorInfo
    {
        public short ColorNum { get; set; }
        public short Reserved { get; set; }
        public CmykColor Color { get; set; }
        public string ColorName;
        public byte[] SepPercentage;
    }
}
