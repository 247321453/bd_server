namespace Core.Extension
{
    public static class BitUtils
    {
        public static int LODWORD(ulong value)
        {
            return (int)value;
        }

        public static int HIDWORD(ulong value)
        {
            return (int)(value >> 32);
        }

        public static ulong LODWORD(ulong value, int i)
        {
            return (value & 0xFFFFFFFF00000000L) | (ulong)(i & 0x00000000FFFFFFFFL);
        }

        public static int RotateIntLeft(int value, int bits)
        {
            var result = ((uint)value << bits) | ((uint)value >> (32 - bits));
            return (int)result;
        }

        public static int RotateIntRight(int value, int bits)
        {
            var result = ((uint)value >> bits) | ((uint)value << (32 - bits));
            return (int)result;
        }

        public static ulong HIDWORD(ulong value, int i)
        {
            return (value & 0x00000000FFFFFFFFL) | ((((ulong)i) << 32) & 0xFFFFFFFF00000000L);
        }
    }
}