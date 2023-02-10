using System.Net.Sockets;
using System.Text.Json;

namespace MyNetworkLibrary
{
    public class Client
    {
        TcpClient tcpClient;
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = "Unknown";
        public int Status { get; set; }

        public Guid RoomID { get; set; } = Guid.Empty;
        public bool IsAuth { get; set; } = false;

        public bool IsCnnect { get; private set; } = false;
        public bool IsAvailable { get { return tcpClient.Available >= 4; } }

        public Client(string hostIP, int port)
        {
            tcpClient = new TcpClient(hostIP, port);
        }

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public string Read()
        {
            string result = string.Empty;
            byte[] dataBytes = new byte[sizeof(int)];
            if (IsAvailable)
            {
                NetworkStream stream = tcpClient.GetStream();

                stream.Read(dataBytes, 0, dataBytes.Length);
                int dataSize = BitConverter.ToInt32(dataBytes);


                stream.Read(dataBytes, 0, dataSize);
                result = JsonSerializer.Deserialize<string>(dataBytes);
                
            }

            return result;
        }

        public void Write(string data)
        {
            var dataBytes = JsonSerializer.SerializeToUtf8Bytes(data);
            var dataSize = BitConverter.GetBytes(dataBytes.Length);

            List<byte> bytesRecive = new List<byte>();
            bytesRecive.AddRange(dataSize);
            bytesRecive.AddRange(dataBytes);

            tcpClient.GetStream().Write(bytesRecive.ToArray(), 0, bytesRecive.Count);
        }
    }
}
