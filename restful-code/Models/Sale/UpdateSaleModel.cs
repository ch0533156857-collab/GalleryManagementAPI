using System.ComponentModel.DataAnnotations;

namespace restful_code.Models.Sale
{
    public class UpdateSaleModel
    {
        [Required(ErrorMessage = "מזהה היצירה הוא שדה חובה")]
        [Range(1, int.MaxValue, ErrorMessage = "מזהה יצירה חייב להיות חיובי")]
        public int ArtworkId { get; set; }

        [Required(ErrorMessage = "שם הקונה הוא שדה חובה")]
        [StringLength(100, ErrorMessage = "שם הקונה לא יכול להיות ארוך מ-100 תווים")]
        public string BuyerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "אימייל הקונה הוא שדה חובה")]
        [EmailAddress(ErrorMessage = "כתובת אימייל לא תקינה")]
        [StringLength(100, ErrorMessage = "אימייל לא יכול להיות ארוך מ-100 תווים")]
        public string BuyerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "מחיר מכירה הוא שדה חובה")]
        [Range(0.01, double.MaxValue, ErrorMessage = "מחיר מכירה חייב להיות חיובי")]
        public decimal SalePrice { get; set; }

        [StringLength(50, ErrorMessage = "אמצעי תשלום לא יכול להיות ארוך מ-50 תווים")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "סטטוס הוא שדה חובה")]
        [RegularExpression("^(pending|completed|cancelled)$", ErrorMessage = "סטטוס לא תקין")]
        public string Status { get; set; } = string.Empty;
    }
}
