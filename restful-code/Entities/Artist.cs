namespace restful_code.Entities
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Style { get; set; } = string.Empty;
        public string Status { get; set; } = "active"; // active, inactive
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

