namespace restful_code.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public int ArtworkId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string BuyerEmail { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; } = string.Empty; // כרטיס אשראי, מזומן, העברה בנקאית
        public string Status { get; set; } = "completed"; // completed, pending, cancelled
        public string? Notes { get; set; }
    }
}
