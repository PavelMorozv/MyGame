using System.Net.Sockets;

namespace MyNetworkLibrary
{
    public class Server
    {

        private TcpListener TcpListener { get; set; }
        private Queue<TcpClient> clients = new Queue<TcpClient>();

        public TcpClient GetClients { get { Console.WriteLine("Клиентов в очереди на подключение: " + IsNewConnected);  return clients.Dequeue(); } }
        public int IsNewConnected { get { return clients.Count; } }
        public bool isRun { get; private set; }



        public void Start()
        {
            isRun = true;

            TcpListener.Start();
            Task.Run(() => {
                while (isRun)
                {
                    Process();
                    Thread.Sleep(100);
                }
            });
        }
        public void Stop()
        {
            TcpListener.Stop();
        }



        public void Process()
        {
            ConnectingСlients();
            Console.WriteLine("Await client connection...");
        }
        
        public void ConnectingСlients()
        {
            if (!TcpListener.Pending()) return;
            Console.WriteLine("New connection...");
            clients.Enqueue(TcpListener.AcceptTcpClient());
        }
    }
}