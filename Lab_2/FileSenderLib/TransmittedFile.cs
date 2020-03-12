using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FileSenderLib
{
    public class TransmittedFile
    {
        private const int Offset = 0;

        private FileDetails _fileDetails;

        private TransmittedFile()
        {
            _fileDetails = new FileDetails();
        }

        public TransmittedFile(string pathToFile) : this()
        {
            using (var fileStream = new FileStream(pathToFile,
                FileMode.Open, FileAccess.Read))
            {
                _fileDetails.Name = Path.GetFileNameWithoutExtension(fileStream.Name);
                _fileDetails.Extension = Path.GetExtension(fileStream.Name);
                _fileDetails.Content = new byte[fileStream.Length];

                fileStream.Read(_fileDetails.Content, Offset,
                    _fileDetails.Content.Length);
            }
        }

        public TransmittedFile(byte[] fileDetails) : this()
        {
            _fileDetails = DeserializeFileDetails(fileDetails);
        }

        public void Create(string pathToFolder)
        {
            using (var fileStream = new FileStream(pathToFolder + _fileDetails.Name 
                + _fileDetails.Extension, FileMode.Create, 
                FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fileStream.Write(_fileDetails.Content, Offset,
                    _fileDetails.Content.Length);
            }
        }

        public byte[] SerializeFileDetails()
        {
            byte[] serializedFileDetails;

            using (var memoryStream = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(typeof(FileDetails));
                xmlSerializer.Serialize(memoryStream, _fileDetails);

                memoryStream.Position = 0;
                serializedFileDetails = new byte[memoryStream.Length];
                memoryStream.Read(serializedFileDetails, Offset,
                    serializedFileDetails.Length);
            }

            return serializedFileDetails;
        }

        private FileDetails DeserializeFileDetails(byte[] serializedFileDetails)
        {
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(serializedFileDetails, Offset,
                    serializedFileDetails.Length);
                memoryStream.Position = 0;

                var xmlSerializer = new XmlSerializer(typeof(FileDetails));
                return (FileDetails)xmlSerializer.Deserialize(memoryStream);
            }
        }
    }
}
