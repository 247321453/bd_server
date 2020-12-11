using System;

namespace Core.Extension
{
    public static class BufferUtil
    {
        public static int GetInt(ref byte[] buffer, int startIndex)
        {
            return BitConverter.ToInt32(buffer, startIndex);
        }

        public static short GetShort(ref byte[] buffer, int startIndex)
        {
            return BitConverter.ToInt16(buffer, startIndex);
        }

        public static ulong ToULong(int value)
        {
            return (ulong)value & 0xFFFFFFFF;
        }

        public static int ReadD3(ref byte[] buffer, int offset)
        {
            int value = buffer[offset];
            value |= buffer[offset + 1] << 8;
            value |= buffer[offset + 2] << 16;
            return value;
        }
    }
}
