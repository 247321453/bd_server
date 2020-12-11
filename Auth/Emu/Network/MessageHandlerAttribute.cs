using Auth.Emu.Enum;
using System;

namespace Auth.Emu.Network
{
    public class MessageHandlerAttribute : Attribute
    {
        public MessageIds MessageId { get; }

        public MessageHandlerAttribute(MessageIds messageId)
        {
            MessageId = messageId;
        }
    }
}
