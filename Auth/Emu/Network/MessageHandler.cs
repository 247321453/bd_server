using System.IO;

namespace Auth.Emu.Network
{
    public abstract class MessageHandler<TMessageType> : IMessageHandler
        where TMessageType : IMessage, new()
    {
        public bool Handle(ClientConnection connection, BinaryReader reader)
        {
            // TODO Track session state.

            var messageInstance = new TMessageType();
            messageInstance.Deserialize(reader);

            return Process(connection, messageInstance);
        }

        protected abstract bool Process(ClientConnection connection, TMessageType message);
    }
}