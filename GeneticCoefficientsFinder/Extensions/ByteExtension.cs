using System;

namespace AlgorytmGenetyczny.Extensions
{
    public static class ByteExtension
    {
        public static string ToBinaryString(this byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }

        public static byte ToggleBit(this byte b, int position)
        {
            if (position < 0 || position > 7)
                throw new WrongValueException("Index must be in the range of 0-7.");

            var ret = (byte)(b ^ (1 << position));

            return ret;
        }

        public static bool IsBitSet(this byte b, int pos)
        {
            if (pos < 0 || pos > 7)
                throw new WrongValueException("Index must be in the range of 0-7.");

            return (b & (1 << pos)) != 0;
        }
    }
}
