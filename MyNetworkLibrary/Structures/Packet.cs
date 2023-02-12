using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace MyNetworkLibrary.Enums
{
    public struct Packet
    {
        [JsonIgnore]
        public Guid clientId { get; set; }
        public PacketType Type { get; set; }
        public PacketStatus Status { get; set; }
        public byte[] Data { get; set; }
    }
}
