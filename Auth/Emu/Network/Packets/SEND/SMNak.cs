using Auth.Emu.Enum;
using Core.Enums;
using Core.Extension;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.SEND
{
    [Message(MessageIds.SMNak)]
    public class SMNak : IMessage
    {
        private ErrorStrings _errorString = ErrorStrings.NONE;
        private uint _messageHash;

        public ErrorStrings ErrorString
        {
            get => _errorString;
            set
            {
                _errorString = value;
                _messageHash = HashUtil.GetErrorHash(_errorString);
            }
        }

        public MessageIds MessageId { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((uint)_messageHash); // 971622511 -> eErrNoLoginFaildTypeBlocked
            writer.Write((ushort)MessageId);
            writer.Write((ulong)0);
        }

        public void Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}