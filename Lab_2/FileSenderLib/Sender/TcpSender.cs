using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FileSenderLib.Sender
{
    public class TcpSender
    {
        private readonly ManualResetEvent _connectDone;
        private readonly ManualResetEvent _sendDone;

        private Socket _sender;

        public TcpSender()
        {
            _connectDone = new ManualResetEvent(false);
            _sendDone = new ManualResetEvent(false);
        }

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

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            var sender = (Socket)asyncResult.AsyncState;

            sender.EndConnect(asyncResult);

            // Client connected!

            _connectDone.Set();
        }

        public void SendFile(string pathToFile)
        {
            var transmittedFile = new TransmittedFile(pathToFile);
            var serializedFileDetails = transmittedFile.SerializeFileDetails();

            _sender.BeginSend(serializedFileDetails,
                0, serializedFileDetails.Length,
                0, new AsyncCallback(SendCallback), _sender);
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                var sender = (Socket)asyncResult.AsyncState;

                int bytesSent = sender.EndSend(asyncResult);

                // Send

                _sendDone.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Stop()
        {
            _sender.Shutdown(SocketShutdown.Both);
            _sender.Close();
        }
    }
}