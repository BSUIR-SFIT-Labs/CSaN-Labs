using FileSenderLib.Sender;
using System;

namespace Sender
{
    class Sender
    {
        static int Main(string[] args)
        {
            Console.Write("Enter the IP address: ");
            string ipAddress = Console.ReadLine();

            Console.Write("Enter port: ");
            int port;
            try
            {
                port = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: need to enter a number!");
                return 1;
            }

            try
            {
                var tcpSender = new TcpSender();
                tcpSender.Start(ipAddress, port);

                Console.Write("Enter path to file: ");
                string pathToFile = Console.ReadLine();

                tcpSender.SendFile(pathToFile);

                tcpSender.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Stop!");
            Console.ReadLine();
            return 0;
        }
    }
}
