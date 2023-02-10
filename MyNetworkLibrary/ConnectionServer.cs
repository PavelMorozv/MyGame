using System.Net;
using System.Net.Sockets;

namespace MyNetworkLibrary
{
    public class ConnectionServer
    {
        private TcpListener TcpListener { get; set; }
        private Queue<TcpClient> clients = new Queue<TcpClient>();

        public TcpClient GetClients { get { Console.WriteLine("Клиентов в очереди на подключение: " + IsNewConnected);  return clients.Dequeue(); } }
        public int IsNewConnected { get { return clients.Count; } }
        public bool isRun { get; private set; }



        public ConnectionServer(string ip, int port)
        {
            Console.WriteLine("Запуск сервера по адресу: " + ip + ":" + + port);
            TcpListener = new TcpListener(IPAddress.Parse(ip), port);
        }
        public void Start()
        {
            isRun = true;

            TcpListener.Start();
            Task.Run(() => {
                TcpListener.Start();
                while (isRun)
                {
                    Process();
                    Thread.Sleep(1000);
                }

                TcpListener.Stop();
            });
        }
        public void Stop()
        {
            isRun = false;
        }



        public void Process()
        {
            ConnectingСlients();
            //Console.WriteLine("Await client connection...");
        }
        
        public void ConnectingСlients()
        {
            if (!TcpListener.Pending()) return;
            Console.WriteLine("New connection...");
            clients.Enqueue(TcpListener.AcceptTcpClient());
        }
    }
}