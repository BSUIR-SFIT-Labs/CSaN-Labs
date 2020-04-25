using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FingerLib.Client
{
    /// <summary>
    /// Finger client.
    /// </summary>
    public class FingerClient
    {
        private const int FingerPort = 79;

        private readonly ManualResetEvent _connectDone;
        private readonly ManualResetEvent _sendDone;
        private static ManualResetEvent _receiveDone;

        public List<string> ServerResponse { get; private set; }

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        public FingerClient()
        {
            _connectDone = new ManualResetEvent(false);
            _sendDone = new ManualResetEvent(false);
            _receiveDone = new ManualResetEvent(false);
        }

        /// <summary>
        /// Communicates with the Finger server.
        /// </summary>
        /// <param name="remoteIPString">IP address of the server.</param>
        public void SendQuery(string remoteIPString, string query)
        {
            try
            {
                var remoteIPAddress = IPAddress.Parse(remoteIPString);
                var remoteEndPoint = new IPEndPoint(remoteIPAddress, FingerPort);

                var client = new Socket(remoteIPAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEndPoint,
                    new AsyncCallback(ConnectCallback), client);

                _connectDone.WaitOne();

                SendData(client, query);
                _sendDone.WaitOne();
 
                Receive(client);
                _receiveDone.WaitOne();

                client.Shutdown(SocketShutdown.Both);
                client.Close();
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
                var client = (Socket)asyncResult.AsyncState;

                client.EndConnect(asyncResult);

                _connectDone.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Starts sending data to the server.
        /// </summary>
        /// <param name="client">Client socket.</param>
        /// <param name="query">Query string.</param>
        private void SendData(Socket client, string query)
        {
            string pcName = Environment.MachineName + "\n";

            byte[] dataToSend = Encoding.ASCII.GetBytes(pcName + query);

            const int offset = 0;
            client.BeginSend(dataToSend, offset,
                dataToSend.Length, SocketFlags.None,
                new AsyncCallback(FinishSending), client);
        }

        /// <summary>
        /// Finishes sending data to the server.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void FinishSending(IAsyncResult asyncResult)
        {
            try
            {
                var client = (Socket)asyncResult.AsyncState;

                int bytesSent = client.EndSend(asyncResult);

                _sendDone.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Starts receiving data from the server.
        /// </summary>
        /// <param name="client">Client socket.</param>
        private void Receive(Socket client)
        {
            try
            {  
                var state = new ClientState
                {
                    WorkSocket = client
                };

                client.BeginReceive(state.Buffer, 0, ClientState.BufferSize, 0,
                    new AsyncCallback(FinishReceiving), state);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Finishes receiving data from the server.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void FinishReceiving(IAsyncResult asyncResult)
        {
            try
            {
                var state = (ClientState)asyncResult.AsyncState;
                Socket client = state.WorkSocket;

                int bytesRead = client.EndReceive(asyncResult);

                byte[] data = state.Buffer.Take(bytesRead).ToArray();

                ServerResponse = ClientNames.Deserialize(data).Names;
                
                _receiveDone.Set();
            }
            catch
            {
                throw;
            }
        }
    }
}
