
using Core.Crypt;
using Core.Generics;
using Core.Utils;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Auth.Emu.Network
{
    public class ClientConnection
    {
        private const ushort InHeaderSize = 3;
        private const ushort OutHeaderSize = 3;

        private readonly Socket _socket;
        private bool _disconnected;

        private readonly byte[] _receiveBuffer = new byte[0x4000]; // 16K
        public BDOCrypt BDOCrypt { get; }

        public ClientConnection(Socket socket)
        {
            _socket = socket;
            _socket.NoDelay = true;
            BDOCrypt = new BDOCrypt(0);

            BeginReceive();
        }

        private void BeginReceive()
        {
            if (_disconnected)
                return;

            _socket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, null);
        }

        private void Receive(IAsyncResult ar)
        {
            var receivedBytes = 0;

            try
            {
                receivedBytes = _socket.EndReceive(ar);
            }
            catch (Exception ex)
            {
                Disconnect($"Receive Exception: {ex}");
            }

            // Check if the socket was closed.
            if (receivedBytes == 0)
            {
                Disconnect();
                return;
            }

            // Check the message header.
            if (receivedBytes < InHeaderSize)
            {
                Disconnect("The received packets header size is incorrect.");
                return;
            }

            // TODO Properly Process incomming bytes.
            using (var stream = new MemoryStream(_receiveBuffer, 0, receivedBytes))
            using (var reader = new BinaryReader(stream))
            {
                var size = reader.ReadUInt16();
                var encrypted = reader.ReadByte() == 1;

                var body = reader.ReadBytes(size - InHeaderSize);

                if (encrypted)
                {
                    body = BDOCrypt.Xor(body, 0, body.Length);
                }

                using (var messageStream = new MemoryStream(body))
                using (var message = new BinaryReader(messageStream))
                {
                    var messageId = message.ReadUInt16(); // MessageId

                    Logging.Server($"Received(Size:{size}, Encrypted: {encrypted}, MessageId: {messageId}, 0x{messageId:X2})");
                    //Logging.Server(BufferExtensions.ToHex(body.Skip(2).ToArray()));

                    var handler = MessageTable.FindMessageHandler(messageId);

                    if (handler != null)
                    {
                        handler.Handle(this, message);
                    }
                    else
                    {
                        Logging.Server($"Unknown Message Received: Size:{size}, Encrypted: {encrypted}, MessageId: {messageId} (0x{messageId:X2})");
                        Logging.Server(BufferExtensions.ToHex(body.Skip(2).ToArray()));
                    }
                }
            }

            // Start receiving more data.
            BeginReceive();
        }

        public void SendBody(ushort opCode, byte[] body, bool encrypted = true)
        {
            byte[] outputBuffer;

            using (var memoryStream = new MemoryStream())
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write(opCode);
                writer.Write(body);
                writer.Flush();

                outputBuffer = memoryStream.ToArray();
            }

            if (encrypted)
            {
                outputBuffer = BDOCrypt.Xor(outputBuffer);
            }

            var messageSize = outputBuffer.Length + OutHeaderSize;

            Logging.Server($"Send(Size:{body.Length}, Encrypted: {encrypted}, MessageId: {opCode})");

            using (var memoryStream = new MemoryStream())
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write((ushort)messageSize); // Message size;
                writer.Write((byte)(encrypted ? 1 : 0)); // IsEncrypted -> 0 = false, 1 = true
                writer.Write(outputBuffer);
                writer.Flush();
                Send(memoryStream.ToArray());
            }
        }

        public void Send(IMessage message)
        {
            var messageData = MessageTable.FindMessageData(message);

            if (messageData == null)
                throw new Exception($"Could not find message attribute for {nameof(message)}");

            byte[] bodyBuffer;
            var encrypted = messageData.Encrypted;

            using (var bodyStream = new MemoryStream())
            using (var bodyWriter = new BinaryWriter(bodyStream))
            {
                bodyWriter.Write((ushort)messageData.MessageId); // MessageId
                message.Serialize(bodyWriter); // Body
                bodyWriter.Flush();

                bodyBuffer = bodyStream.ToArray();
            }

            var messageId = (ushort)messageData.MessageId;

            Logging.Server($"Send(Size:{bodyBuffer.Length}, Encrypted: {encrypted}, MessageId: {messageData.MessageId}, {messageId}, 0x{messageId:X2})");
            //Logging.Server(BufferExtensions.ToHex(bodyBuffer.Skip(2).ToArray()));

            if (encrypted)
            {
                bodyBuffer = BDOCrypt.Xor(bodyBuffer);
            }

            using (var sendBuffer = new MemoryStream())
            using (var messageBuilder = new BinaryWriter(sendBuffer))
            {
                // Build the message.
                messageBuilder.Write((ushort)(bodyBuffer.Length + OutHeaderSize));
                messageBuilder.Write((byte)(encrypted ? 1 : 0)); // Encrypted
                messageBuilder.Write(bodyBuffer);
                messageBuilder.Flush();

                // Send data
                var messageBuffer = sendBuffer.ToArray();
                Send(messageBuffer);
            }
        }
        private void Send(byte[] buffer)
        {
            try
            {
                _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
                Logging.Server($"Send: {buffer.Length} bytes");
            }
            catch (Exception ex)
            {
                Disconnect($"Send Exception: {ex}");
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndSend(ar); // Returns sent bytes.
            }
            catch (Exception ex)
            {
                Disconnect($"Send Exception: {ex}");
            }
        }

        public void Disconnect(string reason = "disconnected")
        {
            if (_disconnected)
                return;

            _disconnected = true;

            Logging.UserB($"Connection disconnected from {_socket.RemoteEndPoint} Reason: {reason}");

            Disconnected?.Invoke(this, new DisconnectedEventArgs(reason));

            try
            {
                _socket.Close(0);
                _socket.Dispose();
            }
            catch (Exception)
            {
                // Do nothing.
            }
        }

        public EventHandler<DisconnectedEventArgs> Disconnected;
    }
}