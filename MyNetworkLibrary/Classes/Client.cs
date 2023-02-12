using System.Net.Sockets;
using System.Text.Json;

namespace MyNetworkLibrary.Classes
{
    public class Client
    {
        TcpClient tcpClient;
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = "Unknown";
        public int Status { get; set; }

        public Guid RoomID { get; set; } = Guid.Empty;
        public bool IsAuth { get; set; } = false;

        public bool IsConnect { get; private set; }
        public bool IsAvailable() 
        {
            try
            {
                return tcpClient.Available >= 4;
            }
            catch (Exception)
            {

            }

            IsConnect = false;
            return false;

        }

        public Client(string hostIP, int port)
        {
            tcpClient = new TcpClient(hostIP, port);
            IsConnect = true;
        }

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            IsConnect = true;
        }

        public string Read()
        {
            string result = string.Empty;
            try
            {
                byte[] dataBytes = new byte[4];
                NetworkStream stream = tcpClient.GetStream();

                stream.Read(dataBytes, 0, dataBytes.Length);
                int dataSize = BitConverter.ToInt32(dataBytes);

                dataBytes = new byte[dataSize];
                stream.Read(dataBytes, 0, dataSize);
                return JsonSerializer.Deserialize<string>(dataBytes);
            }
            catch (Exception e)
            {
                IsConnect = false;
                return result;
            }

        }

        public void Write(string data)
        {
            try
            {
                var dataBytes = JsonSerializer.SerializeToUtf8Bytes(data);
                var dataSize = BitConverter.GetBytes(dataBytes.Length);

                List<byte> bytesRecive = new List<byte>();
                bytesRecive.AddRange(dataSize);
                bytesRecive.AddRange(dataBytes);

                tcpClient.GetStream().Write(bytesRecive.ToArray(), 0, bytesRecive.Count);
            }
            catch (Exception e)
            {
                IsConnect = false;
            }
        }
    }
}