using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace net_ocad_lib
{
    internal class FileIOHelpers
    {
        /// <summary>
        /// Reads the specified number of bytes from the stream, takes care och System.IO not reading all the specified bytes at the same time
        /// </summary>
        /// <param name="s"></param>
        /// <param name="numBytes"></param>
        /// <returns></returns>
        public static byte[] ReadBytesFromStream(Stream s, int numBytes)
        {
            byte[] ret = new byte[numBytes];
            int numRead = 0;
            while (numRead < numBytes)
            {
                int nr = s.Read(ret, numRead, numBytes - numRead);
                numRead += nr;
            }
            return ret;
        }

        public static int ReadInt32FromStream(Stream s)
        {
            byte[] data = ReadBytesFromStream(s, 4);
            return BitConverter.ToInt32(data, 0);
        }

        public static bool ReadBoolFromStream(Stream s)
        {
            byte[] data = ReadBytesFromStream(s, 1);
            return BitConverter.ToBoolean(data, 0);
        }

        public static short ReadInt16FromStream(Stream s)
        {
            byte[] data = ReadBytesFromStream(s, 2);
            return BitConverter.ToInt16(data, 0);
        }

        public static long ReadInt64FromStream(Stream s)
        {
            byte[] data = ReadBytesFromStream(s, 8);
            return BitConverter.ToInt64(data, 0);
        }

        public static string ReadDelphiStringFromStream(Stream s)
        {
            byte[] data = ReadBytesFromStream(s, 32);
            int numChars = (int)data[0];
            return System.Text.Encoding.GetEncoding(1252).GetString(data.Skip(1).Take(numChars).ToArray());
        }
    }
}
