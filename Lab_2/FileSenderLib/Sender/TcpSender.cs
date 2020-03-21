using FileSenderLib.TFile;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FileSenderLib.Sender
{
    /// <summary>
    /// Allows to send files over the network using TCP.
    /// </summary>
    public class TcpSender
    {
        private const int BufferSize = 4096;

        private readonly ManualResetEvent _connectDone;
        private readonly ManualResetEvent _sendDone;

        private Socket _sender;

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        public TcpSender()
        {
            _connectDone = new ManualResetEvent(false);
            _sendDone = new ManualResetEvent(false);
        }

        /// <summary>
        /// Establishes a connection at a specific IP address and port.
        /// </summary>
        /// <param name="remoteIPString">Remote IP Address.</param>
        /// <param name="remotePort">Remote port.</param>
        public void Start(string remoteIPString, int remotePort)
        {
            try
            {
                var remoteIPAddress = IPAddress.Parse(remoteIPString);
                var remoteEndPoint = new IPEndPoint(remoteIPAddress, remotePort);

                _sender = new Socket(remoteIPAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                _sender.BeginConnect(remoteEndPoint,
                    new AsyncCallback(ConnectCallback), _sender);

                _connectDone.WaitOne();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Finishes the connection.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void ConnectCallback(IAsyncResult asyncResult)
        {
            try
            {
                var sender = (Socket)asyncResult.AsyncState;

                sender.EndConnect(asyncResult);

                _connectDone.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends a file over the network.
        /// </summary>
        /// <param name="pathToFile">The path to the file to be transferred over the network.</param>
        public void SendFile(string pathToFile)
        {
            var transmittedFile = new TransmittedFile(pathToFile);

            SendFileDetails(transmittedFile);

            byte[] fileContentBuffer = new byte[BufferSize];

            long fileContentLength = transmittedFile.GetFileContentLength();
            long fileContentByteSent = 0;

            while (fileContentByteSent != fileContentLength)
            {
                int numberOfBytesToSend = ((fileContentLength - fileContentByteSent) / BufferSize > 0) ?
                    BufferSize : (int)(fileContentLength - fileContentByteSent);

                const int bufferOffset = 0;
                transmittedFile.ReadBytes(fileContentBuffer, bufferOffset,
                    numberOfBytesToSend, fileContentByteSent);

                _sender.BeginSend(fileContentBuffer, bufferOffset,
                    fileContentBuffer.Length, SocketFlags.None,
                    new AsyncCallback(SendFileCallback), _sender);

                fileContentByteSent += numberOfBytesToSend;
                Array.Clear(fileContentBuffer, bufferOffset, fileContentBuffer.Length);
            }
        }

        /// <summary>
        /// Sends a file details over the network.
        /// </summary>
        /// <param name="transmittedFile">The file to be transferred over the network.</param>
        private void SendFileDetails(TransmittedFile transmittedFile)
        {
            byte[] fileDetails = transmittedFile.GetByteArrayFileDetails();
            byte[] fileDetailsLength = BitConverter.GetBytes((long)fileDetails.Length);
            byte[] fileContentLength = BitConverter.GetBytes(transmittedFile.GetFileContentLength());

            byte[] fileDetailsBuffer = new byte[fileDetailsLength.Length
                + fileDetails.Length + fileContentLength.Length];

            const int stertIndex = 0;
            fileDetailsLength.CopyTo(fileDetailsBuffer, stertIndex);
            fileDetails.CopyTo(fileDetailsBuffer, fileDetailsLength.Length);
            fileContentLength.CopyTo(fileDetailsBuffer,
                fileDetailsLength.Length + fileDetails.Length);

            const int offset = 0;
            _sender.BeginSend(fileDetailsBuffer, offset,
                fileDetailsBuffer.Length, SocketFlags.None,
                new AsyncCallback(SendFileCallback), _sender);
        }

        /// <summary>
        /// Finishes sending a file over the network.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void SendFileCallback(IAsyncResult asyncResult)
        {
            try
            {
                var sender = (Socket)asyncResult.AsyncState;

                int bytesSent = sender.EndSend(asyncResult);

                _sendDone.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Stop()
        {
            _sender.Shutdown(SocketShutdown.Both);
            _sender.Close();
        }
    }
}
