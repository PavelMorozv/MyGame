using MyNetworkLibrary.Classes;

namespace App_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ManagementServer server = new ManagementServer(new ConnectionServer("127.0.0.1", 8888));
            server.Start();


            string com;

            do
            {
                com = Console.ReadLine();

                if (com == "stop")
                {
                    server.Stop();
                }

            } while (server.isRun);

        }
    }
}