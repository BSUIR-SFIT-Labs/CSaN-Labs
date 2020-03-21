using System;
using System.IO;
using System.Xml.Serialization;

namespace FileSenderLib.TFile
{
    /// <summary>
    /// Allows to work with file details.
    /// </summary>
    [Serializable]
    public class FileDetails
    {
        public string Name { get; set; }
        public string Extension { get; set; }

        /// <summary>
        /// Converts file details to an array of bytes.
        /// </summary>
        /// <param name="fileDetails">File details.</param>
        /// <returns>Converted file details to byte array.</returns>
        public static byte[] ConvertToByteArray(FileDetails fileDetails)
        {
            byte[] serializedFileDetails;

            using (var memoryStream = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(typeof(FileDetails));
                xmlSerializer.Serialize(memoryStream, fileDetails);

                memoryStream.Position = 0;
                serializedFileDetails = new byte[memoryStream.Length];

                const int memoryStreamOffset = 0;
                memoryStream.Read(serializedFileDetails, memoryStreamOffset,
                    serializedFileDetails.Length);
            }

            return serializedFileDetails;
        }

        /// <summary>
        /// Converts a byte array containing file details to file details.
        /// </summary>
        /// <param name="byteArrayFileDetails">File details represented as an array of bytes.</param>
        /// <returns>File details</returns>
        public static FileDetails ConvertToFileDetails(byte[] byteArrayFileDetails)
        {
            using (var memoryStream = new MemoryStream())
            {
                const int memoryStreamOffset = 0;

                memoryStream.Write(byteArrayFileDetails, memoryStreamOffset,
                    byteArrayFileDetails.Length);
                memoryStream.Position = 0;

                var xmlSerializer = new XmlSerializer(typeof(FileDetails));
                return (FileDetails)xmlSerializer.Deserialize(memoryStream);
            }
        }
    }
}
