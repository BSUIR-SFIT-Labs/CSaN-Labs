using System.Net.Sockets;

namespace FingerLib.Server
{
    /// <summary>
    /// The state of the Server.
    /// </summary>
    class ServerState
    {
        public const int BufferSize = 65536;

        public Socket WorkSocket { get; set; }

        public byte[] Buffer { get; set; }

        public string PcName { get; set; }

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        public ServerState()
        {
            WorkSocket = null;
            Buffer = new byte[BufferSize];
            PcName = string.Empty;
        }
    }
}
