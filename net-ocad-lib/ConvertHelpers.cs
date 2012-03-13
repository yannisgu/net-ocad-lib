using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace net_ocad_lib
{
    internal class ConvertHelpers
    {
        public static T FromStream<T>(Stream s)
        {
            byte[] data = new byte[System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            ReadArrayFromStream(s, data);
            return FromByteArray<T>(data);
        }

        internal static void ReadArrayFromStream(Stream fs, byte[] data)
        {
            int toRead = data.Length;
            int numRead = 0;
            do
            {
                int r = fs.Read(data, numRead, toRead - numRead);
                numRead += r;
            } while (numRead < toRead);
        }

        public static T FromByteArray<T>(byte[] rawValue)
        {
            GCHandle handle = GCHandle.Alloc(rawValue, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        public static byte[] ToByteArray(object value, int maxLength)
        {
            int rawsize = Marshal.SizeOf(value);
            byte[] rawdata = new byte[rawsize];
            GCHandle handle =
                GCHandle.Alloc(rawdata,
                GCHandleType.Pinned);
            Marshal.StructureToPtr(value,
                handle.AddrOfPinnedObject(),
                false);
            handle.Free();
            if (maxLength < rawdata.Length)
            {
                byte[] temp = new byte[maxLength];
                Array.Copy(rawdata, temp, maxLength);
                return temp;
            }
            else
            {
                return rawdata;
            }
        }
    }
}
