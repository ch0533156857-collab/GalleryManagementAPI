namespace restful_code.Entities
{
    public class Exhibition
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string CuratorName { get; set; } = string.Empty;
        public List<int> ArtworkIds { get; set; } = new List<int>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
