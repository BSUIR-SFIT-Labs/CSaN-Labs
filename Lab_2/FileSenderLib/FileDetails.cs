using System;

namespace FileSenderLib
{
    [Serializable]
    public class FileDetails
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
    }
}