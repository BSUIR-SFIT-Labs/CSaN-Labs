using FileSenderLib.Listener;
using System;

namespace Listener
{
    class Listener
    {
        static void Main(string[] args)
        {
            var listener = new TcpListener();
            listener.Start("127.0.0.1", 13);

            listener.Stop();
            Console.WriteLine("END!");
            Console.ReadLine();
        }
    }
}
