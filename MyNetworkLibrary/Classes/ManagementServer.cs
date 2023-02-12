using MyNetworkLibrary.Enums;
using System.Text;
using System.Text.Json;

namespace MyNetworkLibrary.Classes
{
    public class ManagementServer
    {
        private ConnectionServer connectionServer;
        private List<Client> clients;
        private List<Packet> commands;

        private DateTime deltaTime = DateTime.Now;

        public bool isRun { get { return connectionServer.isRun; } }

        public ManagementServer(ConnectionServer connectionServer)
        {
            this.connectionServer = connectionServer;
            clients = new List<Client>();
            commands = new List<Packet>();
        }

        public void Start()
        {
            if (!connectionServer.isRun) connectionServer.Start();

            Task.Run(() =>
            {
                while (connectionServer.isRun)
                {
                    Process();
                    Thread.Sleep(100);
                }
            });
        }

        public void Stop()
        {
            connectionServer.Stop();
        }




        public void Process()
        {
            AwaitClients();
            ReceivingPackages();
            CommandProcessing();
            RemoveLostClient();
            Console.WriteLine("[Server][Clients is connected] - " + clients.Count);
        }

        private void AwaitClients()
        {
            while (connectionServer.IsNewConnected > 0)
            {
                var client = new Client(connectionServer.GetClients);
                client.Id = Guid.NewGuid();
                clients.Add(client);
            }
        }

        private void ReceivingPackages()
        {
            Packet tempPacket;
            foreach (var client in clients)
            {
                if (client.IsAvailable())
                {
                    try
                    {
                        string temp = client.Read();
                        if (temp == string.Empty) return;
                        tempPacket = JsonSerializer.Deserialize<Packet>(temp);
                        tempPacket.clientId = client.Id;

                        Console.WriteLine("[Server][Command][New][" +
                            tempPacket.clientId + "]: " +
                            tempPacket.Type.ToString());

                        commands.Add(tempPacket);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Пакет неверного формата" + e);
                    }
                }
            }
        }

        private void CommandProcessing()
        {
            foreach (var comand in commands)
            {
                switch (comand.Type)
                {
                    case PacketType.Info:
                        var com = new Packet()
                        {
                            Type = PacketType.Info,
                            Status = PacketStatus.OK,
                            Data = Encoding.UTF8.GetBytes(DateTime.Now.ToLongTimeString())
                        };
                        clients.FirstOrDefault(c => c.Id == comand.clientId)?.Write(JsonSerializer.Serialize(com));
                        break;
                    case PacketType.Message:
                        break;
                    case PacketType.Auth:
                        break;
                    default:
                        break;
                }
            }
            commands.Clear();
        }

        private void RemoveLostClient()
        {
            clients.Where(c => c.IsConnect == false).ToList().ForEach((c) => Console.WriteLine($"[Server][Client: {c.Id}] Lost connection"));
            clients.RemoveAll(c => c.IsConnect == false);
        }
    }
}
