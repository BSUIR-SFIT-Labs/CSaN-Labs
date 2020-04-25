using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FingerLib
{
    /// <summary>
    /// Allows to work with client names.
    /// </summary>
    [Serializable]
    public class ClientNames
    {
        public List<string> Names { get; private set; }

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        public ClientNames()
        {
            Names = new List<string>();
        }

        /// <summary>
        /// Returns the specified name from the list of names.
        /// </summary>
        /// <param name="name">The name to search.</param>
        /// <returns>Client name.</returns>
        public string GetUsernameByName(string name)
        {
            foreach (var clientName in Names)
            {
                if (clientName == name)
                {
                    return clientName;
                }
            }

            return null;
        }

        /// <summary>
        /// Serializes data.
        /// </summary>
        /// <param name="clientNames">A class containing customer names.</param>
        /// <returns>Array of bytes.</returns>
        public static byte[] Serialize(ClientNames clientNames)
        {
            byte[] serializedClientNames;

            using (var memoryStream = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(typeof(ClientNames));
                xmlSerializer.Serialize(memoryStream, clientNames);

                memoryStream.Position = 0;
                serializedClientNames = new byte[memoryStream.Length];

                const int memoryStreamOffset = 0;
                memoryStream.Read(serializedClientNames, memoryStreamOffset,
                    serializedClientNames.Length);
            }

            return serializedClientNames;
        }

        /// <summary>
        /// Deserializes data.
        /// </summary>
        /// <param name="byteArrayClientNames">Array of bytes.</param>
        /// <returns>A class containing customer names.</returns>
        public static ClientNames Deserialize(byte[] byteArrayClientNames)
        {
            using var memoryStream = new MemoryStream();
            const int memoryStreamOffset = 0;

            memoryStream.Write(byteArrayClientNames, memoryStreamOffset,
                byteArrayClientNames.Length);
            memoryStream.Position = 0;

            var xmlSerializer = new XmlSerializer(typeof(ClientNames));
            return (ClientNames)xmlSerializer.Deserialize(memoryStream);
        }
    }
}
