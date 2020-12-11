using System.IO;
using System.Text;

namespace Core.Extension
{
    public static class BinaryWriterExtensions
    {
        public static void WriteString(this BinaryWriter writer, string value, int length)
        {
            // TODO Improve for overflow -1 (strings are null padded).

            var bytesOfString = Encoding.Unicode.GetBytes(value);

            writer.Write(bytesOfString);

            if (bytesOfString.Length < length)
            {
                writer.Write(new byte[length - bytesOfString.Length]);
            }

            // BinaryWriter
        }

        public static void WriteStringAscii(this BinaryWriter writer, string value, int length)
        {
            // TODO Improve for overflow -1 (strings are null padded).

            var bytesOfString = Encoding.ASCII.GetBytes(value);

            writer.Write(bytesOfString);

            if (bytesOfString.Length < length)
            {
                writer.Write(new byte[length - bytesOfString.Length]);
            }

            // BinaryWriter
        }
    }
}