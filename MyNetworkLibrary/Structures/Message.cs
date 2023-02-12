namespace MyNetworkLibrary.Structures
{
    public struct Message
    {
        public Guid FromID { get; set; }
        public Guid ToID { get; set; }
        public string Text { get; set; }
    }
}
