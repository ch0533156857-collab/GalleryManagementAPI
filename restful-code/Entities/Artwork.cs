namespace restful_code.Entities
{
    public class Artwork
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public string Medium { get; set; } = string.Empty; // שמן על בד, צבעי מים וכו'
        public int YearCreated { get; set; }
        public decimal Price { get; set; }
        public string Dimensions { get; set; } = string.Empty; // למשל "100x80 ס\"מ"
        public string Status { get; set; } = "available"; // available, sold, on_loan, reserved
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
