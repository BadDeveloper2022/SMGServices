namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    public class BitConvert
    {
        public static uint bytes2Uint(byte[] bs, int index)
        {
            byte[] dst = new byte[4];
            Buffer.BlockCopy(bs, index, dst, 0, 4);
            byte num = dst[0];
            dst[0] = dst[3];
            dst[3] = num;
            num = dst[1];
            dst[1] = dst[2];
            dst[2] = num;
            return BitConverter.ToUInt32(dst, 0);
        }

        public static byte[] uint2Bytes(uint u)
        {
            byte[] bytes = BitConverter.GetBytes(u);
            byte num = bytes[0];
            bytes[0] = bytes[3];
            bytes[3] = num;
            num = bytes[1];
            bytes[1] = bytes[2];
            bytes[2] = num;
            return bytes;
        }
    }
}

