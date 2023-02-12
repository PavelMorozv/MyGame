using MyNetworkLibrary.Classes;

namespace MyNetworkLibrary
{
    public class Room
    {
        private List<Client> clients;
        
        
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = "No name";
        public int MaxCount { get; set; } = 2;



        public bool IsFull { get { return clients.Count < MaxCount; } }



        public Room(Guid id, string name, int maxCount)
        {
            Id= id;
            Name = name;
            MaxCount = maxCount;
            clients = new List<Client>();
        }

        public void AddClient(Client client)
        {
            
        }
    }
}
