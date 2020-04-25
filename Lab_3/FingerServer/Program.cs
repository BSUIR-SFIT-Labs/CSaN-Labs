using System;

namespace FingerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server is running!");
            Console.WriteLine("Waiting for user connection...");

            var server = new FingerLib.Server.FingerServer();
            server.Start("127.0.0.1");
        }
    }
}
