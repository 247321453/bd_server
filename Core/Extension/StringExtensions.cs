using System;
using System.Globalization;

namespace Core.Extension
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string hexString)
        {
            byte[] numArray = new byte[hexString.Length / 2];
            for (int index = 0; index < numArray.Length; ++index)
            {
                string s = hexString.Substring(index * 2, 2);
                numArray[index] = byte.Parse(s, NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture);
            }
            return numArray;
        }
    }
}
