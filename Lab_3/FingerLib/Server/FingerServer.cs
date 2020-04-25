using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FingerLib.Server
{
    /// <summary>
    /// Finger server.
    /// </summary>
    public class FingerServer
    {
        private const int MaxConnections = 100;
        private const int FingerPort = 79;

        private readonly ManualResetEvent _allDone;

        private readonly ClientNames _clientNames;

        /// <summary>
        /// Initializes class fields.
        /// </summary>
        public FingerServer()
        {
            _allDone = new ManualResetEvent(false);
            _clientNames = new ClientNames();
        }

        /// <summary>
        /// Starts listening on port 79 at the specified IP address.
        /// </summary>
        /// <param name="ipString">IP address.</param>
        public void Start(string ipString)
        {
            try
            {
                var ipAddress = IPAddress.Parse(ipString);
                var endpoint = new IPEndPoint(ipAddress, FingerPort);

                var server = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                server.Bind(endpoint);
                server.Listen(MaxConnections);

                while (true)
                {
                    _allDone.Reset();

                    server.BeginAccept(
                        new AsyncCallback(AcceptConnection),
                        server);

                    _allDone.WaitOne();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Accepts connections. Starts to receive data from the client.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void AcceptConnection(IAsyncResult asyncResult)
        {
            _allDone.Set();

            var server = (Socket)asyncResult.AsyncState;
            var handler = server.EndAccept(asyncResult);

            var state = new ServerState
            {
                WorkSocket = handler
            };

            const int offset = 0;
            handler.BeginReceive(state.Buffer, offset,
                ServerState.BufferSize, SocketFlags.None,
                new AsyncCallback(FinishReceiving), state);
        }

        /// <summary>
        /// Finishes accepting data from the client.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void FinishReceiving(IAsyncResult asyncResult)
        {
            var state = (ServerState)asyncResult.AsyncState;
            var handler = state.WorkSocket;

            int bytesRead = handler.EndReceive(asyncResult);

            if (bytesRead > 0)
            {
                (string pcName, string query) = ParseReceivedData(state.Buffer, bytesRead);

                state.PcName = pcName;

                byte[] dataToSend = ParseQuery(query);

                Send(state, dataToSend);
            }
        }

        /// <summary>
        /// Starts sending data to the client.
        /// </summary>
        /// <param name="handler">Client socket</param>
        /// <param name="data">Data to send to the client.</param>
        private void Send(ServerState state, byte[] data)
        {
            var handler = state.WorkSocket;

            handler.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(FinishSending), state);
        }

        /// <summary>
        /// Finishes sending data to the client.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void FinishSending(IAsyncResult asyncResult)
        {
            try
            {
                var state = (ServerState)asyncResult.AsyncState;
                var handler = state.WorkSocket;

                int bytesSent = handler.EndSend(asyncResult);

                //_clientNames.Names.Remove(state.PcName);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Parses the received data.
        /// </summary>
        /// <param name="data">Data received from the client.</param>
        /// <param name="count">The number of bytes received.</param>
        /// <returns>PC name and query string.</returns>
        private (string, string) ParseReceivedData(byte[] data, int count)
        {
            string receivedData = Encoding.ASCII.GetString(data, 0, count);

            string[] dataArray = receivedData.Split("\n");

            _clientNames.Names.Add(dataArray[0]);

            return (dataArray[0], dataArray[1]);
        }

        /// <summary>
        /// Parses the received request from the client and determines what data to send to the client in response.
        /// </summary>
        /// <param name="query">Request string received from the client.</param>
        /// <returns>Data to send to the client.</returns>
        private byte[] ParseQuery(string query)
        {
            string[] partsOfQuery = query.Split(" ");

            if (partsOfQuery.Length > 1)
            {
                var resultClientNames = new ClientNames();
                resultClientNames.Names.Add(_clientNames.GetUsernameByName(partsOfQuery[1]));

                return ClientNames.Serialize(resultClientNames);
            }

            return ClientNames.Serialize(_clientNames);
        }   
    }
}
