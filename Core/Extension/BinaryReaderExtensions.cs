using System.IO;
using System.Text;

namespace Core.Extension
{
    public static class BinaryReaderExtensions
    {
        public static string ReadUnicodeString(this BinaryReader reader, int length)
        {
            var buffer = reader.ReadBytes(length);

            return Encoding.Unicode.GetString(buffer).TrimEnd('\0');
        }

        public static string ReadStringL(this BinaryReader reader, int length)
        {
            var buffer = reader.ReadBytes(length);

            return Encoding.UTF8.GetString(buffer);
        }
    }
}
