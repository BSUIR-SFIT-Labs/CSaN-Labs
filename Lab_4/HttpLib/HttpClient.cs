using System;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace HttpLib
{
    public class HttpClient : IDisposable
    {
        public const int HttpPort = 80;

        private readonly TcpClient _tcpClient;
        private NetworkStream _networkStream;

        public HttpClient(string hostname)
        {
            _tcpClient = new TcpClient(hostname, HttpPort);
        }

        public string SendRequest(string query)
        {
            _networkStream = _tcpClient.GetStream();

            _networkStream.Write(Encoding.ASCII.GetBytes(query));

            byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];

            int currentNumOfBytesRead;
            var stringBuilder = new StringBuilder();
            do
            {
                try
                {
                    currentNumOfBytesRead = _networkStream.Read(buffer);
                }
                catch (IOException)
                {
                    Dispose();
                    throw new IOException(ExceptionMessages.IOExceptionMessage);
                }

                if (currentNumOfBytesRead == 0)
                {
                    break;
                }

                stringBuilder.AppendLine(Encoding.ASCII.GetString(buffer, 0,
                    currentNumOfBytesRead));
                Array.Clear(buffer, 0, buffer.Length);
            }
            while (currentNumOfBytesRead == buffer.Length);

            return stringBuilder.ToString();
        }

        public void Dispose()
        {
            _networkStream?.Close();
            _tcpClient?.Close();
        }
    }
}
