using Auth.Emu.Enum;
using Core.Extension;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.SEND
{
    [Message(MessageIds.SMGetWorldInformations, true)]
    public class SMGetWorldInformations : IMessage
    {
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(TimestampUtil.UnixTimestamp());
            writer.Write((byte)1);
            writer.Write(new byte[10]);
            writer.Write((short)1);
            for (int i = 0; i < 1; ++i)
            {
                writer.Write((short)1);
                writer.Write((short)1);
                writer.Write((short)1);
                writer.Write((short)0);
                writer.Write((short)4);
                writer.WriteString("Olvia", 62);
                writer.WriteString("Black Desert", 62);
                writer.Write((byte)0);
                writer.WriteStringAscii("127.0.0.1", 62);
                writer.Write(new byte[39]);
                writer.Write((short)8889);
                writer.Write((byte)2);
                writer.Write((byte)1);
                writer.Write((byte)1);
                writer.Write((byte)1);
                writer.Write((byte)0);
                writer.Write((short)0);
                writer.Write((long)0);
                writer.Write(TimestampUtil.UnixTimestamp());
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((short)0);
                writer.Write(2000000);
                writer.WriteString("", 62);
                writer.Write(new byte[] {
                    0x40, 0x42, 0x0F, 0x00, 0x40, 0x42, 0x0F, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x03
            });
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}