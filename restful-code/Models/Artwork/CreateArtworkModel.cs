using System.ComponentModel.DataAnnotations;

namespace restful_code.Models.Artwork
{
    public class CreateArtworkModel
    {
        [Required(ErrorMessage = "כותרת היצירה היא שדה חובה")]
        [StringLength(200, ErrorMessage = "כותרת לא יכולה להיות ארוכה מ-200 תווים")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "מזהה האמן הוא שדה חובה")]
        [Range(1, int.MaxValue, ErrorMessage = "מזהה אמן חייב להיות חיובי")]
        public int ArtistId { get; set; }

        [StringLength(100, ErrorMessage = "סוג החומר לא יכול להיות ארוך מ-100 תווים")]
        public string Medium { get; set; } = string.Empty;

        [Range(1, 9999, ErrorMessage = "שנת יצירה חייבת להיות בין 1 ל-9999")]
        public int YearCreated { get; set; }

        [StringLength(50, ErrorMessage = "מידות לא יכולות להיות ארוכות מ-50 תווים")]
        public string Dimensions { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "תיאור לא יכול להיות ארוך מ-500 תווים")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "מחיר הוא שדה חובה")]
        [Range(0, int.MaxValue, ErrorMessage = "מחיר לא יכול להיות שלילי")]
        public int Price { get; set; }
    }
}

