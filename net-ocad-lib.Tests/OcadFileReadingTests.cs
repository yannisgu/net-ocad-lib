using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using net_ocad_lib.Loaders;
using NUnit.Framework;
using System.Linq;
using net_ocad_lib.OcadFileType;

namespace net_ocad_lib.Tests
{
    [TestFixture]
    public class OcadFileReadingTests
    {
        [Test]
        public void TestDetectOcad8File()
        {
            string testfile = getPathToTestfile("TestFiles\\Elseholm.ocd");
            int version = OcadFileLoader.GetFileMajorVersion(testfile);
            Assert.AreEqual(8, version);
        }

        [Test]
        public void TestReadOcad8File()
        {
            string testfile = getPathToTestfile("TestFiles\\Elseholm.ocd");

             FileStream fs = File.OpenRead(testfile);
            OcadFileType.Ocad8FileHeader header = ConvertHelpers.FromStream<OcadFileType.Ocad8FileHeader>(fs);
            List<int> symbolIdx = OcadFileLoader.ReadSymbolIndexes<OcadFileType.Ocad8SymbolIndexBlock>(fs, header.FirstSymbolIndexBlock);
            Assert.AreEqual(161, symbolIdx.Count);
            fs.Close();
        }

        [Test]
        public void TestReadOcad8FileSymbols()
        {
            string testfile = getPathToTestfile("TestFiles\\Elseholm.ocd");

            FileStream fs = File.OpenRead(testfile);
            OcadFileType.Ocad8FileHeader header = ConvertHelpers.FromStream<OcadFileType.Ocad8FileHeader>(fs);
            OcadFileType.Ocad8SymHeader symheader = OcadFileType.Ocad8SymHeader.ReadFromStream(fs);
            List<int> symbolIdx = OcadFileLoader.ReadSymbolIndexes<OcadFileType.Ocad8SymbolIndexBlock>(fs, header.FirstSymbolIndexBlock);
            Assert.AreEqual(161, symbolIdx.Count);

            
             var Colors = symheader.aColorInfo.Select(x => new ColorInfo() { ColorName = x.ColorName, ColorNum = x.ColorNum, SepPercentage = x.SepPercentage, Color = new CmykColor() { black = x.Color.black, yellow = x.Color.yellow, magenta = x.Color.magenta, cyan = x.Color.cyan } }).ToArray();


             var symbols = Ocad8Loader.ReadSymbols(fs, symbolIdx, Colors);
            VerifySymbolsInElseholm(symbols);
            fs.Close();
        }

        [Test]
        public void TestReadOcad8FileSymbolsFromFile()
        {
            string testfile = getPathToTestfile("TestFiles\\Elseholm.ocd");

            IOcadFile of = OcadFile.OpenFile(testfile);

            VerifySymbolsInElseholm(of.Symbols.ToList());
        }

        [Test]
        public void TestReadOcad8FileObjectFromFile()
        {
            string testfile = getPathToTestfile("TestFiles\\Gunn0101.ocd");

            IOcadFile of = OcadFile.OpenFile(testfile);

            Assert.AreEqual(533, of.Objects.Count());

            net_ocad_lib.drawing.MapDrawer.DrawMap(of, getPathToTestfile("test.png"), 1000, 3000);

        }

        private static void VerifySymbolsInElseholm(List<BaseSymbol> symbols)
        {
            Assert.AreEqual(161, symbols.Count);

            Assert.AreEqual("Höjdkurva", symbols[0].Description);
            Assert.AreEqual(1010, symbols[0].SymbolNumber);

            Assert.AreEqual(52, symbols.Where(x => x is PointSymbol).Count());
            Assert.AreEqual(63, symbols.Where(x => x is LineSymbol).Count());
            Assert.AreEqual(30, symbols.Where(x => x is AreaSymbol).Count());
            Assert.AreEqual(14, symbols.Where(x => x is TextSymbol).Count());
            Assert.AreEqual(0, symbols.Where(x => x is LineTextSymbol).Count());
            Assert.AreEqual(2, symbols.Where(x => x is RectangleSymbol).Count());

        }


        [Test]
        [ExpectedException(ExpectedException=typeof(IOException))]
        public void TestDetectNonOcadFile()
        {
            string testfile = getPathToTestfile("TestFiles\\Bitmap1.bmp");
            int version = OcadFileLoader.GetFileMajorVersion(testfile);
        }

        [Test]
        public void TestGetOcad8Loader()
        {
            IOcadFileLoader loader = OcadFileLoader.GetLoaderForVersion(8);
            Assert.IsInstanceOf(typeof(Ocad8Loader), loader);
            loader = OcadFileLoader.GetLoaderForVersion(10);
            Assert.IsInstanceOf(typeof(Ocad10Loader), loader);
        }


        string getPathToTestfile(string file)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), file);
        }
    }
}
