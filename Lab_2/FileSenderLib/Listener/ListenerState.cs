using System.Net.Sockets;

namespace FileSenderLib.Listener
{
    /// <summary>
    /// The state of the TcpListener object.
    /// </summary>
    class ListenerState
    {
        public const int BufferSize = 4096;

        public Socket WorkSocket { get; set; }
        public byte[] Buffer { get; set; }

        public long NeedToReceivedBytes { get; set; }
        public long ByteReceived { get; set; }
        public bool IsFirstBlockReceived { get; set; }

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        public ListenerState()
        {
            WorkSocket = null;
            Buffer = new byte[BufferSize];

            NeedToReceivedBytes = 0;
            ByteReceived = 0;
            IsFirstBlockReceived = true;
        }
    }
}
