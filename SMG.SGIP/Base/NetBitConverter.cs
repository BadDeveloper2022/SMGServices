using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    public class NetBitConverter
    {
        public static uint ToUInt32(byte[] values , int index)
        {
            byte[] dst = new byte[4];
            Array.Copy(values, index, dst, 0, 4);
            dst = dst.Reverse().ToArray();

            return BitConverter.ToUInt32(dst, 0);
        }

        public static byte[] GetBytes(uint value)
        {
            byte[] dst = BitConverter.GetBytes(value);
            dst = dst.Reverse().ToArray();

            return dst;
        }

    }
}
