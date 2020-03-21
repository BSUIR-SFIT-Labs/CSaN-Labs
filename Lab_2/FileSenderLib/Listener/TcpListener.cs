using FileSenderLib.TFile;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FileSenderLib.Listener
{
    /// <summary>
    /// Allows to receive files over the network using TCP.
    /// </summary>
    public class TcpListener
    {
        private const int MaxConnections = 100;

        private readonly ManualResetEvent _allDone;

        private readonly string _saveFolderPath;
        private TransmittedFile _transmittedFile;

        private Socket _listener;

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        public TcpListener(string saveFolderPath)
        {
            _allDone = new ManualResetEvent(false);
            _saveFolderPath = saveFolderPath;
        }

        /// <summary>
        /// Starts listening on a port on a specific IP address.
        /// </summary>
        /// <param name="ipString">IP address.</param>
        /// <param name="port">Port.</param>
        public void Start(string ipString, int port)
        {
            try
            {
                var ipAddress = IPAddress.Parse(ipString);
                var endpoint = new IPEndPoint(ipAddress, port);

                _listener = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
            
                _listener.Bind(endpoint);
                _listener.Listen(MaxConnections);

                while (true)
                {
                    _allDone.Reset();

                    _listener.BeginAccept(new AsyncCallback(AcceptConnection),
                        _listener);

                    _allDone.WaitOne();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Accepts connections. Starts receiving a file.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void AcceptConnection(IAsyncResult asyncResult)
        {
            _allDone.Set();

            var listener = (Socket)asyncResult.AsyncState;
            var handler = listener.EndAccept(asyncResult);

            var listenerState = new ListenerState
            {
                WorkSocket = handler
            };

            const int bufferOffset = 0;
            handler.BeginReceive(listenerState.Buffer, bufferOffset,
                ListenerState.BufferSize, SocketFlags.None,
                new AsyncCallback(EndReceive), listenerState);
        }

        /// <summary>
        /// Finishes receiving a file over the network.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void EndReceive(IAsyncResult asyncResult)
        {
            var listenerState = (ListenerState)asyncResult.AsyncState;
            var handler = listenerState.WorkSocket;

            int bytesRead = handler.EndReceive(asyncResult);

            if (bytesRead > 0)
            {
                if (listenerState.IsFirstBlockReceived)
                {   
                    _transmittedFile = ReceiveFileDetails(listenerState);

                    Array.Clear(listenerState.Buffer, 0, listenerState.Buffer.Length);
                    listenerState.IsFirstBlockReceived = false;
                }
                else
                {
                    int numberOfBytesToWrite = ((listenerState.NeedToReceivedBytes - listenerState.ByteReceived)
                        / ListenerState.BufferSize > 0) ? ListenerState.BufferSize
                        : (int)(listenerState.NeedToReceivedBytes - listenerState.ByteReceived);

                    const int bufferOffset = 0;
                    _transmittedFile.WriteBytes(_saveFolderPath, listenerState.Buffer,
                        bufferOffset, numberOfBytesToWrite);

                    const int startIndex = 0;
                    Array.Clear(listenerState.Buffer, startIndex, listenerState.Buffer.Length);
                    listenerState.ByteReceived += numberOfBytesToWrite;
                }
                
                if (listenerState.NeedToReceivedBytes != listenerState.ByteReceived)
                {
                    const int bufferOffset = 0;
                    handler.BeginReceive(listenerState.Buffer, bufferOffset,
                        ListenerState.BufferSize, SocketFlags.None,
                        new AsyncCallback(EndReceive), listenerState);
                }
            }
        }

        /// <summary>
        /// Receives file details.
        /// </summary>
        /// <param name="listenerState">TCP listener status.</param>
        /// <returns>Transmitted file, with installed file details.</returns>
        private TransmittedFile ReceiveFileDetails(ListenerState listenerState)
        {
            const int reservedСellSize = 8;
            const int startIndex = 0;

            long fileDetailsLength = BitConverter.ToInt64(listenerState.Buffer
                .Take(reservedСellSize).ToArray(), startIndex);

            byte[] fileDetails = listenerState.Buffer
                .Skip(reservedСellSize).Take((int)fileDetailsLength).ToArray();

            listenerState.NeedToReceivedBytes = BitConverter.ToInt64(listenerState.Buffer
                .Skip(reservedСellSize + fileDetails.Length).ToArray(), startIndex);

            return new TransmittedFile(fileDetails);
        }

        /// <summary>
        /// Stops listening on a port on a specific IP address.
        /// </summary>
        public void Stop()
        {
            try
            {
                _listener.Shutdown(SocketShutdown.Both);
                _listener.Close();
            }
            catch
            {
                throw;
            }
        }
    }
}
