using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FileSenderLib.Listener
{
    public class TcpListener
    {
        //Remove
        private const int Offset = 0;

        private const int MaxConnections = 100;

        private readonly ManualResetEvent _allDone;

        private Socket _listener;

        public TcpListener()
        {
            _allDone = new ManualResetEvent(false);
        }

        public void Start(string ipString, int port)
        {
            var ipAddress = IPAddress.Parse(ipString);
            var endpoint = new IPEndPoint(ipAddress, port);

            _listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _listener.Bind(endpoint);
                _listener.Listen(MaxConnections);

                while (true)
                {
                    _allDone.Reset();

                    // Notify SERVER START

                    _listener.BeginAccept(new AsyncCallback(AcceptConnection),
                        _listener);

                    _allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AcceptConnection(IAsyncResult asyncResult)
        {
            _allDone.Set();

            var listener = (Socket)asyncResult.AsyncState;
            var handler = listener.EndAccept(asyncResult);

            var listenerState = new ListenerState
            {
                WorkSocket = handler
            };
            handler.BeginReceive(listenerState.Buffer, Offset,
                ListenerState.BufferSize, SocketFlags.None,
                new AsyncCallback(EndRecive), listenerState);
        }

        private void EndRecive(IAsyncResult asyncResult)
        {
            var listenerState = (ListenerState)asyncResult.AsyncState;
            var handler = listenerState.WorkSocket;

            int bytesRead = handler.EndReceive(asyncResult);

            if (bytesRead > 0)
            {
                Console.WriteLine("Yes!");
                var transmittedFile = new TransmittedFile(listenerState.Buffer);
                transmittedFile.Create(@"C:\Users\mrkon\Downloads\");
                /*if (listenerState.IsFirstRecive)
                {
                    // Rename
                    byte[] needToRecive = listenerState.Buffer.Take(8).ToArray();

                    listenerState.NeedToRecive = BitConverter.ToInt32(needToRecive,
                        needToRecive.Length);

                    listenerState.IsFirstRecive = false;
                }*/
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        public void Stop()
        {
            _listener.Shutdown(SocketShutdown.Both);
            _listener.Close();
        }
    }
}