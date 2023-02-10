namespace MyNetworkLibrary
{
    public class ManagementServer
    {
        private ConnectionServer connectionServer;
        private List<Client> clients;

        private DateTime deltaTime = DateTime.Now;

        public bool isRun { get { return connectionServer.isRun; } }




        public ManagementServer(ConnectionServer connectionServer)
        {
            this.connectionServer = connectionServer;
            clients = new List<Client>();
        }

        public void Start()
        {
            if (!connectionServer.isRun) connectionServer.Start();

            Task.Run(() => {
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
            GetAwaitClients();

            if ((DateTime.Now - deltaTime).TotalSeconds > 5)
            {
                deltaTime = DateTime.Now;
                BroadCastTime();
            }

            RemoveLostClient();
        }

        public void GetAwaitClients()
        {
            while (connectionServer.IsNewConnected>0)
            {
                var client = new Client(connectionServer.GetClients);
                client.Id = Guid.NewGuid();
                clients.Add(client);
            }
        }

        public void BroadCastTime()
        {
            var message = DateTime.Now.ToLongTimeString();
            Console.WriteLine("Send message: " + message);

            if (clients.Count > 0)
            {
                clients.ForEach((c) => c.Write(message));
            }
        }

        public void RemoveLostClient()
        {
            clients.RemoveAll((c) => c.IsConnect == false);
        }
    }
}
