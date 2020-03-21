using System.IO;

namespace FileSenderLib.TFile
{
    /// <summary>
    /// Allows to work with a file sent over the network.
    /// </summary>
    public class TransmittedFile
    {
        private readonly FileDetails _fileDetails;
        private readonly string _pathToFile;

        /// <summary>
        /// Initializes a file details information field.
        /// </summary>
        private TransmittedFile()
        {
            _fileDetails = new FileDetails();
        }

        /// <summary>
        /// Sets the path to the file sent over the network and file details.
        /// </summary>
        /// <param name="pathToFile">The path to the file to be sent over the network.</param>
        public TransmittedFile(string pathToFile) : this()
        {
            _pathToFile = pathToFile;


            var fileInfo = new FileInfo(pathToFile);
            _fileDetails.Name = Path.GetFileNameWithoutExtension(fileInfo.Name);
            _fileDetails.Extension = Path.GetExtension(fileInfo.Name);
        }

        /// <summary>
        /// Sets file details.
        /// </summary>
        /// <param name="byteArrayFileDetails">File details represented as an array of bytes.</param>
        public TransmittedFile(byte[] byteArrayFileDetails) : this()
        {
            _fileDetails = FileDetails.ConvertToFileDetails(byteArrayFileDetails);
        }

        /// <summary>
        /// Returns file details as an array of bytes.
        /// </summary>
        /// <returns>file details represented as an array of bytes.</returns>
        public byte[] GetByteArrayFileDetails()
        {
            return FileDetails.ConvertToByteArray(_fileDetails);
        }

        /// <summary>
        /// Returns the length of the file content.
        /// </summary>
        /// <returns>The length of the file content.</returns>
        public long GetFileContentLength()
        {
            var fileInfo = new FileInfo(_pathToFile);
            return fileInfo.Length;
        }

        /// <summary>
        /// Reads a file bytes into the buffer.
        /// </summary>
        /// <param name="buffer">Buffer for reading a file.</param>
        /// <param name="bufferOffset">The offset from the beginning of the buffer.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <param name="fileOffset">Offset from the beginning of the file.</param>
        public void ReadBytes(byte[] buffer, int bufferOffset, int count, long fileOffset)
        {
            using (var fileStream = new FileStream(_pathToFile,
                FileMode.Open, FileAccess.Read))
            {
                fileStream.Seek(fileOffset, SeekOrigin.Begin);

                fileStream.Read(buffer, bufferOffset, count);
            }
        }

        /// <summary>
        /// Writes bytes from the buffer to the file.
        /// </summary>
        /// <param name="pathToFolder">The path to the folder with the file.</param>
        /// <param name="content">An array of bytes to write to the file.</param>
        /// <param name="bufferOffset">The offset from the beginning of the buffer.</param>
        /// <param name="count">The number of bytes to write.</param>
        public void WriteBytes(string pathToFolder, byte[] content, int bufferOffset, int count)
        {
            using (var fileStream = new FileStream(pathToFolder + _fileDetails.Name
                + _fileDetails.Extension, FileMode.Append,
                FileAccess.Write, FileShare.Write))
            {
                fileStream.Write(content, bufferOffset, count);
            }
        }
    }
}
