using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Auth.Emu.Network
{
    public static class MessageTable
    {
        private static readonly Dictionary<Type, MessageAttribute> MessageIdLookUp;
        private static readonly Dictionary<ushort, IMessageHandler> MessageHandlersLookUp;
        static MessageTable()
        {
            MessageIdLookUp = new Dictionary<Type, MessageAttribute>();
            MessageHandlersLookUp = new Dictionary<ushort, IMessageHandler>();
        }

        public static void ScanAssembly()
        {
            var messageType = typeof(IMessage);

            var messageTypes = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => messageType.IsAssignableFrom(t) &&
                            t.GetTypeInfo().GetCustomAttribute<MessageAttribute>() != null)
                .ToList();


            //messageTypes.AddRange(Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .Where(t => messageType.IsAssignableFrom(t) &&
            //                t.GetTypeInfo().GetCustomAttribute<MessageAttribute>() != null)
            //    .ToList());

            foreach (var type in messageTypes)
            {
                var messageAttribute = type.GetCustomAttribute<MessageAttribute>();

                MessageIdLookUp.Add(type, messageAttribute);
            }

            var handlerType = typeof(IMessageHandler);

            // Scan the message handlers.
            var messageHandlers = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => handlerType.IsAssignableFrom(t) && t.GetCustomAttribute<MessageHandlerAttribute>() != null)
                .ToList();

            foreach (var type in messageHandlers)
            {
                var handlerAttribute = type.GetCustomAttribute<MessageHandlerAttribute>();

                if (Activator.CreateInstance(type) is IMessageHandler messageHandlerInstance)
                {
                    MessageHandlersLookUp.Add((ushort)handlerAttribute.MessageId, messageHandlerInstance);
                }
            }

            Logging.Info(
                $"Found {MessageIdLookUp.Count} Messages; {MessageHandlersLookUp.Count} Message Handlers");
        }

        public static IMessageHandler FindMessageHandler(ushort message)
        {
            return MessageHandlersLookUp.TryGetValue(message, out var result)
                ? result
                : null;
        }

        public static MessageAttribute FindMessageData(IMessage message)
        {
            return MessageIdLookUp.TryGetValue(message.GetType(), out var result)
                ? result
                : null;
        }
    }
}