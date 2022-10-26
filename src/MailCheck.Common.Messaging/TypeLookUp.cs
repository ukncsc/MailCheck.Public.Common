using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Messaging
{
    public interface ITypeLookUp
    {
        bool TryGetMessageType(string typeName, out Type type);
    }

    internal sealed class TypeLookUp : ITypeLookUp
    {
        private static readonly Type MessageType = typeof(Message);
        private static readonly IDictionary<string, Type> MessageTypesByName = ScanForMessageTypes();

        public bool TryGetMessageType(string typeName, out Type type)
        {
            return MessageTypesByName.TryGetValue(typeName, out type);
        }

        private static Dictionary<string, Type> ScanForMessageTypes()
        {
            var allConcreteMessageTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => MessageType.IsAssignableFrom(type) && !type.IsAbstract)
                .ToArray();

            var duplicateMessageNames = allConcreteMessageTypes
                .GroupBy(type => type.Name)
                .Where(typeGroup => typeGroup.Count() > 1)
                .Select(typeGroup => typeGroup.Key)
                .ToArray();

            if (duplicateMessageNames.Length > 0)
            {
                throw new Exception($"The following message type names are not unique in the loaded asemblies: {string.Join(", ", duplicateMessageNames)}");
            }

            return allConcreteMessageTypes.ToDictionary(messageType => messageType.Name);
        }
    }
}
