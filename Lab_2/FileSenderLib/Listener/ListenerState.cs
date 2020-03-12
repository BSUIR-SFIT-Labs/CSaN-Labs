using System.Net.Sockets;

namespace FileSenderLib.Listener
{
    internal class ListenerState
    {
        public const int BufferSize = 214748364;

        public Socket WorkSocket { get; set; }
        public byte[] Buffer { get; set; }

        public ListenerState()
        {
            WorkSocket = null;
            Buffer = new byte[BufferSize];
        }
    }
}