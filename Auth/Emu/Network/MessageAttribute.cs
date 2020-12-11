using Auth.Emu.Enum;
using System;

namespace Auth.Emu.Network
{
    public class MessageAttribute : Attribute
    {
        public MessageIds MessageId { get; }
        public bool Encrypted { get; }

        public MessageAttribute(MessageIds messageId, bool encrypted = true)
        {
            MessageId = messageId;
            Encrypted = encrypted;
        }
    }
}