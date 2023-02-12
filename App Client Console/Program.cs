using MyNetworkLibrary.Classes;
using MyNetworkLibrary.Enums;
using System.Text;
using System.Text.Json;

namespace App_Client_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 8888);
            Packet packet;
            do
            {
                if (client.IsAvailable())
                {
                    packet = JsonSerializer.Deserialize<Packet>(client.Read());
                    if (packet.Status == PacketStatus.OK)
                    {
                        Console.WriteLine("Server recive Status OK" + Encoding.UTF8.GetString(packet.Data));
                    }
                    else if (packet.Status == PacketStatus.Error)
                    {
                        Console.WriteLine("Server recive status Error" + Encoding.UTF8.GetString(packet.Data));
                    }
                }
                Thread.Sleep(500);

                packet = new Packet()
                {
                    Type = PacketType.Info
                };

                client.Write(JsonSerializer.Serialize(packet));
            } while (true);
        }
    }
}