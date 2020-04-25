using System;

namespace FingerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the query string: ");

            string query = Console.ReadLine();

            Console.WriteLine("Connecting to the server...");

            var client = new FingerLib.Client.FingerClient();
            client.SendQuery("127.0.0.1", query);

            Console.WriteLine();

            Console.WriteLine("Server response:");
            for (int i = 0; i < client.ServerResponse.Count; i++)
            {
                Console.WriteLine($"{i + 1}.) {client.ServerResponse[i]}");
            }

            Console.WriteLine();
            Console.WriteLine("Disconnected from the server.");
            Console.WriteLine("Press ENTER to exit the program...");
            Console.ReadLine();
        }
    }
}
