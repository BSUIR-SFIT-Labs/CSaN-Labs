using FileSenderLib.Listener;
using System;
using System.Threading.Tasks;

namespace Listener
{
    class Listener
    {
        static int Main(string[] args)
        {
            Console.Write("Enter the IP address: ");
            string ipAddress = Console.ReadLine();

            Console.Write("Enter port: ");
            int port = 0;
            try
            {
                port = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: need to enter a number!");
                return 1;
            }

            Console.Write("Enter the folder to save the received files: ");
            string pathToFolder = Console.ReadLine() + @"\";

            TcpListener listener = null;
         
           
            try
            {
                listener = new TcpListener(pathToFolder);
                Task.Run(() => listener.Start(ipAddress, port));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Server is running!");
 

            Console.Write("Enter 'stop' to stop accepting files: ");
            string command = Console.ReadLine();

            if (command == "stop" && listener != null)
            {
                try
                {
                    listener.Stop();
                }
                catch
                {
                    Console.WriteLine("The server is stopped.");
                }
            }

            Console.WriteLine("To close the program, press ENTER...");
            Console.ReadLine();
            return 0;
        }
    }
}
