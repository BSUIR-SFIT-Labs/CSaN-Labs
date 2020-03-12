using FileSenderLib.Sender;
using System;

namespace Sender
{
    class Sender
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path to file: ");
            string pathToFile = Console.ReadLine();

            var tcpSender = new TcpSender();
            tcpSender.Start("127.0.0.1", 13);
            tcpSender.SendFile(pathToFile);
            tcpSender.Stop();

            Console.WriteLine("END!");
            Console.ReadLine();
        }
    }
}
