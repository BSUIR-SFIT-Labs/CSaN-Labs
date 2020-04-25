using System.Net.Sockets;
using System.Text;

namespace FingerLib.Client
{
    /// <summary>
    /// The state of the Client.
    /// </summary>
    class ClientState
    {
        public const int BufferSize = 65536;

        public Socket WorkSocket { get; set; }

        public byte[] Buffer { get; set; }

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        public ClientState()
        {
            WorkSocket = null;
            Buffer = new byte[BufferSize];
        }
    }
}
