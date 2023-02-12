using MyNetworkLibrary.Classes;

namespace App_Client_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 8888);

            do
            {
                if (client.IsAvailable)
                {
                    Console.WriteLine("Server recive" + client.Read());
                }
                Thread.Sleep(100);
            } while (true);
        }
    }
}