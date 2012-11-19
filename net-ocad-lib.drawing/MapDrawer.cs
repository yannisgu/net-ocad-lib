using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net_ocad_lib.OcadFileType;

namespace net_ocad_lib.drawing
{
    public class MapDrawer
    {
        public static void DrawMap(IOcadFile file, string filename, int width, int height)
        {
            double miny = double.MaxValue, minx = double.MaxValue, maxy = double.MinValue, maxx = double.MinValue;
            foreach (var o in file.Objects)
            {
                if (o != null)
                {
                    foreach (var c in o.Coordinates)
                    {
                        if (c.X < minx)
                            minx = c.X;
                        if (c.X > maxx)
                            maxx = c.X;
                        if (c.Y < miny)
                            miny = c.Y;
                        if (c.Y > maxy)
                            maxy = c.Y;
                    }
                }
            }


            double sx = (maxx - minx) / width;
            double sy = (maxy - miny) / height;

            if (sx < sy)
                sx = sy;
            else
                sy = sx;

            using (Image img = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    g.Clear(Color.White);
                    foreach (var o in file.Objects)
                    {
                        if (o != null)
                        {
                            var sym = file.Symbols.Where(x => x.SymbolNumber == o.SymbolNumber).FirstOrDefault();
                            if (sym == null)
                                continue;

                            if (o is PointObject)
                            {
                                double x = (o.Coordinates[0].X - minx)/sx;
                                double y = (maxy-o.Coordinates[0].Y)/sy;
                                g.FillEllipse(Brushes.Black, (float)(x - 1), (float)(y - 1), 3, 3);
                            }
                            else if (o is LineObject)
                            {
                                Pen p = getPenFromSymbol(sym);
                                g.DrawLines(p, ToPoints(o.Coordinates, minx, sx, maxy, sy));
                            }
                        }
                    }
                }
                img.Save(filename);
            }

        }

        private static Pen getPenFromSymbol(BaseSymbol sym)
        {
            if (sym is LineSymbol)
            {
                LineSymbol ls = sym as LineSymbol;
                Pen p = new Pen(getColor(ls.LineColor), (float)ls.LineWidth/72);
                p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                return p;
            }
            else
                return Pens.LightGray;
        }

        private static Color getColor(CmykColor cmykColor)
        {
            int[] rgb = cmykToRgb(cmykColor.cyan, cmykColor.magenta, cmykColor.yellow, cmykColor.black);
            return Color.FromArgb(rgb[0], rgb[1], rgb[2]);
        }

        public static int[] cmykToRgb(int cyan, int magenta, int yellow, int black)
        {
            if (black != 255)
            {
                int R = ((255 - cyan) * (255 - black)) / 255;
                int G = ((255 - magenta) * (255 - black)) / 255;
                int B = ((255 - yellow) * (255 - black)) / 255;
                return new int[] { R, G, B };
            }
            else
            {
                int R = 255 - cyan;
                int G = 255 - magenta;
                int B = 255 - yellow;
                return new int[] { R, G, B };
            }
        }

        private static PointF[] ToPoints(Coordinate[] coordinate, double minx, double sx, double maxy, double sy)
        {
            PointF[] ret = new PointF[coordinate.Length];
            for (int i = 0; i < coordinate.Length; i++)
            {
                double x = (coordinate[i].X - minx) / sx;
                double y = (maxy - coordinate[i].Y) / sy;
                ret[i] = new PointF((float)x, (float)y);

            }
            return ret;
        }

    }
}
